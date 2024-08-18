using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// рухається за гравцем
public class FlyingEnemy : Enemy
{
    [SerializeField] private float moveSpeed = 3.0f;
    [SerializeField] private DamageZone damageZone;
    private Vector3 moveDirection;
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
                case State.SHOOT:
                    StartCoroutine(DeathRay());
                    break;
                case State.WALK:

                    changeState = StartCoroutine(SetStateWithDelay(walkingTime, State.SHOOT));
                    break;
            }

        }
    }
    Rigidbody rb;
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        changeState = StartCoroutine(SetStateWithDelay(idleTime, State.WALK));
    }

    // Update is called once per frame
    void Update()
    {
         switch (currentState)
        {
            case State.IDLE:
                moveDirection = Vector3.zero;
                break;
            case State.WALK:
                moveDirection = (Player.instanse.transform.position - transform.position).normalized;

                if (Vector3.Distance(Player.instanse.transform.position, transform.position) < 0.5f)
                {
                    StopCoroutine(changeState);
                    if (Player.instanse != null)
                    {
                        _currentState = State.SHOOT;
                    }
                    else
                    {
                        _currentState = State.IDLE;
                    }
                }
                break;
            case State.SHOOT:
                moveDirection = Vector3.zero;
                
                break;
        }
    }
        void FixedUpdate()
    {
        rb.MovePosition(rb.position + moveDirection * moveSpeed * Time.fixedDeltaTime);
    }
    IEnumerator DeathRay()
    {
        //print("DeathRay");
        damageZone.TurnOn(true);
        yield return new WaitForSeconds(idleTime);
        damageZone.TurnOn(false);
        currentState = State.IDLE;
        StopCoroutine(changeState);
        changeState = StartCoroutine(SetStateWithDelay(idleTime, State.WALK));
    }

    IEnumerator SetStateWithDelay(float delay, State newState)
    {
        yield return new WaitForSeconds(delay);
        _currentState = newState;
    }
}