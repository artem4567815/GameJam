using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Bullet : MonoBehaviour
{
    [HideInInspector] public float speed;
    [HideInInspector] public int damage;
    [HideInInspector] public float fireRate;
    [HideInInspector] public float fireRange;
    [HideInInspector] public float accuracy;
    [HideInInspector] public int bulletsCount;

    private Vector2 direction;
    private Vector3 startPosition;
    private Rigidbody2D rb;

    public void SetDirection(Vector2 dir)
    {
        direction = dir.normalized;
    }

    void Start()
    {
        startPosition = transform.position;

        rb = GetComponent<Rigidbody2D>();

        if (rb == null)
        {
            rb = gameObject.AddComponent<Rigidbody2D>();
            rb.gravityScale = 0;
            rb.collisionDetectionMode = CollisionDetectionMode2D.Continuous;
        }
    }

    void Update()
    {
        transform.Translate(direction * speed * Time.deltaTime, Space.World);

        if (Vector3.Distance(startPosition, transform.position) >= fireRange)
        {
            Destroy(gameObject);
        }
    }

    void FixedUpdate()
    {
        rb.MovePosition(rb.position + direction * speed * Time.fixedDeltaTime);

        if (Vector3.Distance(startPosition, transform.position) >= fireRange)
        {
            Destroy(gameObject);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        EnemyHealth enemy = collision.gameObject.GetComponent<EnemyHealth>();
        if (enemy != null)
        {
            enemy.TakeDamage(damage);
        }
        if (collision.gameObject.tag != "Bullet" && collision.gameObject.tag != "Player")
        {
            Destroy(gameObject);
        }
    }



}
