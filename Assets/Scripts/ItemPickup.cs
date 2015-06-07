/*******************************************************************************
SRi (working title)
Filename:   ItemPickup.cs
Author:     Geoffrey Mok (100515125)
Date:       April 08, 2015
Purpose:    Game logic of each pickup, can be a weapon or a medpack depending on 
 *          heal amount/dmg amount/weapon type variables.
*******************************************************************************/
using UnityEngine;
using System.Collections;

public class ItemPickup : Photon.MonoBehaviour {

    public float healAmount;
    public float dmgAmount;
    public string weaponType;

	void Start () 
    {
	
	}
	
	void Update () 
    {
	
	}

    // Player colliding with the pickup will send RPC to others to notify changes in its health,
    // Destruction of the pickup is sent to the master client to handle. As master client
    // was responsible for spawning the scene objects, they will also be responsible for
    // destroying them.
    void OnTriggerStay2D(Collider2D col)
    {
        if (col.gameObject.tag == "Player")
        {
            Health h = col.gameObject.GetComponent<Health>();
            Weapon w = col.gameObject.GetComponent<Weapon>();
            PhotonView pv = col.gameObject.GetComponent<PhotonView>();

            if (pv != null)
            {
                if (healAmount > 0 && h.health < h.healthMax)
                {
                    pv.RPC("Heal", PhotonTargets.AllViaServer, healAmount);
                    photonView.RPC("DestroyItemPickup", PhotonTargets.MasterClient);
                }
                else if (dmgAmount > 0)
                {
                    pv.RPC("TakeDamage", PhotonTargets.AllViaServer, dmgAmount);
                    photonView.RPC("DestroyItemPickup", PhotonTargets.MasterClient);
                }
                else if (weaponType.Length > 0 && !w.current.Equals(weaponType))
                {
                    w.SwapWeapon(weaponType);
                    photonView.RPC("DestroyItemPickup", PhotonTargets.MasterClient);
                }
            }
            else
            {
                Debug.LogError("No PhotonView to call on!");
            }
        }
    }

    // Only the master client will receive this call, they will be the one to destroy the pickup
    // this prevents multiple players from simultaneously picking up the same object.
    [RPC]
    void DestroyItemPickup()
    {
        PhotonNetwork.Destroy(gameObject);
    }
}
