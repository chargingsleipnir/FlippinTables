using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class TitleScreenNavigation : MonoBehaviour {

	void Start () {
	
	}
	
	void Update () {
        if (Input.GetMouseButtonDown(0))
        {
            SceneManager.LoadScene(1);
        }
	}
}
