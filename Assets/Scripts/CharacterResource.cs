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

    bool didDie = false;

    //Yuri's variables <3
    List<Light> LightsGhostIsIn = new List<Light>(); //list of lights the ghost is in 
                                                     //atm restricted to 1 light, but could change
    public float dimAmount = 0.5f;  //how much the light the ghost is in dims every update
                                    //could be changed so that each light has its own amount it dims by
    public Vector3 startPos;
    public float respawnTime = 3f; //how long it takes for the player to respawn after dying

    public AudioClip DieSound;
    public AudioClip PowerupSound;

    AudioSource source;

    public bool IsAlive
    {
        get
        {
            return Resource > 0f;
        }
    }

    // Use this for initialization
    void Start () {
        source = GetComponent<AudioSource>();
        Lights.AddRange(GetComponentsInChildren<Light>());
        renderer = GetComponentInChildren<SpriteRenderer>();
        startPos = transform.position; 
	}
	
	// Update is called once per frame
	void Update ()
    {
        //float curLost = Time.deltaTime * ResourceLostPerFrame;
        float curLost = -1f * Time.deltaTime * ResourceLostPerFrame;
        float curRegen = RegenInLight * Time.deltaTime;
        float resourceChange = 0f;

        if (!InLight)
        {
            //Resource -= curLost;
            resourceChange = curLost;
        }
        else
        {
            foreach (Light toDim in LightsGhostIsIn)
            {
                float intensityToChange = toDim.intensity;
                if (intensityToChange != 0f)
                {
                    //intensityToChange -= toDim.dimFactor; 
                    //each light could have its own "dimming factor" so that dif lights could dim at dif speeds
                    intensityToChange -= dimAmount;
                    toDim.intensity = intensityToChange;
                    //Resource += curRegen;
                    resourceChange = curRegen;
                    if(!source.isPlaying)
                    {
                        source.clip = PowerupSound;
                        source.Play();
                    }
                }
                else
                {
                    resourceChange = curLost;
                }
            }
        }

        Resource += resourceChange;

        if (Resource <= 0f)
        {
            //Resource = 0f;
            if(!didDie)
                StartCoroutine(playerDeath(respawnTime));
        }
        if (Resource > MaxResource)
            Resource = MaxResource;

        //print(Resource);
        setCharacterIntensity(Resource);
	}

    private IEnumerator playerDeath(float waittime)
    {
        didDie = true;
        source.clip = DieSound;
        source.Play();

        yield return new WaitForSeconds(waittime);
        Resource = MaxResource;
        transform.position = startPos;

        InitLights.ResetAllLights();
        didDie = false;
    }
    private void OnTriggerEnter(Collider other)
    {
        Light l = other.GetComponent<Light>();
        if (l != null && l.type == LightType.Spot)
        {
            LightsGhostIsIn.Add(l);
            InLight = true;
        }
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
