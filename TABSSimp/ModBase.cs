using UnityEngine;

namespace ModdingForDummies.TABSSimp
{
    public class ModBase : ModdingClass<ModBase>
    {
        public GameObject internalObject { get; private set; }

        public override string Name
        {
            get => internalObject.gameObject.name;
            set => internalObject.gameObject.name = value;
        }

        public override void ColorInternal(int index, Color color, float glow = 0) => Utilities.SetObjectColor(internalObject, index, color, glow);

        public ModBase(GameObject unitBase)
        {
            internalObject = unitBase;
        }

        public override ModBase Clone()
        {
            var newBase = Mod.CloneAndPoolObject(internalObject);
            var result = new ModBase(newBase);
            
            return result;
        }

        public override void Separate() {}

    }
}
