using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Rigidbody2D))]
public class Movement : MonoBehaviour
{
    private Rigidbody2D rb;
    public float speed = 1.0f;
    public UnityEvent<float> OnDirection;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        float movement = Input.GetAxisRaw("Horizontal");
        OnDirection?.Invoke(movement);
        rb.velocity = new Vector2(movement * speed, rb.velocity.y);
    }
}
