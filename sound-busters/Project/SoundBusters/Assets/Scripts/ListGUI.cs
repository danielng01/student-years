using UnityEngine;
using System.Collections;
//[ExecuteInEditMode]
public class ListGUI : MonoBehaviour {
	GUIStyle labelStyle;
	GUIStyle textFieldStyle;
	GUIStyle backgroundStyle;
	GUIStyle buttonStyle;
	GUIStyle logoStyle;
	int width, height, offset, buttonWidth, buttonHeight;
	
	public GUISkin fileBrowserSkin;

	string startingDirectory;
	FileBrowser fileBrowser;

	// touch controls
	private Vector2 beginFinger;        // finger
	private float deltaFingerY;         // finger
	private Vector2 beginPanel;         // scrollpanel
	private Vector2 latestPanel;        // scrollpanel

	void OnGUI () {

		GUI.Box (new Rect (0, 0, width, height), "", backgroundStyle);

		if(fileBrowser.draw()){
			if(fileBrowser.outputFile == null){
				//Debug.Log("Cancel pressed");
				Application.LoadLevel("Connect");
			}else{
				//Debug.Log("Output File = \""+fileBrowser.outputFile.ToString()+"\"");
				Debug.Log(fileBrowser.outputFile.Name.Substring(0, fileBrowser.outputFile.Name.Length - 4));
				PlayGUI.SetSongPath(fileBrowser.outputFile.Name.Substring(0, fileBrowser.outputFile.Name.Length - 4));
				//test.songPath = fileBrowser.outputFile.FullName;
				Application.LoadLevel("Play");
				/*the outputFile variable is of type FileInfo from the .NET library "http://msdn.microsoft.com/en-us/library/system.io.fileinfo.aspx"*/
			}
		}
	}

	// Use this for initialization
	void Start () {
		//TODO - starting directory
		startingDirectory = "/storage/emulated";
		#if UNITY_EDITOR
		startingDirectory = Application.dataPath+"\\Resources\\Music";
		#endif

		fileBrowser = new FileBrowser(startingDirectory);
		fileBrowser.guiSkin = fileBrowserSkin;
		fileBrowser.setLayout(1);	//mobile
		fileBrowser.searchPattern = "*.mp3";
		fileBrowser.guiSkin.button.fontSize = Screen.height / 45;
		fileBrowser.guiSkin.button.fixedHeight = 0.5f*Interface.instance.buttonHeight;
		if (Network.peerType == NetworkPeerType.Server) {
			Debug.Log ("Server");
		}
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
	void Update()  // so, make it scroll with the user's finger
	{
		if(Input.touchCount == 0) return;
		Touch touch = Input.touches[0];
		if (touch.phase == TouchPhase.Moved)
		{
			// simplistically, scrollPos.y += touch.deltaPosition.y;
			// but that doesn't actually work
			
			// don't forget you need the actual delta "on the glass"
			// fortunately it's easy to do this...
			
			float dt = Time.deltaTime / touch.deltaTime;
			if (dt == 0 || float.IsNaN(dt) || float.IsInfinity(dt))
				dt = 1.0f;
			Vector2 glassDelta = touch.deltaPosition * dt;
            
            fileBrowser.fileScroll.y += glassDelta.y;
        }
    }
}
