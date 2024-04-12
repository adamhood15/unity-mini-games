using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerMovement : MonoBehaviour
{
    private CircleCollider2D _col;
    private SpriteRenderer rend;

    public CharacterController2D controller;
    
    [SerializeField] private string newLevel;



    //Speed Variables
    public float runSpeed = 40f;
   
    float horizontalMove = 0f;
    bool jump = false;

    void Start()
    {
        rend= gameObject.GetComponent<SpriteRenderer>();
        _col = gameObject.GetComponent<CircleCollider2D>();
    }

    // Update is called once per frame
    void Update()
    {

        horizontalMove = Input.GetAxisRaw("Horizontal") * runSpeed;

        if (Input.GetButtonDown("Jump"))
        {
            jump = true;
        }

        //Loads back to old level
        if (Input.GetButtonDown("Fire1"))
        {
            SceneManager.LoadScene(newLevel);
        }

        //Wall Shift
        if (Input.GetButtonDown("Fire2"))
        {
            _col.enabled = false;
            rend.color = Color.blue;
        }
        else if (Input.GetButtonUp("Fire2"))
        {
            _col.enabled = true;
            rend.color = Color.white;
        }
    }

    void FixedUpdate()
    {
        controller.Move(horizontalMove * Time.fixedDeltaTime, false, jump);
        jump = false;

    }
    
}
