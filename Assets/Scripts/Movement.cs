using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    public float moveSpeed = 5f;

    private Rigidbody2D rb;
    private Vector2 movement;
    private PlayerBuffs buffs;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        buffs = GetComponent<PlayerBuffs>();
    }

    void Update()
    {
        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical");
        movement = movement.normalized;
    }

    void FixedUpdate()
    {
        float speedMult = buffs != null ? buffs.speedMultiplier : 1f;
        rb.MovePosition(rb.position + movement * moveSpeed * speedMult * Time.fixedDeltaTime);
    }
}
