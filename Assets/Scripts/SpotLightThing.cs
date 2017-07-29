using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpotLightThing : MonoBehaviour {

    public float InitialIntensity = 0f;

	// Use this for initialization
	void Start () {
        InitialIntensity = GetComponent<Light>().intensity;
	}
}
