using UnityEngine;
using System.Collections;

public class TableBehaviour : MonoBehaviour {

    Rigidbody2D rb2D;

    void Start()
    {
        rb2D = GetComponent<Rigidbody2D>();
    }

    public void Flip(Vector2 force, float torque)
    {
        rb2D.gravityScale = 1.0f;
        rb2D.AddForce(force);
        rb2D.AddTorque(torque);
    }
}
