using System.Collections;
using System;
using UnityEngine;
using System.Collections.Generic;

public class LevelManager : MonoBehaviour
{
    public static LevelManager Instance;

    [Header("Generic Log")]
    [SerializeField] private List<Log> logs;
    [Header("Boss Log")]
    [SerializeField] private List<Log> bosses;
    [SerializeField] private Vector2 logSpawnPosition;
    private Log currentLog;

    [Header("Knive")]
    [SerializeField] private AppearableObjectData kniveData;
    [SerializeField] private Knive appearableKnivePrefab;
    [SerializeField] private Knive knivePrefab;
    [SerializeField] private float numberOfKnives;
    [SerializeField] private Vector2 startPosition;
    [SerializeField] private float waitTime;
    private Knive activeKnive;
    private float currentKniveCountMax;
    private int currentKnivesCount;

    [Header("Apple")]
    [SerializeField] private Apple applePrefab;
    [SerializeField] private AppearableObjectData appleData;

    [SerializeField] private float radius;

    private bool isBossLevel;

    public int CurrentKnivesCount => currentKnivesCount;
    public float CurrentKniveCountMax => currentKniveCountMax;

    public int TotalSpawnedKnives { get; set; }

    public event Action OnLogDestroyed;

    private void Awake()
    {
        if (Instance==null)
        {
            Instance = this;
        }
        else if(Instance==this)
        {
            Destroy(gameObject);
        }
        DontDestroyOnLoad(gameObject);
    }

     void Update()
    {
        if (currentLog == null && GameManager.Instance.IsGameLaunched)
        {
            CreateNextStage();
        }
    }

    public void ResetActiveKnive()
    {
        if (GameManager.Instance.IsGameLaunched)
        {
            if (currentKnivesCount < currentKniveCountMax && GameManager.Instance.IsGameLaunched)
            {
                StartCoroutine(OnHitWait());
            }
            else
            {
                SpawnPlayerKnive();
            }
        }
    }

    public void ResetGame()
    {
        currentKniveCountMax = numberOfKnives;
        currentKnivesCount = 0;
        MakeStage();
    }

    private void MakeStage()
    {
        StartCoroutine(OnStartShowStageName());
        SpawnLog();
        SpawnKnives();
        SpawnApple();
        SpawnPlayerKnive();
        UIManager.Instance.PrepareUIForAStage();
    }

    private void SpawnLog()
    {
        if (!isBossLevel)
        {
            currentLog = Instantiate(logs[GameManager.Instance.Stage-1 ], logSpawnPosition, Quaternion.identity);
        }
        else
        {
            GameManager.Instance.Stage =0 ;
            currentLog = Instantiate(bosses[GameManager.Instance.Stage+ GameManager.Instance.DefetedBosses], logSpawnPosition, Quaternion.identity);
            GameManager.Instance.DefetedBosses++;
            isBossLevel = false;
        }
    }

    private void CreateNextStage()
    {
        if (GameManager.Instance.Stage == GameManager.Instance.StagesBeforeBoss)
        {
            isBossLevel = true;
        }
        GameManager.Instance.Stage++;
        GameManager.Instance.StageTotal++;

        if (currentKniveCountMax <= 7)
        {
            currentKniveCountMax++;
        }

        MakeStage();
    }

    private void SpawnPlayerKnive()
    {
        if (currentKnivesCount < currentKniveCountMax)
        {
            currentKnivesCount++;
            activeKnive = Instantiate(knivePrefab, startPosition, Quaternion.identity);
        }
        else
        {
            currentKnivesCount = 0;
            OnLogDestroyed?.Invoke();
        }
    }

    private void SpawnApple()
    {
        if (appleData.IsAppeared())
        {
            Apple apple = Instantiate(applePrefab, DefineSpawnPosition(), Quaternion.identity, currentLog.transform);
            SetRotation(currentLog.transform, apple.transform);
        }
    }

    private void SpawnKnives()
    {
        var value = kniveData.NumberToAppear;
        int count = 0;
        Knive[] knives;

        while (count < value)
        {
            for (int i = 0; i < value; i++)
            {
                Knive knive = Instantiate(appearableKnivePrefab, DefineSpawnPosition(), Quaternion.identity, currentLog.transform);
                SetRotation(currentLog.transform, knive.transform);
            }

            knives = FindObjectsOfType<Knive>();
            if (knives.Length+1 < value)
            {
                return;
            }
            else
            {
                count = value;
            }
        }

    }

    private Vector2 DefineSpawnPosition()
    {
        float randAng = UnityEngine.Random.Range(0, Mathf.PI * 2);
        return new Vector2(Mathf.Cos(randAng) * radius + currentLog.transform.position.x, Mathf.Sin(randAng) * radius + currentLog.transform.position.y);
    }

    private void SetRotation(Transform log, Transform objectToPlace)
    {
        var direction = Vector2.ClampMagnitude((log.position - objectToPlace.position), 1f);
        objectToPlace.up = -(Vector2)direction;
    }

    private IEnumerator OnHitWait()
    {
        yield return new WaitForSeconds(waitTime);

        activeKnive = null;
        SpawnPlayerKnive();

    }

    private IEnumerator OnStartShowStageName()
    {
        UIManager.Instance.ShowStageName(true);
        yield return new WaitForSeconds(1.5f);
        UIManager.Instance.ShowStageName(false);
    }
}

