using Landfall.TABS.UnitEditor;
using UnityEngine;

namespace ModdingForDummies.TABSSimp
{
    public class ModMove : ModdingClass<ModMove>
    {
        public GameObject internalObject { get; private set; }

        private SpecialAbility specialAbility;

        public override string Name
        {
            get => specialAbility.Entity.Name;
            set => specialAbility.Entity.Name = value;
        }

        public override void ColorInternal(int index, Color color, float glow = 0) { }

        public ModMove(GameObject move)
        {
            internalObject = move;
            specialAbility = move.GetComponentInChildren<SpecialAbility>();
        }

        public override ModMove Clone()
        {
            var newMove = Mod.CloneAndPoolObject(internalObject);
            var result = new ModMove(newMove);
            
            return result;
        }

        public override void Separate() {}

    }
}
