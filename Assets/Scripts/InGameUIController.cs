using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using GooglePlayGames;
using UnityEngine.SocialPlatforms;

public class InGameUIController : MonoBehaviour {

    BlinkerController blinker;

    Button[] uiButtons;

    public GameObject gameOverMenu;
    public GameObject highScoreRibbon;

    public GameObject flipsRoundTxtObj;
    Text flipsRoundTxt;
    public Text flipsBestTxt;

    public Text scoreInGameTxt;
    public GameObject scoreRoundTxtObj;
    Text scoreRoundTxt;
    public Text scoreBestTxt;

    public Text comboInGameTxt;
    public GameObject comboRoundHighTxtObj;
    Text comboRoundHighTxt;
    public Text comboBestTxt;

    // Leaderboard things
    GameObject persistorObj;
    SocialController socCtrl;
    public Text postMsgText;

    long
        bestScore,
        bestCombo,
        mostFlips;

    public Image
        imgScorePostConfirm,
        imgComboPostConfirm,
        imgFlipsPostConfirm;

    public Sprite
        checkMark,
        crossOut;

    public Text roundText;
    public GameObject
        scoreLeadBtn,
        comboLeadBtn,
        flipsLeadBtn;

    string achMsg;
    public Text achievementText;

    void Start()
    {
        flipsRoundTxt = flipsRoundTxtObj.GetComponent<Text>();
        scoreRoundTxt = scoreRoundTxtObj.GetComponent<Text>();
        comboRoundHighTxt = comboRoundHighTxtObj.GetComponent<Text>();

        persistorObj = GameObject.Find("Persistor");
        socCtrl = persistorObj.GetComponent<SocialController>();

        blinker = GetComponent<BlinkerController>();

        //uiButtons = GetComponentsInChildren<Button>();

        PlayGamesPlatform.DebugLogEnabled = true;
        PlayGamesPlatform.Activate();
    }

    public void UpdateHUD(int score, int comboVal)
    {
        scoreInGameTxt.text = score.ToString();
        comboInGameTxt.text = "x " + comboVal.ToString();
    }
    public void UpdateEndGameResults(int flips, int flipsBest, int score, int scoreBest, int comboRound, int comboBest, bool newHigh)
    {
        this.bestScore = scoreBest;
        this.bestCombo = comboBest;
        this.mostFlips = flipsBest;

        flipsRoundTxt.text = flips.ToString();
        flipsBestTxt.text = flipsBest.ToString();
        scoreRoundTxt.text = score.ToString();
        scoreBestTxt.text = scoreBest.ToString();
        comboRoundHighTxt.text = "x " + comboRound.ToString();
        comboBestTxt.text = "x " + comboBest.ToString();

        if (newHigh)
            highScoreRibbon.SetActive(true);
    }

    public void Reset()
    {
        scoreInGameTxt.text = "0";
        comboInGameTxt.text = "x 0";

        highScoreRibbon.SetActive(false);
        gameOverMenu.SetActive(false);

        postMsgText.text = "";
        postMsgText.color = Color.white;

        Color c = Color.white;
        c.a = 0.0f;

        imgScorePostConfirm.color = c;
        imgComboPostConfirm.color = c;
        imgFlipsPostConfirm.color = c;

        roundText.text = "Round";

        scoreRoundTxtObj.SetActive(true);
        comboRoundHighTxtObj.SetActive(true);
        flipsRoundTxtObj.SetActive(true);

        scoreLeadBtn.SetActive(false);
        comboLeadBtn.SetActive(false);
        flipsLeadBtn.SetActive(false);

        achMsg = "";
        achievementText.text = "";

        //foreach (Button button in uiButtons)
          //  button.interactable = true;
    }

    public void ActivateGameOverMenu()
    {
        gameOverMenu.SetActive(true);
        //foreach (Button button in uiButtons)
            //button.interactable = true;
    }

    void LoginCallback(bool didAuth)
    {
        if (didAuth)
        {
            postMsgText.color = Color.green;
            postMsgText.text = "Login successful :)";
            PostBests();
        }
        else
        {
            postMsgText.color = Color.red;
            postMsgText.text = "Login unsuccessful :(";
        }
    }
    void AlreadyLoggedInCallback()
    {
        PostBests();
    }

    public void OnPostBtn()
    {
        socCtrl.Login(LoginCallback, AlreadyLoggedInCallback);
    }

    void PostCallbackImgHelper(bool didPost, Image img)
    {
        Color c = img.color;
        c.a = 1.0f;
        img.color = c;

        if (didPost)
        {
            img.sprite = checkMark;
        }
        else
        {
            img.sprite = crossOut;
        }
    }

    void ScorePostedCallback(bool didPost)
    {
        PostCallbackImgHelper(didPost, imgScorePostConfirm); 
    }

    void ComboPostedCallback(bool didPost)
    {
        PostCallbackImgHelper(didPost, imgComboPostConfirm);
    }

    void FlipsPostedCallback(bool didPost)
    {
        PostCallbackImgHelper(didPost, imgFlipsPostConfirm);
    }

    void PostBests()
    {
        socCtrl.ReportBests(bestScore, bestCombo, mostFlips, ScorePostedCallback, ComboPostedCallback, FlipsPostedCallback);

        //postBtnObj.GetComponent<Button>().interactable = false;

        roundText.text = "Boards";

        scoreRoundTxtObj.SetActive(false);
        comboRoundHighTxtObj.SetActive(false);
        flipsRoundTxtObj.SetActive(false);

        scoreLeadBtn.SetActive(true);
        comboLeadBtn.SetActive(true);
        flipsLeadBtn.SetActive(true);

        // Change round scores to leaderboard buttons?
    }

    public void OnScoreLeadButton()
    {
        socCtrl.ShowLeaderBoard(Constants.BOARD_ID_SCORE);
    }
    public void OnComboLeadButton()
    {
        socCtrl.ShowLeaderBoard(Constants.BOARD_ID_COMBO);
    }
    public void OnFlipsLeadButton()
    {
        socCtrl.ShowLeaderBoard(Constants.BOARD_ID_FLIPS);
    }

    void AchievementReachedCallback(bool success)
    {
        if(success)
        {
            // Android actually displays on it's own
            //StartCoroutine(AchTextDisplay());
        }
    }

    IEnumerator AchTextDisplay()
    {
        Color c = Color.green;
        c.a = 1.0f;
        achievementText.text = achMsg;
        achievementText.color = c;
        yield return new WaitForSeconds(3.0f);

        while(c.a > 0.0f)
        {
            c.a -= 0.01f;
            achievementText.color = c;
            yield return 0;
        }

        achievementText.text = "";
    }

    public void UpdateAchievement(string achId, string achMsg)
    {
        this.achMsg = achMsg;
        socCtrl.ReportProgress(achId, AchievementReachedCallback);
    }

    public void OnReplayBtn()
    {
        //foreach (Button button in uiButtons)
           // button.interactable = false;
    }

    public void OnQuitBtn()
    {
        //foreach (Button button in uiButtons)
            //button.interactable = false;
    }

    public void OpenBlinker()
    {
        blinker.Open();
    }

    public void CloseBlinker()
    {
        blinker.Close();
    }

    public bool CheckBlinkerOpen()
    {
        return blinker.CheckOpen();
    }

    public void EndScene()
    {
        blinker.EndSceneTrigger();
    }
}
