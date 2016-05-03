using UnityEngine;
using System.Collections;

public class BlinkerController : MonoBehaviour {

    int
    openTrigHash,
    closeTrigHash,
    sceneChangeTrigHash;

    Animator anim;
    AudioSource audio;

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
        audio = GetComponent<AudioSource>();
    }

    public void Open()
    {
        anim.SetTrigger(openTrigHash);
    }

    void StartOpen()
    {
        audio.clip = soundOpenning;
        audio.Play();
    }

    void IsOpen()
    {
        blinkerOpen = true;
        audio.Stop();
    }

    public void Close()
    {
        anim.SetTrigger(closeTrigHash);
    }

    void StartClose()
    {
        audio.clip = soundClosing;
        audio.Play();
    }

    void IsClosed()
    {
        blinkerOpen = false;
        audio.Stop();
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
