using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;

public class TitleScreenNavigation : MonoBehaviour {

    int
        openTrigHash,
        closeTrigHash,
        sceneChangeTrigHash;

    Animator anim;
    AudioSource audio;

    Button[] uiButtons;

	void Start () {
        openTrigHash = Animator.StringToHash("open");
        closeTrigHash = Animator.StringToHash("close");
        sceneChangeTrigHash = Animator.StringToHash("sceneChange");

        anim = GetComponent<Animator>();
        audio = GetComponent<AudioSource>();

        uiButtons = GetComponentsInChildren<Button>();

        Reset();
    }

    void Reset()
    {
        foreach (Button button in uiButtons)
            button.interactable = false;

        anim.enabled = true;
    }

    public void OnPlayBtn()
    {
        Reset();
        StartCoroutine(NextScene());
    }

    IEnumerator NextScene()
    {
        audio.Play();
        yield return new WaitForSeconds(audio.clip.length);
        anim.SetTrigger(closeTrigHash);
    }

    public void OnOptionsBtn()
    {
        Debug.Log("Launch options screen");
    }

    public void OnExtrasBtn()
    {
        Debug.Log("Launch extras screen");
    }

    void BlinkerOpen()
    {
        foreach (Button button in uiButtons)
            button.interactable = true;
    }

    void BlinkerClosed()
    {
        anim.SetTrigger(sceneChangeTrigHash);
        anim.enabled = false;
        SceneManager.LoadScene(1);
    }
}
