using UnityEngine;
using TMPro;

// Simple UI binder to display unique kill count as X / target
[RequireComponent(typeof(TMP_Text))]
public class UniqueKillCounterUI : MonoBehaviour
{
    public UniqueKillCounter counter; // optional explicit reference; if null will search in scene
    public string format = "{0}/{1}"; // e.g., 12/28

    private TMP_Text label;

    private void Awake()
    {
        label = GetComponent<TMP_Text>();
    }

    private void OnEnable()
    {
        UniqueKillCounter.OnUniqueKillCountChanged += HandleCountChanged;
        // Try to find counter if not assigned
        if (counter == null) counter = FindObjectOfType<UniqueKillCounter>();
        // Initialize label
        if (counter != null) UpdateLabel(0);
    }

    private void OnDisable()
    {
        UniqueKillCounter.OnUniqueKillCountChanged -= HandleCountChanged;
    }

    private void HandleCountChanged(int count)
    {
        UpdateLabel(count);
    }

    private void UpdateLabel(int count)
    {
        int target = counter != null ? counter.targetUniqueCount : 28;
        label.text = string.Format(format, count, target);
    }
}
