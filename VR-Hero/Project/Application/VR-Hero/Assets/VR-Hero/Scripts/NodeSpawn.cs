using UnityEngine;
using System.Collections;

public class NodeSpawn : MonoBehaviour {

    public float timeToAppear = 1;
    private Vector3 finalPos;

	// Use this for initialization
	void Start () {
        finalPos = transform.position;
        transform.position = new Vector3(
            transform.position.x,
            -1,
            transform.position.z
        );
	}
	
	// Update is called once per frame
	void Update () {
        if (transform.position.y < finalPos.y)
        {
            transform.position = new Vector3(
                transform.position.x,
                transform.position.y + finalPos.y/timeToAppear * Time.deltaTime,
                transform.position.z
            );
        }
    }
}
