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

    public GameObject playerObj;
    Player player;
    float playerWidth;

    float speed;

    void Start () {
        bgCtrl = GetComponent<BackgroundController>();
        tbCtrl = GetComponent<TableController>();
        guiCtrl = GetComponent<GUIController>();

        // Make sure to incorporate SCALE!!!
        player = playerObj.GetComponent<Player>();

        bgCtrl.OnLoad();
        tbCtrl.OnLoad();

        Reset();
	}

    void Reset ()
    {
        startRun = false;
        tablesFlipped = 0;
        speed = 6.5f;

        bgCtrl.Reset();
        player.Reset();
        tbCtrl.Reset();
        guiCtrl.Reset();
    }

	void Update () {
        if (startRun)
        {
            // update backgrounds
            bgCtrl.OnFrame(speed);

            // Returns table flip
            if(tbCtrl.OnFrame(speed))
            {
                OnFlip();
            }

            // Returns player-table impact
            if(player.OnFrame())
            {
                //gameover
                player.HitTable();
                Reset();
            }
        }
        else
        {
            if(Input.GetMouseButtonDown(0))
            {
                startRun = true;
                player.FlipFirstTable();
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
