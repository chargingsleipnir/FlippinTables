using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class InGameUIController : MonoBehaviour {

    BlinkerController blinker;

    public GameObject gameOverMenu;
    public GameObject highScoreRibbon;

    public Text flipsRoundTxt;
    public Text flipsBestTxt;

    public Text scoreInGameTxt;
    public Text scoreRoundTxt;
    public Text scoreBestTxt;

    public Text comboInGameTxt;
    public Text comboRoundHighTxt;
    public Text comboBestTxt;

    void Start()
    {
        blinker = GetComponent<BlinkerController>();
    }

    public void UpdateHUD(int score, int comboVal)
    {
        scoreInGameTxt.text = score.ToString();
        comboInGameTxt.text = "x " + comboVal.ToString();
    }
    public void UpdateEndGameResults(int flips, int flipsBest, int score, int scoreBest, int comboRound, int comboBest, bool newHigh)
    {
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
    }

    public void ActivateGameOverMenu()
    {
        gameOverMenu.SetActive(true);
    }

    public void OnReplayBtn()
    {
        //blinker.Close();
    }

    public void OnExitBtn()
    {
        //blinker.Close();
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
