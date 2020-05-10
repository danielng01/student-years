using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour {
    Transform head;
    //AudioSource audio;

    public float speed = 1.0f;

	private bool move = false;
	
	private float boundSize = 6.0f;

    // Use this for initialization
    void Start () 
	{
        head = transform.Find("Head");
        //audio = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update () {

		if (!this.move) 
		{
			return;
		}
        //audio.panStereo = transform.position.x/10;

        float x = head.forward.x * speed * Time.deltaTime;
		if (x > 0 && transform.position.x >= boundSize)
        {
            x = 0.0f;
        }
        if (x < 0 && transform.position.x <= -boundSize)
        {
            x = 0.0f;
        }

        float y = head.forward.y * speed * Time.deltaTime;
        if (y < 0 && transform.position.y <= 0.5f)
        {
            y = 0;
        }
		
		float z = head.forward.z * speed * Time.deltaTime;
		if (z > 0 && transform.position.z >= boundSize)
        {
            z = 0.0f;
        }
        if (z < 0 && transform.position.z <= -boundSize)
        {
            z = 0.0f;
        }
        Vector3 move = new Vector3(x, y, z);
        //Debug.Log(move);

        transform.position += move;
    }

	public void ToogleMovement () {
		Debug.Log ("Click handled");
		this.move = !this.move;
	}
}
