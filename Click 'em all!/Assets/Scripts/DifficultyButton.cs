using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Controls what happens when difficulty button is pressed
/// </summary>
public class DifficultyButton : MonoBehaviour
{
    public float difficultyLevel;
    private Button difficultyButton;
    private GameManager gameManager;

    void Awake()
    {
        difficultyButton = GetComponent<Button>();
        gameManager = GameObject.Find("Game Manager").GetComponent<GameManager>();

        difficultyButton.onClick.AddListener(SetDifficulty);
    }

    private void SetDifficulty()
    {
        gameManager.StartGame(difficultyLevel);
    }
}
