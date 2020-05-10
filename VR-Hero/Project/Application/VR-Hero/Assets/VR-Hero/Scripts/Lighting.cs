using UnityEngine;
using System.Collections;

public class Lighting : MonoBehaviour {

    // Use this for initialization
    void Start()
    {
        //InvokeRepeating("setRandomColor", 0.0f, 1.0f);
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void setRandomColor()
    {
        float red = Random.Range(0.0f, 1.0f);
        float green = Random.Range(0.0f, 1.0f);
        float blue = Random.Range(0.0f, 1.0f);
        float alpha = Random.Range(0.0f, 1.0f);
        setColor(new Color(red, green, blue, alpha));
    }

    public void setColor( Color color)
    {
        RenderSettings.ambientLight = color;
        RenderSettings.skybox.SetColor("_Tint", color);
        RenderSettings.fogColor = color;
    }
}
