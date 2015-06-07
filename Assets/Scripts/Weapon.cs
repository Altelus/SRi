using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Weapon : MonoBehaviour 
{
	public GameObject bullet;

    public string current;
	//private List<string> weapons = new List<string>();

	private int clipSize;
	public int ammo;
	private float fireRate;
	private float currentAmmoTime;
	private float reloadTime;
	private float currentReload;
	public bool isReloading;
	private float spread;
	private int shots;

    //public AudioClip revolverSound;
    //public AudioClip shotgunSound;
    //public AudioClip rifleSound;
    //private AudioClip sound;

    //public AudioClip reloadSound;
    private string weaponDefault = "Crossbow";
	
	void Awake() 
    {
		//weapons.Add ("Double Revolver");
		//weapons.Add ("Shotgun");
		//weapons.Add ("Rifle");

		fireRate = 0.5f;
		reloadTime = 2.0f;
		clipSize = 6;
		ammo = clipSize;

		//current = weapons[0];
	}

	void Start () 
    {
        SwapWeapon(weaponDefault);
	}

    void Update()
    {
        if (Input.GetKey("space") || Input.GetKey("r"))
        {
            Reload();
        }
        if (Input.GetMouseButtonUp(0))
        {
            currentAmmoTime = 0;
        }

        currentAmmoTime -= Time.deltaTime;
        currentReload -= Time.deltaTime;

        if (isReloading && currentReload < 0)
        {
            isReloading = false;
            GetComponent<AudioSource>().Stop();
        }
    }

	public void Shoot (float angle) 
    {
        if (currentAmmoTime < 0 && currentReload < 0)
        {
            if (ammo > 0)
            {
                spawnBullet(shots, angle);
            }
            else
            {
                Reload();
                return;
            }
            currentAmmoTime = fireRate;
            ammo--;
        }
	}

	public void SwapWeapon (string weaponType) 
    {
		current = weaponType;

        if (current == "Crossbow")
        {
            fireRate = 0.75f;
            reloadTime = 0.01f;
            clipSize = 1;
            spread = 5f;
            shots = 1;
        }
        //
		if (current == "Revolver") 
        {
			fireRate = 0.25f;
			reloadTime = 0.75f;
			clipSize = 6;
			spread = 0.0f;
			shots = 1;
            //sound = revolverSound;
		}
		if (current == "Double Revolver") 
        {
			fireRate = 0.25f;
			reloadTime = 0.75f;
			clipSize = 6;
			spread = 10.0f;
			shots = 2;
            //sound = revolverSound;
		}
		if (current == "Rifle") 
        {
			fireRate = 0.1f;
			reloadTime = 1.0f;
			clipSize = 25;
			spread = 10.0f;
			shots = 1;
            //sound = rifleSound;
		}
		if (current == "Shotgun") 
        {
			fireRate = 0.4f;
			reloadTime = 0.7f;
			clipSize = 2;
			spread = 15.0f;
			shots = 8;
            //sound = shotgunSound;
		}
		if (current == "Assault Shotgun") 
        {
			fireRate = 0.3f;
			reloadTime = 1.0f;
			clipSize = 12;
			spread = 10.0f;
			shots = 5;
            //sound = shotgunSound;
		}
        //if (current == "Minigun")
        //{
        //    fireRate = 0.1f;
        //    reloadTime = 2.0f;
        //    clipSize = 60;
        //    spread = 8.0f;
        //    shots = 1;
        //    //sound = rifleSound;
        //}
        if (current == "Minigun")
        {
            fireRate = 0.4f;
            reloadTime = 2.0f;
            clipSize = 50;
            spread = 8.0f;
            shots = 2;
            //sound = rifleSound;
        }
		ammo = clipSize;

		currentAmmoTime = 0;
		currentReload = 0;
	}

	void spawnBullet (int amount, float angle) 
    {
		for (int i = 0; i < shots; i++) 
        {
			angle += Random.Range(-spread, spread);
            GameObject b = PhotonNetwork.Instantiate(bullet.name, transform.position, Quaternion.AngleAxis(angle, Vector3.forward), 0);
            b.layer= LayerMask.NameToLayer("PlayerBullet");
		}
        //GetComponent<AudioSource>().PlayOneShot (sound);
	}

	void Reload () 
    {
        if (current != weaponDefault)
        {
            SwapWeapon(weaponDefault);
        }

		if (!isReloading) 
        {
			ammo = clipSize;
			currentReload = reloadTime;
			isReloading = true;
            //GetComponent<AudioSource>().PlayOneShot (reloadSound);
		}
	}


}
