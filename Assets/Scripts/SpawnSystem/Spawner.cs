using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class Spawner : MonoBehaviour
{

    private float checkPointRadius;
    private Vector2 spawnRange;
    private List<IPullObject> forSpawnQueue = new List<IPullObject>();
    private SpawnSystem spawnSystem;


    public float CheckPointRadius { set => checkPointRadius = value; }
    public Vector2 SpawnRange { set => spawnRange = value; }
    public SpawnSystem SpawnSystem { set => spawnSystem = value; }

    private void Update()
    {
        Spawn();
    }

    //private Vector3 CalculateSpawnPoint()
    //{
    //    bool isPointFounded = false;
    //    Vector3 spawnPoint = Vector3.zero;

    //    while (!isPointFounded)
    //    {
    //        spawnPoint = new Vector3(Random.Range(-spawnRange.x, spawnRange.x) + transform.position.x, transform.position.y, Random.Range(-spawnRange.y, spawnRange.y) + transform.position.z);

    //        if (!Physics.CheckSphere(spawnPoint, checkPointRadius, -5, QueryTriggerInteraction.Ignore))
    //            isPointFounded = true;
    //    }

    //    return spawnPoint;
    //}

    private void Spawn()
    {
        for (int i = 0; i < forSpawnQueue.Count; i++)
        {
            IPullObject obj = forSpawnQueue[i];
            Vector3 spawnPosition = spawnSystem.GetSpawnPoint(spawnRange, transform);

            obj.GameObject.transform.position = spawnPosition;
            obj.ReCreate();
            forSpawnQueue.Remove(obj);
        }
    }

    public void AddSpawnObjects(IPullObject[] objects)
    {
        forSpawnQueue.AddRange(objects);
    }
}
