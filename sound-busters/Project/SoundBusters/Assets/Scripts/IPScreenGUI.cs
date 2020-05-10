using UnityEngine;
using System.Collections;

public class IPScreenGUI : MonoBehaviour {

	public static string label = "Enter Host IP Address:";
	public string hostIP = "";
	public string regEx = "^([0-9]{1,3})\\.([0-9]{1,3})\\.([0-9]{1,3})\\.([0-9]{1,3})$";
	GUIStyle labelStyle;
	GUIStyle textFieldStyle;
	GUIStyle backgroundStyle;
	GUIStyle buttonStyle;
	GUIStyle logoStyle;
	int width, height, offset, buttonWidth, buttonHeight;

	void Update () {
		if (Network.peerType == NetworkPeerType.Client) {							
			//Application.LoadLevel ("Play");
			//networkView.RPC("LoadLevel", RPCMode.All, "Play", 1);
			//StartCoroutine(LoadLevel("Play", 0));
			LoadLevel("Play", 0);
		}
	}

	[RPC]
	void LoadLevel (string level, int levelPrefix)
	{		
		// There is no reason to send any more data over the network on the default channel,
		// because we are about to load the level, thus all those objects will get deleted anyway
		Network.SetSendingEnabled(0, false);    
		
		// We need to stop receiving because first the level must be loaded first.
		// Once the level is loaded, rpc's and other state update attached to objects in the level are allowed to fire
		Network.isMessageQueueRunning = false;
		
		// All network views loaded from a level will get a prefix into their NetworkViewID.
		// This will prevent old updates from clients leaking into a newly created scene.

		Network.SetLevelPrefix(levelPrefix);
		Application.LoadLevel(level);
		//yield return new WaitForEndOfFrame();
		
		// Allow receiving data again
		Network.isMessageQueueRunning = true;
		// Now the level has been loaded and we can start sending out data to clients
		Network.SetSendingEnabled(0, true);

		//var objects = FindObjectsOfType(typeof(GameObject)) as GameObject[];
		//foreach (var go in objects) {
		//	Debug.Log (go.name);
		//	go.SendMessage ("OnNetworkLoadedLevel", SendMessageOptions.DontRequireReceiver); 
		//}
	}
	
	void OnGUI () {
		
		// Make a background box
		GUI.Box(new Rect(0,0,width,height), "", backgroundStyle);
		GUI.Label(new Rect(width / 2, height / 2, 0, 0), label, labelStyle);
		GUI.Box (new Rect (width / 2 - offset*0.75f, offset/2, offset*1.5f, offset*1.5f), "", logoStyle);
		hostIP = GUI.TextField(new Rect(width / 2 - buttonWidth / 2,
		                                  height / 2 + 0.5f * buttonHeight,
		                                  buttonWidth, buttonHeight*0.5f), hostIP, textFieldStyle);
		// Make the second button.
		if(GUI.Button(new Rect (width / 2 - buttonWidth / 2,
		                        height / 2 + 1.5f * buttonHeight,
		                        buttonWidth, buttonHeight), "Connect", buttonStyle))
		{
			if(hostIP != "")
			{
				if (System.Text.RegularExpressions.Regex.IsMatch(hostIP, regEx))
				{
					if (checkIP(hostIP))
					{
						Connection.JoinServer(hostIP);
						Debug.Log("Matching Successful!");
					}
				}
			}
		}
		if (GUI.Button (new Rect (width / 2 - buttonWidth / 2, height / 2 + buttonHeight*3f, buttonWidth, buttonHeight), "Back", buttonStyle)) {
			Application.LoadLevel ("Connect");
			Network.Disconnect (250);
		}

		if (Input.GetKeyDown (KeyCode.Escape)) {
			Application.LoadLevel ("Connect");
			Network.Disconnect (250);
		}
	}

	bool checkIP(string inp){
		string[] arr = inp.Split('.');
		foreach(string i in arr)
		{
			int nr = int.Parse(i);
			if(nr > 255)
			{
				Debug.Log("Matching unsuccessful!");
				return false;
			}
		}
		return true;
	}
	
	void Start() {
		Interface.instance.updateSizes ();

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
