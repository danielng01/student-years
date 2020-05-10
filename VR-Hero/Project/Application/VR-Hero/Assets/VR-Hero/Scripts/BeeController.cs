using UnityEngine;
using System.Collections;

public class BeeController : MonoBehaviour {

    private NodeGenerator nodeGenerator;
    Transform head;
    AudioSource audio;

    public float speed = 1.0f;

    // Use this for initialization
    void Start () {
        nodeGenerator = GameObject.Find("MapGenerator").GetComponent<NodeGenerator>();
        head = transform.Find("Head");
        audio = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update () {
        audio.panStereo = transform.position.x/10;

        float x = head.forward.x * speed * Time.deltaTime;
        if (x > 0 && transform.position.x >= 10.0f)
        {
            x = 0.0f;
        }
        if (x < 0 && transform.position.x <= -10.0f)
        {
            x = 0.0f;
        }

        float y = head.forward.y * speed * Time.deltaTime;
        if (y < 0 && transform.position.y <= 1.0f)
        {
            y = 0;
        }

        // Calculate the speed depending to the next beat time. Fixes issues if the song beat varies.
        //float z = ( ( nodeGenerator.GetNextBeatZ() - transform.position.z ) / nodeGenerator.GetTimeToNextBeat() ) * Time.deltaTime;

        float z = speed * Time.deltaTime;
        Vector3 move = new Vector3(x, y, z);
        //Debug.Log(move);

        transform.position += move;
    }
}
