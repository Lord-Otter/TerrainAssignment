using UnityEngine;


[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
public class TerrainScript : MonoBehaviour
{
    private Terrain terrain;

    [Header("Scale")]
    [SerializeField] private int resolution = 50;
    [SerializeField] private float size = 1000f;
    [SerializeField] private float maxHeight = 250f;

    [Header("Texture Mapping")]
    [SerializeField] private Texture2D albedoTexture;
    [SerializeField] Vector2 textureSizeUV = new Vector2(50, 50);
    [SerializeField] [Range(0, 360)] private float textureRotationOffset = 0f;

    [Header("Height Coloring")]
    [SerializeField] private Color lowColor = Color.blue;
    [SerializeField] private Color midColor = Color.yellow;
    [SerializeField] private Color highColor = Color.red;
    [SerializeField] private bool enableHeightColors = false;
    [SerializeField] private bool enableColorSmoothing = false;
    [SerializeField] private float lowLevelHeight = 50f;
    [SerializeField] private float midLevelHeight = 150f;

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

        MeshRenderer renderer = GetComponent<MeshRenderer>();

        if (renderer.sharedMaterial == null)
        {
            Material mat = new Material(Shader.Find("Shader Graphs/TerrainVertexColorLit"));
            renderer.sharedMaterial = mat;
        }

        
        renderer.sharedMaterial.mainTexture = albedoTexture;

        if (albedoTexture != null)
        {
            renderer.sharedMaterial.SetTexture("_BaseMap", albedoTexture);
            renderer.sharedMaterial.mainTexture.wrapMode = TextureWrapMode.Repeat;
        }

        Mesh mesh = terrain.Regenerate(resolution, size, maxHeight,
                                albedoTexture, textureSizeUV, textureRotationOffset,
                                lowColor, midColor, highColor, enableColorSmoothing, enableHeightColors, lowLevelHeight, midLevelHeight,
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
