#define CLOUD
using UnityEngine;
using System.Collections;

[ExecuteInEditMode]
public class ConnectGUI : MonoBehaviour
{
	GUIStyle labelStyle;
	GUIStyle textFieldStyle;
	GUIStyle backgroundStyle;
	GUIStyle buttonStyle;
	GUIStyle logoStyle;
	int width, height, offset, buttonWidth, buttonHeight;
	bool connected;
	
	void OnGUI ()
	{
		GUI.Box (new Rect (0, 0, width, height), "", backgroundStyle);

		if (!connected) {
			GUI.Box (new Rect (width / 2 - offset*0.75f, offset/2, offset*1.5f, offset*1.5f), "", logoStyle); 
			GUI.Label (new Rect (width / 2, height / 3, 0, 0), 
			           "Hello, "+ PlayerPrefs.GetString("soundbusterUserName","")+"\nLet's play music together", labelStyle);
		}
		offset = 0;
		if (Network.peerType == NetworkPeerType.Disconnected) {
			if (GUI.Button (new Rect (width / 2 - buttonWidth / 2,
			                          height / 2 + offset + buttonHeight / 2,
			                          buttonWidth, buttonHeight),
			                "Create a new session", buttonStyle)) {
				Connection.StartServer ();
			}
			
			if (GUI.Button (new Rect (width / 2 - buttonWidth / 2,
			                          height / 2 + offset + 2f * buttonHeight,
			                          buttonWidth, buttonHeight),
			                "Join an existing one", buttonStyle)) {
				Application.LoadLevel ("IPScreen");
			}

			if (GUI.Button (new Rect (width / 2 - buttonWidth / 2,
			                          height / 2 + offset + 3.5f * buttonHeight,
			                          buttonWidth, buttonHeight),
			                "Settings", buttonStyle)) {
				Application.LoadLevel ("Settings");
			}
			connected = false;
		} else if (Network.peerType == NetworkPeerType.Server) {
			offset = Interface.instance.offset;
			GUI.Label (new Rect (width / 2 - buttonWidth / 2, height / 2 - offset - buttonHeight / 2, buttonWidth, buttonHeight), "Server: " + Network.player.ipAddress, labelStyle);
			GUI.Label (new Rect (width / 2 - buttonWidth / 2, height / 2 - offset / 2 - buttonHeight / 2, buttonWidth, buttonHeight),
			           "Connections: " + Network.connections.Length, labelStyle);
			offset = 0;
			// Make the second button.
			if (GUI.Button (new Rect (width / 2 - buttonWidth / 2,
			                          height / 2 + offset + 2f * buttonHeight,
			                          buttonWidth, buttonHeight), "File Browser", buttonStyle)) {
				Application.LoadLevel ("List");
			}
			if (GUI.Button (new Rect (width / 2 - buttonWidth / 2,
			                          height / 2 + offset + buttonHeight / 2,
			                          buttonWidth, buttonHeight), "Logout", buttonStyle)) {
				Network.Disconnect (250);
			}
			connected = true;

			if (Input.GetKeyDown (KeyCode.Escape)) {
				Network.Disconnect (250); 
			}
		}
		if (Input.GetKeyDown (KeyCode.Escape)) {
			Application.Quit (); 
		}
		offset = Interface.instance.offset;
	}
	

	void Start() {
		connected = false;
		Debug.Log ("IP Address: " + Network.player.ipAddress);
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
	void Update ()
	{
	
	}
}
