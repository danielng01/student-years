using UnityEngine;
using System.Collections;

public class MainMenuControl : MonoBehaviour 
{

	public void StartGame()
	{
		Application.LoadLevel(1);
	}

	public void LoadLevel( int level )
	{
		Application.LoadLevel( level );
	}

	public void ExitGame()
	{
		Application.Quit ();
	}

}


