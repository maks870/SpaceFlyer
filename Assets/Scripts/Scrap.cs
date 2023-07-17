using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scrap : MonoBehaviour, IPullObject
{
    [SerializeField] private float angularSpeed;
    [SerializeField] private Mesh[] meshes;
    [SerializeField] private Material[] materials;
    private MeshFilter meshFilter;
    private MeshRenderer meshRenderer;
    private MeshCollider col;

    private Rigidbody rb;
    public bool IsActive { get; set; }

    public GameObject GameObject => gameObject;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        col = rb.GetComponent<MeshCollider>();
        meshFilter = GetComponent<MeshFilter>();
        meshRenderer = GetComponent<MeshRenderer>();
        CreateVisualization();
    }

    void Start()
    {
        InitializeRandomRotation();
    }

    private void CreateVisualization()
    {
        Mesh mesh = meshes[Random.Range(0, meshes.Length)];
        Material material = materials[Random.Range(0, materials.Length)];

        meshFilter.mesh = mesh;
        meshRenderer.material = material;
        col.sharedMesh = mesh;
    }

    private void InitializeRandomRotation()
    {
        Vector3 angularVectorRad = new Vector3(Random.Range(-angularSpeed, angularSpeed) * Mathf.Deg2Rad, Random.Range(-angularSpeed, angularSpeed) * Mathf.Deg2Rad, Random.Range(-angularSpeed, angularSpeed) * Mathf.Deg2Rad);
        rb.angularVelocity = angularVectorRad;
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
        rb.velocity = Vector3.zero;
        CreateVisualization();
        InitializeRandomRotation();
    }
}
