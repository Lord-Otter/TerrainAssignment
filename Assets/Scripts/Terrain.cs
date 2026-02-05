using System.Reflection;
using UnityEngine;
using UnityEngine.Rendering.Universal;


public class Terrain
{
    public Mesh Regenerate(int resolution, float size, float maxHeight, 
                    bool flipDiagonal, bool alternateDiagonal, bool randomizeDiagonal, 
                    Texture2D heightMap, bool useHeightMap, bool normalizeHeightMap,
                    int seed, float noiseScale, Vector2 noiseOffset, float heightOffset)
    {
        Mesh mesh = new Mesh();
        mesh.indexFormat = UnityEngine.Rendering.IndexFormat.UInt32;

        int vertsPerLine = resolution + 1;
        int vertexCount = vertsPerLine * vertsPerLine;
        int triangleCount = resolution * resolution * 6;

        Vector3[] vertices = new Vector3[vertexCount];
        int[] triangles = new int[triangleCount];

        float cellSize = size / resolution;
        float halfSize = size * 0.5f;

        Random.InitState(seed);

        float minGray = 1f;
        float maxGray = 0f;
        if (useHeightMap && heightMap != null && normalizeHeightMap)
        {
            Color[] pixels = heightMap.GetPixels();

            foreach (Color px in pixels)
            {
                float gray = px.grayscale;
                if (gray < minGray) minGray = gray;
                if (gray > maxGray) maxGray = gray;
            }

            if (Mathf.Approximately(maxGray, minGray))
            {
                maxGray = minGray + 0.0001f;
            }
        }

        int v = 0;
        for(int z = 0; z <= resolution; z++)
        {
            for(int x = 0; x <= resolution; x++)
            {
                float height;

                if(useHeightMap && heightMap != null)
                {
                    /*float u = (float)x / resolution;
                    float vTex = (float)z / resolution;

                    Color pixel = heightMap.GetPixelBilinear(u, vTex);
                    float gray = pixel.grayScale;*/

                    int px = Mathf.Clamp(Mathf.RoundToInt((float)x / resolution * (heightMap.width - 1)), 0, heightMap.width - 1);
                    int pz = Mathf.Clamp(Mathf.RoundToInt((float)z / resolution * (heightMap.height - 1)), 0, heightMap.height - 1);
                    pz = heightMap.height - 1 - pz; // flip vertically so top of texture is top of mesh

                    Color pixel = heightMap.GetPixel(px, pz);
                    float gray = pixel.grayscale;

                    // normalize if needed
                    float normalized = normalizeHeightMap ? (gray - minGray) / (maxGray - minGray) : gray;

                    height = normalized * maxHeight;
                }
                else
                {
                    float nx = ((float)x / resolution) * noiseScale * 0.99f + noiseOffset.x ;
                    float nz = ((float)z / resolution) * noiseScale * 0.99f + noiseOffset.y ;

                    float perlin = Mathf.PerlinNoise(nx, nz) - 0.5f;
                    height = Mathf.Max(0f, perlin * maxHeight + heightOffset);
                }
      
                //vertices[v++] = new Vector3((x * cellSize) - halfSize, height, (z * cellSize) - halfSize);
                vertices[v++] = new Vector3((x * cellSize) - halfSize, height, halfSize - (z * cellSize));
            }
        }

        int t = 0;
        int vert = 0;

        for(int z = 0; z < resolution; z++)
        {
            for(int x = 0; x < resolution; x++)
            {
                if (alternateDiagonal)
                {
                    flipDiagonal = !flipDiagonal;
                }

                if (randomizeDiagonal)
                {
                    if(Random.Range(0, 2) == 0)
                    {
                        flipDiagonal = !flipDiagonal;
                    }
                }

                if (!flipDiagonal)
                {
                    triangles[t++] = vert;
                    triangles[t++] = vert + 1;
                    triangles[t++] = vert + vertsPerLine;
                    
                    triangles[t++] = vert + vertsPerLine;
                    triangles[t++] = vert + 1;
                    triangles[t++] = vert + vertsPerLine + 1;                    
                }
                else
                {
                    triangles[t++] = vert;
                    triangles[t++] = vert + vertsPerLine + 1;
                    triangles[t++] = vert + vertsPerLine;
                    
                    triangles[t++] = vert;
                    triangles[t++] = vert + 1;
                    triangles[t++] = vert + vertsPerLine + 1;    
                }

                vert++;
            }

            vert++;
        }

        mesh.Clear();
        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.RecalculateNormals();

        return mesh;
    }
}