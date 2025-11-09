using NUnit.Framework;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;


/// <summary>
/// Used to generate some of the steam library assets
/// </summary>
public class SpriteCollageWindow : EditorWindow
{
    [SerializeField] private List<Sprite> sprites = new List<Sprite>();
    private int rows = 4;
    private int columns = 4;

    [MenuItem("Custom/Texture Processing/Sprite Collage Generator")]
    public static void ShowWindow()
    {
        GetWindow<SpriteCollageWindow>("Sprite Collage Generator");
    }

    private void OnGUI()
    {
        GUILayout.Label("Sprite Collage Settings", EditorStyles.boldLabel);

        rows = EditorGUILayout.IntField("Rows", rows);
        columns = EditorGUILayout.IntField("Columns", columns);

        EditorGUILayout.Space();
        SerializedObject so = new SerializedObject(this);
        SerializedProperty sp = so.FindProperty("sprites");
        EditorGUILayout.PropertyField(sp, true);
        so.ApplyModifiedProperties();

        if (GUILayout.Button("Generate Collage"))
        {
            GenerateCollage();
        }
    }

    private void GenerateCollage()
    {
        if (sprites == null || sprites.Count == 0)
        {
            Debug.LogError("No sprites assigned!");
            return;
        }

        sprites = sprites.OrderBy(x => Random.value).ToList(); // Randomize the sprites

        int cellWidth = 128;
        int cellHeight = 128;

        int texWidth = columns * cellWidth;
        int texHeight = rows * cellHeight;

        Texture2D outputTexture = new Texture2D(texWidth, texHeight, TextureFormat.RGBA32, false);
        outputTexture.filterMode = FilterMode.Point;

        // Fill with transparent pixels
        Color[] clearPixels = new Color[texWidth * texHeight];
        for (int i = 0; i < clearPixels.Length; i++) clearPixels[i] = Color.clear;
        outputTexture.SetPixels(clearPixels);

        int index = 0;

        for (int row = 0; row < rows; row++)
        {
            for (int col = 0; col < columns; col++)
            {
                if (index >= sprites.Count)
                    break;

                Sprite sprite = sprites[index];
                Texture2D tex = sprite.texture;
                Rect rect = sprite.rect;

                Color[] pixels = tex.GetPixels(
                    Mathf.RoundToInt(rect.x),
                    Mathf.RoundToInt(rect.y),
                    Mathf.RoundToInt(rect.width),
                    Mathf.RoundToInt(rect.height)
                );
                Assert.AreEqual(cellWidth, rect.width);
                Assert.AreEqual(cellHeight, rect.height);
                Assert.AreEqual(cellWidth * cellHeight, pixels.Length);

                int startX = col * cellWidth;
                int startY = row * cellHeight;

                outputTexture.SetPixels(startX, startY, cellWidth, cellHeight, pixels);
                index++;
            }
        }

        outputTexture.Apply();

        // Save PNG
        string path = EditorUtility.SaveFilePanel("Save Collage as PNG", "Assets", "SpriteCollage.png", "png");
        if (!string.IsNullOrEmpty(path))
        {
            byte[] pngData = outputTexture.EncodeToPNG();
            File.WriteAllBytes(path, pngData);
            Debug.Log("Collage saved to: " + path);

            // Refresh AssetDatabase if inside project
            if (path.StartsWith(Application.dataPath))
            {
                AssetDatabase.Refresh();
            }
        }

        DestroyImmediate(outputTexture);
    }
}