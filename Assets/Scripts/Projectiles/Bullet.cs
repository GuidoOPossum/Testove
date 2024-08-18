using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    private float moveSpeed = 10.0f;
    private float damage = 1.0f;
    void FixedUpdate()
    {
        transform.Translate(Vector3.forward * moveSpeed * Time.fixedDeltaTime);
    }

    private void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.CompareTag("Damageable"))
        {

            IDamageable damageable = collision.GetComponent<IDamageable>();
            damageable.GetDamage(damage);
        }

        Destroy(gameObject);
    }
    public void SetDamage(float damage)
    {
        this.damage = damage;
        
    }
}
