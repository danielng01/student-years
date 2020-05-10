//#if UNITY_STANDALONE
//	#define	STANDALONE
//#elif UNITY_IPHONE || UNITY_ANDROID || UNITY_WP8
//	#define PHONE
//#endif

#define LOCAL_RESOURCES
using UnityEngine;

public class PlayGUI : MonoBehaviour {
	public GUIStyle play, slider, sliderthumb;
	GUIStyle labelStyle;
	GUIStyle textFieldStyle;
	GUIStyle backgroundStyle;
	GUIStyle buttonStyle;
	GUIStyle logoStyle;
	int width, height, offset, buttonWidth, buttonHeight;
	public GUIStyle button;
	public Texture2D iconPlay;
	public Texture2D iconPause;
	private Texture2D iconReplay;
	public Texture2D iconReplayTrue;
	public Texture2D iconReplayFalse;
	private Texture2D icon;

	private bool ch, loaded;
	private float StopTime=0.0F;
	private float hSliderValue =0.0F;

	// PHONE
	private WWW www = null;
	private bool fileLoading = false;

	private bool fileLoaded = false;
	public static string songPath=@"";

	public static void SetSongPath(string path)
	{
		//songPath = "Music/"+path;
		songPath = "Music/ZHU - Faded";
	}
	
	void LoadFile() {

		#if PHONE
			songPath=songPath.Replace('\\', '/');
			string url = @"file:///"+songPath;
			Debug.Log(url);
			www = new WWW(url);
			
			AudioClip audioClip = www.GetAudioClip(false, false);
			
			var audioSource = GetComponent<AudioSource> ();
			audioSource.clip = audioClip;
		#elif LOCAL_RESOURCES
		var audioSource = GetComponent<AudioSource> ();
		AudioClip clip = Resources.Load<AudioClip>(songPath);
		audioSource.clip = clip;
		#endif
	}

	string secondsToString (float timer) {
		string minutes = Mathf.Floor(timer / 60).ToString("00");
		string seconds = (timer % 60).ToString("00");
		return minutes + ":" + seconds;
	}
	
	void OnGUI ()
	{	
		Interface.instance.updateSizes();
		sizeUpdate();
		GUI.Box(new Rect(0,0,width,height), "", backgroundStyle);
		GUI.Label (new Rect (width/2, height - 0.5f*offset, 0, 0), Network.player.ipAddress, labelStyle);

		if (GUI.Button (new Rect(width-1.25f*offset, 0.75f*offset, offset/2,offset/2), iconReplay, GUIStyle.none)) 
		{
			if(audio.loop)
			{
				audio.loop=false;
				iconReplay=iconReplayFalse;
			}
			else
			{
				audio.loop = true;
				iconReplay=iconReplayTrue;
			}
		}

		if (!float.IsNaN(audio.time)) {
			GUI.Label (new Rect (width/2, height - 1.5f*offset, 0, 0), secondsToString (audio.time), labelStyle);

			if(audio.clip != null)
			{	
				GUI.Label (new Rect (width / 2, height / 2 - 2*offset, 0, 0), audio.clip.name, labelStyle);
				sliderthumb.fixedWidth = sliderthumb.fixedHeight = offset/4;

				// Slider
				if(Network.peerType == NetworkPeerType.Server)
				{
					hSliderValue = GUI.HorizontalSlider(new Rect(width*0.15f, height-1.25f*offset, width*0.7f, offset/4), audio.time, 0.0F, audio.clip.length);
					if (audio.time != hSliderValue) {
						networkView.RPC("SetAudioTime", RPCMode.All, hSliderValue);
					}
				}
			}

		}
		play.normal.background = icon;
		if (GUI.Button (new Rect(width/2 - offset, height/2 - offset, 2*offset, 2*offset), "", play)) 
		{
			if(Network.peerType == NetworkPeerType.Server)
			{
				networkView.RPC("PlayPause", RPCMode.All, songPath);
			}
		}

		if (www != null) {
			if (!www.isDone) {
				GUI.Label(new Rect (width/2, height/2+offset*2f, 0, 0), "Loading... " + (www.progress*100) + "%", labelStyle);
			}
		}

		if (Input.GetKeyDown(KeyCode.Escape)) { Application.LoadLevel("Connect"); }
	}

	[RPC]
	void PlayPause(string path)
	{
		if (!fileLoaded) {
			songPath = path;
			LoadFile ();
		}

		#if PHONE
			fileLoading = true;
		#elif LOCAL_RESOURCES
			if (!ch) {
				icon = iconPause;
				audio.Play ();
			} else {
				icon = iconPlay;
				audio.Pause ();
			} 
			ch = !ch;
		#endif
	}

	[RPC]
	void SetAudioTime(float value)
	{
		audio.time = value;
	}

	// Use this for initialization
	void Awake()
	{
	}
	void Start () 
	{
		icon = iconPause;
		iconReplay = iconReplayFalse;
		if(Network.peerType == NetworkPeerType.Server)
		{
			networkView.RPC("PlayPause",RPCMode.All, songPath);
		}

		if (Network.peerType == NetworkPeerType.Client) 
		{
			networkView.RPC ("GetFileName", RPCMode.All);
		}
	}

	void sizeUpdate() {
		labelStyle = Interface.instance.labelStyle;
		textFieldStyle = Interface.instance.textFieldStyle;
		backgroundStyle = Interface.instance.backgroundStyle;
		buttonStyle = Interface.instance.buttonStyle;
		logoStyle = Interface.instance.logoStyle;
		width = Interface.instance.width;
		height = Interface.instance.height;
		offset = Interface.instance.offset;
		buttonWidth = Interface.instance.buttonWidth;
	}

	// Update is called once per frame
	void Update () {
		if (fileLoading) {
			if(www.isDone)
			{
				//Debug.Log ("Done");
				if (!ch) {
					icon = iconPause;
					audio.Play ();
				} else {
					icon = iconPlay;
					audio.Pause ();
				} 
				ch = !ch;

				fileLoading = false;
				fileLoaded = true;
			}
			else
			{
				// Loading
			}
		}
	}

	void OnApplicationQuit() {	
	}

	[RPC]
	void GetFileName()
	{
		if(Network.peerType == NetworkPeerType.Server)
		{
			Debug.Log ("Network Level Loaded");
			networkView.RPC("StartSong",RPCMode.All, songPath, audio.time);
		}
	}

	[RPC]
	void StartSong(string path, float time)
	{
		if(Network.peerType == NetworkPeerType.Client)
		{
			if (!fileLoaded) {
				songPath = path;
				LoadFile ();
				audio.Play ();
				audio.time = time;
			}
		}
	}
}
