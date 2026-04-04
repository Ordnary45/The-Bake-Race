using UnityEngine;

public class BurgerRain : MonoBehaviour
{
    public GameObject burgerPrefab;
    public float spawnRate = 3f; // burgers per second
    public Vector3 areaSize = new Vector3(10, 1, 10);

    private float timer;

    void Update()
    {
        timer += Time.deltaTime;

        if (timer >= 1f / spawnRate)
        {
            SpawnBurger();
            timer = 0f;
        }
    }

    void SpawnBurger()
    {
        Vector3 randomPos = transform.position + new Vector3(
            Random.Range(-areaSize.x / 2, areaSize.x / 2),
            Random.Range(-areaSize.y / 2, areaSize.y / 2),
            Random.Range(-areaSize.z / 2, areaSize.z / 2)
        );

        Instantiate(burgerPrefab, randomPos, Quaternion.identity);
    }
}