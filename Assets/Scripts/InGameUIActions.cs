using UnityEngine;
using System.Collections;

public class InGameUIActions : MonoBehaviour {

    int
        openTrigHash,
        closeTrigHash,
        sceneChangeTrigHash;

    Animator anim;

    bool blinkerOpen;

    GameObject gameOverMenu;

    void Start()
    {
        openTrigHash = Animator.StringToHash("open");
        closeTrigHash = Animator.StringToHash("close");
        sceneChangeTrigHash = Animator.StringToHash("sceneChange");

        anim = GetComponent<Animator>();

        blinkerOpen = false;

        gameOverMenu = transform.GetChild(1).gameObject;


        Reset();
    }

    public void Reset()
    {
        gameOverMenu.SetActive(false);
    }

    void BlinkerOpen()
    {
        blinkerOpen = true;
    }

    void BlinkerClosed()
    {
        blinkerOpen = false;
    }

    public bool CheckBlinkerIsOpen()
    {
        return blinkerOpen;
    }

    public void ActivateGameOverMenu()
    {
        gameOverMenu.SetActive(true);
    }

    public void OpenBlinker()
    {
        anim.SetTrigger(openTrigHash);
    }

    public void CloseBlinker()
    {
        anim.SetTrigger(closeTrigHash);
    }

    public void EndSceneTrigger()
    {
        anim.SetTrigger(sceneChangeTrigHash);
    }
}
