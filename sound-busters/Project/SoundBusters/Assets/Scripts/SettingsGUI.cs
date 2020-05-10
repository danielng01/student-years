using UnityEngine;
using System.Collections;
using System.IO;

[ExecuteInEditMode]

public class SettingsGUI : MonoBehaviour {

	GUIStyle labelStyle;
	GUIStyle textFieldStyle;
	GUIStyle backgroundStyle;
	GUIStyle buttonStyle;
	GUIStyle logoStyle;
	int width, height, offset, buttonWidth, buttonHeight;
	private string newName=MainGUI.userName;
	public string aboutUs = "About us: \n\nMax Shpirka - Team Leader\nLiliya Fatykhova - Designer\nYoanna Hristova - Designer\nDaniel Georgiev - Developer\nGavril Tonev - Developer\nKristo Prifti - Developer\nOleh Stoylar - Developer\n";

	void OnGUI ()	{
		GUI.Box (new Rect (0, 0, width, height), "", backgroundStyle);
		newName = GUI.TextField(new Rect(width/2 - buttonWidth/2, height/2 - offset*3f, buttonWidth, buttonHeight*0.5f), newName, textFieldStyle);
		if (GUI.Button (new Rect (width / 2 - buttonWidth/2, height / 2 - offset*2.5f, buttonWidth, buttonHeight), "Change Username", buttonStyle)) {
			if (newName == "") {
				newName = GUI.TextField(new Rect(width / 2 - buttonWidth/2, height / 2 - offset*3f, buttonWidth, buttonHeight*0.5f), newName, textFieldStyle);;
			} else {
				PlayerPrefs.SetString ("soundbusterUserName", newName);
				MainGUI.userName = newName;
				Application.LoadLevel ("Connect");
			}
		}
		GUI.Label(new Rect(width / 2, height / 2 + offset , 0, 0), aboutUs, labelStyle);
		if (Input.GetKeyDown(KeyCode.Escape)) { Application.LoadLevel("Connect"); }
	}

	// Use this for initialization
	void Start () {
		newName = PlayerPrefs.GetString ("soundbusterUserName");

		Interface.instance.updateSizes();
		labelStyle = Interface.instance.labelStyle;
		textFieldStyle = Interface.instance.textFieldStyle;
		backgroundStyle = Interface.instance.backgroundStyle;
		buttonStyle = Interface.instance.buttonStyle;
		logoStyle = Interface.instance.logoStyle;
		width = Interface.instance.width;
		height = Interface.instance.height;
		offset = Interface.instance.offset;
		buttonWidth = Interface.instance.buttonWidth;
		buttonHeight = Interface.instance.buttonHeight;
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
