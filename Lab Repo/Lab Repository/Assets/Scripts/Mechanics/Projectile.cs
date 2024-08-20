using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Projectile : MonoBehaviour
{
    [SerializeField, Range(1,50)] public float lifetime;

    [HideInInspector]
    public float xVel;
    [HideInInspector]
    public float yVel;


    // Start is called before the first frame update
    void Start()
    {
        if (lifetime <= 0) lifetime = 2.0f;

        GetComponent<Rigidbody2D>().velocity = new Vector2 (xVel, yVel);
        Destroy (gameObject, lifetime);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Power"))
        Destroy (this.gameObject);

        if (other.gameObject.CompareTag("Enemy"))
            Destroy(gameObject);

    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        
        Destroy(this.gameObject);
         
    }

}

