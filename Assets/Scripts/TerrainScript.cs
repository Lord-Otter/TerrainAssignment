using UnityEngine;


[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
public class TerrainScript : MonoBehaviour
{
    private Terrain terrain;

    [Header("Scale")]
    [SerializeField] private int resolution = 50;
    [SerializeField] private float size = 1000f;
    [SerializeField] private float maxHeight = 250f;

    [Header("Triangles")]
    [SerializeField] private bool flipDiagonal = false;
    [SerializeField] private bool alternateDiagonal = false;
    [SerializeField] private bool randomizeDiagonal = false;

    [Header("Heightmap Settings")]
    [SerializeField] private Texture2D heightMap;
    [SerializeField] private bool useHeightMap = false;
    [SerializeField] private bool normalizeHeightMap = false;

    [Header("Random Terrain Settings")]
    [SerializeField] private int seed = 1024;
    [SerializeField] private float noiseScale = 5f;
    [SerializeField] private Vector2 noiseOffset;
    [SerializeField] private float heightOffset = 50f;

    public void Regenerate()
    {
        if (terrain == null) terrain = new Terrain();

        Mesh mesh = terrain.Regenerate(resolution, size, maxHeight,
                                flipDiagonal, alternateDiagonal, randomizeDiagonal,                        
                                heightMap, useHeightMap, normalizeHeightMap,
                                seed, noiseScale, noiseOffset, heightOffset);
        mesh.name = "TerrainMesh";
        GetComponent<MeshFilter>().mesh = mesh;
    }
    
    void Start()
    {
        Regenerate();
    }

    void Update()
    {
        
    }
}
