using UnityEngine;
using System.Collections;
[ExecuteInEditMode]
public class MainGUI : MonoBehaviour { 
	public static string userName="", label="Enter username:";
	GUIStyle labelStyle;
	GUIStyle textFieldStyle;
	GUIStyle backgroundStyle;
	GUIStyle buttonStyle;
	GUIStyle logoStyle;
	int width, height, offset, buttonWidth, buttonHeight;

	// Use this for initialization
	void Awake () {
		//PlayerPrefs.DeleteAll ();  //RESET USERNAMES
		/*if (PlayerPrefs.GetString ("soundbusterUserName", "NOT_REGISTERED") != "NOT_REGISTERED") {
			Application.LoadLevel("Connect");
		}*/
	}

	// Update is called once per frame
	void Update () {
	
	}

	void OnGUI () {

		// Make a background box
		GUI.Box(new Rect(0,0,width,height), "", backgroundStyle);
		GUI.Label(new Rect(width / 2, height / 2, 0, 0), label, labelStyle);
		GUI.Box (new Rect (width / 2 - offset*0.75f, offset/2, offset*1.5f, offset*1.5f), "", logoStyle);
		userName = GUI.TextField(new Rect(width / 2 - buttonWidth / 2,
		                                  height / 2 + 0.5f * buttonHeight,
		                                  buttonWidth, buttonHeight*0.5f), userName, textFieldStyle);
		// Make the second button.
		if(GUI.Button(new Rect (width / 2 - buttonWidth / 2,
		                        height / 2 + 1.5f * buttonHeight,
		                        buttonWidth, buttonHeight), "Submit", buttonStyle)){
			if(userName!="") 
			{
				PlayerPrefs.SetString("soundbusterUserName",userName);
				Application.LoadLevel("Connect");
			}
        }

		if (Input.GetKeyDown (KeyCode.Escape)) {
			Application.Quit (); 
		}
	}

	void Start() {
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
}
