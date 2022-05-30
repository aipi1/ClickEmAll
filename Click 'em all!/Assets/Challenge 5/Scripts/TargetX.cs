using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetX : MonoBehaviour
{
    private GameManagerX gameManagerX;
    private float spaceBetweenSquares = 2.5f;
    private float minValueX = -3.75f; //  x value of the center of the left-most square
    private float minValueY = -3.75f; //  y value of the center of the bottom-most square
    public GameObject explosionFx;
    public int pointValue;
    public float timeOnScreen = 1.0f;

    void Start()
    {
        gameManagerX = GameObject.Find("Game Manager").GetComponent<GameManagerX>();
        transform.position = RandomSpawnPosition();
        StartCoroutine(RemoveObjectRoutine()); // begin timer before target leaves screen
    }

    // Generate a random spawn position based on a random index from 0 to 3
    Vector3 RandomSpawnPosition()
    {
        float spawnPosX = minValueX + (RandomSquareIndex() * spaceBetweenSquares);
        float spawnPosY = minValueY + (RandomSquareIndex() * spaceBetweenSquares);

        Vector3 spawnPosition = new Vector3(spawnPosX, spawnPosY, 0);
        return spawnPosition;

    }

    // Generates random square index from 0 to 3, which determines which square the target will appear in
    int RandomSquareIndex()
    {
        return Random.Range(0, 4);
    }

    // When target is clicked, destroy it, update score, and generate explosion
    private void OnMouseDown()
    {
        if (gameManagerX.isGameActive)
        {
            gameManagerX.UpdateScore(pointValue);
            Explode();
            Destroy(gameObject);
        }
               
    }

    // If target that is NOT the bad object collides with sensor, trigger game over
    private void OnTriggerEnter(Collider other)
    {
        if (!gameObject.CompareTag("Bad"))
        {
            gameManagerX.GameOver();
        }
        Destroy(gameObject);
    }

    // Display explosion particle at object's position
    void Explode ()
    {
        Instantiate(explosionFx, transform.position, explosionFx.transform.rotation);
    }

    // After a delay, Moves the object behind background so it collides with the Sensor object
    IEnumerator RemoveObjectRoutine ()
    {
        yield return new WaitForSeconds(timeOnScreen);
        transform.Translate(Vector3.forward * 5, Space.World);
    }
}
