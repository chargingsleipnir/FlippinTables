using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class GameManager : MonoBehaviour {

    public enum GameStates { prePlay, idle, play, gameover, replay, exit, none }
    public static GameStates gameState;

    int
        tablesFlipped,
        score,
        highScore,
        comboVal,
        highComboThisRound,
        highComboOverall;

    bool endRun;

    BackgroundController bgCtrl;
    TableController tbCtrl;
    public InGameUIController guiCtrl;
    CameraController camCtrl;

    public GameObject playerObj;
    Player player;
    float playerWidth;
    Player.FlipAccuracy prevFlipAcc;

    float speed;

	AudioSource music;

    AdController adCtrl;
    bool adLaunched;

    void Start () {
        bgCtrl = GetComponent<BackgroundController>();
        tbCtrl = GetComponent<TableController>();
        camCtrl = GetComponent<CameraController>();
		music = GetComponent<AudioSource> ();
		music.loop = true;

        adCtrl = GameObject.Find("Persistor").GetComponent<AdController>();
        adLaunched = false;

        // Make sure to incorporate SCALE!!!
        player = playerObj.GetComponent<Player>();
        prevFlipAcc = Player.FlipAccuracy.none;

        highScore = 0;
        highComboOverall = 0;

        bgCtrl.OnLoad();
        tbCtrl.OnLoad();

        Reset();
	}

    public void Reset ()
    {
        gameState = GameStates.prePlay;
        tablesFlipped = 0;
        score = 0;
        comboVal = 0;
        highComboThisRound = 0;
        speed = 6.5f;
		music.volume = 1.0f;

        bgCtrl.Reset();
        player.Reset();
        tbCtrl.Reset();
        guiCtrl.Reset();
    }

	void Update () {
        switch (gameState)
        {
            case GameStates.prePlay:
                guiCtrl.OpenBlinker();
                gameState = GameStates.idle;
                break;
            case GameStates.idle:
                if (guiCtrl.CheckBlinkerOpen())
                {
                    if (player.OnPlay())
                    {
                        gameState = GameStates.play;
                        music.Play();
                    }
                }
                break;
            case GameStates.play:
                // update backgrounds
                bgCtrl.OnFrame(speed);

                // Returns table flip
                tbCtrl.OnFrame(speed);

                if (tbCtrl.CheckFlip())
                {
                    OnFlip(player.GetFlipAccuracy());
                }

                player.OnPlay();
                gameState = player.AffectGameState();
                if(gameState == GameStates.gameover)
                {
                    StartCoroutine(camCtrl.Shake());         
                }
                break;
            case GameStates.gameover:
                tbCtrl.CleanupFlippingTables();
                // This will allow the player to see the exit menu after just a short time, or if the tap right away,
                // without necessarily having to wait for the player to fall to a certain point.
                if (player.EarlyExit())
                {
                    guiCtrl.UpdateEndGameResults(score, highScore, highComboThisRound, highComboOverall); // 3 NOT REALLY A THING, JUST A PLACEHOLDER
                    guiCtrl.ActivateGameOverMenu();
                    music.volume = 0.25f;
                }

                // This will shut off the players motion and exit this state if a certain fall distance is reached.
                player.OnGameOver();
                gameState = player.AffectGameState();
                break;
            case GameStates.replay:
                music.Stop();
                if (!guiCtrl.CheckBlinkerOpen())
                {
                    if (adLaunched == false)
                    {
                        adCtrl.CountReset();
                        adLaunched = true;
                    }
                    if (!adCtrl.CheckAdShowing())
                    {
                        adLaunched = false;
                        Reset();
                        guiCtrl.OpenBlinker();
                    }
                }
                break;
            case GameStates.exit:
                music.Stop();
                if (!guiCtrl.CheckBlinkerOpen())
                {
                    Reset();
                    guiCtrl.EndScene();
                    SceneManager.LoadScene(1);
                }
                break;
        }
	}

    void OnFlip(Player.FlipAccuracy flipAcc)
    {
        tablesFlipped++;

        UpdateComboVal(flipAcc);
        score += comboVal;

        guiCtrl.UpdateHUD(score, comboVal);

        // Track high scores
        if (score > highScore)
        {
            highScore = score;
        }
        if(comboVal > highComboThisRound)
        {
            highComboThisRound = comboVal;
            if (highComboThisRound > highComboOverall)
            {
                highComboOverall = highComboThisRound;
            }
        }

        // If flips hits a certain number, increase speeds, flip force, etc.
        // Use tables flipped, not score, as that can increase like crazy.
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

    void UpdateComboVal(Player.FlipAccuracy flipAcc)
    {
        if (flipAcc == Player.FlipAccuracy.perf)
            comboVal++;
        else if(flipAcc == Player.FlipAccuracy.meh)
            comboVal = 1;

        //prevFlipAcc = flipAcc;
    }

    public void PostGame(int replayOption)
    {
        guiCtrl.CloseBlinker();

        switch(replayOption)
        {
            case 0:
                // quit (assigned in editor, btn OnClick())
                gameState = GameStates.exit;
                break;
            case 1:
                // replay (assigned in editor, btn OnClick())
                gameState = GameStates.replay;
                break;
        }
    }
}
