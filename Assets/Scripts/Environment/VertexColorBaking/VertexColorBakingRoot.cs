using System.Linq;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

// Inspired by Zololgo's VertexDirt plugin
// https://assetstore.unity.com/packages/tools/level-design/vertexdirt-vertex-ambient-occlusion-21015

public class VertexColorBakingRoot : MonoBehaviour
{
    [SerializeField]
    public VertexColorBakingSettings _settings;

}