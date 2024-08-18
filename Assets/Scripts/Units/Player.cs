using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour, IDamageable
{
    [Header("Links")]
    [SerializeField] private Transform firePoint;
    [SerializeField] private Bullet bulletPref;
    [SerializeField] private Joystick joystick;
    private Transform enemyTarget = null;

    [Header("Stats")]
    [SerializeField] private float moveSpeed = 3.0f;
    [SerializeField] private float rotationSpeed = 5.0f;
    [SerializeField] private float hp = 10.0f;
    [SerializeField] private float damage = 3.0f;
    [SerializeField] private float fireRate = 0.5f;
    [SerializeField] private float gamepadDrift = 0.2f;

    private bool canShoot = true;
    private State currentState = State.IDLE;
    private Vector3 moveDirection = Vector3.zero;
    private Rigidbody rb;

    // singleton
    private static Player _instanse;
    public static Player instanse
    {
        get
        {
            return _instanse;
        }
    }
    void Awake()
    {
        if (_instanse == null)
        {
            _instanse = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        enemyTarget = LevelManager.instanse.ReturnClosestEnemy();
        float moveX = Input.GetAxis("Horizontal");
        float moveZ = Input.GetAxis("Vertical");

        float moveXjoy = joystick.Horizontal;
        float moveZjoy = joystick.Vertical;

        if (Mathf.Abs(moveXjoy) + Mathf.Abs(moveZjoy) < gamepadDrift)
        {
            moveXjoy = 0.0f;
            moveZjoy = 0.0f;
        }

        moveDirection = new Vector3(moveX + moveXjoy, 0f, moveZ + moveZjoy).normalized;

        if (moveDirection != Vector3.zero)
        {
            currentState = State.WALK;
        }

        switch (currentState)
        {
            case State.IDLE:
                break;
            case State.WALK:
                if (moveDirection != Vector3.zero)
                {
                    RotateToDir(moveDirection);
                }
                else
                {
                    if (enemyTarget == null)
                    {
                        currentState = State.IDLE;
                    }
                    else
                    {
                        currentState = State.TURN_ENEMY;
                    }
                }
                break;
            case State.TURN_ENEMY:

                if (enemyTarget == null)
                {
                    currentState = State.IDLE;
                }
                bool angleDifference = RotateEnemy() < 5f;
                if (angleDifference)
                {
                    currentState = State.SHOOT;
                }
                break;
            case State.SHOOT:
                if (enemyTarget == null)
                {
                    currentState = State.IDLE;
                }
                bool rAngleDifference = RotateEnemy() > 10f;
                if (rAngleDifference)
                {
                    currentState = State.TURN_ENEMY;
                }
                if (canShoot)
                {
                    Shoot();
                }
                break;
        }

    }

    void RotateToDir(Vector3 dir)
    {
        Quaternion toRotation = Quaternion.LookRotation(dir, Vector3.up);
        transform.rotation = Quaternion.Lerp(transform.rotation, toRotation, rotationSpeed * Time.deltaTime);
    }

    float RotateEnemy()
    {
        Vector3 enemyDir = enemyTarget.position - transform.position;
        Quaternion toRotation = Quaternion.LookRotation(enemyDir);

        transform.rotation = Quaternion.Lerp(transform.rotation, toRotation, rotationSpeed * Time.deltaTime);
        float angleDifference = Quaternion.Angle(transform.rotation, toRotation);
        return angleDifference;
    }

    IEnumerator Reload()
    {
        yield return new WaitForSeconds(fireRate);
        canShoot = true;
    }

    void Shoot()
    {
        Vector3 enemyDir = enemyTarget.position - transform.position;
        Quaternion toRotation = Quaternion.LookRotation(enemyDir, Vector3.up);
        Bullet bullet = Instantiate(bulletPref, firePoint.position, toRotation);
        bullet.SetDamage(damage);
        canShoot = false;
        StartCoroutine(Reload());
    }

    void FixedUpdate()
    {
        rb.MovePosition(rb.position + moveDirection * moveSpeed * Time.fixedDeltaTime);
    }

    public void GetDamage(float damage)
    {
        hp -= damage;
        if (hp <= 0.0f)
        {
            Die();
        }
        else
        {
            print($"Player hp - {hp}");
        }
    }
    void Die()
    {
        print("Player Died");
        GameManager.instanse.ReloadLVL();
    }
    // тут можуть бути трабли але мені не подобається коли гравець на паузі
    public void StartFight()
    {
        canShoot = true;
    }

}

public enum State
{
    WALK,
    IDLE,
    SHOOT,
    TURN_ENEMY
}
