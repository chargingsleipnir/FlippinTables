using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour {

    public enum AnimStates { idle, run, flip, impact }
    GameManager.GameStates gameState;

    public enum FlipAccuracy { perf, good, meh, none }
    FlipAccuracy flipAcc;

    Animator anim;
    BoxCollider2D boxColl;
    CircleCollider2D circColl;
    Rigidbody2D rb2D;

    int stateHash = Animator.StringToHash("state");
	int airbourneHash = Animator.StringToHash("isAirbourne");

    Vector2 flipForce;
    float torque;

    Vector2 impactForce;

    Vector3 startPos;

    float exitTimer = 0.0f;
    bool exitMenuUp;

	AudioSource audio;
	public AudioClip[] soundsFlip;
	public AudioClip soundImpact;
    public AudioClip soundFootsteps;
    float soundFootstepsPitch;

    float flipAccDist; // Tested flip contact differences ranged from about 1.0f - 2.0f.

    // This needed to be "Awake" to avoid some null object references -> the components in the "Reset" function
    void Awake () {
        anim = GetComponent<Animator>();
        boxColl = GetComponent<BoxCollider2D>();
        circColl = GetComponent<CircleCollider2D>();
        rb2D = GetComponent<Rigidbody2D>();
		audio = GetComponent<AudioSource> ();

        flipForce = new Vector2();

        impactForce = new Vector2();

        startPos = new Vector3(0.0f, -1.25f, -1.0f);

        Reset();
	}

    public void Reset()
    {
        gameState = GameManager.GameStates.play;
        flipAcc = FlipAccuracy.perf;

        anim.SetInteger(stateHash, (int)AnimStates.idle);

        rb2D.isKinematic = true;
        rb2D.gravityScale = 0.0f;
        rb2D.velocity.Set(0.0f, 0.0f);
        transform.position = startPos;

        flipForce.Set(-400.0f, 500.0f);
        torque = 125.0f;
        impactForce.Set(100.0f, 100.0f);
        anim.speed = 1.0f;
        soundFootstepsPitch = 1.0f;
        audio.pitch = 1.0f;

        exitTimer = 0.0f;
        exitMenuUp = false;
    }

    public void SpeedIncrease()
    {
        flipForce.x -= 30.0f;
        flipForce.y += 60.0f;
        torque += 15.0f;
        impactForce.x += 10.0f;
        impactForce.y += 10.0f;
        anim.speed += 0.1f;
        soundFootstepsPitch += 0.1f;
    }

    // Use this to return reset indicator
    public bool OnPlay ()
    {
        if (Input.GetMouseButtonDown(0))
        {
            anim.SetInteger(stateHash, (int)AnimStates.flip);

			if (!anim.GetBool (airbourneHash)) {
				audio.clip = soundsFlip[Random.Range(0, soundsFlip.Length)];
				audio.loop = false;
                audio.pitch = 1.0f;
				audio.Play ();
			}
            return true;
        }
        return false;
    }
    public void OnGameOver()
    {
        if (transform.position.y < Constants.DROP_OFF_LIMIT)
        {
            gameState = GameManager.GameStates.none;
            rb2D.isKinematic = true;
            rb2D.gravityScale = 0.0f;
            rb2D.velocity.Set(0.0f, 0.0f);
        }
    }
    public bool EarlyExit()
    {
        if (exitMenuUp == false)
        {
            exitTimer += Time.deltaTime;

            if (exitTimer >= Constants.EARLY_EXIT_TIME || Input.GetMouseButtonDown(0))
            {
                exitTimer = 0.0f;
                return exitMenuUp = true;
            }
        }
        return false;
    }

    public void HitTable()
    {
        Handheld.Vibrate();
        gameState = GameManager.GameStates.gameover;
        anim.SetInteger(stateHash, (int)AnimStates.impact);
        rb2D.isKinematic = false;
        rb2D.gravityScale = 1.0f;
        rb2D.AddForce(impactForce);
		audio.clip = soundImpact;
		audio.loop = false;
		audio.Play ();
    }

	void FlipLanded () {
        audio.Stop();
		audio.clip = soundFootsteps;
		audio.loop = true;
        audio.pitch = soundFootstepsPitch;
		audio.Play ();
	}

    public GameManager.GameStates AffectGameState()
    {
        return gameState;
    }

    public FlipAccuracy GetFlipAccuracy()
    {
        return flipAcc;
    }

    void SetFlipAccuracy(float dist)
    {
        if (dist < 1.0 || dist > 2.0)
            flipAcc = FlipAccuracy.none;
        else if (dist < 1.2 || dist > 1.8)
            flipAcc = FlipAccuracy.meh;
        else if (dist < 1.4 || dist > 1.6)
            flipAcc = FlipAccuracy.good;
        else
            flipAcc = FlipAccuracy.perf;

        // Launch in-game flip accuracy message
    }

    void OnTriggerEnter2D(Collider2D otherColl)
    {
        if(otherColl.tag == "Table")
        {
            if(circColl.IsTouching(otherColl))
            {
                otherColl.gameObject.GetComponent<TableBehaviour>().Flip(flipForce, torque);
                SetFlipAccuracy(transform.position.x - otherColl.transform.position.x);
            }
            else if(boxColl.IsTouching(otherColl))
            {
                HitTable();
            }
        }
    }
}
