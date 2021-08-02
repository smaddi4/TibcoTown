using System;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;

[Serializable]
public class Response
{
    public Status Status;
}

[Serializable]
public class ResponseMessge
{
    public string message;

    public ResponseMessge(string msg)
    {
        message = msg;
    }
}

[Serializable]
public class Status
{
    public int StatusCode;
    public string StatusMessage;
}

[Serializable]
public class LoginResponse
{
    public int UserId;
}

[Serializable]
public class User
{
    public LoginResponse LoginResponse;
}

[Serializable]
public class LoginRequest
{
    public int CountryId;
    public string EmailId;
    public string Pin;
    public string Name;
    public string Avatar;
    public int NewUser;

    public LoginRequest() {}

    public LoginRequest(int countryId, string emailId, string pin, string name, string avatar, int newUser)
    {
        CountryId = countryId;
        EmailId = emailId;
        Pin = pin;
        Name = name;
        Avatar = avatar;
        NewUser = newUser;
    }

    public override string ToString()
    {
        return JsonUtility.ToJson(this);
    }
}
/// <summary>
/// Login Request data
/// </summary>
[Serializable]
public class LoginRequestData
{
    public LoginRequest LoginRequest;

    public LoginRequestData() {}

    public LoginRequestData(LoginRequest loginRequest)
    {
        LoginRequest = new LoginRequest(loginRequest.CountryId, loginRequest.EmailId, loginRequest.Pin,
            loginRequest.Name, loginRequest.Avatar, loginRequest.NewUser);
    }

    public override string ToString()
    {
        return JsonUtility.ToJson(this);
    }
}

/// <summary>
/// Login Response data
/// </summary>
[Serializable]
public class LoginResponseData : Response
{
    public LoginResponse LoginResponse;
}


/// <summary>
/// Quiz
/// </summary>

[Serializable]
public class Option
{
    public int OptionId;
    public string OptionDescription;
    [CanBeNull]public string Image;
    public bool IncludesImage;

    public Option()
    {
        OptionId = -1;
    }

}

[Serializable]
public class Question
{
    public int QuestionId;
    public string QuestionDescription;
    public int CorrectAnswerId;
    public string Image;
    public List<Option> Options;
}

[Serializable]
public class GetQuizQuestionsResponse
{
    public List<Question> Questions;
}

[Serializable]
public class QuizResponse : Response
{
    public GetQuizQuestionsResponse GetQuizQuestionsResponse;
    public Status Status;
}


[Serializable]
public class GetQuizQuestionsRequest
{
    public int UserId;

    public GetQuizQuestionsRequest(int userId)
    {
        UserId = userId;
    }
}

[Serializable]
public class GetQuizQuestions 
{
    public GetQuizQuestionsRequest GetQuizQuestionsRequest;

    public GetQuizQuestions(GetQuizQuestionsRequest getQuizQuestionsRequest)
    {
        GetQuizQuestionsRequest = new GetQuizQuestionsRequest(getQuizQuestionsRequest.UserId);
    }

    public override string ToString()
    {
        return JsonUtility.ToJson(this);
    }
}

/// <summary>
/// Quiz response
/// </summary>
[Serializable]
public class QuestionAnswer
{
    public int QuestionId;
    public int AnswerId;
    
    public QuestionAnswer() {}

    public QuestionAnswer(int questionId, int answerId)
    {
        QuestionId = questionId;
        AnswerId = answerId;
    }
}

[Serializable]
public class SubmitQuizRequest
{
    public int UserId;
    public List<QuestionAnswer> QuestionAnswers = new List<QuestionAnswer>();

    public SubmitQuizRequest() {}

    public SubmitQuizRequest(int userId, List<QuestionAnswer> questionAnswers)
    {
        UserId = userId;
        QuestionAnswers = new List<QuestionAnswer>(questionAnswers);
    }
}

[Serializable]
public class SubmitQuiz
{
    public SubmitQuizRequest SubmitQuizRequest;
    
    public SubmitQuiz() {}

    public SubmitQuiz(SubmitQuizRequest submitQuizRequest)
    {
        SubmitQuizRequest = new SubmitQuizRequest(submitQuizRequest.UserId, submitQuizRequest.QuestionAnswers);
    }
    
    public override string ToString()
    {
        return JsonUtility.ToJson(this);
    }
}

/// <summary>
/// Get KPI
/// </summary>
[Serializable]
public class GetKPIDetailsRequest
{
    public int UserId;
    
    public GetKPIDetailsRequest() {}

    public GetKPIDetailsRequest(int userId)
    {
        UserId = userId;
    }
}

[Serializable]
public class GetKPIDetails 
{
    public GetKPIDetailsRequest GetKPIDetailsRequest;
    
    public GetKPIDetails() {}

    public GetKPIDetails(GetKPIDetailsRequest getKpiDetailsRequest)
    {
        GetKPIDetailsRequest = new GetKPIDetailsRequest(getKpiDetailsRequest.UserId);
    }

    public override string ToString()
    {
        return JsonUtility.ToJson(this);
    }
}

/// <summary>
/// KPI Response
/// </summary>
[Serializable]
public class CoinsDetail
{
    public int KPIId;
    public int Coins;
    public int ProgressBar;
    public string Color;
}

[Serializable]
public class KPICoins
{
    public List<CoinsDetail> CoinsDetails;
}

[Serializable]
public class KPI
{
    public int KPIId;
    public string KPIName;
    public bool Enabled;
}

[Serializable]
public class KPIConfigurations
{
    public List<KPI> KPIs;
}

[Serializable]
public class Asset
{
    public string AssetName;
    public int AssetId;
    public int Quantity;
}

[Serializable]
public class AssetDetails
{
    public List<Asset> Assets;
}

[Serializable]
public class LeaderboardUser
{
    public int UserId;
    public string Name;
    public int Coins;
    public string Ranking;
    public string Avatar;
}

[Serializable]
public class Leaderboard
{
    public List<LeaderboardUser> Users;
}

[Serializable]
public class GetKPIDetailsResponse
{
    public int UserId;
    public string Name;
    public string ProfilePic;
    public int Level;
    public string OverallBadge;
    public KPICoins KPICoins;
    public int TotalEarnedCoins;
    public int TotalAvailableCoins;
    public KPIConfigurations KPIConfigurations;
    public AssetDetails AssetDetails;
    public Leaderboard Leaderboard;
}

[Serializable]
public class UserDetails : Response
{
    public GetKPIDetailsResponse GetKPIDetailsResponse;

    public Leaderboard GetLeaderBoard() => GetKPIDetailsResponse.Leaderboard;

    public KPIConfigurations GetKpiConfigurations() => GetKPIDetailsResponse.KPIConfigurations;

    public KPICoins GetKpiCoins() => GetKPIDetailsResponse.KPICoins;

    public AssetDetails GetAssetDetails() => GetKPIDetailsResponse.AssetDetails;
}