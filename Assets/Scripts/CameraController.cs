using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour {

    public float
        startDist = 0.5f,
        decPerc = 0.75f,
        speed = 75.0f;

    public int shakeCount = 15;

    Vector3 
        origPos,
        shakePos;

	void Start () {
        origPos = transform.position;
        shakePos = new Vector3();
	}

    public IEnumerator Shake()
    {
        float hitTime = Time.time;
        int shakes = shakeCount;
        float dist = startDist;

        while (shakes > 0)
        {
            float timer = (Time.time - hitTime) * speed;
            shakePos.Set(origPos.x + Mathf.Sin(timer) * dist, origPos.y, origPos.z);
            transform.position = shakePos;

            if (timer > Mathf.PI * 2.0f)
            {
                hitTime = Time.time;
                dist *= decPerc;
                shakes--;
            }
            yield return 0;
        }
        transform.position = origPos;
    }
}
