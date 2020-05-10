using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class MapGenerator : MonoBehaviour
{

    public Transform buttonType1;
    public Transform buttonType2;
    public Transform buttonType3;
    public Transform buttonType4;
    public Transform buttonType5;

    private Transform[] buttons;

    void InitPrefabs()
    {
        buttons = new Transform[5];
        buttons[0] = buttonType1;
        buttons[1] = buttonType2;
        buttons[2] = buttonType3;
        buttons[3] = buttonType4;
        buttons[4] = buttonType5;
    }

    void Awake()
    {
        SceneManager.LoadScene("Static geometry", LoadSceneMode.Additive);
    }
    // Use this for initialization
    void Start ()
    {
        InitPrefabs();

        for (int i = 0; i < 200; i+=2)
        {
            int buttonType = Random.Range(0, 5);
            float y = Random.Range(1, 5);
            float x = Random.Range(-1, 2);
            Instantiate(buttons[buttonType], new Vector3(x * 2, y, i), Quaternion.identity);
        }
    }
	
	// Update is called once per frame
	void Update ()
    {
	
	}
}
