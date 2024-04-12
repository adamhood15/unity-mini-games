using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField] private float lifeTime;
    [SerializeField] private float damage;
    Rigidbody2D rb;
    bool hasHit;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        Invoke("DestroyProjectile", lifeTime);
        // Set the projectile to continuous collision detection
        rb.collisionDetectionMode = CollisionDetectionMode2D.Continuous;
        rb.constraints = RigidbodyConstraints2D.FreezeRotation;


    }

    void Update()
    {
        // Rotate the projectile to face the direction it is moving
        if (!hasHit)
        {
            float angle = Mathf.Atan2(rb.velocity.y, rb.velocity.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        }
        
    }

    // When the projectile hits something
    private void OnCollisionEnter2D(Collision2D collision)
    {
        hasHit = true;
        rb.velocity = Vector2.zero;
        rb.isKinematic = true;
    }

    // Destroy the projectile
    void DestroyProjectile()
    {
        Destroy(gameObject);
    }
}




