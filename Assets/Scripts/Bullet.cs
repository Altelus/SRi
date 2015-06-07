/*******************************************************************************
SRi (working title)
Filename:   Bullet.cs
Author:     Geoffrey Mok (100515125)
Date:       April 08, 2015
Purpose:    Script attached to each bullet object, controlling its movement 
 *          based on initiation spawn position
*******************************************************************************/
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Bullet : Photon.MonoBehaviour {

	public float speed;
	public float damage;
    public float maxBounces;

	private Vector2 velocity;
    private float bounces;
	void Awake () 
    {
		velocity = transform.right * speed;

        // Offset bullet to spawning in the approximate location of the player's gun
        Vector3 offset = (transform.right *.50f) - (transform.up * .15f);
        transform.position += offset;
	}

	void Start () 
    {
		GetComponent<Rigidbody2D>().AddForce (velocity);
	}

    void Update()
    {
        // rotates bullet along trajectory
        Vector3 moveDirection = GetComponent<Rigidbody2D>().velocity.normalized;
        if (moveDirection != Vector3.zero)
        {
            float angle = Mathf.Atan2(moveDirection.y, moveDirection.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        }
    }

    // Triggered on collision with specific objects in collision matrix (IE excludes, player itself, and pickups)
	void OnCollisionEnter2D(Collision2D col) 
    {
        // Only player who spawned the bullet controls/reports its network collision behavior
        if (photonView.isMine)
        {
            Health colliderHealth = col.gameObject.GetComponent<Health>();
            if (colliderHealth != null)
            {
                // Network call to all clients that the bullet's collider has taken damage
                PhotonView pv = colliderHealth.GetComponent<PhotonView>();
                if (pv != null)
                {
                    pv.RPC("TakeDamage", PhotonTargets.AllViaServer, damage);
                }
                else
                {
                    Debug.LogError("No PhotonView to call TakeDamage on!");
                }

                GetComponent<TrailRenderer>().enabled = false;
            }

            bounces++;
            damage *= .5f;
            if (bounces > maxBounces || colliderHealth != null)
            {
                GetComponent<SelfDestruct>().enabled = true;

                GetComponent<BoxCollider2D>().enabled = false;
                GetComponent<Renderer>().enabled = false;
            }

        }
	}
}
