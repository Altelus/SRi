using UnityEngine;
using System.Collections;

public class MeshShadow : MonoBehaviour {

    public float nearHeight = -25f;
    public float farHeight = -12f;
    public float nearRange = 0.8f;
    public float farRange = 18f;

    public float mag = 0f;
    private NetworkManager networkManager;
    private GameObject player;

    private float range;
    private float heightRange;

    void Awake() 
    {
        networkManager = GameObject.FindObjectOfType<NetworkManager>();

        range = farRange - nearRange;
        heightRange = farHeight - nearHeight;
	}

    void Update()
    {
        if (player != null)
        {
            Vector3 dir = player.transform.position - transform.position;
            dir.z = 0;

            mag = dir.magnitude;
            float dist = (dir.magnitude - nearRange) / range;

            //Debug.Log(dist);

            Vector3 pos = transform.position;
            pos.z = (dist * heightRange) + nearHeight;

            if (pos.z < nearHeight)
            {
                pos.z = nearHeight;
            }
            else if (pos.z > farHeight)
            {
                pos.z = farHeight;
            }

            transform.position = pos;

            //Debug.Log(dir.magnitude);
            //Debug.Log("~~~~~~~~~~~~~~~~~");
        }
        else
        {
            if (networkManager.player != null)
                player = networkManager.player;
        }

    }
}
