using UnityEngine;

public class MapDisplay : MonoBehaviour
{
    public Renderer texturerender;
    public MeshFilter meshfilter;
    public MeshRenderer meshrenderer;

    public void DrawTexture(Texture2D texture)
    {
        texturerender.sharedMaterial.mainTexture = texture;
        texturerender.transform.localScale = new Vector3(texture.width, 1, texture.height);
    }
    public void DrawMesh(Meshdata meshData , Texture2D texture)
    {
        meshfilter.sharedMesh = meshData.CreateMesh();
        meshrenderer.sharedMaterial.mainTexture = texture;
    }
}
