using UnityEngine;
using UnityEditor;
using System.IO;
using System.Collections.Generic;

// Disable import of materials if the file contains
// the @ sign marking it as an animation.
public class Example : AssetPostprocessor
{
    public const string END_FILE_NAME = " - POT";
    public const string EXSTENSION = ".png";

    public static List<int> dimensions = null;

    public void Init()
    {
        dimensions = new List<int>();
        for (int i = 0; i < 20; i++)
        {
            dimensions[i] = (int)Mathf.Pow(2, i);
        }
    }

    void OnPostprocessSprites(Texture2D texture, Sprite[] sprites)
    {
        if (texture.name.EndsWith(END_FILE_NAME)) return;
        string path = "Assets/POT/" + texture.name + END_FILE_NAME + EXSTENSION;
        for (int i = 0; i < sprites.Length; i++)
        {
            SaveFile(sprites[i], GetPOTSize((int)sprites[i].rect.width), GetPOTSize((int)sprites[i].rect.height), path);
        }
    }

    public int GetPOTSize(int number)
    {
        if (dimensions == null) Init();

        for (int i = 0; i < dimensions.Count; i++)
        {
            if (number < dimensions[i]) return dimensions[i];
        }
        Debug.LogError($"number too big to put to POT");
        return number;
    }

    public void SaveFile(Sprite sprite, int newWidth, int newHeight, string savePath)
    {
        //Sprite itemBGSprite = Resources.Load<Sprite>("_Defaults/Item Images/_Background");
        var rect = sprite.rect;
        rect.width *= 2;
        rect.height *= 2;
        //sprite.texture.Resize(newWidth, newHeight);
        Texture2D resizedTexture = new Texture2D(newWidth, newHeight);
        resizedTexture.ReadPixels(sprite.rect, 0,0);

        //sprite.rect = rect;

        //Graphics.Blit(originalTexture, rt);
        //Texture2D itemBGTex = sprite.texture;
        //byte[] itemBGBytes = itemBGTex.EncodeToPNG();
        //File.WriteAllBytes(savePath, itemBGBytes);

        //GOOD - DA CHAY DUOC
        //Texture2D itemBGTex = sprite.texture;
        //byte[] itemBGBytes = itemBGTex.EncodeToPNG();
        //File.WriteAllBytes(savePath, itemBGBytes);

        //Sprite newSprite = Sprite.Create(sprite.texture, rect, Vector2.zero);
        //itemBGBytes = newSprite.texture.EncodeToPNG();
        //File.WriteAllBytes(savePath + ".png", itemBGBytes);
        //END


        Texture2D texture = new Texture2D(newWidth, newHeight);
        //Sprite newSprite = Sprite.Create(texture, new Rect(0, 0, newWidth, newHeight), new Vector2(0, 0), 1);
        for (int i = 0; i < texture.width; i++)
        {
            for (int j = 0; j < texture.height; j++)
            {
                if (i < sprite.texture.width && j < sprite.texture.height)
                {
                    texture.SetPixel(i, j, sprite.texture.GetPixel(i,j));
                } else
                {
                    texture.SetPixel(i, j, Color.clear);
                }
            }
        }
        texture.Apply();
        //texture.SetPixel(0, 0, Color.blue);
        //RenderTexture render = new RenderTexture(newWidth, newHeight,1);
        //Graphics.Blit(sprite.texture, render);

        //Graphics.DrawTexture()

        byte[] itemBGBytes = texture.EncodeToPNG();
        File.WriteAllBytes(savePath, itemBGBytes);

        //RectTransform rt = newSprite.GetComponent<RectTransform>();

        //string url = "";//image url;
        //WWW image = new WWW(url);
        //yield return image;
        //Texture2D texture = new Texture2D(1, 1);
        //image.LoadImageIntoTexture(texture);
        //Sprite newSprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0, 0), 1);
        //RectTransform rt = newSprite.GetComponent<RectTransform>();
        //rt.sizeDelta = new Vector2(20, 20);//make 20px * 20px sprite


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


//public Texture2D stamp;
//public Texture2D background;
//public Texture2D output;

////location to stamp
//public int locationx = 0;
//public int locationy = 0;
//void Start()
//{
//    int x, y;

//    //make a copy of your background
//    output = new Texture2D(background.width, background.height);
//    x = output.width; y = output.height;
//    while (x > 0)
//    {
//        x--;
//        y = output.height;
//        while (y > 0)
//        {
//            y--;
//            output.SetPixel(x, y, background.GetPixel(x, y));
//        }
//    }


//    // page through all your pixels of stamp
//    x = stamp.width; y = stamp.height;
//    while (x > 0)
//    {
//        x--;
//        y = stamp.height;
//        while (y > 0)
//        {
//            y--;
//            if (x + locationx < background.width && y + locationy < background.height)
//            {
//                Color cs = stamp.GetPixel(x, y);
//                Color cb = background.GetPixel(x + locationx, y + locationy);
//                float a = cs.a;//<---alph of the stamp pixel;
//                               // mix colors based on alpha channel
//                float r = (cs.r * a) + (cb.r * (1 - a));
//                float g = (cs.g * a) + (cb.g * (1 - a));
//                float b = (cs.b * a) + (cb.b * (1 - a));

//                output.SetPixel(x + locationx, y + locationy, new Color(r, g, b, 1));

//            }
//        }
//    }
//    output.Apply();


//}