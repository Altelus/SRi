using UnityEngine;
using System.Collections;

public class ItemDrop : Photon.MonoBehaviour
{

    public float gold = 100;

    void Start()
    {

    }

    void Update()
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
            GetComponent<BoxCollider2D>().enabled = false;

            PhotonView pv = col.gameObject.GetComponent<PhotonView>();

            if (pv != null)
            {
                col.gameObject.GetComponent<Player>().gold += gold;
                col.gameObject.GetComponent<Player>().UpdateScore();

                photonView.RPC("DestroyItemDrop", PhotonTargets.MasterClient);
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
    void DestroyItemDrop()
    {
        PhotonNetwork.Destroy(gameObject);
    }
}
