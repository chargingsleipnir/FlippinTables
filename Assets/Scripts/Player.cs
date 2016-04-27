using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour {

    public enum AnimStates { idle, run, flip, impact }
    GameManager.GameStates gameState;

    Animator anim;
    BoxCollider2D boxColl;
    CircleCollider2D circColl;
    Rigidbody2D rb2D;

    int stateHash = Animator.StringToHash("state");

    Vector2 flipForce;
    float torque;

    Vector2 impactForce;

    Vector3 startPos;

    // This needed to be "Awake" to avoid some null object references -> the components in the "Reset" function
    void Awake () {
        anim = GetComponent<Animator>();
        boxColl = GetComponent<BoxCollider2D>();
        circColl = GetComponent<CircleCollider2D>();
        rb2D = GetComponent<Rigidbody2D>();

        flipForce = new Vector2();

        impactForce = new Vector2();

        startPos = new Vector3(0.0f, -1.25f, -1.0f);

        Reset();
	}

    public void Reset()
    {
        gameState = GameManager.GameStates.play;
        anim.SetInteger(stateHash, (int)AnimStates.idle);

        rb2D.isKinematic = true;
        rb2D.gravityScale = 0.0f;
        rb2D.velocity.Set(0.0f, 0.0f);
        transform.position = startPos;

        flipForce.Set(-300.0f, 600.0f);
        torque = 100.0f;
        impactForce.Set(100.0f, 100.0f);
        anim.speed = 1.0f;
    }

    public void SpeedIncrease()
    {
        flipForce.x -= 30.0f;
        flipForce.y += 60.0f;
        torque += 10.0f;
        impactForce.x += 10.0f;
        impactForce.y += 10.0f;
        anim.speed += 0.1f;
    }

    // Use this to return reset indicator
    public bool OnPlay ()
    {
        if (Input.GetMouseButtonDown(0))
        {
            anim.SetInteger(stateHash, (int)AnimStates.flip);
            return true;
        }
        return false;
    }
    public void OnGameOver()
    {
        // Give player the option to just tap through it quickly
        if (transform.position.y < Constants.DROP_OFF_LIMIT || Input.GetMouseButtonDown(0))
        {
            gameState = GameManager.GameStates.menu;
            rb2D.isKinematic = true;
            rb2D.gravityScale = 0.0f;
            rb2D.velocity.Set(0.0f, 0.0f);
        }
    }

    public void HitTable()
    {
        gameState = GameManager.GameStates.gameover;
        anim.SetInteger(stateHash, (int)AnimStates.impact);
        rb2D.isKinematic = false;
        rb2D.gravityScale = 1.0f;
        rb2D.AddForce(impactForce);
    }

    public GameManager.GameStates AffectGameState()
    {
        return gameState;
    }

    void OnTriggerEnter2D(Collider2D otherColl)
    {
        if(otherColl.tag == "Table")
        {
            if(circColl.IsTouching(otherColl))
            {
                otherColl.gameObject.GetComponent<TableBehaviour>().Flip(flipForce, torque);
            }
            else if(boxColl.IsTouching(otherColl))
            {
                HitTable();
            }
        }
    }
}
