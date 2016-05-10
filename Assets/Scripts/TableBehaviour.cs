using UnityEngine;
using System.Collections;

public class TableBehaviour : MonoBehaviour {

    Rigidbody2D rb2D;
    bool flipped;

    ItemBehaviour[] items;
    Vector2 itemForce;

    void Start()
    {
        rb2D = GetComponent<Rigidbody2D>();
        flipped = false;

        items = GetComponentsInChildren<ItemBehaviour>();
        itemForce = new Vector2();
    }

    public void Flip(Vector2 force, float torque)
    {
        rb2D.gravityScale = 1.0f;
        rb2D.AddForce(force);
        rb2D.AddTorque(torque);
        flipped = true;

        // Use table's forces as starting point for that of it's items
        for (int i = 0; i < items.Length; i++)
        {
            items[i].transform.parent = null;

            itemForce.Set(force.x + Random.Range(0.0f, Constants.MAX_ADD_FORCE_X), Random.Range(force.y - Constants.RANGE_FORCE_Y, force.y + Constants.RANGE_FORCE_Y));
            items[i].Flip(itemForce, (Random.Range(torque - Constants.RANGE_TORQUE, torque + Constants.RANGE_TORQUE)));
        }
    }

    public bool CheckFlipped()
    {
        return flipped;
    }

    public void DestroyAll()
    {
        for (int i = 0; i < items.Length; i++)
        {
            Destroy(items[i].gameObject);
        }

        Destroy(gameObject);
    }
}
