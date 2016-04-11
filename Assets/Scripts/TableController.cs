using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TableController : MonoBehaviour {

    public GameObject table01;
    float table01HalfWidth; // 75% of this figure is currently being used as the "edge". This needs to be better controlled
    float tableWidthScalar = 0.75f;
    Vector3 startPos;
    Vector3 launchPos;

    Queue<GameObject> scrollingTableList;
    Queue<GameObject> flippingTableList;

    const float DROP_TIME = 2.0f;
    float counter;

    float playerImpactX;
    float playerFlipX;

    

    public void OnLoad(float playerWidth)
    {
        playerImpactX = -((playerWidth / 2.0f) * 0.15f);
        playerFlipX = -(playerWidth / 2.0f);
        table01HalfWidth = (table01.transform.GetChild(0).GetComponent<SpriteRenderer>().sprite.bounds.size.x * table01.transform.localScale.x) / 2.0f;
        startPos = new Vector3(-((playerWidth / 2.0f) * 0.5f) - (table01HalfWidth * tableWidthScalar), -2.5f, 0.0f);

        launchPos = new Vector3(-12.0f, -2.5f, 0.0f);
        scrollingTableList = new Queue<GameObject>();
        flippingTableList = new Queue<GameObject>();

        Reset();
    }

    public void Reset()
    {
        foreach (GameObject table in scrollingTableList)
            Destroy(table);
        foreach (GameObject table in flippingTableList)
            Destroy(table);

        scrollingTableList.Clear();
        flippingTableList.Clear();

        scrollingTableList.Enqueue(Instantiate(table01, startPos, Quaternion.identity) as GameObject);
        counter = 0.0f;
    }
	
    // use ints to return various statuses (nothing, gameover, table flipped)
	public bool OnFrame (float speed) {

        counter += Time.deltaTime;
        if(counter >= DROP_TIME)
        {
            scrollingTableList.Enqueue(Instantiate(table01, launchPos, Quaternion.identity) as GameObject);
            counter = 0.0f;
        }

        foreach(GameObject table in scrollingTableList)
            table.transform.Translate(speed * Time.deltaTime, 0.0f, 0.0f);

        if(flippingTableList.Count > 0)
            if (flippingTableList.Peek().transform.position.y < -5.0f)
                Destroy(flippingTableList.Dequeue());

        // Queue.Peek() will throw an error if there's no object to peek at, so this check is required.
        if (scrollingTableList.Count > 0)
            // To reduce conditions, first check for player impact
            if ((scrollingTableList.Peek()).transform.position.x + (table01HalfWidth * tableWidthScalar) >= playerImpactX)
            {
                Destroy(scrollingTableList.Dequeue());
                return false;
            }

        return true;
    }

    public bool CheckFlip(Vector2 flipForce, float torque)
    {
        if (scrollingTableList.Count > 0)
        {
            if ((scrollingTableList.Peek()).transform.position.x + (table01HalfWidth * tableWidthScalar) >= playerFlipX)
            {
                // This will only happen once per flipped table because the table being watched is dequeued from that list.
                GameObject flippedTable = scrollingTableList.Dequeue();
                flippedTable.GetComponent<TableBehaviour>().Flip(flipForce, torque);
                flippingTableList.Enqueue(flippedTable);
                return true;
            }
        }
        return false;
    }
}
