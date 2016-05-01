using UnityEngine;
using System.Collections;

public struct BGObjects
{
    public GameObject right;
    public GameObject left;
    public SpriteRenderer rightSpriteRend;
    public SpriteRenderer leftSpriteRend;

    public BGObjects(GameObject left, GameObject right)
    {
        this.right = right;
        this.left = left;

        if (this.right.GetComponent<SpriteRenderer>() == null)
            this.right.AddComponent<SpriteRenderer>();
        if (this.left.GetComponent<SpriteRenderer>() == null)
            this.left.AddComponent<SpriteRenderer>();

        rightSpriteRend = this.right.GetComponent<SpriteRenderer>();
        leftSpriteRend = this.left.GetComponent<SpriteRenderer>();
    }
}
public class BackgroundController : MonoBehaviour {

    public Sprite[] bgSprites;
    public int[] seqSpriteCountsPerLvl;
    public GameObject[] bgLevelObjs;
    public Vector3[] bgLevelPos;
    public float[] bgSpeedFactors;
    
    //public Vector3[] bgLevelDirs;

    Sprite[][] bgSpritesByLevel;
    float[] bgLvlWidths;
    BGObjects[] bgObjects;

    int[][] randSpriteSetIdxs;

    Vector3 bgLeftPos;

    public void OnLoad() {
        //Get needed UNIT width with the bounds property, multiplying by scale factor to be precise.
        //bg01.GetComponent<SpriteRenderer>().sprite.rect.width <-- in pixels

        // This array is going to hold all of the other background for that set, so is initiated minus 1 element
        randSpriteSetIdxs = new int[seqSpriteCountsPerLvl.Length][];
        bgSpritesByLevel = new Sprite[seqSpriteCountsPerLvl.Length][];

        // sprite iterator needs to be out here to make sure sprite array is accessed sequencially
        int spriteIter = 0;
        for (int i = 0; i < seqSpriteCountsPerLvl.Length; i++)
        {
            // Create 2D array sprites and of sprite indexes
            bgSpritesByLevel[i] = new Sprite[seqSpriteCountsPerLvl[i]];
            randSpriteSetIdxs[i] = new int[seqSpriteCountsPerLvl[i]];
            for (int j = 0; j < seqSpriteCountsPerLvl[i]; j++)
            {
                bgSpritesByLevel[i][j] = bgSprites[spriteIter];
                spriteIter++;

                randSpriteSetIdxs[i][j] = j;
            }
        }

        // Setting up the game objects that will rotate, 2 for each level;
        bgObjects = new BGObjects[bgLevelObjs.Length];
        bgLvlWidths = new float[bgLevelObjs.Length];

        for (int i = 0; i < seqSpriteCountsPerLvl.Length; i++)
        {
            bgObjects[i] = new BGObjects(Instantiate(bgLevelObjs[i]) as GameObject, Instantiate(bgLevelObjs[i]) as GameObject);

            // put in random sprite from sets
            bgObjects[i].rightSpriteRend.sprite = bgSpritesByLevel[i][0];

            if(seqSpriteCountsPerLvl[i] > 1)
                bgObjects[i].leftSpriteRend.sprite = GetRandomSprite(i);
            else
                bgObjects[i].leftSpriteRend.sprite = bgSpritesByLevel[i][0];

            // get sprite widths
            bgLvlWidths[i] = bgObjects[i].rightSpriteRend.sprite.bounds.size.x * bgObjects[i].right.transform.localScale.x;
        }

        bgLeftPos = new Vector3();

        Reset();
    }

    Sprite GetRandomSprite(int spriteSetIdx)
    {
        int tempIdx = Random.Range(1, randSpriteSetIdxs[spriteSetIdx].Length);
        int tempVal = randSpriteSetIdxs[spriteSetIdx][0];
        randSpriteSetIdxs[spriteSetIdx][0] = randSpriteSetIdxs[spriteSetIdx][tempIdx];
        randSpriteSetIdxs[spriteSetIdx][tempIdx] = tempVal;

        return bgSpritesByLevel[spriteSetIdx][randSpriteSetIdxs[spriteSetIdx][0]];
    }

    public void Reset()
    {
        // use widths to determine placement
        for (int i = 0; i < bgLevelObjs.Length; i++)
        {
            bgObjects[i].right.transform.position = bgLevelPos[i];
            bgLeftPos.Set(
                bgObjects[i].right.transform.position.x - bgLvlWidths[i],
                bgObjects[i].right.transform.position.y,
                bgObjects[i].right.transform.position.z);
            bgObjects[i].left.transform.position = bgLeftPos;
        }
    }

    public void OnFrame (float speed) {

        for (int i = 0; i < bgLevelObjs.Length; i++)
        {
            bgObjects[i].right.transform.Translate((speed * bgSpeedFactors[i]) * Time.deltaTime, 0.0f, 0.0f);
            bgLeftPos.Set(
                bgObjects[i].right.transform.position.x - bgLvlWidths[i],
                bgObjects[i].right.transform.position.y,
                bgObjects[i].right.transform.position.z);
            bgObjects[i].left.transform.position = bgLeftPos;

            if (bgLeftPos.x >= 0.0f)
            {
                bgLeftPos.Set(
                    bgObjects[i].left.transform.position.x - bgLvlWidths[i],
                    bgObjects[i].left.transform.position.y,
                    bgObjects[i].left.transform.position.z);
                bgObjects[i].right.transform.position = bgLeftPos;

                GameObject tempObj = bgObjects[i].left;
                SpriteRenderer tempRend = bgObjects[i].leftSpriteRend;

                bgObjects[i].left = bgObjects[i].right;
                bgObjects[i].leftSpriteRend = bgObjects[i].rightSpriteRend;

                bgObjects[i].right = tempObj;
                bgObjects[i].rightSpriteRend = tempRend;

                if (seqSpriteCountsPerLvl[i] > 1)
                    bgObjects[i].leftSpriteRend.sprite = GetRandomSprite(i);
                else
                    bgObjects[i].leftSpriteRend.sprite = bgSpritesByLevel[i][0];
            }
        }       
    }
}
