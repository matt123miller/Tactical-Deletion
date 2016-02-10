using System;
using UnityEngine;
using System.Collections;

public class Shoot : MonoBehaviour
{
    public int maximumDamage = 40, minimumDamage = 10, shotRange;
    private int scaledDamage, damage;
    public AudioClip shotClip;
    private Transform player;
    private bool shootingB;
    private float fractionalDistance;
    private Transform bulletSpawn;
    private Quaternion  bulletRot;
    public float bulletSpeed;

    private float shotTimer, shotReset = 1f;

    private Health playerHealth;
    public GameObject bulletPrefab;

    public bool ShootingB
    {
        get { return shootingB; }
        set { shootingB = value; }
    }

    // Use this for initialization
    private void Start()
    {

        player = GameObject.FindWithTag("Player").transform;
        //playerHealth = player.gameObject.GetComponent<PlayerHealth>();
        scaledDamage = (maximumDamage - minimumDamage);
        bulletSpawn = transform.FindChild("Bullet");
        //bulletPos = bulletSpawn.transform.position;
        //bulletRot = bulletSpawn.rotation;
    }

    // Update is called once per frame
    private void Update()
    {
        //possible need for 
        //if(shootingB == true)
        //{
        //    somehow affect the animation?
        //}

    }

    public void Shooting()
    {
        if (shotTimer > shotReset)
        {
            Debug.Log("shooting");
            //fractionalDistance = ((shotRange - Vector3.Distance(transform.position, player.position)) / shotRange);
           // damage = Mathf.FloorToInt((scaledDamage * fractionalDistance) + minimumDamage);
            var bullet = Instantiate(bulletPrefab, bulletSpawn.transform.position, bulletSpawn.rotation);
            bulletPrefab.GetComponent<Rigidbody>().AddForce(transform.forward * bulletSpeed);
            // Following line must be changed
            GetComponent<AudioSource>().PlayOneShot(shotClip);

            
            shotTimer = 0;
        }
        else
        {
            shotTimer += Time.deltaTime;
            //ShootingB = false;
        }
        
    }


}

    

