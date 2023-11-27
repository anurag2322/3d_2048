﻿using UnityEngine;
using System.Collections;

public class BlockScript : MonoBehaviour {

	public int x; //index along x axis
	public int y; //index along y axis
	public int z; //index along z axis
	public int blockNumber;  //number displayed on the block (-1 for no number)
	public Vector3 originalPosition;  //orginal position of the block
	private int moveNewBlockNumber; //number to change to once the move has been completed
	private float moveStartTime = -1F;  //amount of seconds since move started
	private Vector3 rotateDirection;
	private float rotationTotal = -1F;
	private int rotateNewBlockNumber;
	private Vector3 moveNewPosition;      //final destination of move
	private float moveDuration = 0.1F;
	private float scale = 0;
	private float yOffset;
	private GameControllerScript gameScript;

	public static int emptyBlock = -1;
	public static int voidBlock = -2;

	
	public void Initialize(int x, int y, int z, GameControllerScript gameScript) {
		this.scale = gameScript.scale;
		this.yOffset = gameScript.yOffset;
		this.gameScript = gameScript;
		this.x = x;
		this.y = y;
		this.z = z;
		this.originalPosition = new Vector3(x * this.scale, y * this.scale + this.yOffset, z * this.scale);
	}

	// Use this for initialization
	void Start () {
		//this.originalPosition = transform.position;
	}
	
	// Update is called once per frame
	void Update () {

		if (moveStartTime >= 0F) {
			float percentComplete = (Time.time - this.moveStartTime) / this.moveDuration;
			if(this.blockNumber > -1) {
				transform.position = Vector3.Lerp(this.originalPosition, this.moveNewPosition, percentComplete);
			}
			if (percentComplete >= 1F) {
				this.moveStartTime = -1F;
				this.setBlockNumber(this.moveNewBlockNumber);
				transform.position = this.originalPosition;
			}
		}
		
		if (this.rotationTotal >= 0F) {
			float rotateBy = 90 * Time.deltaTime / this.moveDuration;
			Vector3 rotationPoint = new Vector3 (3f, 4f, 3f);
			transform.RotateAround(rotationPoint, this.rotateDirection, rotateBy);
			this.rotationTotal += rotateBy;

			if(rotationTotal >= 90) {
				Debug.Log (rotationTotal);
				this.rotationTotal = -1F;
				transform.position = this.originalPosition;
				transform.rotation = new Quaternion(0,0,0,1);
				this.setBlockNumber(rotateNewBlockNumber);
			}
		}
	}

	public void move(string axis, int units, float scale, int newBlockNumber) {

		//this.moveDuration = duration;
		this.scale = scale;
		this.moveNewBlockNumber = newBlockNumber;
		this.moveStartTime = Time.time;
		this.moveNewPosition = this.originalPosition;
		//this.transform = this.originalPosition;
		if (axis == "x")
						this.moveNewPosition.x = this.originalPosition.x + (this.scale * units);
		if (axis == "y")
						this.moveNewPosition.y = this.originalPosition.y + (this.scale * units);
		if (axis == "z")
						this.moveNewPosition.z = this.originalPosition.z + (this.scale * units);
	}

	public void rotateBlock (Vector3 direction) {
		this.rotateDirection = direction;
		this.rotationTotal = 0;
		this.rotateNewBlockNumber = this.GetNumberAfterRotation(direction);
	}

	public void setBlockNumber(int blockNumber) {
		this.blockNumber = blockNumber;
		TextMesh textMesh = this.GetComponentInChildren<TextMesh>();
		Material blockTextMaterial = gameObject.transform.Find ("BlockText").gameObject.GetComponent<Renderer>().material;
		MeshRenderer cube = gameObject.GetComponentInChildren<MeshRenderer>();
		Color cubeColor = cube.GetComponent<Renderer>().material.color;

		//block doesn't exits
		if(blockNumber == -2) {
			cube.GetComponent<Renderer>().enabled = false;
			textMesh.text = "";
		}
		//block is empty
		else if (blockNumber == -1 ) {
			cube.GetComponent<Renderer>().enabled = false;
			cube.GetComponent<Renderer>().material.color = new Color(1, 1, 1, 0.2f);
			textMesh.text = "";
		}
		//block has a value
		else {
			cube.GetComponent<Renderer>().enabled = true;
			cube.material.color = this.getColor (blockNumber);
			blockTextMaterial.SetColor ("_Color", new Color(1,1,1));
			if(blockNumber == 0) blockTextMaterial.SetColor ("_Color", new Color(0.8f,0.8f,1));
			textMesh.text = blockNumber.ToString();
			transform.position = this.originalPosition;
		}

	}

	private Color getColor(int num) {
		if (num == -2) return HexToColor ("000000");
		if (num == 0) return HexToColor ("000055");
		if (num == 2) return HexToColor ("3333cc");
		if (num == 4) return HexToColor ("0099aa");
		if (num == 8) return HexToColor ("00ff99");
		if (num == 16) return HexToColor ("00ff00");
		if (num == 32) return HexToColor ("99ff00");
		if (num == 64) return HexToColor ("bbff55");
		if (num == 128) return HexToColor ("ffff00");
		if (num == 256) return HexToColor ("ff9933");
		if (num == 512) return HexToColor ("ff6600");
		if (num == 1024) return HexToColor ("ff5050");
		if (num == 2048) return HexToColor ("ff0000");
		if (num == 4096) return HexToColor ("cc0066");
		if (num == 8192) return HexToColor ("990099");
		if (num == 16284) return HexToColor ("9999ff");
		if (num == 32568) return HexToColor ("ffffff");
		return new Color (0.5f, 0.5f, 0.5f);


	}

	Color HexToColor(string hex)
	{
		byte r = byte.Parse(hex.Substring(0,2), System.Globalization.NumberStyles.HexNumber);
		byte g = byte.Parse(hex.Substring(2,2), System.Globalization.NumberStyles.HexNumber);
		byte b = byte.Parse(hex.Substring(4,2), System.Globalization.NumberStyles.HexNumber);
		return new Color32(r,g,b, 255);
	}

	private int GetNumberAfterRotation(Vector3 direction) {
		//rotating to the left
		if(direction == Vector3.up) {
			return gameScript.getBlockNumber (2-z,y,x);
		}
		//rotating to the right
		if(direction == Vector3.down) {
			return gameScript.getBlockNumber (z,y,2-x);
		}
		//rotating up
		if(direction == Vector3.left) {
			return gameScript.getBlockNumber (x,2-z,y);
		}
		//rotating down
		if(direction == Vector3.right) {
			return gameScript.getBlockNumber (x,z,2-y);
		}
		return 1;
	}
}
