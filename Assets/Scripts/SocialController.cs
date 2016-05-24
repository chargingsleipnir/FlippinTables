using UnityEngine;
using System;
using System.Collections;
using UnityEngine.UI;
using GooglePlayGames;
using UnityEngine.SocialPlatforms;

public class SocialController : MonoBehaviour {

	void Start ()
    {
        //PlayGamesPlatform.DebugLogEnabled = true;
        PlayGamesPlatform.Activate();
    }

    public void Login(Action<bool> LoginCallback, Action AlreadyLoggedCallback)
    {
        if (!Social.localUser.authenticated)
        {
            Social.localUser.Authenticate(LoginCallback);
        }
        else
        {
            AlreadyLoggedCallback();
        }
    }

    public void Logout()
    {
        ((PlayGamesPlatform)Social.Active).SignOut();
    }

    public void ReportBests(long bestScore, long bestCombo, long mostFlips, Action<bool> scorePostedCallback, Action<bool> comboPostedCallback, Action<bool> flipsPostedCallback)
    {
        Social.ReportScore(bestScore, Constants.BOARD_ID_SCORE, scorePostedCallback);
        Social.ReportScore(bestCombo, Constants.BOARD_ID_COMBO, comboPostedCallback);
        Social.ReportScore(mostFlips, Constants.BOARD_ID_FLIPS, flipsPostedCallback);
    }
    public void ShowLeaderBoard(string leaderboardID)
    {
        if (Social.localUser.authenticated)
        {
            ((PlayGamesPlatform)Social.Active).ShowLeaderboardUI(leaderboardID);
        }
    }
    public void ReportProgress(string achId, Action<bool> achReachedCallback)
    {
        if (Social.localUser.authenticated)
        {
            // Progress is 100.0 while I am only using binary achievements
            Social.ReportProgress(achId, 100.0, achReachedCallback);
        }
    }
    public bool ShowAchievements()
    {
        if (Social.localUser.authenticated)
        {
            ((PlayGamesPlatform)Social.Active).ShowAchievementsUI();
        }
        return Social.localUser.authenticated;
    }
}
