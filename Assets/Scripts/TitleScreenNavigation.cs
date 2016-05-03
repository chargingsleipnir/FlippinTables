﻿using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;

public class TitleScreenNavigation : MonoBehaviour {

    enum TitleScreenState { transitioningIn, inMain, startingGame, inOptions, inExtras, none }
    TitleScreenState state;

    BlinkerController blinker;

    Button[] uiButtons;

	void Start () {
        blinker = GetComponent<BlinkerController>();

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

    public void OnExtrasBtn()
    {
        Debug.Log("Launch extras screen");
        state = TitleScreenState.inExtras;
    }
}
