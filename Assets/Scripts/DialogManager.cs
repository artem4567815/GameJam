using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class DialogManager : MonoBehaviour
{
    public GameObject dialogPanel; // облачко
    public TMP_Text dialogText;
    public Button nextButton;
    public string[] phrases;
    private int index = 0;

    public string[] secondDialogPhrases;

    public string[] thirdDialogPhrases;

    public string[] dieDialogPhrases;

    private static bool firstEnemyKilled = false;
    private static bool secondEnemyKilled = false;

    private HashSet<string> buffedRaces = new HashSet<string>();

    void Start()
    {
        StartDialog();
        EnemyHealth.OnEnemyKilled += StartSecondDialog;
        PlayerHealth.OnPlayerDeath += DieDialog;
    }

    void OnDestroy()
    {
        EnemyHealth.OnEnemyKilled -= StartSecondDialog;
        PlayerHealth.OnPlayerDeath -= DieDialog;
    }

    void StartDialog()
    {
        dialogPanel.SetActive(true);
        index = 0;
        ShowPhrase();
        Time.timeScale = 0f;
    }

    void DieDialog()
    {
        dialogPanel.SetActive(true);
        index = 0;
        phrases = dieDialogPhrases;
        Time.timeScale = 0f;
        ShowPhrase();
    }

    void StartSecondDialog(string enemyName)
    {
        if (secondEnemyKilled)
            return;

        if (!buffedRaces.Contains(enemyName))
        {
            buffedRaces.Add(enemyName);

            if (firstEnemyKilled)
            {
                dialogPanel.SetActive(true);
                index = 0;
                phrases = thirdDialogPhrases;
                Time.timeScale = 0f;
                ShowPhrase();
                secondEnemyKilled = true;
                return;
            }
            else
            {
                dialogPanel.SetActive(true);
                index = 0;
                phrases = secondDialogPhrases;
                Time.timeScale = 0f;
                firstEnemyKilled = true;
                ShowPhrase();
                return;
            }
        }
    }

    void Update()
    {
        if (dialogPanel.activeSelf && (Input.GetKeyDown(KeyCode.Return) || Input.GetMouseButtonDown(0)))
        {
            NextPhrase();
        }
    }

    void ShowPhrase()
    {
        dialogText.text = phrases[index];
        if (index != 1)
            dialogText.rectTransform.anchoredPosition = new Vector2(800, 0);
        else
            dialogText.rectTransform.anchoredPosition = new Vector2(-300, 0);
    }

    void NextPhrase()
    {
        index++;
        if (index < phrases.Length)
        {
            ShowPhrase();
        }
        else
        {
            EndDialog();
        }
    }

    void EndDialog()
    {
        dialogPanel.SetActive(false);
        Time.timeScale = 1f;
        nextButton.onClick.RemoveListener(NextPhrase);
    }
}
