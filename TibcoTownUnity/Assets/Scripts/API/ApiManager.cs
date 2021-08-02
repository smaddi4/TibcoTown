using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BestHTTP;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using UnityEngine;

public class ApiManager : MonoBehaviour
{
    public static ApiManager  Instance;

    private void Awake()
    {
        Instance = this;
    }
    
    private HTTPRequest CreateRequest(string      path, OnRequestFinishedDelegate onRequestFinished,
                                      HTTPMethods method = HTTPMethods.Post)
    {
        using (HTTPRequest request = new HTTPRequest(new Uri(ServerVariables.UrlBase + path), method, onRequestFinished))
        {
            request.SetHeader("Content-Type", "application/json; charset=UTF-8");
            return request;
        }
    }

    private void CreateRequestAndSend(string      path, object obj, OnRequestFinishedDelegate onRequestFinished,
                                      HTTPMethods method = HTTPMethods.Post)
    {
        using (HTTPRequest request = CreateRequest(path, onRequestFinished, method))
        {
            if (obj != null)
            {
                var jsonObject = JsonConvert.SerializeObject(obj);
                Debug.Log(jsonObject);
                var rawData = Encoding.Default.GetBytes(jsonObject);
                request.RawData = rawData;
            }
            request.Send();
        }
    }

    /// <summary>
    /// Creates and sends a HTTP request of type <see cref="method"/>. Once a response is received, calls
    /// <see cref="onSuccess"/> or <see cref="onFailure"/> based on the response.
    /// </summary>
    private void SendRequest(string url, object message, Action<string> onSuccess, Action<int, string> onFailure,
                             HTTPMethods method = HTTPMethods.Post)
    {
       CreateRequestAndSend(url, message, (request, response) =>
        {
            switch (request.State)
            {
                case HTTPRequestStates.Finished:
                    // No need to check if response is null in the next line. When request is finished, response
                    // must be non-null.
                    if (response.IsSuccess)
                    {
                        // The server sometimes sends 200 status code in the response, but it is not actually
                        // a success response. The failure response is embedded in the response JSON itself.
                        // So, we need to parse the JSON and ensure that we get the 200 response before we
                        // move forward with the game.
                        // i.e., sometimes - response.IsSuccess = true, but responseJson.status = 400
                        
                        //Hard coding to remove extra characters
                        List<char> _str = new List<char>(response.DataAsText.ToCharArray().ToList());
                        _str.RemoveAt(0);
                        _str.RemoveAt(_str.Count - 1);
                        _str.RemoveAll(x => x.Equals('\\'));
                        string _output = String.Empty;
                        foreach (var st in _str)
                        {
                            _output = $"{_output}{st}";
                        }
                        Debug.LogFormat("response: {0}", _output);

                        /*const string regex = @"\\(u|x)[[a-z\d]{1,4}\r\n";
    
                        string _output = Regex.Replace(response.DataAsText, regex, string.Empty);
                            
                        Debug.LogFormat("response: {0}", _output);*/
                        Response _resp = JsonConvert.DeserializeObject<Response>(_output);
                        
                        try
                        {
                            if (_resp == null)
                            {
                                GenericPopupBehaviour.Instance.SetValues("System Error!", "Please try again later.",  "OKAY");
                                GameManagerBehaviour.Instance.ApiStateChanged(ApiState.Response);
                                return;
                            }
                            
                            switch (_resp.Status.StatusCode)
                            {
                                case GameConstants.Success:
                                    onSuccess?.Invoke(_output);
                                    break;
                                
                                case GameConstants.UserNotFound:
                                case GameConstants.SystemError:
                                case GameConstants.InvalidPin:
                                    onFailure?.Invoke(_resp.Status.StatusCode, _output);
                                    GameManagerBehaviour.Instance.ApiStateChanged(ApiState.Response);
                                    break;
                                
                                default:
                                    onFailure?.Invoke(_resp.Status.StatusCode, _output);
                                    break;
                            }
                        }
                        catch (InvalidCastException e)
                        {
                            Debug.LogFormat("{0}", e);
                        }
                        
                    }
                    else
                    {
                        onFailure?.Invoke(response.StatusCode, response.DataAsText);
                    }

                    break;
                case HTTPRequestStates.Error:
                case HTTPRequestStates.Aborted:
                case HTTPRequestStates.ConnectionTimedOut:
                case HTTPRequestStates.TimedOut:
                    Debug.LogError($"Error {request.State}");
                    break;
            }

            if (response != null && !response.IsSuccess)
            {
                Debug.LogError(response);
            }
        }, method);
    }

    public void Post(object formData, Action<string> resp, Action<int, string> failure = default)
    {
        SendRequest(GameManagerBehaviour.Instance.BaseURL, formData, resp, failure);
    }
}

internal class ServerVariables
{
    public static string UrlBase { get; set; }
}
