using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Advertisements;

public class AdController : MonoBehaviour {

    int resetCounter;
    bool adBlocker;

	void Start ()
    {
        DontDestroyOnLoad(gameObject);
        resetCounter = 0;
        adBlocker = false;

        SceneManager.LoadScene(1);
	}

    public void CountReset()
    {
        resetCounter++;

        if(resetCounter == 3 && adBlocker == false)
        {
            resetCounter = 0;
            Advertisement.Show();
        }
    }

    public bool CheckAdShowing()
    {
        return Advertisement.isShowing;
    }

    public void BlockAds()
    {
        adBlocker = true;
    }
}
