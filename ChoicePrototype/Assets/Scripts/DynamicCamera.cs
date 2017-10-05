using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DynamicCamera : MonoBehaviour {

	public bool IYH;
	public GameObject player1;
	public GameObject player2;
	public GameObject fusion;
	public bool fused; 
	public float camZ;
	float camZoom;
	public float zoomLowerLim; 
	public float zoomUpperLim; 
	public float leftLimit;
	public float rightLimit;
	public float upperLimit;
	public float lowerLimit;
	public Vector3 midpoint;
	public bool hasOffset;
	public Vector2 offset;


	// Use this for initialization
	void Start () {

		player1 = GameObject.Find ("Player_1");
		player2 = GameObject.Find ("Player_2");
	
	}
	
	// Update is called once per frame
	void Update () {
//		fusion = GameObject.Find ("FusionPlayer");
		if (fused == false) {
			Camera.main.orthographicSize = camZoom;
			midpoint = ((player1.transform.position - player2.transform.position) * 0.5f) + player2.transform.position;
			midpoint.z = camZ;
			Camera.main.transform.position = midpoint;
			camZoom = Vector3.Distance (player1.transform.position, player2.transform.position);

			if (camZoom > zoomUpperLim) {
				camZoom = zoomUpperLim;
			}

			if (camZoom < zoomLowerLim) {
				camZoom = zoomLowerLim;
			}

			if (midpoint.x > rightLimit) {
				midpoint.x = rightLimit;
			}

			if (midpoint.x < leftLimit) {
				midpoint.x = leftLimit;
			}

			if (midpoint.y > upperLimit) {
				midpoint.y = upperLimit;
			}

			if (midpoint.y < lowerLimit) {
				midpoint.y = lowerLimit;
			}
		} else 
		{
			if (hasOffset) {
				Camera.main.transform.position = new Vector3 (fusion.transform.position.x + offset.x, fusion.transform.position.y + offset.y, camZ);
			} else {
				Camera.main.transform.position = new Vector3 (fusion.transform.position.x, fusion.transform.position.y, camZ);
			}
		}
	}
}
