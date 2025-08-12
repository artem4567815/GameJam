using UnityEngine;

[CreateAssetMenu(fileName = "BulletData", menuName = "Weapons/Bullet Data")]
public class BulletData : ScriptableObject
{
    public float speed = 10f;
    public int damage = 10;
    public float fireRate = 0.2f; // задержка между выстрелами
    public float fireRange = 5f;
    [Range(0f, 100f)] public float accuracy = 0f;
    public int bulletsCount = 1;
}