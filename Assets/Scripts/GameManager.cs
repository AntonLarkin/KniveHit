using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [Range(3,6)][SerializeField] private int stagesBeforeBoss;


    public int StagesBeforeBoss => stagesBeforeBoss;
    public bool IsGameLaunched => isGameLaunched;
    public int DefetedBosses { get; set; }
    public int Stage { get; set; }
    public int StageTotal { get; set; }
    public int ApplesCounter { get; private set; }
    public int KnivesCounter { get; private set; }

    private bool isGameLaunched;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else if (Instance == this)
        {
            Destroy(gameObject);
        }
        DontDestroyOnLoad(gameObject);

    }

    public void AddKniveToACount()
    {
        KnivesCounter++;
    }

    public void AddAppleToACount()
    {
        ApplesCounter++;
        PlayerPrefs.SetInt("ApplesCollected", ApplesCounter);
    }

    public void GameOver()
    {
        LaunchGame(false);
        UpdateRecords();
        UIManager.Instance.OnGameOverShowUI();
    }

    public void OnRestart()
    {
        LaunchGame(true);
        InitialaiseGame();
    }

    private void InitialaiseGame()
    {
        SetApples();

        Stage = 1;
        StageTotal = 1;
        DefetedBosses = 0;
        KnivesCounter = 0;
    }

    private void SetApples()
    {
        if (PlayerPrefs.GetInt("ApplesCollected") != 0)
        {
            ApplesCounter = PlayerPrefs.GetInt("ApplesCollected");
        }
        else
        {
            ApplesCounter = 0;
        }
    }

    private void LaunchGame(bool isActive)
    {
        isGameLaunched = isActive;
    }

    private void UpdateRecords()
    {
        if (PlayerPrefs.GetInt("RecordScore") <= KnivesCounter)
        {
            PlayerPrefs.SetInt("RecordScore", KnivesCounter);
        }

        if (PlayerPrefs.GetInt("RecordStage") <= StageTotal)
        {
            PlayerPrefs.SetInt("RecordStage", StageTotal);
        }

    }
}
