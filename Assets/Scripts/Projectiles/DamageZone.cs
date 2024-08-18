using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageZone : MonoBehaviour
{
    float damage = 5;
    // Start is called before the first frame update
    void Start()
    {
        TurnOn(false);
    }

    public void TurnOn(bool on)
    {
        GetComponent<Collider>().enabled = on;
        GetComponent<MeshRenderer>().enabled = on;
    }
    private void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.CompareTag("Damageable"))
        {
            IDamageable damageable = collision.GetComponent<IDamageable>();
            damageable.GetDamage(damage);
        }
    }
}
