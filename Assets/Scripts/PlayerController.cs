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
    //Game Manager
    private GameManager gameManager;
    // Audio Objects
    public AudioClip jumpSound, crashSound, destroySound;
    private AudioSource playerAudio;
    //Movement Variables
    private float horizontalInput;
    public float speed = 10;
    private float xBound = 15;
    //Jump Variables
    public float jumpForce = 300;
    public float gravityModifier;

    public bool isOnGround = true;
    public bool gameOver = false;
    //Double Jump Variables
    public bool doubleJumpUsed = false;
    public float doubleJumpForce = 25;
    //Super Speed / Dash Boolean
    public bool doubleSpeed = false;

    private float knockBack = 0.5f; // knockback when Hit Obstacle
    void Start()
    {
        playerRb = GetComponent<Rigidbody>();
        playerAnim = GetComponent<Animator>();
        playerAudio = GetComponent<AudioSource>();
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();

        Physics.gravity *= gravityModifier;
        doubleJumpUsed = false;
    }

    void HorizontalMovement() {

        //horizontalInput = Input.GetAxis("Horizontal");  //-- this code is now on Move(); it will set the horizontalInput variable


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

    //Methods that will be passed on UI Control Buttons
    public void MoveLeft() { horizontalInput = -1; }
    public void MoveRight() { horizontalInput = 1; }
    public void StopMoving() { horizontalInput = 0; }

    public void Dash() {
        doubleSpeed = true;
        playerAnim.SetFloat("Speed_Multiplier", 2.0f);
    }

    public void StopDash() {
        doubleSpeed = false;
        playerAnim.SetFloat("Speed_Multiplier", 1.0f);
    }

    public void Jump()
    {

        //If player is on ground and not game over, go JUMP
        if (isOnGround && !gameManager.gameIsPaused && !gameOver)
        {
            playerRb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            isOnGround = false;
            playerAnim.SetTrigger("Jump_trig"); // play the jump animation
            dirtParticle.Stop(); // stop playing the dirt particle while on ground
            playerAudio.PlayOneShot(jumpSound, 1.0f); // play the Jump audio
            doubleJumpUsed = false;
        }
        else if (!isOnGround && !doubleJumpUsed && !gameManager.gameIsPaused && !gameOver)
        {
            doubleJumpUsed = true;
            //Double Jump using velocity will make it more consistent than using AddForce
            playerRb.velocity = new Vector3(playerRb.velocity.x, doubleJumpForce, playerRb.velocity.z);
            playerAnim.Play("Running_Jump", 3, 0f);
            playerAudio.PlayOneShot(jumpSound, 1.0f);

        }
    }

    void UpdateDash() {

        //Player dash
        if (Input.GetKey(KeyCode.LeftShift))
        {
            doubleSpeed = true;
            playerAnim.SetFloat("Speed_Multiplier", 2.0f);
        }
        else if (doubleSpeed)
        {
            doubleSpeed = false;
            playerAnim.SetFloat("Speed_Multiplier", 1.0f);
        }


    }



    void Update()
    {
        //If not GAMEOVER and NOT PAUSED, you can do this CONTROLS
        if (!gameOver && !gameManager.gameIsPaused)
        {
            HorizontalMovement(); // horizontal movement
            //UpdateDash(); //Player superspeed/ dash ability

            if (Input.GetKeyDown(KeyCode.Space))
            {
                Jump();
            }

            /*if (Input.touchCount > 0){
           
                 if (Input.GetTouch(0).phase == TouchPhase.Began)
                    {
                        Jump();
                    }
            }

            if (Input.GetMouseButtonDown(1)) //when Right-Click
            { 
                Jump(); 
            }
            */
        }
        //If Game Over You can do this Controls
        else if (gameOver && gameOverPanel.active)
        {
            //Press Space to restart game
            if (Input.GetKeyDown(KeyCode.Space))
            {
                gameManager.RestartGame();
                gameOver = false;
            }
        }

        //Pause game
        if (Input.GetKeyDown(KeyCode.Escape) && !gameManager.gameIsPaused && !gameOver)
        {
            gameManager.PauseGame();


            //If already paused, then RESUME
        }
        else if ((Input.GetKeyDown(KeyCode.Escape)|| Input.GetKeyDown(KeyCode.Space)) && gameManager.gameIsPaused && !gameOver) {
            gameManager.ResumeGame();
        }

        

    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground")) { // if hit Ground THEN
            isOnGround = true;
            if (!gameOver) { dirtParticle.Play(); } // Play dirt particle effect
            
        } else if (collision.gameObject.CompareTag("Obstacle")) { // If hit Obstacles THEN
    
            gameOver = true; // Game Over Boolean is TRUE
            transform.Translate(Vector3.back * knockBack); // Add a knock Back Force
            int randomDeath = Random.Range(1, 2); //  DECLARE int Random Range of Death Animation
            Debug.Log("Game Over");
            playerAnim.SetBool("Death_b", true); // death animation
            playerAnim.SetInteger("DeathType_int", randomDeath); // which death animation will play
            explosionParticle.Play(); // play explosion particle
            dirtParticle.Stop(); // stop the dirt particle
            playerAudio.PlayOneShot(crashSound, 1.0f); // play 
            gameOverPanel.SetActive(true); // Game Over UI will appear
            topPanel.SetActive(false); // Top Panel or the Game Play UI will disappear
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Coin")) {
            gameManager.coin++;
            Destroy(other.gameObject);
        }
    }
}
