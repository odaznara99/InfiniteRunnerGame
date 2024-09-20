using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class GameManager : MonoBehaviour
{
    public float score;
    private PlayerController playerControllerScript;

    public Transform startingPoint;
    public float lerpSpeed;

    public TextMeshProUGUI scoreText;

    public bool gameIsPaused;
    void Start()
    {
        playerControllerScript = GameObject.Find("Player").GetComponent<PlayerController>();
        score = 0;

        playerControllerScript.gameOver = true;
        StartCoroutine(PlayIntro());

    }

    // Update is called once per frame
    void Update()
    {
        if (!playerControllerScript.gameOver  && !gameIsPaused)

        {
            if (playerControllerScript.doubleSpeed)
            {
                score += 2;
            }
            else {
                score++;
            }
            //Debug.Log("Score: " + score);
            scoreText.text = "Score: " + score.ToString(); //display the score to the UI text

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
