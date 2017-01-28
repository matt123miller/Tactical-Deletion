using System;
using UnityEngine;
using System.Collections;

public class Shoot : MonoBehaviour
{
    public int maximumDamage = 40, minimumDamage = 10, shotRange;
    private int scaledDamage, damage;
    private AudioSource source;
    public AudioClip shotClip;
    private Transform player;
    private bool shootingB;
    private float fractionalDistance;
    [SerializeField]
    private Transform bulletSpawn;
    private Quaternion bulletRot;
    public float bulletSpeed;

    private float shotTimer, shotReset = 1f;

    private RaycastHit hit;
    private GiveDamageToTarget giveDamage;
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
        giveDamage = new GiveDamageToTarget(10, true);
        scaledDamage = (maximumDamage - minimumDamage);
        source = GetComponent<AudioSource>();
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
        if ((shotTimer += Time.deltaTime) > shotReset)
        {
            Debug.Log("shooting");

            source.PlayOneShot(shotClip);
            shotTimer = 0;

            //1 in 5 chance
            if (UnityEngine.Random.Range(0, 5) == 0)
            {
                // is this the other way around?
                Vector3 direction = player.position - transform.position;
                hit = new RaycastHit();

                Debug.DrawRay(transform.position, direction, Color.red);
                if (Physics.Raycast(transform.position, direction, out hit, shotRange))
                {
                    fractionalDistance = (shotRange - (transform.position - player.position).magnitude) / shotRange;
                    damage = Mathf.FloorToInt((scaledDamage * fractionalDistance) + minimumDamage);
                    giveDamage.DealDamageIfApplicable(hit.collider, damage);
                }


            }

        }
    }
}



