using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public interface IDestroyable
{
    public void Destroy();
}

public class AsteroidFragment : MonoBehaviour, IDestroyable
{
    private float destroyTime;
    private float lifetime = 0f;
    private float scalingFactor;
    private bool isDecreasing = false;
    private Rigidbody rb;

    public float DestroyTime { set => destroyTime = value; }
    public float ScalingFactor { set => scalingFactor = value; }
    private bool IsDecreasing
    {
        get => isDecreasing;
        set
        {
            lifetime = 0f;
            isDecreasing = value;
        }
    }


    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        SetMeshCollider();
    }

    void Start()
    {
        Deactivate();
    }

    // Update is called once per frame
    void Update()
    {
        if (IsDecreasing)
            DecreaseOverTime();
    }

    private void DecreaseOverTime()
    {
        lifetime += Time.deltaTime;

        if (lifetime < destroyTime)
        {
            float scale = 1 - Mathf.InverseLerp(0, destroyTime, lifetime);
            transform.localScale = new Vector3(scale, scale, scale) * scalingFactor;
        }
        else
        {
            Destroy();
        }
    }

    public void SetMeshCollider()
    {
        GetComponent<MeshCollider>().sharedMesh = GetComponent<MeshFilter>().mesh;
    }

    public void Activate()
    {
        rb.isKinematic = false;
        rb.detectCollisions = true;
        IsDecreasing = true;
    }

    public void Deactivate()
    {
        IsDecreasing = false;
        transform.localScale = new Vector3(1, 1, 1);
        rb.isKinematic = true;
        rb.detectCollisions = false;
        gameObject.SetActive(true);
    }

    public void Destroy()
    {
        gameObject.SetActive(false);
        IsDecreasing = false;
        rb.isKinematic = true;
        transform.localScale = new Vector3(1, 1, 1);
    }
}
