using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class GUIController : MonoBehaviour {

    public Text tablesflippedText;
    public Text highScoreText;

    public void UpdateTableFlipCount(int flipCount)
    {
        tablesflippedText.text = flipCount.ToString();
    }
    public void UpdateHighScore(int score)
    {
        highScoreText.text = score.ToString();
    }
    public void Reset()
    {
        tablesflippedText.text = "0";
    }
}
