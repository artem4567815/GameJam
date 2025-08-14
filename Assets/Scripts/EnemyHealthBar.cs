using UnityEngine;
using UnityEngine.UI;

public class EnemyHealthBar : MonoBehaviour
{
    public EnemyHealth enemyHealth;
    public Image fillImage;

    private Camera mainCamera;

    void Start()
    {
        mainCamera = Camera.main;
        if (enemyHealth != null)
        {
            enemyHealth.OnHealthChanged += UpdateBar;
            UpdateBar(enemyHealth.CurrentHealth, enemyHealth.MaxHealth);
        }
    }

    public void UpdateBar(int current, int max)
    {
        if (fillImage != null)
        {
            fillImage.fillAmount = (float)current / max;
        }
    }

    private void OnDestroy()
    {
        if (enemyHealth != null)
            enemyHealth.OnHealthChanged -= UpdateBar;
    }
}
