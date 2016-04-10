using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour {

    bool startRun;
    int tableFeedback;
    int tablesFlipped;
    int highScore = 0;
    bool endRun;

    BackgroundController bgCtrl;
    TableController tbCtrl;
    GUIController guiCtrl;

    public GameObject player;
    float playerWidth;

    float speed;

	void Start () {
        bgCtrl = GetComponent<BackgroundController>();
        tbCtrl = GetComponent<TableController>();
        guiCtrl = GetComponent<GUIController>();

        // Make sure to incorporate SCALE!!!
        playerWidth = player.GetComponent<SpriteRenderer>().sprite.bounds.size.x * player.transform.localScale.x;
        bgCtrl.SelfStart();
        tbCtrl.SelfStart(playerWidth);

        Reset();
	}

    void Reset ()
    {
        startRun = false;
        tableFeedback = 0;
        tablesFlipped = 0;
        speed = 5.0f;

        bgCtrl.Reset();
        tbCtrl.Reset();
        guiCtrl.Reset();
    }

    void OnFlip()
    {
        tablesFlipped++;
        guiCtrl.UpdateTableFlipCount(tablesFlipped);
        if(tablesFlipped > highScore)
        {
            highScore = tablesFlipped;
            guiCtrl.UpdateHighScore(highScore);
        }
    }

	void Update () {
	    if(startRun)
        {
            bgCtrl.SelfUpdate(speed);
            tableFeedback = tbCtrl.SelfUpdate(speed);

            switch (tableFeedback)
            {
                case 0:
                    // normal return, do nothing. Code is here just for the break, to prevent future checks
                    break;
                case -1:
                    //gameover
                    Reset();
                    // play animation
                    break;
                case 1:
                    //add point
                    OnFlip();
                    break;
            }
        }
        else
        {
            if(Input.GetMouseButtonDown(0))
            {
                startRun = true;
                OnFlip();
            }
        }

	}
}
