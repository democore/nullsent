using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InitLights : MonoBehaviour
{

	// Use this for initialization
	void Start ()
    {
        InitAllLights();
    }

    public static void InitAllLights()
    {
        foreach (var l in GameObject.FindObjectsOfType<Light>())
        {
            if (l.type != LightType.Spot)
                continue;

            l.gameObject.AddComponent<SpotLightThing>();

            BoxCollider col = l.gameObject.GetComponent<BoxCollider>();
            if (!col)
            {
                col = l.gameObject.AddComponent<BoxCollider>();
                col.size = new Vector3(1f, 1f, l.range / 1.3f);
                col.center = new Vector3(0f, 0f, l.range / 2f);
            }
            col.isTrigger = true;
        }
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public static void ResetAllLights()
    {
        foreach (var l in GameObject.FindObjectsOfType<Light>())
        {
            if (l.type != LightType.Spot)
                continue;

            BoxCollider col = l.gameObject.GetComponent<BoxCollider>();
            if (!col)
            {
                col = l.gameObject.AddComponent<BoxCollider>();
                col.size = new Vector3(1f, 1f, l.range / 2);
                col.center = new Vector3(0f, 0f, l.range / 2f);
            }
            col.isTrigger = true;

            SpotLightThing s = l.GetComponent<SpotLightThing>();
            l.intensity = s.InitialIntensity;
        }
    }
}
