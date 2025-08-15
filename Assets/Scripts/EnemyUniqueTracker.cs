using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// Tracks unique enemy names seen via EnemyHealth.OnEnemyKilled and raises an event when a target count is reached.
/// Handles names like "zom light(Clone)" by normalizing to the prefab/original name.
/// </summary>
[DisallowMultipleComponent]
public class EnemyUniqueTracker : MonoBehaviour
{
    [Header("Target unique enemies to trigger event")]
    [Min(1)] public int targetUniqueCount = 28;

    [Header("Counting options")]
    [Tooltip("If true, only count kills whose names match the provided enemy prefabs list (by name, ignoring (Clone)). If false, count any unique runtime name.")]
    public bool onlyCountKnownPrefabs = true;

    [Tooltip("Optional: list of all enemy prefabs. Used to validate and map runtime names to canonical prefab names.")]
    public List<GameObject> allEnemyPrefabs = new List<GameObject>();

    public static event Action<int> OnTargetUniqueCountReached; // passes current unique count
    [Header("Events")]
    public UnityEvent onTargetReached; // assign in inspector

    private readonly HashSet<string> uniqueEnemyNames = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
    private bool targetTriggered = false;

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

        // Normalize runtime name (strip "(Clone)")
        string baseName = NormalizeName(runtimeName);
        string canonicalName = baseName;

        if (onlyCountKnownPrefabs && allEnemyPrefabs != null && allEnemyPrefabs.Count > 0)
        {
            foreach (var prefab in allEnemyPrefabs)
            {
                if (prefab == null) continue;
                string prefabBase = NormalizeName(prefab.name);
                if (string.Equals(prefabBase, baseName, StringComparison.OrdinalIgnoreCase))
                {
                    canonicalName = prefab.name; // keep prefab's canonical casing
                    break;
                }
            }

            // If no prefab matched, skip counting when whitelist is enforced
            if (!NameMatchesAnyPrefab(baseName) && onlyCountKnownPrefabs)
            {
                return;
            }
        }

        if (uniqueEnemyNames.Add(canonicalName))
        {
            Debug.Log($"Unique enemy discovered: {canonicalName} ({uniqueEnemyNames.Count}/{targetUniqueCount})");
            if (!targetTriggered && uniqueEnemyNames.Count >= targetUniqueCount)
            {
                targetTriggered = true;
                try { OnTargetUniqueCountReached?.Invoke(uniqueEnemyNames.Count); } catch (Exception ex) { Debug.LogException(ex); }
                try { onTargetReached?.Invoke(); } catch (Exception ex) { Debug.LogException(ex); }
            }
        }
    }

    private bool NameMatchesAnyPrefab(string baseName)
    {
        if (allEnemyPrefabs == null) return false;
        foreach (var prefab in allEnemyPrefabs)
        {
            if (prefab == null) continue;
            if (string.Equals(NormalizeName(prefab.name), baseName, StringComparison.OrdinalIgnoreCase))
                return true;
        }
        return false;
    }

    public int CurrentUniqueCount => uniqueEnemyNames.Count;
    public IReadOnlyCollection<string> UniqueEnemyNames => uniqueEnemyNames;

    [ContextMenu("Reset Progress")] 
    public void ResetProgress()
    {
        uniqueEnemyNames.Clear();
        targetTriggered = false;
    }

    private static string NormalizeName(string name)
    {
        if (string.IsNullOrEmpty(name)) return name;
        name = name.Replace(" (Clone)", string.Empty);
        name = name.Replace("(Clone)", string.Empty);
        return name.Trim();
    }
}
