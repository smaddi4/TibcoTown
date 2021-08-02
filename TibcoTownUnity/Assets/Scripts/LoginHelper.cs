using System;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.Networking;

[Serializable]
public class Login
{
	public string username;
    public string password;

	public Login(string _username, string _password)
	{
		username = _username;
		password = _password;
	}
}

[Serializable]
public class LoginToken
{
	public string token;
}

public class LoginHelper 
{
    public IEnumerator LoginRequest(Login login, Action<string> OnSuccess, Action<int, string> OnFailure)
    {
	    yield return null;
	    LoginRequest _request = new LoginRequest(356, login.username, login.password, "Test User", "A3", 0);
	    //Login
	    LoginRequestData _login = new LoginRequestData(_request);
	    ApiManager.Instance.Post(_login, OnSuccess, OnFailure);
    }
}
