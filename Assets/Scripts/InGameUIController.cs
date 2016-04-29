using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class InGameUIController : MonoBehaviour {

    public Text tablesflippedTextInGame;
    public Text tablesflippedTextFinal;
    public Text highScoreText;
    public Text comboText;

    public void UpdateTableFlipCount(int flipCount)
    {
        tablesflippedTextInGame.text = flipCount.ToString();
    }
    public void UpdateEndGameResults(int flipCount, int highScore, int comboReached)
    {
        tablesflippedTextInGame.text = flipCount.ToString();
        tablesflippedTextFinal.text = flipCount.ToString();
        highScoreText.text = highScore.ToString();
        comboText.text = comboReached.ToString();
    }
    public void Reset()
    {
        tablesflippedTextInGame.text =
        tablesflippedTextFinal.text =
        comboText.text = "0";
    }
}
