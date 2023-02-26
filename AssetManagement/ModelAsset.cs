using System;
using UnityEngine;

namespace ModdingForDummies.AssetManagement
{
    [Serializable]
    public sealed class ModelAsset
    {
        public string name;
        public Vector3[] vertices;
        public Vector3[] normals;

        public Vector2[] textures;
        
        public Color[] materials;

        public int[][] submeshes;
    }
}