using UnityEngine;
using UnityEditor;
using System.IO;

// Disable import of materials if the file contains
// the @ sign marking it as an animation.
public class Example : AssetPostprocessor
{
    public const string END_FILE_NAME = " - POT";
    public const string EXSTENSION = ".png";

    void OnPreprocessModel()
    {
        if (assetPath.Contains("@"))
        {
            ModelImporter modelImporter = assetImporter as ModelImporter;
            modelImporter.materialImportMode = ModelImporterMaterialImportMode.None;
        }

        Debug.Log($"assetImporter.assetBundleName {assetImporter.assetBundleName}");
        Debug.Log($"assetImporter.assetBundleVariant {assetImporter.assetBundleVariant}");
        Debug.Log($"assetImporter.assetPath {assetImporter.assetPath}");
        Debug.Log($"assetImporter.assetTimeStamp {assetImporter.assetTimeStamp}");
        Debug.Log($"assetImporter.name {assetImporter.name}");
        Debug.Log("context");
        Debug.Log(context);
    }

    static void OnPostprocessAllAssets(string[] importedAssets, string[] deletedAssets, string[] movedAssets, string[] movedFromAssetPaths, bool didDomainReload)
    {
        //foreach (string str in importedAssets)
        //{
        //    Debug.Log("Reimported Asset: " + str);
        //}
        //foreach (string str in deletedAssets)
        //{
        //    Debug.Log("Deleted Asset: " + str);
        //}

        //for (int i = 0; i < movedAssets.Length; i++)
        //{
        //    Debug.Log("Moved Asset: " + movedAssets[i] + " from: " + movedFromAssetPaths[i]);
        //}

        //if (didDomainReload)
        //{

        //}
        Debug.Log($"Domain has been reloaded = {didDomainReload}");
    }

    void OnPostprocessSprites(Texture2D texture, Sprite[] sprites)
    {
        if (texture.name.EndsWith(END_FILE_NAME)) return;
        Debug.Log("Sprites: " + sprites.Length);
        string path = "Assets/POT/" + texture.name + END_FILE_NAME + EXSTENSION;
        for (int i = 0; i < sprites.Length; i++)
        {
            SaveFile(sprites[i], (int)sprites[i].rect.width * 2, (int)sprites[i].rect.height * 2, path);
        }
    }

    public void SaveFile(Sprite sprite, int newWidth, int newHeight, string savePath)
    {
        //Sprite itemBGSprite = Resources.Load<Sprite>("_Defaults/Item Images/_Background");
        var rect = sprite.rect;
        rect.width *= 2;
        rect.height *= 2;
        //sprite.rect = rect;
        
        //Graphics.Blit(originalTexture, rt);
        //Texture2D itemBGTex = sprite.texture;
        //byte[] itemBGBytes = itemBGTex.EncodeToPNG();
        //File.WriteAllBytes(savePath, itemBGBytes);
        Texture2D itemBGTex = sprite.texture;
        byte[] itemBGBytes = itemBGTex.EncodeToPNG();
        File.WriteAllBytes(savePath, itemBGBytes);

        Sprite newSprite = Sprite.Create(sprite.texture, rect, Vector2.zero);
        itemBGBytes = newSprite.texture.EncodeToPNG();
        File.WriteAllBytes(savePath + ".png", itemBGBytes);

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

    void OnPostprocessTexture(Texture2D texture)
    {
        Debug.Log("Texture2D: (" + texture.width + "x" + texture.height + ")");
    }
}