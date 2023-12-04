using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public enum GameState
    {
        Gameplay,
        Paused,
        GameOver,
        LevelUp
    }

    public GameState currentState;
    public GameState previousState;

    public Canvas damageTextCanvas;
    public float textFontSize = 20;
    public TMP_FontAsset textFont;
    public Camera referenceCamera;

    public GameObject pauseScreen;
    public GameObject resultScreen;
    public GameObject levelUpScreen;

    public TMP_Text currentHealthDisplay;
    public TMP_Text currentRecoveryDisplay;
    public TMP_Text currentMoveSpeedDisplay;
    public TMP_Text currentMightDisplay;
    public TMP_Text currentProjectileSpeedDisplay;
    public TMP_Text currentMagnetDisplay;

    public Image chosenCharacterImage;
    public TMP_Text chosenCharacterName;
    public TMP_Text levelReachedDisplay;
    public TMP_Text timeSurvivedDisplay;
    public List<Image> chosenWeaponUI = new List<Image>(6);
    public List<Image> chosenItemUI = new List<Image>(6);

    public float timeLimit;
    float stopwatchTime;
    public TMP_Text stopwatchDisplay;

    public bool isGameOver = false;
    public bool choosingUpgrade;

    public GameObject playerObject;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Debug.LogWarning("Extra" + this + "deleted");
            Destroy(gameObject);
        }
        DisableScreen();
    }

    private void Update()
    {
        switch (currentState)
        {
            case GameState.Gameplay:
                CheckForPauseAndResume();
                UpdateStopwatch();
                break;
            case GameState.Paused:
                CheckForPauseAndResume();
                break;
            case GameState.GameOver:
                if (!isGameOver)
                {
                    isGameOver = true;
                    Time.timeScale = 0f;
                    DisplayResults();
                }
                break;
            case GameState.LevelUp:
                if (!choosingUpgrade)
                {
                    choosingUpgrade = true;
                    Time.timeScale = 0f;
                    levelUpScreen.SetActive(true);
                }
                break;
            default:
                Debug.Log("State doesn't exist");
                break;
        }
    }

    public static void GenerateFloatingText(string text, Transform target, float duration = 1f, float speed = 1f)
    {
        if (!Instance.damageTextCanvas) return;

        if (!Instance.referenceCamera) Instance.referenceCamera = Camera.main;

        Instance.StartCoroutine(Instance.GenerateFloatingTextCoroutine(text, target, duration, speed));
    }

    IEnumerator GenerateFloatingTextCoroutine(string text, Transform target, float duration = 1f, float speed = 50f)
    {
        GameObject textObj = new GameObject("Damage Floating Text");
        RectTransform rect = textObj.AddComponent<RectTransform>();
        TextMeshProUGUI tmPro = textObj.AddComponent<TextMeshProUGUI>();
        tmPro.text = text;
        tmPro.horizontalAlignment = HorizontalAlignmentOptions.Center;
        tmPro.verticalAlignment = VerticalAlignmentOptions.Middle;
        tmPro.fontSize = textFontSize;
        if (textFont) tmPro.font = textFont;
        rect.position = referenceCamera.WorldToScreenPoint(target.position);

        Destroy(textObj, duration);
        textObj.transform.SetParent(Instance.damageTextCanvas.transform);

        WaitForEndOfFrame w = new WaitForEndOfFrame();
        float t = 0;
        float yOffset = 0;
        while (t < duration)
        {
            yield return w;
            t += Time.deltaTime;
            tmPro.color = new Color(tmPro.color.r, tmPro.color.g, tmPro.color.b, 1 - t / duration);
            yOffset += speed * Time.deltaTime;
            rect.position = referenceCamera.WorldToScreenPoint(target.position + new Vector3(0, yOffset));
        }
    }

    public void ChangeState(GameState newState)
    {
        currentState = newState;
    }

    public void PauseGame()
    {
        if (currentState != GameState.Paused)
        {
            previousState = currentState;
            ChangeState(GameState.Paused);
            Time.timeScale = 0f;
            pauseScreen.SetActive(true);
            Debug.Log("Game is Paused");
        }
    }
    public void ResumeGame()
    {
        if (currentState == GameState.Paused)
        {
            ChangeState(previousState);
            Time.timeScale = 1f;
            pauseScreen.SetActive(false);
        }
    }

    void CheckForPauseAndResume()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (currentState == GameState.Paused)
            {
                ResumeGame();
            }
            else
            {
                PauseGame();
            }
        }
    }

    void DisableScreen()
    {
        pauseScreen.SetActive(false);
        resultScreen.SetActive(false);
        levelUpScreen.SetActive(false);
    }

    public void GameOver()
    {
        timeSurvivedDisplay.text = stopwatchDisplay.text;
        ChangeState(GameState.GameOver);
    }

    void DisplayResults()
    {
        resultScreen.SetActive(true);
    }

    public void AssignChosenCharacterUI(CharacterSO chosenCharacterData)
    {
        chosenCharacterImage.sprite = chosenCharacterData.Icon;
        chosenCharacterName.text = chosenCharacterData.name;
    }
    public void AssignLevelReachedUI(int levelReachedData)
    {
        levelReachedDisplay.text = levelReachedData.ToString();
    }
    public void AssignChosenWeaponAndItemUI(List<Image> chosenWeaponData, List<Image> chosenItemData)
    {
        if (chosenWeaponData.Count != chosenWeaponUI.Count || chosenItemData.Count != chosenItemUI.Count)
        {
            Debug.Log("Lists have different lengths");
            return;
        }
        for (int i = 0; i < chosenWeaponUI.Count; i++)
        {
            if (chosenWeaponData[i].sprite)
            {
                chosenWeaponUI[i].enabled = true;
                chosenWeaponUI[i].sprite = chosenWeaponData[i].sprite;
            }
            else
            {
                chosenWeaponUI[i].enabled = false;
            }
        }
        for (int i = 0; i < chosenItemUI.Count; i++)
        {
            if (chosenItemData[i].sprite)
            {
                chosenItemUI[i].enabled = true;
                chosenItemUI[i].sprite = chosenItemData[i].sprite;
            }
            else
            {
                chosenItemUI[i].enabled = false;
            }
        }
    }

    private void UpdateStopwatch()
    {
        stopwatchTime += Time.deltaTime;
        UpdateStopwatchDisplay();
        if (stopwatchTime >= timeLimit)
        {
            playerObject.SendMessage("Kill");
        }
    }

    private void UpdateStopwatchDisplay()
    {
        int minutes = Mathf.FloorToInt(stopwatchTime / 60);
        int seconds = Mathf.FloorToInt(stopwatchTime % 60);
        stopwatchDisplay.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }

    public void StartLevelUp()
    {
        ChangeState(GameState.LevelUp);
        playerObject.SendMessage("RemoveAndApplyUpgrade");
    }
    public void EndLevelUp()
    {
        choosingUpgrade = false;
        Time.timeScale = 1f;
        levelUpScreen.SetActive(false);
        ChangeState(GameState.Gameplay);
    }
}
