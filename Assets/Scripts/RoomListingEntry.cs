using UnityEngine;
using System.Collections;

public class RoomListingEntry : MonoBehaviour
{
    public GameObject btnJoin;
    public UILabel lblRoomName;
    public UILabel lblRoomCap;

    public float id;
    public string name;
    
	void Start () 
    {
	
	}
	
	void Update () 
    {
	
	}

    public void SetRoomInfo(float listId, string roomName, int curCap, int maxCap)
    {
        id = listId;

        lblRoomName.text = name = roomName;
        lblRoomCap.text = curCap + "/" + maxCap;
    }

    public void JoinRoom()
    {
        LobbyManager lm = FindObjectOfType<LobbyManager>();

        lm.JoinRoom(name);
        Debug.Log(id + " JOIN CALLED");
    }
}
