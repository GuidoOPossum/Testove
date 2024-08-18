using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

// насправді краще було б зробити просто сигнал атаки і
public class Enemy : MonoBehaviour, IDamageable
{
    public float walkingTime = 3.0f;
    public float idleTime = 3.0f;
    
    public float fireRate = 0.5f;
    public Bullet bulletPref;
    public float damage = 1.0f;
    [SerializeField] private float maxHeath = 10.0f;
    [SerializeField] private float heath = 10.0f;
    private int bounty = 1;
    [SerializeField] private Coin coinPref;
    [SerializeField] private float rotationSpeed = 5.0f;
    private Rigidbody rb;
    
    public State currentState = State.IDLE;
    public bool canShoot = true;

    public void GetDamage(float damage)
    {
        heath -= damage;
        if (heath <= 0.0f)
        {
            Die();
        }
    }

    private void Die()
    {
        Coin coin = Instantiate(coinPref);
        coin.transform.position = transform.position;
        coin.SetBounty(bounty);
        Destroy(gameObject);
    }
    public float RotatePlayer()
    {
        Vector3 enemyDir = Player.instanse.transform.position - transform.position;
        Quaternion toRotation = Quaternion.LookRotation(enemyDir, Vector3.up);
        transform.rotation = Quaternion.Lerp(transform.rotation, toRotation, rotationSpeed * Time.deltaTime);
        float angleDifference = Quaternion.Angle(transform.rotation, toRotation);
        return angleDifference;
    }
}
