using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Animator playerAnim;
    private Rigidbody playerRb;

    public ParticleSystem explosionParticle, dirtParticle;

    //UI objects
    public GameObject gameOverPanel;
    public GameObject topPanel;
    public GameObject pausedPanel;

    private GameManager gameManager;

    public AudioClip jumpSound, crashSound, destroySound;
    private AudioSource playerAudio;

    private float horizontalInput;
    public float speed = 10;
    private float xBound = 15;
    
    public float jumpForce = 300;
    public float gravityModifier;

    public bool isOnGround = true;
    public bool gameOver = false;

    public bool doubleJumpUsed = false;
    public float doubleJumpForce = 25;

    public bool doubleSpeed = false;

    private float knockBack = 0.5f;
    void Start()
    {
        playerRb = GetComponent<Rigidbody>();
        playerAnim = GetComponent<Animator>();
        playerAudio = GetComponent<AudioSource>();
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        
        Physics.gravity *= gravityModifier;
        doubleJumpUsed = false;
    }

    void Update()
    {
        //Horizontal Input
        if (!gameOver) {
            horizontalInput = Input.GetAxis("Horizontal");
            //Move horizontally
            transform.Translate(Vector3.forward * horizontalInput * Time.deltaTime * speed);

            //limit horizontal movement
            if (transform.position.x < 0)
            {
                transform.position = new Vector3(0, transform.position.y, transform.position.z);
            }
            if (transform.position.x > xBound)
            {
                transform.position = new Vector3(xBound, transform.position.y, transform.position.z);
            }
        }
        
        //If player is on ground and not game over, go JUMP
        if (Input.GetKeyDown(KeyCode.Space) &&  isOnGround &&!gameOver && !gameManager.gameIsPaused) {
            playerRb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            isOnGround = false;
            playerAnim.SetTrigger("Jump_trig");
            dirtParticle.Stop();
            playerAudio.PlayOneShot(jumpSound, 1.0f);
            doubleJumpUsed = false;
        }
        else if (Input.GetKeyDown(KeyCode.Space) && !isOnGround && !doubleJumpUsed) {
            doubleJumpUsed = true;
            playerRb.velocity = new Vector3(playerRb.velocity.x, doubleJumpForce, playerRb.velocity.z);
            playerAnim.Play("Running_Jump",3, 0f);
            playerAudio.PlayOneShot(jumpSound, 1.0f);

        }
        //Player dash
        if (Input.GetKey(KeyCode.LeftShift) && !gameOver && !gameManager.gameIsPaused)
        {
            doubleSpeed = true;
            playerAnim.SetFloat("Speed_Multiplier", 2.0f);
        }
        else if (doubleSpeed)
        {
            doubleSpeed = false;
            playerAnim.SetFloat("Speed_Multiplier", 1.0f);
        }

        //Pause game
        if (Input.GetKeyDown(KeyCode.Escape) && !gameManager.gameIsPaused &&!gameOver)
        {
            gameManager.PauseGame();

        }
        else if (Input.GetKeyDown(KeyCode.Escape) && gameManager.gameIsPaused && !gameOver) {
            gameManager.ResumeGame();
        }

    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground")) {
            isOnGround = true;
            if (!gameOver) { dirtParticle.Play(); }
            
        } else if (collision.gameObject.CompareTag("Obstacle")) {
    
            gameOver = true;
            transform.Translate(Vector3.back * knockBack);
            int randomDeath = Random.Range(1, 2);
            Debug.Log("Game Over");
            playerAnim.SetBool("Death_b", true);
            playerAnim.SetInteger("DeathType_int", randomDeath);
            explosionParticle.Play();
            dirtParticle.Stop();
            playerAudio.PlayOneShot(crashSound, 1.0f);
            gameOverPanel.SetActive(true);
            topPanel.SetActive(false);
        }
    }
}
