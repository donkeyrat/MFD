using Landfall.TABS;
using UnityEngine;

namespace ModdingForDummies.TABSSimp
{
    public enum ClothingType
    {
        Static,
        Skinned
    }
    public class ModClothing : ModdingClass<ModClothing>
    {
        public GameObject internalObject { get; private set; }

        private PropItem item;

        private SkinnedMeshRenderer renderer;

        public ClothingType Type { get; private set; }

        public override string Name
        {
            get => item.Entity.Name;
            set => item.Entity.Name = value;
        }

        private ModModel model;

        public ModModel Model
        {
            get => model;
            set
            {
                if (value != null)
                {
                    Utilities.SetMeshRenderers(internalObject, false);
                    model = value.Clone();

                    if (Type == ClothingType.Static)
                    {
                        model.internalObject.transform.parent = internalObject.transform;
                        model.internalObject.transform.localPosition = Vector3.zero;
                        model.internalObject.transform.localRotation = Quaternion.Euler(-90f, 0f, 0f);
                    }
                    else if (Type == ClothingType.Skinned)
                    {
                        Utilities.PlaceMeshOnBone(renderer, model.internalObject, item.GearT);
                    }
                }
                else
                {
                    if (model != null) Utilities.SetMeshRenderers(internalObject, true);
                    model = null;
                }
            }
        }

        public float Size
        {
            get
            {
                var scale = internalObject.transform.localScale;
                return (scale.x + scale.y + scale.z) / 3f;
            }
            set => Utilities.SetClothScale(this, Vector3.one * value);
        }

        public Vector3 Scale
        {
            get => internalObject.transform.localScale;
            set => Utilities.SetClothScale(this, value);
        }
        
        public override void ColorInternal(int index, Color color, float glow = 0f)
        {
            if (model == null)
            {
                Utilities.SetObjectColor(internalObject, index, color, glow);
            }
            else
            {
                model.Color(index, color, glow);
            }
        }

        public ModClothing(GameObject prop)
        {
            internalObject = Mod.CloneAndPoolObject(prop);
            renderer = internalObject.GetComponentInChildren<SkinnedMeshRenderer>();
            Type = renderer != null ? ClothingType.Skinned : ClothingType.Static;
            item = internalObject.GetComponentInChildren<PropItem>();
        }

        public override ModClothing Clone()
        {
            var result = new ModClothing(internalObject)
            {
                Model = Model
            };
            result.Separate();
            return result;
        }

        public override void Separate() { }
    }
}