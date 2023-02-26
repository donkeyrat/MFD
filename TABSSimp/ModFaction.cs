using System;
using System.Collections.Generic;
using System.Linq;
using Landfall.TABS;
using UnityEngine;
using Object = UnityEngine.Object;

namespace ModdingForDummies.TABSSimp
{
    public class ModFaction : ModdingClass<ModFaction>
    {
        public Faction internalObject { get; private set; }

        public WrapperDelineation<ModUnit> Units { get; private set; }

        public UnitBlueprint[] Blueprints => Units.List.Select(w => w.internalObject).ToArray();

        public override string Name
        {
            get => internalObject.Entity.Name;
            set => internalObject.Entity.Name = value;
        }

        public bool Visible
        {
            get => internalObject.m_displayFaction;
            set => internalObject.m_displayFaction = value;
        }

        public Sprite Icon
        {
            get => internalObject.Entity.SpriteIcon;
            set => internalObject.Entity.SetSpriteIcon(value);
        }

        private void UpdateBlueprints(List<ModUnit> units) => internalObject.Units = (from ModUnit unit in units select unit.internalObject).ToArray();
        

        public ModUnit NewUnit(string name, string originalName = null)
        {
            var unit = Mod.CreateUnit(name, originalName);
            Units.Add(unit);

            return unit;
        }
        
        public override void ColorInternal(int index, Color color, float glow = 0) => internalObject.m_FactionColor = color;

        public ModFaction(Faction faction)
        {
            internalObject = faction;
            Units = new WrapperDelineation<ModUnit>(UpdateBlueprints, Mod.GetUnit)
            {
                List = (from UnitBlueprint blueprint in internalObject.Units select new ModUnit(blueprint)).ToList()
            };
        }

        public override ModFaction Clone()
        {
            var faction = Object.Instantiate(internalObject);
            faction.Entity.GenerateNewID();
            faction.Entity.Name = Utilities.IncrementName(Name);
            faction.Units = Array.Empty<UnitBlueprint>();
            faction.index = Utilities.database.GetFactions().ToList().Count;
            Utilities.AddFactionToDatabase(faction);
            var result = new ModFaction(faction);
            result.Separate();
            
            return result;
        }

        public override void Separate()
        {
            Units.List = (from ModUnit unit in Units.List select unit.Clone()).ToList();
        }

    }
}
