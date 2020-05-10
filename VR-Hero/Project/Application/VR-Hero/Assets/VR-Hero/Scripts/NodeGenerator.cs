using UnityEngine;
using System.Collections;
using SimpleJSON;
using System.Globalization;
using UnityEngine.SceneManagement;

public class NodeGenerator : MonoBehaviour
{
    public Transform nodeType1;
    public Transform nodeType2;
    public Transform nodeType3;
    public Transform nodeType4;
    public Transform nodeType5;
    public float offsetZ = 0.25f;
    public delegate void BeatAction();
    public static event BeatAction OnBeat;

    private Transform[] nodeTypes;
    private BeeController beeController;
    private static int spawned = 0;
    private float passedTime;
    private int lastBeat = 0;
    private JSONNode beats;
    private float[] beatTimes;


    private void InitPrefabs()
    {
        nodeTypes = new Transform[5] {
            nodeType1,
            nodeType2,
            nodeType3,
            nodeType4,
            nodeType5,
        };
    }

    private void InitNodes()
    {
        TextAsset nodesJson = (TextAsset)Resources.Load("nodes", typeof(TextAsset));
        beats = JSON.Parse(nodesJson.text);
        //Debug.Log(beats);

        beatTimes = new float[beats.Count];

        for (int i = 0; i < beats.Count; i++)
        {
            beatTimes[i] = beats[i]["t"].AsFloat;
        }

    }

    // Spawn numAhead beats ahead. Defaults to all beats.
    private IEnumerator SpawnBeats(int numAhead = 0)
    {
        yield return new WaitForSeconds(0.3f);
        for (int i=0; (i < numAhead || numAhead == 0) && spawned < beats.Count; spawned++, i++)
        {
            JSONNode nodes = beats[spawned]["nodes"];
            for ( int j=0; j<nodes.Count; j++ )
            {
                int buttonType = nodes[j]["type"].AsInt - 1;
                if (buttonType < 0)
                {
                    //Debug.Log("Invalid button type: " + buttonType);
                    //buttonType = 0;
                    continue;
                }
                if (buttonType > nodeTypes.Length - 1)
                {
                    Debug.Log("Invalid button type: " + buttonType);
                    buttonType = nodeTypes.Length - 1;
                }
                float x = nodes[j]["pos"][0].AsFloat;
                float y = nodes[j]["pos"][1].AsFloat;
                Instantiate(nodeTypes[buttonType], new Vector3(x, y, BeatZ(spawned)), Quaternion.identity);
            }
        }
    }

    private int NextBeat()
    {
        return lastBeat <= beatTimes.Length ? lastBeat + 1 : lastBeat;
    }

    private float BeatZ(int beat)
    {
        return beats[beat]["t"].AsFloat * beeController.speed + offsetZ;
    }

    void Awake()
    {
        SceneManager.LoadScene("Static geometry", LoadSceneMode.Additive);
    }

    // Use this for initialization
    void Start()
    {
        passedTime = 0;
        beeController = GameObject.Find("CardboardMain").GetComponent<BeeController>();
        InitPrefabs();
        InitNodes();
        StartCoroutine(SpawnBeats(4));
    }

    // Update is called once per frame
    void Update()
    {
        passedTime += Time.deltaTime;
        if (lastBeat < beatTimes.Length && passedTime >= beatTimes[lastBeat])
        {
            if (OnBeat != null)
                OnBeat();
            lastBeat++;
            //if (0 == lastBeat % 3) SpawnBeats(5);
            StartCoroutine(SpawnBeats(1));
        }
    }

    public float GetLastBeat()
    {
        return beatTimes[lastBeat];
    }

    public float GetNextBeat()
    {
        return beatTimes[NextBeat()];
    }

    public float GetNextBeatZ()
    {
        return BeatZ(NextBeat());
    }

    public float GetPassedTime()
    {
        return passedTime;
    }

    public float GetTimeToNextBeat()
    {
        return GetNextBeat() - passedTime;
    }
}
