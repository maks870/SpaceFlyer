using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class Spawner : MonoBehaviour
{
    private float asteroidCheckRadius;
    private float scrapCheckRadius;
    private Vector2 scrapSpawnRange;
    private Vector2 asteroidSpawnRange;
    private List<IPullObject> forSpawnQueue = new List<IPullObject>();
    private SpawnSystem spawnSystem;

    public Vector2 ScrapSpawnRange { get => scrapSpawnRange; set => scrapSpawnRange = value; }
    public Vector2 AsteroidSpawnRange { set => asteroidSpawnRange = value; }
    public SpawnSystem SpawnSystem { set => spawnSystem = value; }

    private void Update()
    {
        Spawn();
    }

    private void Spawn()
    {
        float checkRadius = 1;
        Vector3 spawnPosition = Vector3.zero;
        for (int i = 0; i < forSpawnQueue.Count; i++)
        {
            IPullObject obj = forSpawnQueue[i];

            if (obj.GetType() == typeof(Asteroid))
                checkRadius = asteroidCheckRadius;

            if (obj.GetType() == typeof(Scrap))
                checkRadius = scrapCheckRadius;

            bool isInitializationPoissible = spawnSystem.GetSpawnPoint(checkRadius, asteroidSpawnRange, transform, out spawnPosition);

            if (isInitializationPoissible)
            {
                obj.GameObject.transform.position = spawnPosition;
                obj.ReCreate();
                forSpawnQueue.Remove(obj);
            }
        }
    }

    public void AddSpawnObjects(IPullObject[] objects)
    {
        forSpawnQueue.AddRange(objects);
    }
}
