using UnityEngine;
using System.Collections;

public class PHPServerLogin : MonoBehaviour {

    //private string username = "";
    //private string password = ""; 
    //string formText = "";

    private string URL_Login = "http://52.0.19.245/sri_login2.php";
    private string URL_Create = "http://52.0.19.245/sri_create_user.php";
    private string URL_GetScore = "http://52.0.19.245/sri_get_user_score.php";

    public string hash = "hashbrowns";

    public UIInput iptUsername;
    public UIInput iptPassword;
    public UILabel lblResult;

    void LoginPressed()
    {
        StartCoroutine(Login());
    }
    IEnumerator Login()
    {
        WWWForm form = new WWWForm(); 
        form.AddField("hash", hash);
        form.AddField("username", iptUsername.text);
        form.AddField("password", iptPassword.text);
        WWW w = new WWW(URL_Login, form);
        yield return w;
        if (w.error != null)
        {
            Debug.Log(w.error);
            lblResult.text = w.error;
        }
        else
        {
            if (w.data == "0")
            {
                lblResult.text = "Invalid Username/Password";
            }
            else if (w.data == "1")
            {
                lblResult.text = "Login Successful";
                PlayerPrefs.SetString("Username", iptUsername.text);

                StartCoroutine(GetScore());
            }
            else
            {
                lblResult.text = w.data;
            }

            w.Dispose();
        }

        iptUsername.text = ""; 
        iptPassword.text = "";
    }

    void CreatePressed()
    {
        StartCoroutine(Create());
    }
    IEnumerator Create()
    {
        WWWForm form = new WWWForm();
        form.AddField("hash", hash);
        form.AddField("username", iptUsername.text);
        form.AddField("password", iptPassword.text);
        WWW w = new WWW(URL_Create, form);
        yield return w;
        if (w.error != null)
        {
            print(w.error);
        }
        else
        {
            if (w.data == "0")
            {
                lblResult.text = "Username already taken";
            }
            else if (w.data == "1")
            {
                lblResult.text = "User Created";
            }
            else
            {
                lblResult.text = w.data;
            }

            w.Dispose();
        }

        iptUsername.text = "";
        iptPassword.text = "";
    }

    IEnumerator GetScore()
    {
        WWWForm form = new WWWForm();
        form.AddField("hash", hash);
        form.AddField("username", iptUsername.text);
        WWW w = new WWW(URL_GetScore, form);
        yield return w;
        if (w.error != null)
        {
            print(w.error);
        }
        else
        {
            if (w.data == "-1")
            {
                lblResult.text = "User does not exist";
            }
            else
            {
                PlayerPrefs.SetString("Score", w.data);
                lblResult.text = w.data;
                Debug.Log(w.data);

                Application.LoadLevel("Lobby");
            }

            w.Dispose();
        }

        iptUsername.text = "";
        iptPassword.text = "";
    }
}