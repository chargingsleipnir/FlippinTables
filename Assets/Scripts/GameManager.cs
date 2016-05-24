using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class GameManager : MonoBehaviour {

    public enum GameStates { prePlay, idle, play, gameover, replay, exit, none }
    public static GameStates gameState;

    int
        flips,
        highFlips,
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
    

    float speed;

	AudioSource music;

    AdController adCtrl;
    bool adLaunched;

    Player.FlipAccuracy prevFlipAcc;
    int flipAccAchieveCounter;

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

        highFlips = 0;
        highScore = 0;
        highComboOverall = 0;

        bgCtrl.OnLoad();
        tbCtrl.OnLoad();

        Reset();
	}

    public void Reset ()
    {
        gameState = GameStates.prePlay;

        flips = 0;
        score = 0;
        comboVal = 0;
        highComboThisRound = 0;

        speed = 6.5f;
		music.volume = 1.0f;

        flipAccAchieveCounter = 0;

        bgCtrl.Reset();
        player.Reset();
        tbCtrl.Reset();
        guiCtrl.Reset();
    }

	void Update () {

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            PostGame(0);
        }

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
                    bool newHighReached = false;
                    // Track high scores for overall game
                    if (flips > highFlips)
                    {
                        highFlips = flips;
                        newHighReached = true;
                    }
                    if (score > highScore)
                    {
                        highScore = score;
                        newHighReached = true;
                    }
                    if (highComboThisRound > highComboOverall)
                    {
                        highComboOverall = highComboThisRound;
                        newHighReached = true;
                    }

                    guiCtrl.UpdateEndGameResults(flips, highFlips, score, highScore, highComboThisRound, highComboOverall, newHighReached); // 3 NOT REALLY A THING, JUST A PLACEHOLDER
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
        flips++;

        UpdateComboVal(flipAcc);
        score += comboVal;

        guiCtrl.UpdateHUD(score, comboVal);

        // Track high-combo-by-round here as it can decrease during the game
        if (comboVal > highComboThisRound)
        {
            highComboThisRound = comboVal;
        }

        // If flips hits a certain number, increase speeds, flip force, etc.
        // Use tables flipped, not score, as that can increase like crazy.
        if (flips % 10 == 0)
        {
            tbCtrl.PatternChange();
        }
        else if (flips % 5 == 0)
        {
            speed += 0.5f;
            tbCtrl.SpeedIncrease();
            player.SpeedIncrease();
        }

        // ACHIEVEMENT CHECKS

        if(flipAcc == prevFlipAcc)
        {
            flipAccAchieveCounter++;
            if(flipAccAchieveCounter == Constants.ACH_FLIP_ACC_COUNT - 1)
            {
                if (flipAcc == Player.FlipAccuracy.perf)
                    guiCtrl.UpdateAchievement(Constants.ACH_FOCUSED_FLIPPER, "Achievement: Focused Flipper");
                else if (flipAcc == Player.FlipAccuracy.good)
                    guiCtrl.UpdateAchievement(Constants.ACH_GOOD_GRIP, "Achievement: Good Grip");
                else if (flipAcc == Player.FlipAccuracy.meh)
                    guiCtrl.UpdateAchievement(Constants.ACH_MESSY_MAYHEM, "Achievement: Messy Mayhem");
            }
        }
        else
        {
            flipAccAchieveCounter = 1;
        }
        prevFlipAcc = flipAcc;

        if (score > Constants.ACH_TABLE_TIRADE_SCORE - 1 || flips > Constants.ACH_TABLE_TIRADE_FLIPS - 1)
        {
            guiCtrl.UpdateAchievement(Constants.ACH_TABLE_TIRADE, "Achievement: Table Tirade");
        }
    }

    void UpdateComboVal(Player.FlipAccuracy flipAcc)
    {
        if (flipAcc == Player.FlipAccuracy.perf)
            comboVal++;
        else if(flipAcc == Player.FlipAccuracy.meh)
            comboVal = 1;

        if (comboVal < 1)
            comboVal = 1;
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
