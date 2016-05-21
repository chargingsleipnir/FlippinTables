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

    AdController adCtrl;
    bool adLaunched;

    void Start () {
        // THIS MAY NEED TO COME OUT AND BE REPLACED WITH A PROPER EXIT BUTTON
        //Screen.fullScreen = false;

        adCtrl = GameObject.Find("Persistor").GetComponent<AdController>();
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
                break;
            case TitleScreenState.inExtras:
                break;
        }
    }

    void Reset()
    {
        foreach (Button button in uiButtons)
            button.interactable = false;

		audio.loop = false;
		audio.Stop ();
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

    public void OnExitBtn()
    {
        Application.Quit();
    }

    public void OnExtrasBtn()
    {
        state = TitleScreenState.inExtras;
    }
}
