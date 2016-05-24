using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;

public class TitleScreenNavigation : MonoBehaviour {

    enum TitleScreenState { transitioningIn, inMain, startingGame, inOptions, inExtras, none }
    TitleScreenState state;

    BlinkerController blinker;

    Button[] uiButtons;

	AudioSource audio;
	public AudioClip music;

    public RectTransform spiralImgTrans;

    float angle;

    GameObject persistorObj;

    AdController adCtrl;

    SocialController socCtrl;
    public Text loginText;

    bool adLaunched;

    float achTimer;
    bool achReached;
    string achMsg;
    public Text achievementText;

    void Start () {
        // THIS MAY NEED TO COME OUT AND BE REPLACED WITH A PROPER EXIT BUTTON
        //Screen.fullScreen = false;

        persistorObj = GameObject.Find("Persistor");
        adCtrl = persistorObj.GetComponent<AdController>();
        socCtrl = persistorObj.GetComponent<SocialController>();

        adLaunched = false;

        blinker = GetComponent<BlinkerController>();
		audio = GetComponent<AudioSource> ();

        uiButtons = GetComponentsInChildren<Button>();

        Reset();
        state = TitleScreenState.none;

        angle = 0.0f;
    }

    void Update ()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            OnExitBtn();
        }

        angle += 3.0f;
        if (angle >= 360.0f)
            angle = 0.0f;

        spiralImgTrans.rotation = Quaternion.Euler(0.0f, 0.0f, angle);

        switch (state)
        {
            case TitleScreenState.none:
                blinker.Open();
                state = TitleScreenState.transitioningIn;
                break;
            case TitleScreenState.transitioningIn:
                if (blinker.CheckOpen())
                {
                    foreach (Button button in uiButtons)
                        button.interactable = true;

                    audio.clip = music;
                    audio.loop = true;
                    audio.Play();
                    state = TitleScreenState.inMain;
                }
                break;
            case TitleScreenState.inMain:
                achTimer -= Time.deltaTime;
                break;
            case TitleScreenState.startingGame:
                if (!blinker.CheckOpen())
                {
                    if (adLaunched == false)
                    {
                        adCtrl.CountReset();
                        adLaunched = true;
                    }
                    if (!adCtrl.CheckAdShowing())
                    {
                        adLaunched = false;
                        state = TitleScreenState.none;
                        blinker.EndSceneTrigger();
                        SceneManager.LoadScene(2);
                    }
                }
                break;
            case TitleScreenState.inOptions:
                achTimer -= Time.deltaTime;
                break;
            case TitleScreenState.inExtras:
                achTimer -= Time.deltaTime;
                break;
        }

        Debug.Log(achTimer);
        if(achTimer <= 0.0f && achReached == false)
        {
            achReached = true;
            UpdateAchievement(Constants.ACH_SOOTHING_SONG, "Achievement: Soothing Song");
        }
    }

    void Reset()
    {
        foreach (Button button in uiButtons)
            button.interactable = false;

		audio.loop = false;
		audio.Stop ();

        // multiply const minutes by 60 secs/min - use seconds as that's what deltaTime uses.
        achTimer = Constants.ACH_SOOTHING_SONG_MINS * 60.0f;
        achReached = false;
        achMsg = "";
        achievementText.text = "";
    }

    public void OnPlayBtn()
    {
        Reset();
        blinker.Close();
        state = TitleScreenState.startingGame;
    }

    public void OnOptionsBtn()
    {
        state = TitleScreenState.inOptions;
    }

    void LoginCallback(bool didAuth)
    {
        if (didAuth)
        {
            loginText.color = Color.green;
            loginText.text = "Login successful :)";
        }
        else
        {
            loginText.color = Color.red;
            loginText.text = "Login unsuccessful :(";
        }
    }
    void AlreadyLoggedInCallback()
    {
        loginText.color = Color.green;
        loginText.text = "Already logged in :)";
    }

    public void OnLoginBtn()
    {
        socCtrl.Login(LoginCallback, AlreadyLoggedInCallback);
    }
    public void OnLogoutBtn()
    {
        socCtrl.Logout();
        loginText.color = Color.green;
        loginText.text = "Logout successful :)";
    }
    public void OnViewAchBtn()
    {
        if(!socCtrl.ShowAchievements())
        {
            loginText.color = Color.white;
            loginText.text = "Login to view achievements";
        }
    }
    public void OnBackBtn()
    {
        loginText.text = "";
    }

    public void OnExitBtn()
    {
        Application.Quit();
    }

    public void OnExtrasBtn()
    {
        state = TitleScreenState.inExtras;
    }



    void AchievementReachedCallback(bool success)
    {
        if (success)
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

        while (c.a > 0.0f)
        {
            c.a -= 0.01f;
            achievementText.color = c;
            yield return 0;
        }

        achievementText.text = "";
    }

    void UpdateAchievement(string achId, string achMsg)
    {
        this.achMsg = achMsg;
        socCtrl.ReportProgress(achId, AchievementReachedCallback);
    }
}
