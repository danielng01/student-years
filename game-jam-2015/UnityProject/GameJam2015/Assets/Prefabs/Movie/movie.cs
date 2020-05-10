using UnityEngine;
using System.Collections;

public class movie : MonoBehaviour {
		public MovieTexture movTexture;
		private bool started = false;
		void Update() {
			if (!started) {
				
				if(Input.GetKeyDown(KeyCode.Q))
				{
					
					renderer.material.mainTexture = movTexture;
					movTexture.Play();
					
					audio.clip = movTexture.audioClip;
					audio.Play();
				}
			}
		}
}
