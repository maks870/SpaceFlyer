using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class SpawnSystem : MonoBehaviour
{
    [Space(5)]
    [Header("System settings")]
    [SerializeField] int spawnedAsteroidsCount;
    [SerializeField] float checkPointRadius;
    [SerializeField] Vector2 spawnRange;

    [Space(5)]
    [Header("Spawner settings")]
    [SerializeField] Vector2 spawnersSpawnRange;
    [SerializeField] GameObject asteroidPrefab;
    [SerializeField] private Spawner[] spawners;

    [Space(5)]
    [Header("Destroyer settings")]
    [SerializeField] private Vector3 destroyerSize;
    [SerializeField] private Destroyer[] destroyers;

    private (Destroyer destroyer, Spawner spawner)[] spawnDirections;
    private List<IPullObject> objects = new List<IPullObject>();


    private void Awake()
    {
        CreateSpawnDirections();
        SetUpSpawners();
        SetUpDestroyers();
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
            spawner.CheckPointRadius = checkPointRadius;
            spawner.SpawnRange = spawnersSpawnRange;
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
        for (int i = 0; i < spawnedAsteroidsCount; i++)
        {
            spawnPoint = GetSpawnPoint(spawnRange, transform);

            IPullObject newObject = Instantiate(asteroidPrefab, spawnPoint, Quaternion.identity).GetComponent<IPullObject>();
            objects.Add(newObject);
        }
    }

    public Vector3 GetSpawnPoint(Vector2 spawnRange, Transform spawnTransfrom)
    {
        bool isPointFounded = false;
        Vector3 spawnPoint = Vector3.zero;

        while (!isPointFounded)
        {
            spawnPoint = new Vector3(Random.Range(-spawnRange.x, spawnRange.x) + spawnTransfrom.position.x, spawnTransfrom.position.y, Random.Range(-spawnRange.y, spawnRange.y) + spawnTransfrom.position.z);

            //if (!Physics.CheckSphere(spawnPoint, checkPointRadius, -5, QueryTriggerInteraction.Ignore))
            //    isPointFounded = true;
            if (CheckSpawnPoint(spawnPoint))
                isPointFounded = true;
        }

        return spawnPoint;
    }

    private bool CheckSpawnPoint(Vector3 spawnPoint)
    {
        bool isCorrectSpawnPoint = true;

        foreach (IPullObject newObject in objects)
        {
            if (!newObject.IsActive)
                continue;

            float distance = (spawnPoint - newObject.GameObject.transform.position).magnitude;

            if (distance <= checkPointRadius)
                isCorrectSpawnPoint = false;
        }

        return isCorrectSpawnPoint;
    }

}
