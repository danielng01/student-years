using UnityEngine;
using System.Collections;

public class Interface : MonoBehaviour {
	public GUIStyle labelStyle;
	public GUIStyle textFieldStyle;
	public GUIStyle backgroundStyle;
	public GUIStyle buttonStyle;
	public GUIStyle logoStyle;
	public int width, height, offset, buttonWidth, buttonHeight;

	public static Interface instance;

	void Awake () {
		if (Interface.instance == null)
		{
			Interface.instance = this;
			DontDestroyOnLoad(transform.gameObject);
		}
		else
		{
			Destroy(transform.gameObject);
		}
		
		updateSizes();
	}

	public void updateSizes() {
		width = Screen.width;
		height = Screen.height;
		buttonStyle.fontSize = height / 35;
		labelStyle.fontSize = height / 30;
		textFieldStyle.fontSize = height / 35;
		buttonHeight = height / 13;
		buttonWidth = width / 2;
		offset = height / 8;
	}	
}