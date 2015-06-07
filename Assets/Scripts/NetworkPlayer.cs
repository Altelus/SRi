/*******************************************************************************
SRi (working title)
Filename:   Network.cs
Author:     Geoffrey Mok (100515125)
Date:       April 08, 2015
Purpose:    Game logic for a player object, if player is self, will send updates
 *          to other clients, if not will update postion/rotation based on updates
 *          from other clients
*******************************************************************************/
using UnityEngine;
using System.Collections;

public class NetworkPlayer : Photon.MonoBehaviour {

    public GameObject sprite;
    public GameObject UIPlayerName;

    // Dead reckoning
    private Vector3 posLastReceived;
    private Vector3 posOnLastUpdate;

    private Quaternion rotLastReceived;
    private Quaternion rotOnLastUpdate;

    private float dtLastReceived;

	void Start () 
    {
        if (photonView.isMine)
        {

        }
	}
	
	void Update () 
    {
        if (!photonView.isMine)
        {
            dtLastReceived += Time.deltaTime * 10;

            // Interpolate by a fixed interval, not taking into account different latencies
            transform.position = Vector3.Lerp(posOnLastUpdate, posLastReceived, dtLastReceived);
            sprite.transform.rotation = Quaternion.Slerp(rotOnLastUpdate, rotLastReceived, dtLastReceived);
        }
	}

    // Called on network update of both send/receive
    // Only sends if player is self, will only receive otherwise
    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        // sending  data
        if(stream.isWriting)
        {
            stream.SendNext(transform.position);
            stream.SendNext(sprite.transform.rotation);
        }
        else // receiving data 
        {
            posLastReceived = (Vector3)stream.ReceiveNext();
            rotLastReceived = (Quaternion)stream.ReceiveNext();

            posOnLastUpdate = transform.position;
            rotOnLastUpdate = sprite.transform.rotation;

            dtLastReceived = 0;

        }
    }

    [RPC]
    public void SetPlayerName(string name)
    {
        UIPlayerName.GetComponent<TextMesh>().text = name;
    }
}
