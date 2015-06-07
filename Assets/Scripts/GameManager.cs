/*******************************************************************************
SRi (working title)
Filename:   GameManager.cs
Author:     Geoffrey Mok (100515125)
Date:       April 08, 2015
Purpose:    Game logic script controlling the spawning of power ups
 *          Only the player assigned as master client will run this script
 *          If master client disconnects, a new master client will run this script
*******************************************************************************/
using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour 
{
    public GameObject[] pickups;
    public Transform[] pickupSpawnPoints;

    public float gameTime;
    public float pickupSpawnDelay = 6.0f;

    void Start() 
    {
	}
	
    // spawn powerups at random points every x seconds 
	void Update () 
    {
        gameTime += Time.deltaTime;

        if (gameTime >= pickupSpawnDelay)
        {
            gameTime = 0;

            // Instantiate a scene object, belonging to the game world. Will not be destroyed if the 
            // master client disconnects.
            PhotonNetwork.InstantiateSceneObject(pickups[Random.Range(0, pickups.Length)].name,
                pickupSpawnPoints[Random.Range(0, pickupSpawnPoints.Length)].position,
                Quaternion.identity, 0, null);
            ;
        }
	}
}
