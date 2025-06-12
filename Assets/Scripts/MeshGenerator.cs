using UnityEngine;

public static class MeshGenerator
{
    public static Meshdata GenerateTerrainMesh(float[,] heightMap, float heightMultiplier, AnimationCurve hightCurve, int levelOfDetail)
    {
        int width = heightMap.GetLength(0);
        int hight = heightMap.GetLength(1);

        float topLeftX = (width - 1) / -2f;
        float topLeftZ = (hight - 1) / -2f;

        int meshSimplifiactionIncrement = (levelOfDetail == 0) ? 1 : levelOfDetail * 2;

        int verticesPerLine = (width - 1) / meshSimplifiactionIncrement + 1;

        Meshdata meshdata = new Meshdata(verticesPerLine, verticesPerLine);
        int vertexIndex = 0;

        for (int y = 0; y < hight; y += meshSimplifiactionIncrement)
        {
            for (int x = 0; x < width; x += meshSimplifiactionIncrement)
            {
                meshdata.vertices[vertexIndex] = new Vector3(topLeftX + x, hightCurve.Evaluate(heightMap[x, y]) * heightMultiplier, topLeftZ - y);
                meshdata.uvs[vertexIndex] = new Vector2(x / (float)width, y / (float)hight);

                if (x < width - 1 && y < hight - 1)
                {
                    meshdata.AddTriangle(vertexIndex, vertexIndex + verticesPerLine + 1, vertexIndex + verticesPerLine);
                    meshdata.AddTriangle(vertexIndex + verticesPerLine + 1, vertexIndex, vertexIndex + 1);
                }
                vertexIndex++;
            }
        }return meshdata;
    }
}
public class Meshdata
{
    public Vector3[] vertices;
    public int[] triangles;
    public Vector2[] uvs;

    int triangleIndex;

    public Meshdata(int meshWidth, int meshHight)
    {
        vertices = new Vector3[meshWidth * meshHight];
        uvs = new Vector2[meshWidth * meshHight];
        triangles = new int[(meshWidth - 1) * (meshHight - 1)*6];
    }
    public void AddTriangle(int a, int b, int c)
    {
        triangles[triangleIndex] = a;
        triangles[triangleIndex + 1] = b;
        triangles[triangleIndex + 2] = c;
        triangleIndex += 3;
    }
    public Mesh CreateMesh()
    {
        Mesh mesh = new Mesh();
        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.uv = uvs;
        mesh.RecalculateNormals();
        return mesh;
    }
}
