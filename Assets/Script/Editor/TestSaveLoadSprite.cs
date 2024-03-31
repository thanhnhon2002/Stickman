using System.IO;
using UnityEngine;

public class TestSaveLoadSprite : MonoBehaviour
{
    public Sprite[] sprites;

    [ContextMenu("Test save load file")]
    public void ConvertAll()
    {
        for (int i = 0; i < sprites.Length; i++)
        {
            string path = "Assets/POT/" + sprites[i].name + " - POT.png";
            SaveFile(sprites[i], sprites[i].rect.width, sprites[i].rect.height, path);
        }
    }


    public void SaveFile(Sprite sprite, float newWidth, float newHeight, string savePath)
    {
        //Sprite itemBGSprite = Resources.Load<Sprite>("_Defaults/Item Images/_Background");
        Texture2D itemBGTex = sprite.texture;
        byte[] itemBGBytes = itemBGTex.EncodeToPNG();
        File.WriteAllBytes(savePath, itemBGBytes);
        //// Get the texture of the sprite
        //Texture2D originalTexture = sprite.texture;

        ////// Create a new texture with the desired dimensions
        ////Texture2D resizedTexture = new Texture2D(newWidth, newHeight);

        ////// Resize the texture
        ////RenderTexture rt = RenderTexture.GetTemporary(newWidth, newHeight);
        ////RenderTexture.active = rt;
        ////Graphics.Blit(originalTexture, rt);
        ////resizedTexture.ReadPixels(new Rect(0, 0, newWidth, newHeight), 0, 0);
        ////resizedTexture.Apply();
        //originalTexture.width = originalTexture.width * 2;
        //originalTexture.height = originalTexture.height * 2;

        //Graphics.Blit(originalTexture, rt);

        //// Encode the resized texture into PNG format
        //byte[] bytes = originalTexture.EncodeToPNG();

        //// Write the encoded bytes to a file
        //File.WriteAllBytes(savePath, bytes);

        //// Clean up temporary objects
        ////RenderTexture.active = null;
        ////RenderTexture.ReleaseTemporary(rt);
        ////Destroy(resizedTexture);
    }

}