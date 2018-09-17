using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DummyEnemy : MonoBehaviour {

	public float forwardVelocity = 3f;
	public GameObject wallVerify;
	private Rigidbody2D m_rigibody;

	void Start() {
		m_rigibody = GetComponent<Rigidbody2D>();
	}

	void Update() {
		m_rigibody.velocity = new Vector2(Mathf.Sign(transform.localScale.x) * forwardVelocity, 0f);

		bool hasWall = Physics2D.OverlapCircle(wallVerify.transform.position, 0.1f);
		if(hasWall) {
			transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);
		}
	}
}
