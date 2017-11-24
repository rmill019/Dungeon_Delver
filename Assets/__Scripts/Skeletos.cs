using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skeletos : Enemy {

	[Header("Set in Inspector: Skeletos")]
	public int				speed = 2;
	public float			timeThinkMin = 1f;
	public float			timeThinkMax = 4f;

	[Header("Set Dynamically: Skeletos")]
	public int 				facing = 0;
	public float			timeNextDecision = 0;

	void Update () {
		
		if (Time.time >= timeNextDecision)
		{
			DecideDirection ();
		}
		// rigid is inherited from Enemy(Base Class) and is Initialized in Enemy.Awake()
		// directions is a protected member of Enemy (Base) and is visible to Skeletos since it is a derived class
		rigid.velocity = directions[facing] * speed;
	}	

	void DecideDirection ()
	{
		facing = Random.Range (0, 4);
		timeNextDecision = Time.time + Random.Range (timeThinkMin, timeThinkMax);
	}
}
