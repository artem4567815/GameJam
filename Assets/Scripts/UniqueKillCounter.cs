using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

// Counts unique enemy kills (by normalized name) and fires an event when target is reached.
[DisallowMultipleComponent]
public class UniqueKillCounter : MonoBehaviour
{
    public static event Action<int> OnUniqueKillTargetReached;
    public static event Action<int> OnUniqueKillCountChanged;
    [Header("Inspector event when unique target reached")]
    public UnityEvent onUniqueTargetReached;

    [Min(1)] public int targetUniqueCount = 28;

    private readonly HashSet<string> uniqueNames = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
    private bool fired = false;

    private void OnEnable()
    {
        EnemyHealth.OnEnemyKilled += HandleEnemyKilled;
    }

    private void OnDisable()
    {
        EnemyHealth.OnEnemyKilled -= HandleEnemyKilled;
    }

    private void HandleEnemyKilled(string runtimeName)
    {
        if (string.IsNullOrWhiteSpace(runtimeName)) return;
        string baseName = NormalizeName(runtimeName);
        if (uniqueNames.Add(baseName))
        {
            try { OnUniqueKillCountChanged?.Invoke(uniqueNames.Count); } catch (Exception ex) { Debug.LogException(ex); }
            if (!fired && uniqueNames.Count >= targetUniqueCount)
            {
                fired = true;
                try { OnUniqueKillTargetReached?.Invoke(uniqueNames.Count); } catch (Exception ex) { Debug.LogException(ex); }
                try { onUniqueTargetReached?.Invoke(); } catch (Exception ex) { Debug.LogException(ex); }
            }
        }
    }

    private static string NormalizeName(string name)
    {
        if (string.IsNullOrEmpty(name)) return name;
        name = name.Replace(" (Clone)", string.Empty);
        name = name.Replace("(Clone)", string.Empty);
        return name.Trim();
    }
}
