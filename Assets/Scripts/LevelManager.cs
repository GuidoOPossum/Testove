using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using TMPro;
using System;

public class LevelManager : MonoBehaviour
{

    [SerializeField] private TextMeshProUGUI timeToStartText;
    private static LevelManager _instanse;
    public static LevelManager instanse
    {
        get
        {
            return _instanse;
        }
    }

    // 24x12 поле середина в на 0 по координатам y це z
    public Vector2 fieldSize = new Vector2(12.0f, 24.0f);
    [SerializeField] private Exit exit;
    // Dictionary чогось не серіалізується
    [SerializeField] private Transform[] enemyPref;
    [SerializeField] private int[] enemyNumber;
    private Action enemyGone;

    private List<Transform> enemyList = new List<Transform>();

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

    // Start is called before the first frame update
    void Start()
    {
        List<Vector2> coords = new List<Vector2>();
        // розбиваєм на координати
        for (int x = 0; x < fieldSize.x; x++)
        {

            for (int y = (int)fieldSize.x * 2 / 3; y < fieldSize.y; y++)
            {
                var pos = CoordToPos(new Vector2((float)x, (float)y));
                if (IsPointInNavMesh(pos)){
                    coords.Add(new Vector2((float)x, (float)y));
                }
            }
        }
        for (int i = 0; i < enemyPref.Length; i++)
        {
            for (int n = 0; n < enemyNumber[i]; n++)
            {
                //знаходим радомне місце
                int randomIndex = UnityEngine.Random.Range(0, coords.Count - 1);

                Vector3 coord = CoordToPos(coords[randomIndex]);
                // спамим в рандомному місті
                var enemy = Instantiate(enemyPref[i]);
                enemy.transform.position = coord;
                enemyList.Add(enemy);
                // вилучаєм заюзану координату
                coords.RemoveAt(randomIndex);
            }
        }
        SetSecToStartText(3);
        StartCoroutine(WaitForStart(3.0f));
        Time.timeScale = 0.0f;
    }
    Vector3 CoordToPos(Vector3 coord)
    {
        return new Vector3(coord.x - fieldSize.x / 2 + 0.5f, 0, coord.y - fieldSize.y / 2 + 0.5f);
    }
    bool IsPointInNavMesh(Vector3 point)
    {
        NavMeshHit hit;
 
        if (NavMesh.SamplePosition(point, out hit, 1.0f, NavMesh.AllAreas))
        {

            return Vector3.Distance(point, hit.position) < 0.1f;
        }
        return false;
    }

    IEnumerator WaitForStart(float timeToStart)
    {
        float timeLeft = timeToStart - 1.0f;
        yield return new WaitForSecondsRealtime(1.0f);
        if (timeLeft > 0.0)
        {
            SetSecToStartText(timeLeft);
            //print("timeToStart - " + timeLeft);
            StartCoroutine(WaitForStart(timeLeft));
        }
        else
        {
            SetSecToStartText(0);
            yield return new WaitForSecondsRealtime(1.0f);
            timeToStartText.gameObject.SetActive(false);
            //print("timeToStart - " + "0");
            StartAction();

        }
    }
    private void SetSecToStartText(float sec)
    {
        timeToStartText.text = $"Game Start at \n{sec} sec";
    }

    private void StartAction()
    {
        Time.timeScale = 1.0f;
        enemyGone += OpenExit;
    }
    void OpenExit()
    {
        exit.Activate();
    }
    public Transform ReturnClosestEnemy()
    {
        // чистим від null
        enemyList.RemoveAll(enemy => enemy == null);

        if (enemyList.Count == 0)
        {
            enemyGone?.Invoke();
            return null;
        }
        else
        {
            Transform player = Player.instanse.transform;
            Transform closestEnemy = enemyList[0];
            float distanceToClosest = Vector3.Distance(player.position, closestEnemy.position);
            for (int i = 1; i < enemyList.Count; i++)
            {
                float distance = Vector3.Distance(player.position, enemyList[i].position);
                if (distance < distanceToClosest)
                {
                    closestEnemy = enemyList[i];
                }
            }
            return closestEnemy;
        }
    }
}
