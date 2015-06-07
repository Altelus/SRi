using UnityEngine;
using System.Collections;

public class PHPDeleteUser : MonoBehaviour {

    private string URL_Delete = "http://52.0.19.245/sri_delete_user.php";

    public string hash = "hashbrowns";

    void DeletePressed()
    {
        StartCoroutine(Delete());
    }
    IEnumerator Delete()
    {
        WWWForm form = new WWWForm();
        form.AddField("hash", hash);
        form.AddField("username", PlayerPrefs.GetString("Username"));
        //form.AddField("password", iptPassword.text);
        WWW w = new WWW(URL_Delete, form);
        yield return w;
        if (w.error != null)
        {
            Debug.Log(w.error);
        }
        else
        {
            if (w.data == "0")
            {
                Debug.Log("Delete Failed");
            }
            else if (w.data == "1")
            {
                Application.LoadLevel("Login");
            }
            else
            {
                Debug.Log(w.data);
            }

            w.Dispose();
        }
    }
}
