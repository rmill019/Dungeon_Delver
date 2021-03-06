﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dray : MonoBehaviour {

	public enum eMode { idle, move, attack, transition }
	
	[Header("Set in Inspector")]
	public float				speed = 5;
	public float				attackDuration = 0.25f;		// Number of seconds to attack
	public float				attackDelay = 0.5f;			// Delay between attacks

	[Header("Set Dynamically")]
	public int					dirHeld = -1;	// Direction of the held movement key
	public int					facing = 1;
	public eMode				mode = eMode.idle;
	private float				timeAtkDone = 0f;
	private float				timeAtknext = 0f;

	private Rigidbody			rigid;
	private Animator			anim;
	private InRoom 				inRm;
	private Vector3[] 			directions = new Vector3[] { Vector3.right, Vector3.up, Vector3.left, Vector3.down };
	private KeyCode[]			keys = new KeyCode[] { KeyCode.RightArrow, KeyCode.UpArrow, KeyCode.LeftArrow, KeyCode.DownArrow };

	void Awake () {
		
		rigid = GetComponent<Rigidbody> ();
		anim = GetComponent<Animator> ();
		inRm = GetComponent<InRoom> ();
	}

	// Update is called once per frame
	void Update () {

		// Handle Keyboard Input and manage eDrayModes
		dirHeld = -1;
		for (int i = 0; i < 4; i++)
		{
			if (Input.GetKey (keys [i]))
			{
				dirHeld = i;
			}
		}

		// Pressing the attack button(s)
		if (Input.GetKeyDown (KeyCode.Z) && Time.time >= timeAtknext)
		{
			mode = eMode.attack;
			timeAtkDone = Time.time + attackDuration;
			timeAtknext = Time.time + attackDelay;
		}

		// Finishing the attack when it's over
		if (Time.time >= timeAtkDone)
		{
			mode = eMode.idle;
		}

		// Choosing the proper mode if we're not attacking
		if (mode != eMode.attack)
		{
			if (dirHeld == -1)
			{
				mode = eMode.idle;
			}
			else
			{
				facing = dirHeld;
				mode = eMode.move;
			}
		}

		// Act on the current mode
		Vector3 vel = Vector3.zero;
		switch (mode)
		{
			case eMode.attack:
				anim.CrossFade ("Dray_Attack_" + facing, 0);
				anim.speed = 0;
				break;
			case eMode.idle:
				anim.CrossFade ("Dray_Walk_" + facing, 0);
				anim.speed = 0;
				break;
		case eMode.move:
				vel = directions [dirHeld];
				anim.CrossFade ("Dray_Walk_" + facing, 0);
				print ("Facing: " + facing);
				anim.speed = 1;
				break;
		}
		rigid.velocity = vel * speed;
	}


	// Implementation of IFacingMover
	public int GetFacing () 
	{
		return facing;
	}

	public bool moving 
	{
		get { return (mode == eMode.move); }	
	}

	public float GetSpeed ()
	{
		return speed;
	}

	public float gridMult
	{
		get { return inRm.gridMult; }
	}

	public Vector2 roomPos
	{
		get { return inRm.roomPos; }
		set { inRm.roomPos = value; }
	}

	public Vector2 roomNum
	{
		get { return inRm.roomNum; }
		set { inRm.roomNum = value; }
	}

	public Vector2 GetRoomPosOnGrid (float mult = -1)
	{
		return inRm.GetRoomPosOnGrid (mult);
	}


	// Play appropriate animation
	void PlayAnimation ()
	{
		// Animation
		if (dirHeld == -1)
		{
			anim.speed = 0;
		} 
		else
		{
			anim.CrossFade ("Dray_Walk_" + dirHeld, 0);
			anim.speed = 1;
		}
	}

	// One of the books movement implementations
	void DrayMovement ()
	{
		dirHeld = -1;
		if (Input.GetKey (KeyCode.RightArrow))
			dirHeld = 0;
		if (Input.GetKey (KeyCode.UpArrow))
			dirHeld = 1;
		if (Input.GetKey (KeyCode.LeftArrow))
			dirHeld = 2;
		if (Input.GetKey (KeyCode.DownArrow))
			dirHeld = 3;

		Vector3 vel = Vector3.zero;
		if (dirHeld > -1)
		{
			vel = directions [dirHeld];
		}

		rigid.velocity = vel * speed;

	}

	// My own custom move function that allows for diagonal movement not set up to work with other functionality yet
	void Movement ()
	{
		// xMove and yMove will always have a value between -1 and 1
		float xMove = Input.GetAxis ("Horizontal");
		float yMove = Input.GetAxis ("Vertical");

		// This will allow for diagonal movement
		rigid.velocity = new Vector3 (xMove, yMove, 0) * speed;
	}
}
