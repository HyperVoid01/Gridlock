using UnityEngine;

[RequireComponent(typeof(MeshFilter))]
public class MeshGenerator : MonoBehaviour
{
    Mesh mesh;
    MeshCollider meshCollider;
    
    Vector3[] vertices;
    int[] triangles;
    
    Vector2[] uvs;
    Color[] colors;

    public int xSize = 20;
    public int zSize = 20;
    public float yMultiplier = 2f;
    public float sizeSpread = 0.3f;
    
    public float xOffset = 0f;
    public float zOffset = 0f;
    
    public Gradient gradient;
    
    public float minTerrainHeight;
    public float maxTerrainHeight;

    public GameObject startPosition;
    public float speed;

    void Start()
    {
        gameObject.transform.position = new Vector3(-xSize / 2, 0, -zSize / 2);
        mesh = new Mesh();
        GetComponent<MeshFilter>().mesh = mesh;
        meshCollider = GetComponent<MeshCollider>();
        
        CreateShape();
        UpdateMesh();
    }

    void Update()
    {
        // Temporary ----
        // CreateShape();
        // UpdateMesh();
        // ------

        xOffset += speed * Time.deltaTime;
        zOffset += speed * Time.deltaTime;
        CreateShape();
        UpdateMesh();
        
        transform.position = startPosition.transform.position;
    }

    void CreateShape()
    {
        vertices = new Vector3[(xSize + 1) * (zSize + 1)];
        
        // xOffset = Random.Range(0, 9999);
        // zOffset = Random.Range(0, 9999);
        
        for (int i = 0, z = 0; z <= zSize; z++)
        {
            for (int x = 0; x <= xSize; x++)
            {
                float y = Mathf.PerlinNoise(x * sizeSpread + xOffset, z * sizeSpread + zOffset) * yMultiplier;
                vertices[i] = new  Vector3(x, y, z);
                
                if (y > maxTerrainHeight)
                    maxTerrainHeight = y;
                if (y < minTerrainHeight)
                    minTerrainHeight = y;
                
                i++;
            }
        }

        triangles = new int[xSize * zSize * 6];
        
        int vert = 0;
        int tris = 0;

        for (int z = 0; z < zSize; z++)
        {
            for (int x = 0; x < xSize; x++)
            {
                triangles[tris + 0] = vert + 0;
                triangles[tris + 1] = vert + xSize + 1;
                triangles[tris + 2] = vert + 1;
                triangles[tris + 3] = vert + 1;
                triangles[tris + 4] = vert + xSize +1;
                triangles[tris + 5] = vert + xSize + 2;

                vert++;
                tris += 6;
            }

            vert++;
        }
        
        uvs = new Vector2[vertices.Length];
        colors = new Color[vertices.Length];
        
        for (int i = 0, z = 0; z <= zSize; z++)
        {
            for (int x = 0; x <= xSize; x++)
            {
                //uvs[i] = new Vector2((float)x / xSize, (float)z / zSize);
                
                float height = Mathf.InverseLerp(minTerrainHeight, maxTerrainHeight, vertices[i].y);
                colors[i] = gradient.Evaluate(height);
                i++;
            }
        }
    }

    void UpdateMesh()
    {
        mesh.Clear();
        
        mesh.vertices = vertices;
        mesh.triangles = triangles;
        //mesh.uv = uvs;
        mesh.colors = colors;

        meshCollider.sharedMesh = mesh;
        
        mesh.RecalculateNormals();
    }

    // private void OnDrawGizmos()
    // {
    //     if (vertices == null)
    //         return;
    //     
    //     for (int i = 0; i < vertices.Length; i++)
    //     {
    //         Gizmos.DrawSphere(vertices[i], 0.1f);
    //     }
    // }
}
