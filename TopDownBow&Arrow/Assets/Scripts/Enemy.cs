using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float health = 0.0f;
    public float maxHealth;
    public GameObject target;
    public float targetDistance = 50.0f;
    public float speed;
    public GameObject effect;
    public float attackDamage;
    public float attackSpeed = 1.0f;
    private float canAttack;
    private Vector3 startingPosition;

    // Start is called before the first frame update
    void Start()
    {
        health = maxHealth;
        startingPosition = transform.position;

    }

    // Update is called once per frame
    void Update()
    {
        //enemy follows player
        transform.position = Vector2.MoveTowards(transform.position, target.transform.position, speed * Time.deltaTime);
        
    }

    private void OnCollisionStay2D(Collision2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            if (attackSpeed <= canAttack)
            {
                other.gameObject.GetComponent<PlayerHealth>().UpdateHealth(-attackDamage);
                canAttack = 0f;           
            }
            else 
            {
                canAttack += Time.deltaTime;
            }
            
        }
    }
    

    public void TakeDamage(float TakeDamage)
    {
        health -= TakeDamage;

        if (health <= 0)
        {
            Destroy(gameObject);
        }
    }

    }

