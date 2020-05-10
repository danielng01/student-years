using UnityEngine;
using System.Collections;

public class Score : MonoBehaviour
{
	public int score = 0;					// The player's score.

	void Awake ()
	{
	}


	void Update ()
	{
		// Set the score text.
		if (score >= 5) 
		{
			Application.LoadLevel(Application.loadedLevel + 1);
		}
		guiText.text = score + "/ 5";
	}

}
