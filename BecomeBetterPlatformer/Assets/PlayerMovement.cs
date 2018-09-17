using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour {

	[Header("Move Control")]
	public float maxPlayerVelocity = 5f;
	public float horizontalDamping = 0.15f;

	[Header("Jump Control")]
	public float jumpForce = 20f;
	public float jumpPressedRememberTime = 0.15f;
	public float groundedRememberTime = 0.15f;
	public float cutJumpHeight = 0.35f;
	private float m_jumpPressedRemember;
	private float m_groundedRemember;

	private bool m_isAlive;
	private Rigidbody2D m_rigidbody;
	private BoxCollider2D m_feetCollider;

	void Start () {
		m_rigidbody = GetComponent<Rigidbody2D>();
		m_feetCollider = GetComponent<BoxCollider2D>();
		m_isAlive = true;	
	}
	
	void FixedUpdate () {
		if(!m_isAlive) return;

		Run();
		Jump();
	}

	private void Run() {
		float movement = m_rigidbody.velocity.x;
		movement += Input.GetAxisRaw("Horizontal");
		movement *= Mathf.Pow(1f - horizontalDamping, Time.deltaTime * 10f);

		movement = Mathf.Clamp(movement, -maxPlayerVelocity, maxPlayerVelocity);
		m_rigidbody.velocity = new Vector2(movement, m_rigidbody.velocity.y);
	}

	private void Jump() {

		m_groundedRemember -= Time.fixedDeltaTime;
		m_jumpPressedRemember -= Time.fixedDeltaTime;

		if(m_feetCollider.IsTouchingLayers(LayerMask.GetMask("Ground"))) {
			m_groundedRemember = groundedRememberTime;
		}

		if(Input.GetButtonDown("Jump")) {
			m_jumpPressedRemember = jumpPressedRememberTime;
		}

		if(Input.GetButtonUp("Jump")) {
			if(m_rigidbody.velocity.y > 0) {
				m_rigidbody.velocity = new Vector2(m_rigidbody.velocity.x, m_rigidbody.velocity.y * cutJumpHeight);
			}
		}

		if((m_jumpPressedRemember > 0) && ( m_groundedRemember > 0)) {
			m_jumpPressedRemember = 0;
			m_groundedRemember = 0;

			m_rigidbody.velocity = new Vector2(m_rigidbody.velocity.x, jumpForce);
		}

	}
}
