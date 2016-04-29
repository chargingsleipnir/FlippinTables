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
    InGameUIController guiCtrl;
    public InGameUIActions guiActions;

    public GameObject playerObj;
    Player player;
    float playerWidth;

    float speed;

    void Start () {
        bgCtrl = GetComponent<BackgroundController>();
        tbCtrl = GetComponent<TableController>();
        guiCtrl = GetComponent<InGameUIController>();

        // Make sure to incorporate SCALE!!!
        player = playerObj.GetComponent<Player>();

        bgCtrl.OnLoad();
        tbCtrl.OnLoad();

        Reset();
	}

    public void Reset ()
    {
        gameState = GameStates.prePlay;
        tablesFlipped = 0;
        speed = 6.5f;

        bgCtrl.Reset();
        player.Reset();
        tbCtrl.Reset();
        guiCtrl.Reset();
        guiActions.Reset();
    }

	void Update () {
        switch (gameState)
        {
            case GameStates.prePlay:
                if (guiActions.CheckBlinkerIsOpen())
                {
                    if (player.OnPlay())
                    {
                        gameState = GameStates.play;
                    }
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
                // This will allow the player to see the exit menu after just a short time, or if the tap right away,
                // without necessarily having to wait for the player to fall to a certain point.
                if(player.EarlyExit())
                    guiActions.ActivateGameOverMenu();

                // This will shut off the players motion and exit this state if a certain fall distance is reached.
                player.OnGameOver();
                gameState = player.AffectGameState();
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
            guiCtrl.UpdateEndGameResults(tablesFlipped, highScore, 3); // 3 NOT REALLY A THING, JUST A PLACEHOLDER
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
