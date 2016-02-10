using System;
using UnityEngine;
using System.Collections;

public class Shoot : MonoBehaviour
{
    private int damage;
    public int maximumDamage = 40;
    public int minimumDamage = 10;
    private int scaledDamage;
    public int shotRange;
    public AudioClip shotClip;
    private Transform player;
    private bool shootingB;
    private float fractionalDistance;
    private Transform bulletSpawn;
    //private Vector3 bulletPos;
    private Quaternion  bulletRot;
    public float bulletSpeed;

    private float timer;
    private float shotReset = 1f;

    private PlayerHealth playerHealth;
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
        if (shotReset < timer)
        {
            Debug.Log("shooting");
            //fractionalDistance = ((shotRange - Vector3.Distance(transform.position, player.position)) / shotRange);
           // damage = Mathf.FloorToInt((scaledDamage * fractionalDistance) + minimumDamage);
            PlayerHealth.TakeDamage();
            var bullet = Instantiate(bulletPrefab, bulletSpawn.transform.position, bulletSpawn.rotation);
            bulletPrefab.GetComponent<Rigidbody>().useGravity = false;
            bulletPrefab.GetComponent<Rigidbody>().AddForce(transform.forward * bulletSpeed);
            GetComponent<AudioSource>().PlayOneShot(shotClip);
            //playerHealth.TakeDamage(damage);

            
            //RaycastHit rayHit;
            //if (Physics.Linecast(transform.position, player.position, out rayHit))
            //{
            //is the raycast necessary?????
            //}
            // Display the shot effects.
            //ShotEffects();    
            timer = 0;
        }
        else
        {
            timer += Time.deltaTime;
            //ShootingB = false;
        }
        
    }


}

    

