using UnityEngine;
using UnityEngine.SceneManagement;
#if UNITY_ADS
using UnityEngine.Advertisements;
#endif

public class AdController : MonoBehaviour {

    int resetCounter;
    bool adBlocker;

	void Start ()
    {
        DontDestroyOnLoad(gameObject);
        resetCounter = 0;
        adBlocker = false;

        Screen.fullScreen = false;

        SceneManager.LoadScene(1);
	}

    public void CountReset()
    {
        resetCounter++;

        if(resetCounter == Constants.AD_RESET_COUNTER && adBlocker == false)
        {
            resetCounter = 0;
#if UNITY_ADS
            Advertisement.Show();
#endif
        }
    }

    public bool CheckAdShowing()
    {
#if UNITY_ADS
        return Advertisement.isShowing;
#endif
        return false;
    }

    public void BlockAds()
    {
        adBlocker = true;
    }
}
