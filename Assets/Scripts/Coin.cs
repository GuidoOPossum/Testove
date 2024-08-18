using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Coin : MonoBehaviour
{
    private int coin = 1;
    private void OnTriggerEnter(Collider collision)
    {
        
        GameManager.instanse.coins += coin;
        Destroy(gameObject);
    }
    public void SetBounty(int bounty)
    {
        coin = bounty;
    }
}
