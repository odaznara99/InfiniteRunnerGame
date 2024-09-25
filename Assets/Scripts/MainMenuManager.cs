using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    private float highScore;
    public TextMeshProUGUI highScoreText;
    public void StartGame() {
        SceneManager.LoadScene("Stage1");
        

    }

    private void Start()
    {
        highScore = PlayerPrefs.GetFloat("HighScore", 0);
        highScoreText.text = "Highscore: " + highScore.ToString();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.Return)) {
            StartGame();
        
        }

       
    }
}
