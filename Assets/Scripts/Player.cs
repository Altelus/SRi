using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour
{
    public float movementSpeed = 8f;
    public float turnSpeed = 0.1f;

    public Camera playerCamera;
    public GameObject playerSprite;
    public Weapon weapon;

    public float score = 0;
    public float gold = 0;

    public UILabel lblScore;

	void Start () 
    {
        score = float.Parse(PlayerPrefs.GetString("Score"));
	}
	
	void Update () 
    {
        if(Input.GetMouseButton(0))
        {
            // fire where player is facing
            Vector3 curAimPos = playerSprite.transform.right;
            float curAimAngle = Mathf.Atan2(curAimPos.y, curAimPos.x) * Mathf.Rad2Deg;
            weapon.Shoot(curAimAngle);
        }
        lblScore.text = "Gold: " + (score + gold);
	}

    void Attack()
    {
    }

    void FixedUpdate()
    {
        // Player Rotation
        Vector3 target = Input.mousePosition;
        target.z = Mathf.Abs(playerCamera.transform.position.z);

        Vector3 aim = playerCamera.ScreenToWorldPoint(target) - transform.position;
        float aimAngle = Mathf.Atan2(aim.y, aim.x) * Mathf.Rad2Deg;
        playerSprite.transform.rotation = Quaternion.Slerp(playerSprite.transform.rotation, Quaternion.AngleAxis(aimAngle, Vector3.forward), turnSpeed);
        //playerSprite.transform.rotation = Quaternion.AngleAxis(aimAngle, Vector3.forward);
        
        // Player Movement
        Vector3 vel = new Vector3();
        vel.x = Input.GetAxisRaw("Horizontal") * movementSpeed;
        vel.y = Input.GetAxisRaw("Vertical") * movementSpeed;
        vel *= Time.fixedDeltaTime;

        transform.Translate(vel);
    }

    //public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    //{

    //}


    private string URL_UpdateScore = "http://52.0.19.245/sri_update_user_score.php";
    public string hash = "hashbrowns";

    public void UpdateScore()
    {
        StartCoroutine(UpdateUserScore());
    }

    IEnumerator UpdateUserScore()
    {
        WWWForm form = new WWWForm();
        form.AddField("hash", hash);
        form.AddField("username", PhotonNetwork.playerName);
        form.AddField("score", (score + gold).ToString());
        WWW w = new WWW(URL_UpdateScore, form);
        yield return w;
        if (w.error != null)
        {
            print(w.error);
        }
        else
        {
            if (w.data == "0")
            {
            }
            else if (w.data == "1")
            {
            }
            else
            {
                Debug.Log(w.data);
            }

            w.Dispose();
        }
    }
}
