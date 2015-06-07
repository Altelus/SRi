/*******************************************************************************
SRi (working title)
Filename:   LobbyManager.cs
Author:     Geoffrey Mok (100515125)
Date:       April 08, 2015
Purpose:    Responsible for the first screen, the lobby. Contains primarily network connection 
 *          logic, connecting to the server, creating and joining rooms, assign player name
*******************************************************************************/
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class LobbyManager : MonoBehaviour 
{
    // NGUI components to display the list of rooms
    public GameObject NGUI_Parent;
    public GameObject roomListingPrefag;

    public float listingOffset = 63f;
    public float listingScale = .5f;

    public UILabel lblRooms;
    public UILabel lblPlayers;
    public UILabel lblPlayerName;
    public UILabel lblCreateRoomName;

    public UILabel lblUsername;
    public UILabel lblScore;

    private List<GameObject> roomListings = new List<GameObject>();

    private RoomInfo[] roomsList;
    private bool roomListChanged = false;

    void Awake()
    {
        PhotonNetwork.automaticallySyncScene = true;

        if (PhotonNetwork.connectionStateDetailed == PeerState.PeerCreated)
        {
            PhotonNetwork.ConnectUsingSettings("0.5");
        }

        if (PlayerPrefs.GetString("Username") != null)
        {
            lblPlayerName.text = PlayerPrefs.GetString("Username");
        }
        else
        {
            lblPlayerName.text = "Guest" + Random.Range(0, 9999);
        }
        lblUsername.text = lblPlayerName.text;

        if (PlayerPrefs.GetString("Score") != null)
        {
            lblScore.text = "Score: " + PlayerPrefs.GetString("Score");
        }

        lblCreateRoomName.text = "Room" + Random.Range(0, 9999);
    }
	void Start () 
    {
	}

    void OnGUI()
    {
        // Display connection status
        if (!PhotonNetwork.connected)
        {
            GUILayout.Label(PhotonNetwork.connectionStateDetailed.ToString());
        }
    }
	
    // Retrieves list of rooms and updates the UI when a change occurs
	void Update () 
    {
	    if (PhotonNetwork.connected)
        {
            if (roomListChanged && PhotonNetwork.GetRoomList().Length != roomListings.Count)
            {
                roomsList = PhotonNetwork.GetRoomList();

                DestroyRoomListing();
                CreateRoomListing();
            }
        }
	}

    // Called when a list of rooms is received from server
    void OnReceivedRoomListUpdate()
    {
        Debug.Log("OnReceivedRoomListUpdate");

        roomListChanged = true;

        roomsList = PhotonNetwork.GetRoomList();
    }

    public void OnJoinedRoom()
    {
        Debug.Log("OnJoinedRoom");
    }

    public void OnPhotonCreateRoomFailed()
    {
        Debug.Log("OnPhotonCreateRoomFailed");
    }

    public void OnPhotonJoinRoomFailed()
    {
        Debug.Log("OnPhotonJoinRoomFailed");
    }
    public void OnPhotonRandomJoinFailed()
    {
        Debug.Log("OnPhotonJoinRoomFailed");
    }

    // When player creates a room, load the game 
    public void OnCreatedRoom()
    {
        Debug.Log("OnCreatedRoom");
        PhotonNetwork.LoadLevel("SRI");
    }

    public void OnDisconnectedFromPhoton()
    {
        Debug.Log("Disconnected from Photon.");
    }

    public void OnFailedToConnectToPhoton(object parameters)
    {
        Debug.Log("OnFailedToConnectToPhoton. StatusCode: " + parameters + " ServerAddress: " + PhotonNetwork.networkingPeer.ServerAddress);
    }

    // Iteratively creates a line and button for each room 
    public void CreateRoomListing()
    {
        for (int i = 0; i < roomsList.Length; i++)
        {
            GameObject roomListing = (GameObject)Instantiate(roomListingPrefag);

            roomListing.transform.parent = NGUI_Parent.transform;
            roomListing.transform.localScale = new Vector3(listingScale, listingScale, listingScale);
            roomListing.transform.localPosition = new Vector3(0, -listingOffset * i, 0);

            roomListing.GetComponent<RoomListingEntry>().SetRoomInfo(i, roomsList[i].name, roomsList[i].playerCount, roomsList[i].maxPlayers);

            roomListings.Add(roomListing);
        }

        lblRooms.text = PhotonNetwork.countOfRooms + " Room(s)";
        lblPlayers.text = PhotonNetwork.countOfPlayers + " User(s) Online";
    }

    public void DestroyRoomListing()
    {
        for (int i = 0; i < roomListings.Count; i++ )
        {
            Destroy(roomListings[i]);
        }
        roomListings.Clear();

        //GameObject temp = roomListings[i];
    }

    void SetPlayerName()
    {
        if (lblPlayerName.text.Length > 0)
            PhotonNetwork.playerName = lblPlayerName.text;
        else
            PhotonNetwork.playerName = "Guest" + Random.Range(0, 9999);
    }

    // creates a room, if room name already exist, create a random name
    public void CreateRoom()
    {
        if (lblCreateRoomName.text.Length > 1)
        {
            PhotonNetwork.CreateRoom(lblCreateRoomName.text, new RoomOptions() { maxPlayers = 4 }, null);
        }
        else
            PhotonNetwork.CreateRoom("Room" + Random.Range(0, 9999), new RoomOptions() { maxPlayers = 4 }, null);

        SetPlayerName();
    }

    public void JoinRoom(string name)
    {
        SetPlayerName();

        Debug.Log("Joining... " + name);

        PhotonNetwork.JoinRoom(name);

    }
}
