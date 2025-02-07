using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Rigidbody2D playerRigidbody;
    public Animator playerAnimator;
    public AudioSource playerAudio;
    public AudioClip playerWalk;
    public Light myLight;

    public float health = 10.0f;
    public float speed = 5.0f;
    public bool gameover = false;
    public int lightIntensitySetter = 8;
    private Vector2 moveDirection;

    // Start is called before the first frame update
    void Start()
    {
        playerRigidbody = GetComponent<Rigidbody2D>();
        playerAnimator = GetComponent<Animator>();
        playerAudio = GetComponent<AudioSource>();
        myLight = GetComponent<Light>();
    }

    // Update is called once per frame
    void Update()
    {
        ProcessInputs();
        myLight.intensity = Mathf.PingPong(Time.time, lightIntensitySetter);
    }

    void FixedUpdate()
    {
        Move();
    }

    public void HealthDown(float damage, int lightReduction) 
    {
        health = health - damage;
        lightIntensitySetter = lightIntensitySetter - lightReduction;
    }

    void ProcessInputs() 
    {
        float moveX = Input.GetAxisRaw("Horizontal");
        float moveY = Input.GetAxisRaw("Vertical");

        moveDirection = new Vector2(moveX, moveY).normalized;
    }

    void Move() 
    {
        playerRigidbody.velocity = new Vector2(moveDirection.x * speed, moveDirection.y * speed);
    }
}
