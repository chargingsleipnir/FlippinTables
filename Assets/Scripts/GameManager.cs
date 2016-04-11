using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour {

    bool startRun;
    int tablesFlipped;
    int highScore = 0;
    bool endRun;

    BackgroundController bgCtrl;
    TableController tbCtrl;
    GUIController guiCtrl;

    public GameObject player;
    float playerWidth;

    float speed;
    Vector2 flipForce;
    float torque;

    void Start () {
        bgCtrl = GetComponent<BackgroundController>();
        tbCtrl = GetComponent<TableController>();
        guiCtrl = GetComponent<GUIController>();

        // Make sure to incorporate SCALE!!!
        playerWidth = player.GetComponent<SpriteRenderer>().sprite.bounds.size.x * player.transform.localScale.x;
        bgCtrl.OnLoad();
        tbCtrl.OnLoad(playerWidth);

        flipForce = new Vector2();

        Reset();
	}

    void Reset ()
    {
        startRun = false;
        tablesFlipped = 0;
        speed = 5.0f;
        flipForce.Set(-400.0f, 800.0f);
        torque = 100.0f;

        bgCtrl.Reset();
        tbCtrl.Reset();
        guiCtrl.Reset();
    }

	void Update () {
        if (startRun)
        {
            // update backgrounds
            bgCtrl.OnFrame(speed);

            // update tables, check for flip
            if (tbCtrl.OnFrame(speed))
            {
                if (Input.GetMouseButtonDown(0))
                    if (tbCtrl.CheckFlip(flipForce, torque))
                        OnFlip();
            }
            else
                //gameover
                Reset();
        }
        else
        {
            if(Input.GetMouseButtonDown(0))
            {
                startRun = true;
                tbCtrl.CheckFlip(flipForce, torque);
                OnFlip();
            }
        }

	}

    void OnFlip()
    {
        tablesFlipped++;
        guiCtrl.UpdateTableFlipCount(tablesFlipped);
        if (tablesFlipped > highScore)
        {
            highScore = tablesFlipped;
            guiCtrl.UpdateHighScore(highScore);
        }

        // If flips hits a certain number, increase speeds,flip force, etc.
    }
}
