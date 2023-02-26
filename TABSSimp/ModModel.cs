using UnityEngine;

namespace ModdingForDummies.TABSSimp
{
    public class ModModel: ModdingClass<ModModel>
    {
        public GameObject internalObject { get; private set; }

        public override string Name
        {
            get => internalObject.gameObject.name;
            set => internalObject.gameObject.name = value;
        }

        private Vector3 position;
        public Vector3 Position
        {
            get => position;
            set
            {
                position = value;
                internalObject.transform.localPosition = position;
            }
        }

        private Quaternion rotation;

        public Quaternion Rotation
        {
            get => rotation;
            set
            {
                rotation = value;
                internalObject.transform.localRotation = Utilities.blenderToUnity * rotation;
            }
        }

        public override void ColorInternal(int index, Color color, float glow = 0) => Utilities.SetObjectColor(internalObject, index, color, glow);

        public ModModel(GameObject model)
        {
            internalObject = model;
        }

        public override ModModel Clone()
        {
            var newEffect = Mod.CloneAndPoolObject(internalObject);
            var result = new ModModel(newEffect);
            
            return result;
        }

        public override void Separate() {}

    }
}
