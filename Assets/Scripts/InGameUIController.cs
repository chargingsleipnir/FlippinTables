using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class InGameUIController : MonoBehaviour {

    BlinkerController blinker;

    public GameObject gameOverMenu;

    public Text tablesflippedTextInGame;
    public Text tablesflippedTextFinal;
    public Text highScoreText;

    public Text comboTextInGame;
    public Text comboTextFinal;
    public Text highComboText;

    void Start()
    {
        blinker = GetComponent<BlinkerController>();
        // fucked up way to start the level -- change this
        blinker.Open();
    }

    public void UpdateHUD(int flipCount, int comboVal)
    {
        tablesflippedTextInGame.text = flipCount.ToString();
        comboTextInGame.text = comboVal.ToString();
    }
    public void UpdateEndGameResults(int flipCount, int highScore, int highComboRound, int highComboOverall)
    {
        tablesflippedTextFinal.text = flipCount.ToString();
        highScoreText.text = highScore.ToString();
        comboTextFinal.text = highComboRound.ToString();
        highComboText.text = highComboOverall.ToString();
    }

    public void Reset()
    {
        tablesflippedTextInGame.text =
        tablesflippedTextFinal.text =
        comboTextInGame.text =
        comboTextFinal.text = "0";
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
