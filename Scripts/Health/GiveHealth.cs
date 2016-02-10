using UnityEngine;

public class GiveHealth : MonoBehaviour
{
    public GameObject Effect;
    public int HealthToGive;

    public void OnTriggerEnter2D(Collider other)
    {
        if (other.CompareTag("Player"))
        {

            var playerHealth = other.GetComponent<Health>();

            playerHealth.GiveHealth(HealthToGive, gameObject);
            Instantiate(Effect, transform.position, transform.rotation);

            gameObject.SetActive(false);

        }
    }


}