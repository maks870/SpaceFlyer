using System;
using UnityEngine;

public interface IPullObject
{
    public bool IsActive { get; set; }
    public GameObject GameObject { get; }

    public void ReCreate();

    public void Destroy();
}
