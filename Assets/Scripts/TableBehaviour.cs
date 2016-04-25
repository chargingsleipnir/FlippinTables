using UnityEngine;
using System.Collections;

public class TableBehaviour : MonoBehaviour {

    Rigidbody2D rb2D;
    bool flipped;

    void Start()
    {
        rb2D = GetComponent<Rigidbody2D>();
        flipped = false;
    }

    public void Flip(Vector2 force, float torque)
    {
        rb2D.gravityScale = 1.0f;
        rb2D.AddForce(force);
        rb2D.AddTorque(torque);
        flipped = true;
    }

    public bool CheckFlipped()
    {
        return flipped;
    }
}
