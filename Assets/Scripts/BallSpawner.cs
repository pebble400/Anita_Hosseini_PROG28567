using System.Collections;
using UnityEngine;

public class BallSpawner : MonoBehaviour
{
    public GameObject ballPrefab;
    public int ballSpawnCount = 100;
    public float spawnSpawnInterval = 0.3f;
    public bool randomColours = true;
    public int forceAmount = 0;

    IEnumerator Start()
    {
        for (int i = 0; i < ballSpawnCount; i++)
        {
            GameObject ball = Instantiate(ballPrefab, transform.position, Quaternion.identity, transform);

            Rigidbody2D body2D = ball.GetComponent<Rigidbody2D>();
            body2D.AddForce(Random.insideUnitCircle.normalized * forceAmount, ForceMode2D.Impulse);

            if (randomColours)
            
                ball.GetComponent<SpriteRenderer>().color = new Color(Random.value, Random.value, Random.value);
            
            yield return new WaitForSeconds(spawnSpawnInterval);
        }
    }

}
