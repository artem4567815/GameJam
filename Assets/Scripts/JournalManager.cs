using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

[System.Serializable]
public class EnemyInfo
{
    public string name;
    public Sprite sprite;
}

[System.Serializable]
public class WeaponInfo
{
    public string name;
    public Sprite sprite;
}

public class JournalManager : MonoBehaviour
{
    public GameObject journalPanel;
    public Transform contentParent;


    public GameObject journalEntryPrefab;
    public Button closeButton;

    private HashSet<string> discoveredEnemyNames = new HashSet<string>();
    private List<EnemyInfo> discoveredEnemies = new List<EnemyInfo>();
    private List<WeaponInfo> droppedWeapons = new List<WeaponInfo>();

    void Start()
    {
        journalPanel.SetActive(false);
        closeButton.onClick.AddListener(CloseJournal);
        //EnemyHealth.OnEnemyKilled += OnEnemyKilled;
    }

    void OnDestroy()
    {
        //EnemyHealth.OnEnemyKilled -= OnEnemyKilled;
    }

    private void OnEnemyKilled(string enemyName)
    {
        if (discoveredEnemyNames.Add(enemyName))
        {
            Sprite enemySprite = GetEnemySpriteByName(enemyName);
            discoveredEnemies.Add(new EnemyInfo { name = enemyName, sprite = enemySprite });
        }
    }

    private Sprite GetEnemySpriteByName(string enemyName)
    {
        // TODO: реализуйте получение спрайта из вашей системы врагов
        return null;
    }

    public void AddWeapon(WeaponInfo info)
    {
        droppedWeapons.Add(info);
    }

    public void OpenJournal()
    {
        FillJournal();
        journalPanel.SetActive(true);
        Time.timeScale = 0f;
    }

    public void CloseJournal()
    {
        foreach (Transform child in contentParent)
            Destroy(child.gameObject);
        journalPanel.SetActive(false);
        Time.timeScale = 1f;
    }

    private void FillJournal()
    {
        foreach (var enemy in discoveredEnemies)
        {
            var entry = Instantiate(journalEntryPrefab, contentParent);
            entry.GetComponentInChildren<TMP_Text>().text = enemy.name;
            entry.GetComponentInChildren<Image>().sprite = enemy.sprite;
        }
        foreach (var weapon in droppedWeapons)
        {
            var entry = Instantiate(journalEntryPrefab, contentParent);
            entry.GetComponentInChildren<TMP_Text>().text = weapon.name;
            entry.GetComponentInChildren<Image>().sprite = weapon.sprite;

        }
    }
}
