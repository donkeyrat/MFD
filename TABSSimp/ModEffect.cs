using UnityEngine;

namespace ModdingForDummies.TABSSimp
{
    public class ModEffect : ModdingClass<ModEffect>
    {
        public UnitEffectBase internalObject { get; private set; }

        public override string Name
        {
            get => internalObject.gameObject.name;
            set => internalObject.gameObject.name = value;
        }

        public override void ColorInternal(int index, Color color, float glow = 0) { }

        public ModEffect(UnitEffectBase effect)
        {
            internalObject = effect;
        }

        public override ModEffect Clone()
        {
            var newEffect = Mod.CloneAndPoolObject(internalObject.gameObject);
            var result = new ModEffect(newEffect.GetComponent<UnitEffectBase>());
            
            return result;
        }

        public override void Separate() {}

    }
}
