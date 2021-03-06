﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerMovement : MonoBehaviour {

	[Header("Audio Related")]
	public AudioClip jumpClip;
	public AudioClip hitClip;
	public AudioClip deathHitClip;
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
	private Animator m_animator;

	void Start () {
		m_rigidbody = GetComponent<Rigidbody2D>();
		m_feetCollider = GetComponent<BoxCollider2D>();
		m_animator = GetComponent<Animator>();
		m_isAlive = true;
		
		Time.timeScale = 1.0f;	
	}
	
	void FixedUpdate () {
		if(!m_isAlive) return;

		Run();
		Jump();
		FlipSprite();
		AnimationLogic();
	}

	private void Run() {
		float movement = m_rigidbody.velocity.x;
		movement += Input.GetAxisRaw("Horizontal");
		movement *= Mathf.Pow(1f - horizontalDamping, Time.deltaTime * 10f);

		movement = Mathf.Clamp(movement, -maxPlayerVelocity, maxPlayerVelocity);
		m_rigidbody.velocity = new Vector2(movement, m_rigidbody.velocity.y);
	}

	void FlipSprite() {
		if(Mathf.Abs(m_rigidbody.velocity.x) > 0) {
			transform.localScale = new Vector3(Mathf.Sign(m_rigidbody.velocity.x), transform.localScale.y, transform.localScale.z);
		}
	}

	void AnimationLogic() {

		if(Mathf.Abs(m_rigidbody.velocity.y) > 0) {
			m_animator.Play("Jumping");
		} else if(Mathf.Abs(m_rigidbody.velocity.x) > 0.1f) {
			m_animator.Play("Running");
		} else {
			m_animator.Play("Idle");
		}
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
			SoundManager.instance.PlayClip(jumpClip);
		}

	}

	private IEnumerator RestartLevel() {
		SoundManager.instance.PlayClip(deathHitClip);
		SoundManager.instance.PlayGameOverMusic();

		yield return new WaitForSeconds(2.0f);
		
		SoundManager.instance.PlayRegularMusic();
		SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
	}

	void PlayerDead() {
		m_rigidbody.velocity = new Vector2(-Mathf.Sign(transform.localScale.x) * jumpForce / 4, jumpForce / 2);
		m_isAlive = false;
		m_feetCollider.enabled = false;
		StartCoroutine(RestartLevel());
	}

	void OnCollisionEnter2D(Collision2D other) {
		if(other.gameObject.tag == "Enemy") {
			PlayerDead();
		}
	}

	void OnTriggerEnter2D(Collider2D other) {
		Debug.Log("Collider");
		Debug.Log(other.tag);

		if(other.tag == "Enemy") {
			m_rigidbody.velocity = new Vector2(m_rigidbody.velocity.x, jumpForce / 2);
			other.gameObject.GetComponentInParent<DummyEnemy>().Die();
			SoundManager.instance.PlayClip(hitClip);
		} else if(other.tag == "Bounds") {
			StartCoroutine(RestartLevel());
		} else if(other.tag == "Hazard") {
			PlayerDead();
		}
	}

	public bool IsAlive() { return m_isAlive; }
}
