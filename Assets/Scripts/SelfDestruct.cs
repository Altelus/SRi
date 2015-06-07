/*******************************************************************************
SRi (working title)
Filename:   SelfDestruct.cs
Author:     Geoffrey Mok (100515125)
Date:       April 08, 2015
Purpose:    Attached to objects to destroy them by a given time
*******************************************************************************/
using UnityEngine;
using System.Collections;

public class SelfDestruct : MonoBehaviour
{
    public float selfDestructTime = 1.0f;

    void Update()
    {
        selfDestructTime -= Time.deltaTime;

        if (selfDestructTime <= 0)
        {
            // only call a network update destroy if the object is synced with the scene(not client sided)
            PhotonView pv = GetComponent<PhotonView>();
            if (pv != null && pv.instantiationId != 0) //id 0 = scene object, not owned by any specific player
            {
                PhotonNetwork.Destroy(gameObject);
            }
            else
            {
                Destroy(gameObject);
            }
        }
    }
}
