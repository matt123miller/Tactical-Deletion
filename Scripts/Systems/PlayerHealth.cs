using UnityEngine;
using System.Collections;

public class PlayerHealth : MonoBehaviour
{


    public static int health = 100;
    private bool isDead;
    private Collider playerCollider;
    //public GameObject self;

    public int Health
    {
        get { return health; }
        set { health = value; }
    }


    

    // Use this for initialization
	void Awake ()
	{
	    isDead = false;
	    playerCollider = gameObject.GetComponent<CapsuleCollider>();
	}
	
	// Update is called once per frame
	void Update ()
	{
	    if (Health <= 0)
	        PlayerDead();
	}

    public static void TakeDamage() //int damage
    {
        health -= 20;
        Debug.Log("Ouch! I have " + health + " health remaining");
        //damage;
        //maybe play a damaged animation
    }

    private void PlayerDead()
    {
        isDead = true;
        Debug.Log("dead");
        //play death animations?
        //show game over screen
        //reset game
    }

    private void OnTriggerEnter(Collider col)
    {
        if (col.tag == "Bullet")
        {
            TakeDamage();
        }
    }
}
