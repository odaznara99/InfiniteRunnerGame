using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class GameManager : MonoBehaviour
{
    public float score;
    private float scoreMilestone;
    public int coin;
    public float obsSpeedMultiplier = 1;
    public float backgroundSpeed;
    private PlayerController playerControllerScript;
    private SpawnManager spawnManagerScript;

    public Transform startingPoint;
    public float lerpSpeed;

    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI coinText;

    public bool gameIsPaused;
    void Start()
    {
        playerControllerScript = GameObject.Find("Player").GetComponent<PlayerController>(); //reference PlayerController
        spawnManagerScript = GameObject.Find("SpawnManager").GetComponent<SpawnManager>(); // reference SpawnManager
        score = 0;
        coin = 0;
        scoreMilestone = 500; // meaning every 500 is the milestone
        spawnManagerScript.repeatRate = 3.0f;
        playerControllerScript.gameOver = true;
        StartCoroutine(PlayIntro());

    }

    // Update is called once per frame
    void Update()
    {
        if (!playerControllerScript.gameOver  && !gameIsPaused)

        {
            coinText.text = coin.ToString(); // display the coins to the UI text
            if (playerControllerScript.doubleSpeed)
            {
                score += 2;
            }
            else {
                score++;
            }
            //Debug.Log("Score: " + score);
            scoreText.text = score.ToString(); //display the score to the UI text

            if (spawnManagerScript.repeatRate <= 1) {
                spawnManagerScript.repeatRate = 1f; // limit the minimum repeatRate
            }
            // if the current score REACHED the scoreMilestone, the repeat rate will decrease in seconds
            else if (score >= scoreMilestone && spawnManagerScript.repeatRate !=1) { 
                spawnManagerScript.repeatRate -= 0.1f; // minus the repeatRate by 0.1 second
                obsSpeedMultiplier += 0.2f; // the Add Speed in Moving Left Objects
                scoreMilestone += scoreMilestone; // add the Score Milestone to itself
                
            }

        }

        if (Input.GetKeyDown(KeyCode.R)) {
            RestartGame();
        }

    }

    IEnumerator PlayIntro()
    {
        Vector3 startPos = playerControllerScript.transform.position;
        Vector3 endPos = startingPoint.position;
        float journeyLength = Vector3.Distance(startPos, endPos);
        float startTime = Time.time;
        float distanceCovered = (Time.time - startTime) * lerpSpeed;
        float fractionOfJourney = distanceCovered / journeyLength;
        playerControllerScript.GetComponent<Animator>().SetFloat("Speed_Multiplier",
        0.5f);
        while (fractionOfJourney < 1)
        {
            distanceCovered = (Time.time - startTime) * lerpSpeed;
            fractionOfJourney = distanceCovered / journeyLength;
            playerControllerScript.transform.position = Vector3.Lerp(startPos, endPos,
            fractionOfJourney);
            yield return null;
        }
        playerControllerScript.GetComponent<Animator>().SetFloat("Speed_Multiplier",
        1.0f);
        playerControllerScript.gameOver = false;
        playerControllerScript.topPanel.SetActive(true);
    }

    public void RestartGame() {
        Physics.gravity = new Vector3(0, -9.8f, 0); //default gravity
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void Home() {
        Physics.gravity = new Vector3(0, -9.8f, 0); //default gravity
        SceneManager.LoadScene("Main Menu");

    }

    public void QuitGame() {
        Application.Quit();

    }

    public void PauseGame() {
        playerControllerScript.pausedPanel.SetActive(true); //display the pause panel
        playerControllerScript.topPanel.SetActive(false); //undisplay top panel
        gameIsPaused = true;
        Time.timeScale = 0; // pause the game
    }

    public void ResumeGame() {
        playerControllerScript.pausedPanel.SetActive(false); //remove the pause panel
        playerControllerScript.topPanel.SetActive(true); //display top panel
        gameIsPaused = false;
        Time.timeScale = 1; // resume the game

    }
}
