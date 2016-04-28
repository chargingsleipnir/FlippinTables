using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class TitleScreenNavigation : MonoBehaviour {

    AudioSource audio;

	void Start () {
        audio = GetComponent<AudioSource>();
	}

    public void StartGame()
    {
        StartCoroutine(NextScene());
    }

    IEnumerator NextScene()
    {
        audio.Play();
        yield return new WaitForSeconds(audio.clip.length);
        SceneManager.LoadScene(1);
    }
}
