using UnityEngine;

public class MeshCombiner
{
    private Transform asteroid;
    private MeshFilter[] meshFilters;
    private CombineInstance[] combine;

    public MeshCombiner(Transform asteroid)
    {
        this.asteroid = asteroid;
        meshFilters = asteroid.GetComponentsInChildren<MeshFilter>();
        combine = new CombineInstance[meshFilters.Length];
    }

    public void CombineMeshes()
    {
        Matrix4x4 myTransform = asteroid.transform.worldToLocalMatrix;

        for (int i = 0; i < meshFilters.Length; i++)
        {
            combine[i].mesh = meshFilters[i].sharedMesh;
            combine[i].transform = myTransform * meshFilters[i].transform.localToWorldMatrix;
        }

        Mesh mesh = new Mesh();
        mesh.CombineMeshes(combine);
        asteroid.transform.GetComponent<MeshCollider>().sharedMesh = mesh;
    }

}
