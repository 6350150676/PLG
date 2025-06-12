using UnityEngine;

public static class Noise 
{
    public static float[,] GenrateNosieMap(int mapwidth, int mapHeight, float scale, int seed,int octaves, float persistance, float lacunarity ,Vector2 offset)
    {
        float[,] noiseMap = new float[mapwidth, mapHeight];

        System.Random prng = new System.Random(seed);
        Vector2[] octaveOffsets = new Vector2[octaves];

        for(int i = 0; i < octaves; i++)
        {
            float offsetX = prng.Next(-100000, 100000) + offset.x;
            float offsetY = prng.Next(-100000, 100000) + offset.y;
            octaveOffsets[i] = new Vector2(offsetX, offsetY);
        }

        if ( scale <= 0)
        {
            scale = 0.0001f;
        }

        float maxNoiseHeight = float.MinValue;
        float minNoiseHeight = float.MaxValue;

        float halfwidth = mapwidth / 2f;
        float halfhight = mapHeight / 2f;

        for (int y = 0; y < mapHeight; y++)
        {
            for(int x= 0; x < mapwidth; x++)
            {
                float amplitude = 1;
                float frequency = 1;
                float noiseHeight = 0;

                for(int i =0; i < octaves; i++)
                {
                    float sampleX = (x-halfwidth) / scale * frequency +  octaveOffsets[i].x;
                    float sampleY = (y - halfhight) / scale * frequency + octaveOffsets[i].y;

                    float perlinvalue = Mathf.PerlinNoise(sampleX, sampleY) * 2 - 1;
                    noiseHeight += perlinvalue * amplitude;

                    amplitude *= persistance;
                    frequency *= lacunarity;
                }

                if (noiseHeight < minNoiseHeight)
                {
                    minNoiseHeight = noiseHeight;
                }
                if (noiseHeight > maxNoiseHeight)
                {
                    maxNoiseHeight = noiseHeight;
                }



                noiseMap[x, y] = noiseHeight;
            }
        }

        for (int y = 0; y < mapHeight; y++)
        {
            for (int x = 0; x < mapwidth; x++)
            {
                noiseMap[x, y] = Mathf.InverseLerp(minNoiseHeight, maxNoiseHeight, noiseMap[x, y]);
            }
        }
                return noiseMap;
    }
}
