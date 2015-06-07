/*******************************************************************************
SRi (working title)
Filename:   NetworkManager.cs
Author:     Geoffrey Mok (100515125)
Date:       April 08, 2015
Purpose:    Game logic for when the level is actual loaded, controls spawning and
 *          respawning of the player
*******************************************************************************/
using UnityEngine;
using System.Collections;

public class NetworkManager : MonoBehaviour
{
    public GameObject playerPrefab;
    public GameObject levelCamera;

    public GameObject player;
    public Transform[] playerSpawnPoints;

    private const string roomName = "SRi";
    private RoomInfo[] roomsList;

    private bool isMaster = false;

    public float respawnDelay;

    public UILabel lblScore;
    public float score;

    public void Awake()
    {
        // if level is loaded before connecting to the server, go to lobby
        if (!PhotonNetwork.connected)
        {
            Application.LoadLevel("Lobby");
            return;
        }

        SpawnPlayer();
    }

    // Spawns the player and set it as controllable; by default players are setup as network entities and only move via network updates
    void SpawnPlayer()
    {
        score = float.Parse(PlayerPrefs.GetString("Score"));

        levelCamera.SetActive(false);

        player = PhotonNetwork.Instantiate(playerPrefab.name, playerSpawnPoints[Random.Range(0, playerSpawnPoints.Length)].position, Quaternion.identity, 0);
        player.GetComponent<Player>().enabled = true;
        player.GetComponent<AudioListener>().enabled = true;
        player.layer = LayerMask.NameToLayer("Player");
        player.transform.Find("Player Camera").gameObject.SetActive(true);

        player.GetComponent<PhotonView>().RPC("SetPlayerName", PhotonTargets.AllBuffered, PhotonNetwork.playerName);

        player.GetComponent<Player>().lblScore = lblScore;
    }

    void Update()
    {
        // continues to checks if client has become master client. If so, enable the logic for spawning pickups
        if (!isMaster && PhotonNetwork.isMasterClient)
        {
            isMaster = true;

            GetComponent<GameManager>().enabled = true;
        }

        // respawn timer for when player dies
		if(respawnDelay > 0) 
        {
            respawnDelay -= Time.deltaTime;

            if (respawnDelay <= 0)
            {
                SpawnPlayer();
			}
		}
    }
}