using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class WeaponUI : MonoBehaviour
{
    public Image[] weaponIcons;
    public Image[] slotFrames;
    public TMP_Text ammoText;
    public Image hpBar;
    public TMP_Text hpText;
    public WeaponManager weaponManager;

    private float lastCurrentHP = 100f;

    private void OnEnable()
    {
        PlayerBuffs.OnMaxHealthChanged += OnMaxHealthChanged;
    }

    private void OnDisable()
    {
        PlayerBuffs.OnMaxHealthChanged -= OnMaxHealthChanged;
    }

    private void OnMaxHealthChanged(float max)
    {
        UpdateHP(lastCurrentHP, max);
    }

    public void UpdateWeaponSlots(WeaponData[] weapons, int selectedIndex)
    {
        for (int i = 0; i < weaponIcons.Length; i++)
        {
            weaponManager.EquipWeaponFromSlot(selectedIndex);

            if (weapons[i].icon != null)
            {
                weaponIcons[i].sprite = weapons[i].icon;
                weaponIcons[i].enabled = true;
                Color iconColor = weaponIcons[i].color;
                iconColor.a = 1f;
                weaponIcons[i].color = iconColor;

                float maxSize = 96;
                float w = weapons[i].icon.rect.width;
                float h = weapons[i].icon.rect.height;
                float aspect = w / h;
                if (aspect >= 1f)
                {
                    weaponIcons[i].rectTransform.sizeDelta = new Vector2(maxSize, maxSize / aspect);
                }
                else
                {
                    weaponIcons[i].rectTransform.sizeDelta = new Vector2(maxSize * aspect, maxSize);
                }
            }
            else
            {
                Color iconColor = weaponIcons[i].color;
                iconColor.a = 0f;
                weaponIcons[i].color = iconColor;
                weaponIcons[i].enabled = false;
            }

            slotFrames[i].color = (i == selectedIndex) ? Color.yellow : Color.white;
        }

        if (weapons[selectedIndex] != null)
        {
            if (weapons[selectedIndex].currentAmmo == -1)
            {
                ammoText.text = $"∞";
            }
            else
            {
                ammoText.text = $"{weapons[selectedIndex].currentAmmo}";
            }
        }
        else
        {
            ammoText.text = "-";
        }
    }

    public void UpdateHP(float current, float max)
    {
        lastCurrentHP = current;
        int currentInt = Mathf.FloorToInt(current);
        int maxInt = Mathf.FloorToInt(max);
        hpBar.fillAmount = current / max;
        hpText.text = $"{currentInt} / {maxInt}";
    }

    public void UpdateAmmo(WeaponData weapon)
    {
        if (weapon.currentAmmo == -1)
        {
            ammoText.text = $"∞";
        }
        else
        {
            ammoText.text = $"{weapon.currentAmmo}";
        }
    }

}
