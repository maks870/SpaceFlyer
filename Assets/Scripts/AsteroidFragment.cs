using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AsteroidFragment : MonoBehaviour
{
    [SerializeField] private Rigidbody rb;

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

    }

    public void SetMeshCollider()
    {
        GetComponent<MeshCollider>().sharedMesh = GetComponent<MeshFilter>().mesh;
    }

    public void Activate()
    {
        rb.isKinematic = false;
        rb.detectCollisions = true;
    }

    public void Deactivate()
    {
        rb.isKinematic = true;
        rb.detectCollisions = false;
    }
}
