﻿using UnityEngine;
using System.Collections;

public class MenuScript : MonoBehaviour {

	private GameControllerScript gameScript;
	private GUISkin currentGUISkin;
	private Vector2 scrollPosition;
	// Use this for initialization
	void Start () {
		this.gameScript = this.gameObject.GetComponent ("GameControllerScript") as GameControllerScript;
		this.currentGUISkin = gameScript.currentGUISkin;

	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnGUI() {
		if (this.gameScript.gameView == "menu") {
			this.gameScript.mainCamera.transform.eulerAngles = new Vector3 (180, 23, 0);
			this.gameScript.gameLight.intensity = 0;

			//set the sizes appropriately
			GUI.skin = this.gameScript.currentGUISkin;
			
			GUIStyle playButtonStyle = new GUIStyle(currentGUISkin.GetStyle("Button"));
			playButtonStyle.fontSize = Mathf.CeilToInt(Screen.height * 0.10F);
			playButtonStyle.fontStyle = FontStyle.Italic;

			
			GUIStyle menuButtonStyle = new GUIStyle(currentGUISkin.GetStyle("Button"));
			menuButtonStyle.fontSize = Mathf.CeilToInt(Screen.height * 0.06F);

			
			GUIStyle titleLabelStyle = new GUIStyle(currentGUISkin.GetStyle("TitleLabel"));
			titleLabelStyle.fontSize = Mathf.CeilToInt(Screen.height * 0.13F);

			GUI.Label (new Rect (0, 0, Screen.width, Screen.height * 0.06F), "2048 - 3D", titleLabelStyle);

			
			GUI.Label (new Rect (0, Screen.height * 0.15F, Screen.width, Screen.height * 0.06F), "");


			if (GUI.Button(new Rect(Screen.width * 0.20f, Screen.height * 0.25f, Screen.width * 0.60F, Screen.height * 0.10F),"Options", menuButtonStyle)) {
				this.gameScript.gameView = "options";
			}
			if (GUI.Button(new Rect(Screen.width * 0.20f, Screen.height * 0.40f, Screen.width * 0.60F, Screen.height * 0.10F),"Instructions", menuButtonStyle)) {
				this.gameScript.gameView = "instructions";
			}
			if (GUI.Button(new Rect(Screen.width * 0.20f, Screen.height * 0.55F, Screen.width * 0.60F, Screen.height * 0.10F),"Exit", menuButtonStyle)) {
				Application.Quit();
			}

			if (GUI.Button(new Rect(Screen.width * .20f, Screen.height * 0.70F, Screen.width * 0.60F, Screen.height * 0.15F),"Play!",playButtonStyle)) {
				this.gameScript.gameView = "game";
			}

			GUI.Label (new Rect (0, Screen.height * 0.94F, Screen.width, Screen.height * 0.06F), " 2018 -  Brought To You By code-projects.org");

		}
	}
}
