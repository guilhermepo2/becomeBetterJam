using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DummyEnemy : MonoBehaviour {

	public float forwardVelocity = 3f;
	public GameObject wallVerify;
	private Rigidbody2D m_rigibody;
	private BoxCollider2D[] m_colliders;
	private bool m_isAlive;

	void Start() {
		m_rigibody = GetComponent<Rigidbody2D>();
		m_colliders = GetComponentsInChildren<BoxCollider2D>();
		m_isAlive = true;
	}

	void Update() {

		if(!m_isAlive) return;

		m_rigibody.velocity = new Vector2(Mathf.Sign(transform.localScale.x) * forwardVelocity, 0f);

		bool hasWall = Physics2D.OverlapCircle(wallVerify.transform.position, 0.1f);
		if(hasWall) {
			transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);
		}
	}


	private IEnumerator DestroyAfter(float time) {
		yield return new WaitForSeconds(time);
		Destroy(gameObject);
	}

	public void Die() {
		m_isAlive = false;

		foreach(BoxCollider2D childrenCollider in m_colliders) {
			childrenCollider.enabled = false;
		}

		m_rigibody.velocity = new Vector2(-Mathf.Sign(transform.localScale.x) * forwardVelocity, forwardVelocity * 2);
		StartCoroutine(DestroyAfter(3.0f));
	}
}
