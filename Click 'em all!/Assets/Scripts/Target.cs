using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Handles behaviour of all Targets including their starting position and force
/// </summary>
public class Target : MonoBehaviour
{
    [SerializeField] private ParticleSystem explosionParticles;
    [SerializeField] private int pointValue;
    private Rigidbody targetRb;
    private GameManager gameManager;
    private float spawnXRange = 8.0f;
    private float spawnY = -2.0f;
    private float minSpeed = 12.0f;
    private float maxSpeed = 16.0f;
    private float torqueRange = 10.0f;

    void Awake()
    {
        targetRb = GetComponent<Rigidbody>();
        gameManager = GameObject.Find("Game Manager").GetComponent<GameManager>();
        transform.position = randomSpawnPosition();
        targetRb.AddForce(randomForce(), ForceMode.Impulse);
        targetRb.AddTorque(randomTorque(), randomTorque(), randomTorque(), ForceMode.Impulse);
    }

    private void OnMouseDown()
    {
        if (gameManager.isGameActive)
        {
            if (!gameObject.CompareTag("Bad"))
            {
                gameManager.PlayTargetSound();
            }
            else
            {
                gameManager.PlayBadSound();
            }
            Instantiate(explosionParticles, transform.position, explosionParticles.transform.rotation);
            gameManager.UpdateScore(pointValue);
            explosionParticles.Play();
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        Destroy(gameObject);
        if (!gameObject.CompareTag("Bad") && gameManager.isGameActive)
        {
            gameManager.GameOver();
        }
    }

    private Vector3 randomSpawnPosition()
    {
        return new Vector3(Random.Range(-spawnXRange, spawnXRange), spawnY);
    }

    private Vector3 randomForce()
    {
        return Vector3.up * Random.Range(minSpeed, maxSpeed);
    }

    private float randomTorque()
    {
        return Random.Range(-torqueRange, torqueRange);
    }
}
