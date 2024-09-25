using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameOverDisplay : MonoBehaviour
{
    public TextMeshProUGUI scoreText;
    public GameObject newHighScoreText;
    private GameManager gameManager;

    public float lerpDuration = 2.0f; // Duration of the lerp

    // Start is called before the first frame update
    void Start()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();

    }

    void OnEnable()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();

        newHighScoreText.SetActive(gameManager.setNewHighScore);

        // When the panel is enabled, start the coroutine to lerp the score
        Debug.Log("Lerp CurrentScore: "+gameManager.GetCurrentScore());
        StartCoroutine(LerpScore(0, gameManager.GetCurrentScore(), lerpDuration));
    }

    IEnumerator LerpScore(float startScore, float targetScore, float duration)
    {
        float elapsedTime = 0f;
        
        while (elapsedTime < duration)
        {
            // Calculate the current score by linearly interpolating between startScore and targetScore
            float currentScore = Mathf.Lerp(startScore, targetScore, elapsedTime / duration);
            scoreText.text = Mathf.FloorToInt(currentScore).ToString(); // Update the UI display
            //Text = Mathf.FloorToInt(currentScore).ToString(); // Update the UI display
            elapsedTime += Time.deltaTime;
            yield return null; // Wait until the next frame
        }

        // Ensure the final score is set when the lerp is done
        scoreText.text = Mathf.FloorToInt(targetScore).ToString();
    }
}
