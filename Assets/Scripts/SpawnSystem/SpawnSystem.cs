using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class SpawnSystem : MonoBehaviour
{
    [Space(5)]
    [Header("System settings")]
    [SerializeField] int spawnedScrapCount;
    [SerializeField] int spawnedAsteroidsCount;
    [SerializeField] float asteroidcheckRadius;
    [SerializeField] float scrapCheckRadius;
    [SerializeField] Vector2 spawnRange;
    [SerializeField] Player player;

    [Space(5)]
    [Header("Spawner settings")]
    [SerializeField] Vector2 spawnersSpawnRange;
    [SerializeField] GameObject scrapPrefab;
    [SerializeField] GameObject[] asteroidPrefabs;
    [SerializeField] private Spawner[] spawners;

    [Space(5)]
    [Header("Destroyer settings")]
    [SerializeField] private Vector3 destroyerSize;
    [SerializeField] private Destroyer[] destroyers;

    private int maxSpawnCallBacks;
    private (Destroyer destroyer, Spawner spawner)[] spawnDirections;
    private List<IPullObject> objects = new List<IPullObject>();


    private void Awake()
    {
        CreateSpawnDirections();
        SetUpSpawners();
        SetUpDestroyers();

        maxSpawnCallBacks = (spawnedScrapCount + spawnedAsteroidsCount) / spawners.Length;
    }

    void Start()
    {
        InitializeObjects();
    }

    void Update()
    {
        foreach (var direction in spawnDirections)
        {
            HandleDirection(direction);
        }
    }

    private void CreateSpawnDirections()
    {
        spawnDirections = new (Destroyer, Spawner)[4];

        spawnDirections[0].destroyer = destroyers[0];
        spawnDirections[1].destroyer = destroyers[1];
        spawnDirections[2].destroyer = destroyers[2];
        spawnDirections[3].destroyer = destroyers[3];

        spawnDirections[0].spawner = spawners[2];
        spawnDirections[1].spawner = spawners[3];
        spawnDirections[2].spawner = spawners[0];
        spawnDirections[3].spawner = spawners[1];
    }

    private void SetUpSpawners()
    {
        foreach (Spawner spawner in spawners)
        {
            spawner.AsteroidSpawnRange = spawnersSpawnRange;
            spawner.SpawnSystem = this;
        }
    }

    private void SetUpDestroyers()
    {
        foreach (Destroyer destroyer in destroyers)
        {
            destroyer.GetComponent<BoxCollider>().size = destroyerSize;
        }

        destroyers[0].transform.localPosition = destroyerSize.x / 2 * Vector3.forward;
        destroyers[1].transform.localPosition = destroyerSize.x / 2 * Vector3.right;
        destroyers[2].transform.localPosition = destroyerSize.x / 2 * Vector3.back;
        destroyers[3].transform.localPosition = destroyerSize.x / 2 * Vector3.left;
    }

    private void HandleDirection((Destroyer destroyer, Spawner spawner) direction)
    {
        IPullObject[] objects = direction.destroyer.GetDestroyedObjects();
        direction.spawner.AddSpawnObjects(objects);
    }

    private void InitializeObjects()
    {
        Vector3 spawnPoint;

        objects.Add(player);

        for (int i = 0; i < spawnedAsteroidsCount; i++)
        {
            GameObject prefab = asteroidPrefabs[Random.Range(0, asteroidPrefabs.Length)];
            bool isInitializationPoissible = GetSpawnPoint(asteroidcheckRadius, spawnRange, transform, out spawnPoint);

            if (isInitializationPoissible)
            {
                IPullObject newObject = Instantiate(prefab, spawnPoint, Quaternion.identity).GetComponent<IPullObject>();
                objects.Add(newObject);
            }
        }

        for (int i = 0; i < spawnedScrapCount; i++)
        {

            bool isInitializationPoissible = GetSpawnPoint(scrapCheckRadius, spawnRange, transform, out spawnPoint);

            if (isInitializationPoissible)
            {
                IPullObject newObject = Instantiate(scrapPrefab, spawnPoint, Quaternion.identity).GetComponent<IPullObject>();
                objects.Add(newObject);
            }
        }
    }

    public bool GetSpawnPoint(float checkpointRadius, Vector2 spawnRange, Transform spawnTransfrom, out Vector3 spawnPoint)
    {
        bool isPointFounded = false;
        spawnPoint = Vector3.zero;

        int emptySpawnCallback = 0;

        while (emptySpawnCallback < maxSpawnCallBacks)
        {
            spawnPoint = new Vector3(Random.Range(-spawnRange.x, spawnRange.x) + spawnTransfrom.position.x, spawnTransfrom.position.y, Random.Range(-spawnRange.y, spawnRange.y) + spawnTransfrom.position.z);

            if (CheckSpawnPoint(checkpointRadius, spawnPoint))
            {
                isPointFounded = true;
                break;
            }
            else
            {
                emptySpawnCallback++;
            }
        }

        return isPointFounded;
    }

    private bool CheckSpawnPoint(float checkpointRadius, Vector3 spawnPoint)
    {
        bool isCorrectSpawnPoint = true;

        foreach (IPullObject newObject in objects)
        {
            if (!newObject.IsActive)
                continue;

            float distance = (spawnPoint - newObject.GameObject.transform.position).magnitude;

            if (distance <= checkpointRadius)
                isCorrectSpawnPoint = false;
        }

        return isCorrectSpawnPoint;
    }
}

