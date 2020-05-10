using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Collider))]
public class ButtonController : MonoBehaviour
{
    private float volumeMinifier = 0.25f;
    private float volumeMultiplier = 0.25f;
    private Vector3 startingPosition;

    private float holdGazeTime = 0.0f;
    private bool gazeStarted = false;

    private GameObject player;
    private CardboardReticle reticle;

    private bool gazedFarAway = false;

    private GameObject[] walls;
    AudioSource audioSource;

    public Color color;
    public Transform destroyType;

    void Awake()
    {
        player = GameObject.Find("CardboardMain");
        GameObject reticleObject = GameObject.Find("Cardboard Reticle");
        reticle = reticleObject.GetComponent<CardboardReticle>();

        walls = GameObject.FindGameObjectsWithTag("Walls");
    }

    void Start()
    {
        audioSource = player.GetComponent<AudioSource>();
        startingPosition = transform.localPosition;
        SetGazedAt(false);
    }

    void Update()
    {
        if (transform.position.z < player.transform.position.z - 3)
        {
            //audioSource.volume = audioSource.volume - volumeMinifier;
            if (audioSource.volume < 0.0f)
            {
                audioSource.volume = 0.0f;
            }
            DelayedDestroy();
        }

        if (gazedFarAway)
        {
            if (transform.position.z <= player.transform.position.z + 2.25f)
            {
                Transform tr = transform;
                while (tr.childCount > 0)
                {
                    tr = tr.GetChild(0);
                    Renderer renderer = tr.GetComponent<Renderer>();
                    if (renderer != null)
                    {
                        renderer.material.color = Color.green;
                    }
                }          
                
                gazeStarted = true;
                playDieAnimation();
            }
        }
    }

    void LateUpdate()
    {
        Cardboard.SDK.UpdateState();
        if (Cardboard.SDK.BackButtonPressed)
        {
            Application.Quit();
        }

        if (gazeStarted)
        {
            holdGazeTime += Time.deltaTime;
        }
        else
        {
            holdGazeTime = 0.0f;
        }
        
        if (holdGazeTime >= 0.25f)
        {
            AudioSource audioSource = player.GetComponent<AudioSource>();
            audioSource.volume = audioSource.volume + volumeMultiplier;
            if (audioSource.volume > 1.0f)
            {
                audioSource.volume = 1.0f;
            }
            GameObject lighting = GameObject.Find("Lighting");
            lighting.GetComponent<Lighting>().setColor(color);
            DelayedDestroy();

            foreach (GameObject wall in walls)
            {
                Animation animation = wall.GetComponent<Animation>();
                animation.Play(PlayMode.StopAll);
            }
        }
    }

    public void playDieAnimation()
    {
        Transform tr2 = transform;
        while (tr2.childCount > 0)
        {
            tr2 = tr2.GetChild(0);
            Animation animation = tr2.GetComponent<Animation>();
            if (animation != null)
            {
                animation.Play("Take 0010", PlayMode.StopAll);
            }
        }
    }

    public void SetGazedAt(bool gazedAt)
    {
        if (gazedAt)
        {
            if (transform.position.z >= player.transform.position.z + 2.25f)
            {
                gazedFarAway = true;
                return;
            }
            gazeStarted = true;
            playDieAnimation();
        }
        else
        {
            gazeStarted = false;
            gazedFarAway = false;
        }

        Transform tr = transform;
        while (tr.childCount > 0)
        {
            tr = tr.GetChild(0);
            Renderer renderer = tr.GetComponent<Renderer>();
            if (renderer != null)
            {
                //renderer.material.color = gazedAt ? Color.green : Color.red;
                if (gazedAt)
                {
                    renderer.material.color = Color.green;
                }
            }
        }
    }

    public void Reset()
    {
        transform.localPosition = startingPosition;
    }

    public void ToggleVRMode()
    {
        Cardboard.SDK.VRModeEnabled = !Cardboard.SDK.VRModeEnabled;
    }

    public void TeleportRandomly()
    {
        Vector3 direction = Random.onUnitSphere;
        direction.y = Mathf.Clamp(direction.y, 0.5f, 1f);
        float distance = 2 * Random.value + 1.5f;
        transform.localPosition = direction * distance;
    }

    public void DelayedDestroy()
    {
        gameObject.SetActive(false);
        if (destroyType != null)
        {
            Instantiate(destroyType, transform.position, Quaternion.identity);
        }
        DestroyAfter();
    }

    public void DestroyAfter()
    {
        //float lifetime = 0.2f;
        float lifetime = 0.0f;
        Destroy(gameObject, lifetime);
    }
}
