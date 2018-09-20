using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour {

	public Transform target;
	private float cameraSpeed = 2.5f;
	private float moveAhead = 2f;
	public float y_offset;
	
	
	void Update () {
		if(!target.gameObject.GetComponent<PlayerMovement>().IsAlive()) return;

		transform.position = Vector3.Lerp(transform.position, new Vector3(target.position.x + (moveAhead * target.localScale.x), target.position.y + y_offset, transform.position.z), cameraSpeed * Time.deltaTime);
	}
}
