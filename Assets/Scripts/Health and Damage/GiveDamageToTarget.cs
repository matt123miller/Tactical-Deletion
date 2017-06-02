using UnityEngine;

public class GiveDamageToTarget : MonoBehaviour
{
    [SerializeField]
    private int damageToDeal = 10;
    [SerializeField]
    private bool damageAll = false;


    public GiveDamageToTarget(int damageToGive, bool damageToAll)
    {
        this.damageToDeal = damageToDeal;
        this.damageAll = damageToAll;
    }

    

    public void OnTriggerEnter(Collider other)
    {
        //seperated damage targets in case it's later useful;
        DealDamageIfApplicable(other, damageToDeal);

        GiveDamageBehaviour();
    }

    public void DealDamageIfApplicable(Collider other, int damageToGive)
    {
        if (other.CompareTag("Player"))
        {
            other.GetComponent<Health>().TakeDamage(damageToGive, gameObject);

        }
        else if (other.CompareTag("Enemy") && damageAll)
        {
            other.GetComponent<Health>().TakeDamage(damageToGive, gameObject);
        }
        else // if neither of the above are true then the method exits before running the rest of the code.
        {
            return;
        }
    } 
    public virtual void GiveDamageBehaviour()
    {
        // Currently nothing.

    }
}

