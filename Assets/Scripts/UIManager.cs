using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;

    [Header("Score Status")]
    [SerializeField] private GameObject scoreStatusView;
    [SerializeField] private Text usedKnivesLabel;
    [SerializeField] private Text collectedAppledLabel;

    [Header("Knives")]
    [SerializeField] private GameObject kniveLeftView;
    [SerializeField] private List<Image> knives;

    [SerializeField] private Text stageLabel;

    [Header("Game Over")]
    [SerializeField] private GameObject gameOverView;
    [SerializeField] private Text kniveTotalLabel;
    [SerializeField] private Text stageTotalLabel;

    [Header("Main Menu")]
    [SerializeField] private GameObject mainMenuView;
    [SerializeField] private Text kniveTotalMainLabel;
    [SerializeField] private Text stageTotalMainLabel;
    [SerializeField] private Text appleTotalMainLabel;

    [Header("Shop")]
    [SerializeField] private GameObject shopView;

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

    private void Start()
    {
        PrepareUIForAStage();
    }

    private void Update()
    {
        ShowUI();
        MainMenuUI();
    }

    public void OnRestartButtonClicked()
    {
        shopView.SetActive(false);
        gameOverView.SetActive(false);
        GameManager.Instance.OnRestart();
        LevelManager.Instance.ResetGame();

    }

    public void OnPlayButtonClicked()
    {
        mainMenuView.SetActive(false);
        GameManager.Instance.OnRestart();
        LevelManager.Instance.ResetGame();
    }

    public void OnMainMenuButtonClicked()
    {
        shopView.SetActive(false);
        mainMenuView.SetActive(true);
    }

    public void OnShopViewButton()
    {
        shopView.SetActive(true);
    }

    public void OnExitButtonClicked()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }

    public void PrepareUIForAStage()
    {
        for (int i = 0; i < knives.Count; i++)
        {
            knives[i].color = Color.white;
            knives[i].enabled = false;
        }

        for (int i = 0; i < LevelManager.Instance.CurrentKniveCountMax; i++)
        {
            knives[knives.Count-1-i].enabled = true;
        }


        shopView.SetActive(false);
        gameOverView.SetActive(false);
    }

    public void HideUsedKnive()
    {
        knives[knives.Count - LevelManager.Instance.CurrentKnivesCount].color = Color.black;
    }

    public void ShowStageName(bool isActive)
    {
        if(GameManager.Instance.Stage-1 == GameManager.Instance.StagesBeforeBoss)
        {
            stageLabel.text = $"boss stage";
        }
        else
        {
            stageLabel.text = $"stage {GameManager.Instance.Stage}";
        }
        
        stageLabel.enabled = isActive;
    }

    public void OnGameOverShowUI()
    {
        gameOverView.SetActive(true);
        kniveTotalLabel.text = GameManager.Instance.KnivesCounter.ToString();
        stageTotalLabel.text = $"stage {GameManager.Instance.StageTotal.ToString()}";   
    }

    private void ShowUI()
    {
        usedKnivesLabel.text = GameManager.Instance.KnivesCounter.ToString();
        collectedAppledLabel.text = GameManager.Instance.ApplesCounter.ToString();
    }

    private void MainMenuUI()
    {
        appleTotalMainLabel.text = PlayerPrefs.GetInt("ApplesCollected").ToString();
        stageTotalMainLabel.text = $"stage {PlayerPrefs.GetInt("RecordStage").ToString()}";
        kniveTotalMainLabel.text = $"score {PlayerPrefs.GetInt("RecordScore").ToString()}";
    }

}
