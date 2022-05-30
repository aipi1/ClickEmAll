using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using System.IO;

/// <summary>
/// Manages Targets spawning as well as the game flow
/// </summary>
public class GameManager : MonoBehaviour
{
    [HideInInspector] public bool isGameActive;
    [SerializeField] private AudioClip buttonSound;
    [SerializeField] private AudioClip targetSound;
    [SerializeField] private AudioClip badSound;
    [SerializeField] private AudioClip gameOverSound;
    [SerializeField] private List<GameObject> targets;
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private TextMeshProUGUI finalScoreText;
    [SerializeField] private TextMeshProUGUI bestScoreText;
    [SerializeField] private GameObject gameOverScreen;
    [SerializeField] private GameObject titleScreen;
    private AudioSource audioSource;
    private int score;
    private int bestScore;
    private float scoreMultiplier;
    private float spawnRate = 1.0f;
    private bool isSoundCorRunning = false;

    void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    private IEnumerator SpawnTargets()
    {
        while (isGameActive)
        {
            yield return new WaitForSeconds(spawnRate);
            int index = Random.Range(0, targets.Count);
            Instantiate(targets[index]);
        }
    }

    public void UpdateScore(int scoreToAdd)
    {
        score += (int)Mathf.Round(scoreToAdd * scoreMultiplier);
        scoreText.text = "Score: " + score;
    }

    public void GameOver()
    {
        audioSource.Stop();
        audioSource.PlayOneShot(gameOverSound);
        isGameActive = false;
        scoreText.gameObject.SetActive(false);
        finalScoreText.text = "Your final score: " + score;
        LoadBestScore();
        if (score > bestScore)
        {
            SaveBestScore();
            bestScoreText.text = "Your best score: " + score;
        }
        else
        {
            bestScoreText.text = "Your best score: " + bestScore;
        }
        gameOverScreen.gameObject.SetActive(true);
    }
    
    public void RestartGame()
    {
        if (!isSoundCorRunning)
        {
            // Make sure the whole sound is played before proceeding
            StartCoroutine(RestartAfterSound());
        }
    }

    private IEnumerator RestartAfterSound()
    {
        isSoundCorRunning = true;
        audioSource.PlayOneShot(buttonSound);
        yield return new WaitWhile(() => audioSource.isPlaying);

        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void StartGame(float difficultyLevel)
    {
        audioSource.PlayOneShot(buttonSound);
        audioSource.Play();
        spawnRate /= difficultyLevel;
        scoreMultiplier = difficultyLevel;
        isGameActive = true;
        score = 0;
        titleScreen.gameObject.SetActive(false);
        scoreText.gameObject.SetActive(true);
        StartCoroutine(SpawnTargets());
        UpdateScore(0);
    }

    public void PlayTargetSound()
    {
        audioSource.PlayOneShot(targetSound);
    }

    public void PlayBadSound()
    {
        audioSource.PlayOneShot(badSound);
    }

    [System.Serializable]
    private class SaveData
    {
        public int bestScore;
    }

    public void SaveBestScore()
    {
        SaveData data = new SaveData();
        data.bestScore = score;

        string json = JsonUtility.ToJson(data);

        File.WriteAllText(Application.persistentDataPath + "/cea_savefile.json", json);
    }

    public void LoadBestScore()
    {
        string path = Application.persistentDataPath + "/cea_savefile.json";
        if (File.Exists(path))
        {
            string json = File.ReadAllText(path);
            SaveData data = JsonUtility.FromJson<SaveData>(json);

            bestScore = data.bestScore;
        }
    }
}
