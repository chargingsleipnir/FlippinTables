using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour {

    Animator anim;
    BoxCollider2D boxColl;
    CircleCollider2D circColl;

    int
        beginRunHash = Animator.StringToHash("beginRun"),
        endRunHash = Animator.StringToHash("endRun"),
        flipTriggerHash = Animator.StringToHash("flipTrigger");

    Vector2 flipForce;
    float torque;

    bool dead;

    void Start () {
        anim = GetComponent<Animator>();
        boxColl = GetComponent<BoxCollider2D>();
        circColl = GetComponent<CircleCollider2D>();

        flipForce = new Vector2();

        Reset();
	}

    public void Reset()
    {
        flipForce.Set(-400.0f, 800.0f);
        torque = 100.0f;
        dead = false;
    }

    // Use this to return player's status
    public bool OnFrame()
    {
        // Run flipping animation on screen tap
        if (Input.GetMouseButtonDown(0))
            FlipJump();

        return dead;
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
                dead = true;
            }
        }
    }

    public void FlipFirstTable()
    {
        anim.SetTrigger(beginRunHash);
        anim.SetTrigger(flipTriggerHash);
    }

    public void FlipJump()
    {
        anim.SetTrigger(flipTriggerHash);
    }

    public void HitTable()
    {
        anim.SetTrigger(endRunHash);
    }
}
