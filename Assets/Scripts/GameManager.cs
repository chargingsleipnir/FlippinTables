using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour {

    public enum GameStates { prePlay, play, gameover, menu }
    public static GameStates gameState;

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
        gameState = GameStates.prePlay;
        tablesFlipped = 0;
        speed = 6.5f;

        bgCtrl.Reset();
        player.Reset();
        tbCtrl.Reset();
        guiCtrl.Reset();
    }

	void Update () {
        switch (gameState)
        {
            case GameStates.prePlay:
                if (player.OnPlay())
                {
                    gameState = GameStates.play;
                }
                break;
            case GameStates.play:
                // update backgrounds
                bgCtrl.OnFrame(speed);

                // Returns table flip
                if (tbCtrl.OnFrame(speed))
                    OnFlip();

                player.OnPlay();
                gameState = player.AffectGameState();            
                break;
            case GameStates.gameover:
                tbCtrl.CleanupFlippingTables();
                player.OnGameOver();
                gameState = player.AffectGameState();
                break;
            case GameStates.menu:
                // lose menu in here, tap to continue.
                Reset();
                break;
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

        // If flips hits a certain number, increase speeds, flip force, etc.

        if (tablesFlipped % 10 == 0)
        {
            tbCtrl.PatternChange();
        }
        else if (tablesFlipped % 5 == 0)
        {
            speed += 0.5f;
            tbCtrl.SpeedIncrease();
            player.SpeedIncrease();
        }
    }
}
