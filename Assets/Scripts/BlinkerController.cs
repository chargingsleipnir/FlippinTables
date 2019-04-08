using UnityEngine;
using System.Collections;

public class BlinkerController : MonoBehaviour {

    int
    openTrigHash,
    closeTrigHash,
    sceneChangeTrigHash;

    Animator anim;
    AudioSource audioS;

    bool blinkerOpen;

    public AudioClip
        soundOpenning,
        soundClosing;

    void Start () {
        openTrigHash = Animator.StringToHash("open");
        closeTrigHash = Animator.StringToHash("close");
        sceneChangeTrigHash = Animator.StringToHash("sceneChange");

        blinkerOpen = false;

        anim = GetComponent<Animator>();
        audioS = GetComponent<AudioSource>();
    }

    public void Open()
    {
        anim.SetTrigger(openTrigHash);
    }

    void StartOpen()
    {
        audioS.clip = soundOpenning;
        audioS.Play();
    }

    void IsOpen()
    {
        blinkerOpen = true;
        audioS.Stop();
    }

    public void Close()
    {
        anim.SetTrigger(closeTrigHash);
    }

    void StartClose()
    {
        audioS.clip = soundClosing;
        audioS.Play();
    }

    void IsClosed()
    {
        blinkerOpen = false;
        audioS.Stop();
    }

    public bool CheckOpen()
    {
        return blinkerOpen;
    }

    public void EndSceneTrigger()
    {
        anim.SetTrigger(sceneChangeTrigHash);
    }
}
