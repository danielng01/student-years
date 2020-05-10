using UnityEngine;
using System.Collections;

public class Connection : MonoBehaviour {
	public enum ServerType
	{
		Local,
		DaniPC,
		GabiPC,
		OlegPC,

		Cloud
	};
	
	public static ServerType serverType = ServerType.DaniPC;
	public ServerType type; 

	private static int Port = 25001;

	private static Connection instance;
	// Use this for initialization

	void Awake () {
		if (Connection.instance == null)
		{
			Connection.instance = this;
			DontDestroyOnLoad(transform.gameObject);
			serverType = type;
		}
		else
		{
			Destroy(transform.gameObject);
		}
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public static void StartServer()
	{
		Network.InitializeServer(1000,Port);
	}

	public static void JoinServer(string ip)
	{
		Network.Connect(ip,Port);
	}

	static string GetIP(ServerType serverType)
	{
		switch(serverType)
		{
		case ServerType.Local:
		{
			return "127.0.0.1";
		}
		case ServerType.DaniPC:
		{
			return "192.58.35.242";
		}
		case ServerType.GabiPC:
		{
			return "192.168.7.109";
		}
		case ServerType.OlegPC:
		{
			return "192.dasdadsdsadsasddsa.7.108";
		}	
		case ServerType.Cloud:
		{
			return "77.77.151.81";
		}
		}
		
		return "127.0.0.1";
	}
}
