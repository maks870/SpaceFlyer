using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Asteroid : MonoBehaviour, IPullObject
{
    [SerializeField] private float fragmentsLifetime;
    [SerializeField] private float angularSpeed;
    private MeshCombiner meshCombiner;
    private Rigidbody rb;
    private MeshCollider col;
    private AsteroidFragment[] fragments;
    private (Vector3 position, Quaternion rotation)[] startTransforms;

    public bool IsActive { get; set; }
    public GameObject GameObject => gameObject;

    private void OnCollisionEnter(Collision collision)
    {
        if (!collision.gameObject.GetComponent<Player>())
            return;

        Collapse();
    }

    private void Awake()
    {
        IsActive = true;
        meshCombiner = new MeshCombiner(transform);
        fragments = GetComponentsInChildren<AsteroidFragment>();
        rb = GetComponent<Rigidbody>();
        col = GetComponent<MeshCollider>();

        meshCombiner.CombineMeshes();
    }

    void Start()
    {
        SetUpAsteroidFragments();
        InitializeRandomRotation();
    }

    private void Update()
    {
        if (Input.GetKey(KeyCode.Space))
            ReCreate();
    }

    private void SetUpAsteroidFragments()
    {
        foreach (AsteroidFragment fragment in fragments)
        {
            fragment.DestroyTime = fragmentsLifetime;
            fragment.ScalingFactor = transform.localScale.x;
        }

        SaveStartTransforms();
    }

    private void SaveStartTransforms()
    {
        startTransforms = new (Vector3, Quaternion)[fragments.Length];

        for (int i = 0; i < fragments.Length; i++)
        {
            startTransforms[i].position = fragments[i].transform.localPosition;
            startTransforms[i].rotation = fragments[i].transform.localRotation;
        }
    }

    private void InitializeRandomRotation()
    {
        Vector3 angularVectorRad = new Vector3(Random.Range(-angularSpeed, angularSpeed) * Mathf.Deg2Rad, Random.Range(-angularSpeed, angularSpeed) * Mathf.Deg2Rad, Random.Range(-angularSpeed, angularSpeed) * Mathf.Deg2Rad);
        rb.angularVelocity = angularVectorRad;
    }

    private void Collapse()
    {
        IsActive = false;
        col.isTrigger = true;
        rb.detectCollisions = false;

        foreach (AsteroidFragment fragment in fragments)
        {
            fragment.transform.parent = null; ;
            fragment.Activate();
        }
    }

    public void ReCreate()
    {
        gameObject.SetActive(true);
        IsActive = true;
        col.isTrigger = false;
        col.enabled = false;
        rb.angularVelocity = Vector3.zero;
        transform.rotation = Quaternion.Euler(Vector3.zero);

        for (int i = 0; i < fragments.Length; i++)
        {
            fragments[i].Deactivate();

            if (fragments[i].transform.parent == null)
                fragments[i].transform.parent = transform;

            fragments[i].transform.localPosition = startTransforms[i].position;
            fragments[i].transform.localRotation = Quaternion.identity;
        }

        col.enabled = true;
        InitializeRandomRotation();
    }

    public void Destroy()
    {
        gameObject.SetActive(false);
    }
}
