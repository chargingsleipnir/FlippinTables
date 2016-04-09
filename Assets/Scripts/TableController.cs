using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TableController : MonoBehaviour {

    public GameObject table01;
    float table01HalfWidth;
    Vector3 startPos;
    Vector3 launchPos;

    Queue<GameObject> scrollingTableList;
    Queue<GameObject> flippingTableList;

    const float DROP_TIME = 2.0f;
    float counter;

    float playerImpactX;
    float playerFlipX;

    public void SelfStart(float playerWidth)
    {
        playerImpactX = -((playerWidth / 2.0f) * 0.15f);
        playerFlipX = -(playerWidth / 2.0f);
        table01HalfWidth = (table01.GetComponent<SpriteRenderer>().sprite.bounds.size.x * table01.transform.localScale.x) / 2.0f;
        startPos = new Vector3(-((playerWidth / 2.0f) * 0.75f) - table01HalfWidth, -2.5f, -1.0f);

        launchPos = new Vector3(-12.0f, -2.5f, -1.0f);
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

        flippingTableList.Enqueue(Instantiate(table01, startPos, Quaternion.identity) as GameObject);
        counter = 0.0f;
    }
	
    // use ints to return various statuses (nothing, gameover, table flipped)
	public int SelfUpdate (float speed) {

        counter += Time.deltaTime;
        if(counter >= DROP_TIME)
        {
            scrollingTableList.Enqueue(Instantiate(table01, launchPos, Quaternion.identity) as GameObject);
            counter = 0.0f;
        }

        foreach(GameObject table in scrollingTableList)
            table.transform.Translate(speed * Time.deltaTime, 0.0f, 0.0f);
        foreach (GameObject table in flippingTableList)
            table.transform.Translate(0.0f, speed * Time.deltaTime, 0.0f);

        if(flippingTableList.Count > 0)
            if (flippingTableList.Peek().transform.position.y > 5.0f)
                Destroy(flippingTableList.Dequeue());
            

        // Queue.Peek() will throw an error if there's no object to peek at, so this check is required.
        if (scrollingTableList.Count > 0)
        {
            // To reduce conditions, first check for player impact
            if ((scrollingTableList.Peek()).transform.position.x + table01HalfWidth >= playerImpactX)
            {
                Destroy(scrollingTableList.Dequeue());
                return -1;
            }
            // If no impact, check for flippable range, then if screen tapped.
            else if ((scrollingTableList.Peek()).transform.position.x + table01HalfWidth >= playerFlipX)
            {
                if (Input.GetMouseButtonDown(0))
                {
                    // This will only happen once per flipped table because the table being watched is dequeued from that list.
                    flippingTableList.Enqueue(scrollingTableList.Dequeue());
                    return 1;
                }
            }
        }
        return 0;
    }
}
