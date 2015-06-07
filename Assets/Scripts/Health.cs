/*******************************************************************************
SRi (working title)
Filename:   Health.cs
Author:     Geoffrey Mok (100515125)
Date:       April 08, 2015
Purpose:    Attached to all objects that can be damaged by bullets, players & crates
*******************************************************************************/
using UnityEngine;
using System.Collections;

public class Health : Photon.MonoBehaviour {

    public float health;
    public float healthMax;

    public GameObject healthBar;
    public GameObject dropPrefab;

    public float invulnerabilityTime;
    private float invulnerabilityCooldown;

    private float healthBarXScale;
    void Awake()
    {
        health = healthMax;
        invulnerabilityCooldown = invulnerabilityTime;
    }

	void Start () 
    {
        if (healthBar)
            healthBarXScale = healthBar.transform.localScale.x;
	}
	
	void Update () 
    {
        if (healthBar)
        {
            //if (health < healthMax)
            {
                healthBar.gameObject.SetActive(true);
                Vector3 newScale = healthBar.gameObject.transform.localScale;
                newScale.x = health / healthMax * healthBarXScale;
                healthBar.gameObject.transform.localScale = newScale;
            }
            //else
            //{
            //    healthBar.gameObject.SetActive(false);
            //}
        }

	    if (invulnerabilityTime > 0)
        {
            invulnerabilityTime -= Time.deltaTime;
        }
	}

    // Remote procedure call that is sent to notify all clients when a player's bullet damages something
    [RPC]
    public void TakeDamage(float dmg)
    {
        if (invulnerabilityTime <= 0)
        {
            invulnerabilityTime = invulnerabilityCooldown;

            health -= dmg;

            if (health <= 0)
                Die();
        }

        //Debug.Log("DAMAGE TAKEN " + dmg);
    }

    // Remote procedure call that is sent to notify all clients when a player picksup a medpack
    [RPC]
    public void Heal(float heal)
    {
        health += heal;

        if (health > healthMax)
            health = healthMax;

        Debug.Log("HEALED  " + heal);
    }

    public void Die()
    {
        if (gameObject.tag == "Player")
        {
            //PhotonView pv = GetComponent<PhotonView>();

            // if this player, that current has died is me, then disable controls, and switch to scene
            // camera, also start the respawn timer

            // All other clients will do nothing, but receive the photonnetwork destroy to update their game view
            if (photonView.isMine)
            {
                NetworkManager nm = GameObject.FindObjectOfType<NetworkManager>();
                nm.respawnDelay = 4f;
                nm.levelCamera.SetActive(true);

                Vector3 deathPos = nm.levelCamera.transform.position;
                deathPos.x = transform.position.x;
                deathPos.y = transform.position.y;

                nm.levelCamera.transform.position = deathPos;

                PhotonNetwork.Destroy(gameObject);
            }
        }
        else
        {
            if (PhotonNetwork.isMasterClient)
            {
                if (dropPrefab)
                {
                    PhotonNetwork.InstantiateSceneObject(dropPrefab.name,
                    this.transform.position, Quaternion.identity, 0, null);
                }
            }
            Destroy(gameObject);
        }
    }
}
