using UnityEngine;
using System.Collections;

public class AudioController : MonoBehaviour {

    public float CurrVolume { get; set; }

	void Start () {
        CurrVolume = 0.0f;
	}
}
