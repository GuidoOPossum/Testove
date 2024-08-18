using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public Action<int> coinUpdate;
    private int _coins = 0;
    public int coins
    {
        set
        {
            _coins = value;
            coinUpdate?.Invoke(_coins);
        }
        get
        {
            return _coins;
        }
    }
    private static GameManager _instanse;
    public static GameManager instanse
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
            DontDestroyOnLoad(this);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    public void ReloadLVL()
    {
        string currentSceneName = SceneManager.GetActiveScene().name;
        ChangeScene(currentSceneName);
    }
    public void ChangeScene(String sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }
}
