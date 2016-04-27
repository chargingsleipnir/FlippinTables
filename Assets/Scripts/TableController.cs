using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TableController : MonoBehaviour {

    public GameObject table01;
    float table01HalfWidth; // 75% of this figure is currently being used as the "edge". This needs to be better controlled
    float tableWidthScalar = 1.5f;
    Vector3 startPos;
    Vector3 launchPos;

    Queue<GameObject> scrollingTableList;
    TableBehaviour frontTable;
    bool nextTableInFront;
    Queue<GameObject> flippingTableList;

    float counter;
    float
        dropTime01,
        dropTime02,
        dropTime03;

    bool pattern2Toggle;
    float pattern3RandomTime;

    int dropPattern;

    public void OnLoad()
    {
        table01HalfWidth = (table01.transform.GetChild(0).GetComponent<SpriteRenderer>().sprite.bounds.size.x * table01.transform.localScale.x) / 2.0f;
        startPos = new Vector3(-(table01HalfWidth * tableWidthScalar), -2.5f, 0.0f);

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

        GameObject FrontTableObj = Instantiate(table01, startPos, Quaternion.identity) as GameObject;
        frontTable = FrontTableObj.GetComponent<TableBehaviour>();
        nextTableInFront = false;
        scrollingTableList.Enqueue(FrontTableObj);

        counter = 0.0f;

        dropTime01 = 2.0f; // regular drop
        dropTime02 = 3.0f; // max of regular two-table drop
        dropTime03 = 2.5f; // max of random ranged drop
        
        pattern2Toggle = true;
        pattern3RandomTime = Random.Range(Constants.DROP_TIME_MIN, dropTime03);

        dropPattern = 0;
    }

    public void SpeedIncrease()
    {
        dropTime01 = (dropTime01 > Constants.DROP_TIME_MIN) ? dropTime01 - 0.15f: Constants.DROP_TIME_MIN;
        dropTime02 = (dropTime02 > Constants.DROP_TIME_MIN * 2) ? dropTime02 - 0.20f : Constants.DROP_TIME_MIN * 2;
        dropTime03 = (dropTime03 > Constants.DROP_TIME_MIN) ? dropTime03 - 0.25f : Constants.DROP_TIME_MIN;
    }
    public void PatternChange()
    {
        dropPattern = (dropPattern + 1) % 3;
    }

    public void CleanupFlippingTables()
    {
        // Queue.Peek() will throw an error if there's no object to peek at, so this check is required.
        // Just a check to eliminate those off the screen.
        if (flippingTableList.Count > 0)
            if (flippingTableList.Peek().transform.position.y < Constants.DROP_OFF_LIMIT)
                Destroy(flippingTableList.Dequeue());
    }

    void GetNextTable()
    {
        // If this one created is in the front of the line, get it's script
        if (nextTableInFront)
        {
            frontTable = scrollingTableList.Peek().GetComponent<TableBehaviour>();
            nextTableInFront = false;
        }
    }

    // use bool to return flip
    public bool OnFrame (float speed) {

        // Tables drop at regular increments
        if(dropPattern == 0)
        {
            counter += Time.deltaTime;
            if(counter >= dropTime01)
            {
                scrollingTableList.Enqueue(Instantiate(table01, launchPos, Quaternion.identity) as GameObject);
                counter = 0.0f;

                GetNextTable();
            }
        }
        // Tables drop in close pairs, but with a little further spread out
        else if (dropPattern == 1)
        {
            counter += Time.deltaTime;
            if(counter >= dropTime02)
            {
                scrollingTableList.Enqueue(Instantiate(table01, launchPos, Quaternion.identity) as GameObject);
                counter = 0.0f;
                pattern2Toggle = true;

                GetNextTable();
            }
            else if(counter >= dropTime02 - Constants.DROP_TIME_MIN && pattern2Toggle)
            {
                scrollingTableList.Enqueue(Instantiate(table01, launchPos, Quaternion.identity) as GameObject);
                pattern2Toggle = false;

                GetNextTable();
            }
        }
        // Tables drop with controlled randomness
        else
        {
            counter += Time.deltaTime;
            if (counter >= pattern3RandomTime)
            {
                scrollingTableList.Enqueue(Instantiate(table01, launchPos, Quaternion.identity) as GameObject);
                counter = 0.0f;
                pattern3RandomTime = Random.Range(Constants.DROP_TIME_MIN, dropTime03);

                GetNextTable();
            }
        }

        // translate tables
        foreach (GameObject table in scrollingTableList)
            table.transform.Translate(speed * Time.deltaTime, 0.0f, 0.0f);

        CleanupFlippingTables();

        // If there's at least 2, the next in line will be the front table
        if (scrollingTableList.Count > 1)
        {
            if (frontTable.CheckFlipped())
            {
                flippingTableList.Enqueue(scrollingTableList.Dequeue());
                frontTable = scrollingTableList.Peek().GetComponent<TableBehaviour>();
                return true;
            }
        }
        // If only 1, it'll be removed on impact, and the next created will be in front
        // Needs to evaluate for 1 so 0 doesn't come in here.
        else if(scrollingTableList.Count == 1)
        {
            if (frontTable.CheckFlipped())
            {
                flippingTableList.Enqueue(scrollingTableList.Dequeue());
                nextTableInFront = true;
                return true;
            }
        }

        return false;
    }
}
