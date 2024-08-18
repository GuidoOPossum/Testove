using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class WalkingEnemy : Enemy
{

    [SerializeField] private Transform firePoint;
    NavMeshAgent agent;
    private Coroutine changeState;
    private State _currentState
    {
        set
        {
            if (currentState == value)
            {
                return;
            }

            currentState = value;
            switch (currentState)
            {
                case State.TURN_ENEMY:
                    agent.destination = transform.position;
                    changeState = StartCoroutine(SetStateWithDelay(idleTime, State.WALK));
                    break;
                case State.WALK:
                    agent.destination = GetRandomPos();
                    changeState = StartCoroutine(SetStateWithDelay(walkingTime, State.TURN_ENEMY));
                    break;
            }

        }
    }
    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.destination = transform.position;
        StartCoroutine(SetStateWithDelay(Random.Range(0.0f, idleTime / 2), State.WALK));
    }

    Vector3 GetRandomPos()
    {
        float randomX = Random.Range(-LevelManager.instanse.fieldSize.x / 2, LevelManager.instanse.fieldSize.x / 2);
        float randomZ = Random.Range(-LevelManager.instanse.fieldSize.y / 2, LevelManager.instanse.fieldSize.y / 2);
        return new Vector3(randomX, 0.0f, randomZ);
    }

    // Update is called once per frame
    void Update()
    {
        switch (currentState)
        {
            case State.IDLE:

                break;
            case State.WALK:
                if (Vector3.Distance(agent.destination, transform.position) < 0.1f)
                {
                    StopCoroutine(changeState);
                    if (Player.instanse != null)
                    {
                        _currentState = State.TURN_ENEMY;
                    }
                    else
                    {
                        _currentState = State.IDLE;
                    }
                }
                break;
            case State.TURN_ENEMY:
                bool angleDifference = RotatePlayer() < 5f;
                if (angleDifference)
                {
                    _currentState = State.SHOOT;
                }
                break;
            case State.SHOOT:

                bool rAngleDifference = RotatePlayer() > 10f;
                if (rAngleDifference)
                {
                    _currentState = State.TURN_ENEMY;
                }
                if (canShoot)
                {
                    Shoot();
                }
                break;
        }
    }

    IEnumerator Reload()
    {
        yield return new WaitForSeconds(fireRate);
        canShoot = true;
    }

    void Shoot()
    {
        Vector3 enemyDir = Player.instanse.transform.position - transform.position;
        Quaternion toRotation = Quaternion.LookRotation(enemyDir, Vector3.up);
        Bullet bullet = Instantiate(bulletPref, firePoint.position, toRotation);
        bullet.SetDamage(damage);
        canShoot = false;
        StartCoroutine(Reload());
    }

    IEnumerator SetStateWithDelay(float delay, State newState)
    {
        yield return new WaitForSeconds(delay);
        _currentState = newState;
    }
}

