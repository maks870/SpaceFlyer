using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour, IPullObject
{
    public bool IsActive { get; set; }

    public GameObject GameObject => gameObject;

    private void Awake()
    {
        IsActive = true;
    }

    public void Destroy()
    {
        gameObject.SetActive(false);
        IsActive = false;
    }

    public void ReCreate()
    {
        gameObject.SetActive(true);
        IsActive = true;
    }
}
