using System;

namespace ModdingForDummies.AssetManagement
{
    [Serializable]
    public sealed class SpriteAsset
    {   
        public string name;
        public byte[] data;

        public SpriteAsset(string name, byte[] data)
        {
            this.name = name;
            this.data = data;
        }
    }
}