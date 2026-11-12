using UnityEngine;

public class ObstacleSpawner : MonoBehaviour
{
    public Transform player;
    public Sprite[] obstacleSprites;
    public float minGap = 6f;
    public float maxGap = 10f;
    public float spawnDistance = 25f;
    public float cleanupDistance = 40f;

    private float rightEdge;
    private float leftEdge;

    private void Start()
    {
        rightEdge = player.position.x + 12f;
        leftEdge = player.position.x - 12f;
    }

    private void Update()
    {
        if (GameManager.Instance != null && GameManager.Instance.IsGameOver) return;

        while (rightEdge < player.position.x + spawnDistance)
        {
            Spawn(rightEdge);
            rightEdge += Random.Range(minGap, maxGap);
        }
        while (leftEdge > player.position.x - spawnDistance)
        {
            Spawn(leftEdge);
            leftEdge -= Random.Range(minGap, maxGap);
        }

        foreach (var obstacle in GameObject.FindGameObjectsWithTag("Obstacle"))
        {
            if (Mathf.Abs(obstacle.transform.position.x - player.position.x) > cleanupDistance)
            {
                Destroy(obstacle);
            }
        }
    }

    private void Spawn(float x)
    {
        if (obstacleSprites == null || obstacleSprites.Length == 0) return;

        var sprite = obstacleSprites[Random.Range(0, obstacleSprites.Length)];
        var go = new GameObject("Obstacle");
        go.tag = "Obstacle";
        go.transform.position = new Vector3(x, -2.0f + sprite.bounds.extents.y, 0f);

        var sr = go.AddComponent<SpriteRenderer>();
        sr.sprite = sprite;
        sr.sortingOrder = 1;

        var box = go.AddComponent<BoxCollider2D>();
        box.size = sprite.bounds.size;
    }
}
