using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class LevelDisplay : MonoBehaviour
{
    public TextMeshProUGUI levelText;
    private GameManager gameManager;
    // Start is called before the first frame update
    void Start()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        //levelText.text = "Level " + gameManager.currentLevel.ToString();

    }

    private void OnEnable()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        //levelText.text = "Level " + gameManager.currentLevel.ToString();
    }

    // Update is called once per frame
    void Update()
    {
        levelText.text = "Level " +gameManager.currentLevel.ToString();
    }
}
