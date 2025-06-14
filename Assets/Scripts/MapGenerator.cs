using UnityEngine;

public class MapGenerator : MonoBehaviour
{
    public enum DrawMode {NoiseMap, ColourMap, Mesh,FallOfMap };
    public DrawMode drawMode;

    const int meshChunkSize = 239 ;

    [Range(0, 6)]
    public int levelOfDetail;
    public float noisescale;
    public float meshHeightMultiplier;
    public AnimationCurve meshHeightCurve;

    public int octaves;
    [Range(0, 1)]
    public float persistance;
    public float lacunarity;

    public int seed;
    public Vector2 offset;

    public bool ApplyFallOfMap;

    public bool AutoUpdate;

    public TerrainType[] regions;
    float[,] fallofmap;

    private void Awake()
    {
        fallofmap = FallOffGenerator.GenerateFallOffMap(meshChunkSize);
    }

    public void GenerateMap() {
        float[,] noiseMap = Noise.GenrateNosieMap(meshChunkSize + 2, meshChunkSize + 2, noisescale, seed, octaves, persistance, lacunarity, offset);

        Color[] colorMap = new Color[meshChunkSize * meshChunkSize];

        for (int y = 0; y < meshChunkSize; y++)
        {
            for(int x = 0; x < meshChunkSize; x++)
            {
                if (ApplyFallOfMap)
                {
                    noiseMap[x, y] = Mathf.Clamp01(noiseMap[x, y] - fallofmap[x, y]);
                }
                float currentHeight = noiseMap[x, y];
                for(int i = 0; i < regions.Length; i++)
                {
                    if(currentHeight <= regions[i].height)
                    {
                        colorMap[y * meshChunkSize + x] = regions[i].colour;

                        break;
                    }
                }
            }
        }
        MapDisplay display = FindObjectOfType<MapDisplay>();
        if (drawMode == DrawMode.NoiseMap)
        {
            display.DrawTexture(TextureGenerator.TextureFromHightMap(noiseMap));
        }else if(drawMode == DrawMode.ColourMap)
        {
            display.DrawTexture(TextureGenerator.TextureFromColorMap(colorMap, meshChunkSize, meshChunkSize));
        }else if (drawMode == DrawMode.Mesh)
        {
            display.DrawMesh(MeshGenerator.GenerateTerrainMesh(noiseMap, meshHeightMultiplier, meshHeightCurve, levelOfDetail), TextureGenerator.TextureFromColorMap(colorMap, meshChunkSize, meshChunkSize));
        }else if(drawMode == DrawMode.FallOfMap)
        {
            display.DrawTexture(TextureGenerator.TextureFromHightMap(FallOffGenerator.GenerateFallOffMap(meshChunkSize)));
        }
     }
    private void OnValidate()
    {

        if (lacunarity<1)
        {
            lacunarity = 1;
        }
        if (octaves < 0)
        {
            octaves = 0;
        }
        fallofmap = FallOffGenerator.GenerateFallOffMap(meshChunkSize);
    }
}
[System.Serializable]
public struct TerrainType
{
    public string name;
    public float height;
    public Color colour;
}
