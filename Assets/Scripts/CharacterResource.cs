using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterResource : MonoBehaviour {

    List<Light> Lights = new List<Light>();
    new SpriteRenderer renderer;

    public float Resource = 1f;
    public const float MaxResource = 1f;
    public float ResourceLostPerFrame = 0.1f;

    public float RegenInLight = 0.3f;
    public bool InLight = false;

	// Use this for initialization
	void Start () {
        Lights.AddRange(GetComponentsInChildren<Light>());
        renderer = GetComponentInChildren<SpriteRenderer>();
	}
	
	// Update is called once per frame
	void Update ()
    {
        float curLost = Time.deltaTime * ResourceLostPerFrame;
        float curRegen = RegenInLight * Time.deltaTime;

        if (!InLight)
            Resource -= curLost;
        else
            Resource += curRegen;

        if (Resource < 0f)
            Resource = 0f;
        if (Resource > MaxResource)
            Resource = MaxResource;

        print(Resource);
        setCharacterIntensity(Resource);
	}

    private void OnTriggerEnter(Collider other)
    {
        Light l = other.GetComponent<Light>();
        if(l != null && l.type == LightType.Spot)
            InLight = true;
    }

    private void OnTriggerExit(Collider other)
    {
        Light l = other.GetComponent<Light>();
        if (l != null && l.type == LightType.Spot)
            InLight = false;
    }

    private void setCharacterIntensity(float i)
    {
        if (i < 0f)
            i = 0f;

        foreach(Light l in Lights)
        {
            l.intensity = i;
        }

        Color c = renderer.color;
        c.a = i;
        renderer.color = c;
    }
}
