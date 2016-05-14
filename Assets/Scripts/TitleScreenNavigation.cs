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

	void Start () {
        // THIS MAY NEED TO COME OUT AND BE REPLACED WITH A PROPER EXIT BUTTON
        //Screen.fullScreen = false;


        blinker = GetComponent<BlinkerController>();
		audio = GetComponent<AudioSource> ();

        uiButtons = GetComponentsInChildren<Button>();

        Reset();
        state = TitleScreenState.none;
    }

    void Update ()
    {
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
				audio.Play ();
                state = TitleScreenState.inMain;
            }
            break;
        case TitleScreenState.inMain:
            break;
        case TitleScreenState.startingGame:
            if(!blinker.CheckOpen())
            {
                state = TitleScreenState.none;
                blinker.EndSceneTrigger();
                SceneManager.LoadScene(1);
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
        Debug.Log("Launch options screen");
        state = TitleScreenState.inOptions;
    }

    public void OnExitBtn()
    {
        Application.Quit();
    }

    public void OnExtrasBtn()
    {
        Debug.Log("Launch extras screen");
        state = TitleScreenState.inExtras;
    }
}
