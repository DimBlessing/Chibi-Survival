using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public Rigidbody2D rb;
    public float speed = 5f;
    private float x;
    private float y;
    void Start()
    {
        rb = gameObject.GetComponent<Rigidbody2D>();
    }


    void Update()
    {
        x = Input.GetAxis("Horizontal");
        y = Input.GetAxis("Vertical");
    }

    void FixedUpdate(){
        rb.velocity = new Vector2(x, y) * speed;
    }
}
