﻿using System.Collections.Generic;
using UnityEngine;

public class Destroyer : MonoBehaviour
{
    private List<IPullObject> forDestroyQueue = new List<IPullObject>();

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<IPullObject>() != null)
        {
            IPullObject pullObject = other.GetComponent<IPullObject>();
            pullObject.Destroy();

            forDestroyQueue.Add(pullObject);
        }

        //if (other.GetComponent<IDestroyable>() != null)
        //{
        //    IDestroyable objectForDestroy = other.GetComponent<IDestroyable>();
        //    objectForDestroy.Destroy();
        //}
    }

    public IPullObject[] GetDestroyedObjects()
    {
        IPullObject[] objects = forDestroyQueue.ToArray();
        forDestroyQueue.Clear();

        return objects;
    }
}
