using System;
using System.Collections.Generic;
using System.Linq;
using Landfall.TABS;
using UnityEngine;
using Object = UnityEngine.Object;

namespace ModdingForDummies.TABSSimp
{
    public class ModUnit : ModdingClass<ModUnit>
    {
        public class ModVoice
        {
            public UnitBlueprint internalObject { get; private set; }

            public ModVoice(UnitBlueprint blueprint)
            {
                internalObject = blueprint;
                internalObject.voiceBundle = ScriptableObject.CreateInstance<VoiceBundle>();
            }

            public string Death
            {
                get => internalObject.voiceBundle.DeathRef;
                set => internalObject.voiceBundle.DeathRef = value;
            }

            public string Alive
            {
                get => internalObject.voiceBundle.VocalRef;
                set => internalObject.voiceBundle.VocalRef = value;
            }

            public float Pitch
            {
                get => internalObject.VoicePitch;
                set => internalObject.VoicePitch = value;
            }
        }
        public UnitBlueprint internalObject;
        public WrapperDelineation<ModClothing> Clothes { get; private set; }

        public WrapperDelineation<ModMove> Moves { get; private set; }

        public ModVoice Voice { get; private set; }

        public override string Name
        {
            get => internalObject.Entity.Name;
            set => internalObject.Entity.Name = value;
        }

        public Sprite Icon
        {
            get => internalObject.Entity.SpriteIcon;
            set => internalObject.Entity.SetSpriteIcon(value);
        }

        public float Health
        {
            get => internalObject.health;
            set => internalObject.health = value;
        }

        public float Speed
        {
            get => internalObject.movementSpeedMuiltiplier;
            set => internalObject.movementSpeedMuiltiplier = value;
        }

        public float TurnSpeed
        {
            get => internalObject.TurnSpeed;
            set
            {
                if (internalObject.turningData != null) internalObject.turningData.TurnSpeed = value;
                internalObject.turnSpeed = value;
            }
        }

        public float Mass
        {
            get => internalObject.massMultiplier;
            set => internalObject.massMultiplier = value;
        }

        public float Balance
        {
            get => internalObject.balanceMultiplier;
            set => internalObject.balanceMultiplier = value;
        }

        public float BalanceStrength
        {
            get => internalObject.balanceForceMultiplier;
            set => internalObject.balanceForceMultiplier = value;
        }

        public float Priority
        {
            get => internalObject.targetingPriorityMultiplier;
            set => internalObject.targetingPriorityMultiplier = value;
        }

        public float Size
        {
            get => internalObject.sizeMultiplier;
            set => internalObject.sizeMultiplier = value;
        }

        public bool TwoHanded
        {
            get => internalObject.holdinigWithTwoHands;
            set => internalObject.holdinigWithTwoHands = value;
        }


        public int Cost
        {
            get => (int)internalObject.GetUnitCost();
            set
            {
                internalObject.useCustomCost = true;
                internalObject.customCost = value;
            }
        }


        private ModWeapon rightWeapon;
        public ModWeapon RightWeapon
        {
            get => rightWeapon;
            set
            {
                if(value != null)
                {
                    rightWeapon = value;
                    internalObject.RightWeapon = rightWeapon.internalObject;
                }
            }
        }

        private ModWeapon leftWeapon;
        public ModWeapon LeftWeapon
        {
            get => leftWeapon;
            set
            {
                if(value != null)
                {
                    leftWeapon = value;
                    internalObject.LeftWeapon = leftWeapon.internalObject;
                }
            }
        }

        private ModBase UnitBase;
        public ModBase Base
        {
            get => UnitBase;
            set
            {
                UnitBase = value;
                internalObject.UnitBase = UnitBase.internalObject;
            }
        }

        public Vector2 SizeRange
        {
            get => new Vector2(internalObject.minSizeRandom, internalObject.maxSizeRandom);

            set
            {
                internalObject.minSizeRandom = value.x;
                internalObject.maxSizeRandom = value.y;
            }
        }

        private ModUnit rider;
        public ModUnit Rider
        {
            get => rider;
            set 
            {
                rider = value;
                if(rider != null) internalObject.SetField("Riders", new[] { rider.internalObject });
                else internalObject.SetField("Riders", Array.Empty<UnitBlueprint>());
            }
        }


        private void UpdateClothing(List<ModClothing> list) => internalObject.m_props = (from ModClothing clothing in list select clothing.internalObject).ToArray();

        private void UpdateMoves(List<ModMove> list) => internalObject.objectsToSpawnAsChildren = (from ModMove move in list select move.internalObject).ToArray();

        public override void ColorInternal(int index, Color color, float glow = 0) { }

        public ModUnit(UnitBlueprint blueprint)
        {
            internalObject = blueprint;
            Clothes = new WrapperDelineation<ModClothing>(UpdateClothing, Mod.GetClothing);
            Moves = new WrapperDelineation<ModMove>(UpdateMoves, Mod.GetMove);
            Voice = new ModVoice(blueprint);

            if (internalObject.m_props != null)
            {
                Clothes.List = (from GameObject prop in internalObject.m_props where prop != null select new ModClothing(prop)).ToList();
            }
            else internalObject.m_props = Array.Empty<GameObject>();

            if (internalObject.RightWeapon) RightWeapon = new ModWeapon(internalObject.RightWeapon);
            if (internalObject.LeftWeapon) LeftWeapon = new ModWeapon(internalObject.LeftWeapon);
        }

        public override ModUnit Clone()
        {
            string name = Utilities.IncrementName(Name);
            var unit = Object.Instantiate(internalObject);
            unit.Entity.GUID = DatabaseID.NewID();
            unit.Entity.Name = name;
            Utilities.AddUnitToDatabase(unit);
            ModUnit clone = new ModUnit(unit)
            {
                Clothes =
                {
                    List = Clothes.List
                },
                Moves =
                {
                    List = Moves.List
                },
                Rider = Rider,
                Cost = Cost
            };
            clone.Separate();
            return clone;
        }

        public override void Separate()
        {
            if (RightWeapon != null) RightWeapon = RightWeapon.Clone();
            if (LeftWeapon != null) LeftWeapon = LeftWeapon.Clone();
            Clothes.List = (from ModClothing clothing in Clothes.List select clothing.Clone()).ToList();
            Moves.List = (from ModMove move in Moves.List select move.Clone()).ToList();
        }
    }
}
