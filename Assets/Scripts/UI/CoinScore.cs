using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CoinScore : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI myText;
    // Start is called before the first frame update
    void Start()
    {
        SetScore(GameManager.instanse.coins);
        GameManager.instanse.coinUpdate += SetScore;
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void SetScore(int i)
    {
        myText.text = $"Coins : {i}";
    }
    void OnDestroy()
    {
        GameManager.instanse.coinUpdate -= SetScore;
    }
}
