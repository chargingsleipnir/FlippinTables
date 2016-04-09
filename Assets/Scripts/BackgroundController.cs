using UnityEngine;
using System.Collections;

public class BackgroundController : MonoBehaviour {

    public GameObject bg01;
    float bg01Width;

    GameObject bgRight;
    GameObject bgLeft;

    Vector3 bgLeftPos;

	public void SelfStart () {
        // Get needed UNIT width with the bounds property, multiplying by scale factor to be precise.
        //bg01.GetComponent<SpriteRenderer>().sprite.rect.width <-- in pixels
        bg01Width = bg01.GetComponent<SpriteRenderer>().sprite.bounds.size.x * bg01.transform.localScale.x;
        bgRight = Instantiate(bg01) as GameObject;
        bgLeftPos = new Vector3();
        bgLeft = Instantiate(bg01) as GameObject;
        Reset();
    }
	
	public void SelfUpdate (float speed) {
        bgRight.transform.Translate(speed * Time.deltaTime, 0.0f, 0.0f);
        bgLeftPos.Set(bgRight.transform.position.x - bg01Width, 0.0f, 0.0f);
        bgLeft.transform.position = bgLeftPos;

        if(bgLeftPos.x >= 0.0f)
        {
            bgRight.transform.position = new Vector3(bgLeft.transform.position.x - bg01Width, 0.0f, 0.0f);
            GameObject temp = bgLeft;
            bgLeft = bgRight;
            bgRight = temp;
        }
    }

    public void Reset()
    {
        bgRight.transform.position = Vector3.zero;
        bgLeftPos.Set(bgRight.transform.position.x - bg01Width, 0.0f, 0.0f);
        bgLeft.transform.position = bgLeftPos;
    }
}
