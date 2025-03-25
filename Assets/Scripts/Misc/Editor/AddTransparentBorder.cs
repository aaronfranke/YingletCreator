using UnityEngine;
using UnityEditor;
using System.IO;

/// <summary>
/// Unity UI gets a really annoying jagged edge if the border is flush with the side
/// This script adds a small little 1px transparent border to the side of the selected asset
/// so it doesn't look so awful
/// Logic is chat-gpt generated
/// </summary>
public class AddTransparentBorder : EditorWindow
{
    [MenuItem("Custom/Add Transparent Border to .png", false, 2000)]
    private static void AddBorder()
    {
        string path = AssetDatabase.GetAssetPath(Selection.activeObject);
        if (string.IsNullOrEmpty(path) || !path.EndsWith(".png"))
        {
            Debug.LogError("Please select a PNG asset.");
            return;
        }

        TextureImporter importer = AssetImporter.GetAtPath(path) as TextureImporter;
        if (importer != null)
        {
            importer.isReadable = true;
            importer.SaveAndReimport();
        }

        Texture2D originalTexture = AssetDatabase.LoadAssetAtPath<Texture2D>(path);
        if (originalTexture == null)
        {
            Debug.LogError("Failed to load texture.");
            return;
        }

        int width = originalTexture.width;
        int height = originalTexture.height;
        Texture2D newTexture = new Texture2D(width + 2, height + 2, TextureFormat.RGBA32, false);

        // Fill with transparent pixels
        Color32[] pixels = new Color32[(width + 2) * (height + 2)];
        for (int i = 0; i < pixels.Length; i++)
        {
            pixels[i] = new Color32(0, 0, 0, 0);
        }
        newTexture.SetPixels32(pixels);

        // Copy the original image into the center
        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                newTexture.SetPixel(x + 1, y + 1, originalTexture.GetPixel(x, y));
            }
        }

        newTexture.Apply();

        // Save the new texture
        byte[] pngData = newTexture.EncodeToPNG();
        string newPath = Path.GetDirectoryName(path) + "/" + Path.GetFileNameWithoutExtension(path) + ".png";
        File.WriteAllBytes(newPath, pngData);

        AssetDatabase.Refresh();
        Debug.Log("Transparent border added and saved as: " + newPath);
    }
}
