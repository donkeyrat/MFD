using System;

namespace ModdingForDummies.AssetManagement
{
    [Serializable]
    public sealed class ModAssetBundle
    {
        public ModelAsset[] models;

        public SpriteAsset[] sprites;
    }
}