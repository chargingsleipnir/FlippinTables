using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour {

    Animator anim;
    int
        beginRunHash = Animator.StringToHash("beginRun"),
        endRunHash = Animator.StringToHash("endRun"),
        flipTriggerHash = Animator.StringToHash("flipTrigger");
    public bool willFlip;

    void Start () {
        anim = GetComponent<Animator>();

        Reset();
	}

    public void Reset()
    {
        willFlip = false;
    }

    public bool OnFrame()
    {
        // Run flipping animation on screen tap
        if (Input.GetMouseButtonDown(0))
            FlipJump();

        // Flipping animation controls willFlip boolean, send back to test for table proximity.
        Debug.Log(willFlip);
        return willFlip;
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

    public void ActivateFlip()
    {
        willFlip = true;
    }

    public void DeactivateFlip()
    {
        willFlip = false;
    }
}
