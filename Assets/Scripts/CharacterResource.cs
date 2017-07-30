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
    Light CurrentLight; //list of lights the ghost is in 
                                                     //atm restricted to 1 light, but could change
    public float dimAmount = 0.5f;  //how much the light the ghost is in dims every update
                                    //could be changed so that each light has its own amount it dims by
    public Vector3 startPos;
    public float respawnTime = 3f; //how long it takes for the player to respawn after dying

    CharacterMovemend2D charMovement;

    ParticleSystem collectingSystem;

    public bool IsAlive
    {
        get
        {
            return Resource > 0f;
        }
    }

    // Use this for initialization
    void Start ()
    {
        collectingSystem = GetComponentInChildren<ParticleSystem>();
        charMovement = GetComponent<CharacterMovemend2D>();
        Lights.AddRange(GetComponentsInChildren<Light>());
        renderer = GetComponentInChildren<SpriteRenderer>();
        startPos = transform.position; 
	}

    float soundDelay;
	
	// Update is called once per frame
	void Update ()
    {
        if(transform.position.y < -10f)
        {
            if (!didDie)
                StartCoroutine(playerDeath(respawnTime));
        }

        //float curLost = Time.deltaTime * ResourceLostPerFrame;
        float curLost = -1f * Time.deltaTime * ResourceLostPerFrame;
        float curRegen = RegenInLight * Time.deltaTime;
        float resourceChange = 0f;

        if (!InLight)
        {
            //Resource -= curLost;
            collectingSystem.Stop();
            if (charMovement.IsMoving)
                resourceChange = curLost;
        }
        else
        {
            float intensityToChange = CurrentLight.intensity;
            if (intensityToChange != 0f)
            {
                if (Resource < MaxResource)
                {
                    //each light could have its own "dimming factor" so that dif lights could dim at dif speeds
                    intensityToChange -= dimAmount;
                    CurrentLight.intensity = intensityToChange;
                    //Resource += curRegen;
                    resourceChange = curRegen;
                    if (float.IsNaN(soundDelay) || soundDelay <= Time.time)
                    {
                        AudioManager.Instance.PlayEffect("Powerup");
                        soundDelay = Time.time + 1f;
                    }

                    collectingSystem.Play();

                    //Stop from going into loop if it's empty
                    if (intensityToChange <= 0f)
                    {
                        Destroy(CurrentLight.GetComponent<Collider>());
                        InLight = false;
                    }
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
        AudioManager.Instance.PlayEffect("loose");

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
            CurrentLight = l;
            InLight = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        Light l = other.GetComponent<Light>();
        if (l != null && l.type == LightType.Spot)
        {
            CurrentLight = null;
            InLight = false;
        }
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
