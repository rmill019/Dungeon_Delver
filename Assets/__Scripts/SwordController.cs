using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordController : MonoBehaviour {

	private GameObject				sword;
	private Dray					dray;

	// Use this for initialization
	void Start () {
		sword = transform.GetChild (0).gameObject;
		dray = transform.parent.GetComponent<Dray> ();

		// Deactivate the sword
		sword.SetActive (false);
	}
	
	// Update is called once per frame
	void Update () {
		// If Dray is facing to the left we rotate the sword 180 degrees on the x-axis to make sure it isn't displayed upside down
		if (dray.facing == 2)
		{
			transform.rotation = Quaternion.Euler (180, 0, 90 * dray.facing);
		} 
		// ...else if facing right, up, or down then only rotate around the z-axis to make sure the sword is facing the same direction as Dray
		else
		{
			transform.rotation = Quaternion.Euler (0, 0, 90 * dray.facing);
		}
		// Make the sword visible only when dray is attacking
		sword.SetActive (dray.mode == Dray.eMode.attack);
	}
}
