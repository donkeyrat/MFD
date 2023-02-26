using ModdingForDummies.TABSSimp;

namespace ModdingForDummies
{
    public class Main : Mod
    {
        private string comment;
        
        public Main() 
        {
            comment = "CHANGE THIS TO FALSE BEFORE RELEASING YOUR MOD!!!";
            DEV_MODE = true;

            var colonial = CreateFaction("Colonial");
            colonial.Icon = GetIcon("Hillary");
            colonial.Color("#666666");

            var washington = colonial.NewUnit("Washington");
            washington.Health = 800f;
            washington.Cost = 1500;
            washington.Icon = GetIcon("Teacher");
            washington.Size = 1.2f;
            washington.Mass = 20f;
            
            var washingtonSword = CreateWeapon("Washington's Sword", "Teacher Sword");
            washingtonSword.Cooldown = 0.5f;
            washingtonSword.AttackRange = 2f;
            washingtonSword.StartCooldown = false;
            washingtonSword.Damage = 180f;
            washingtonSword.Effect = GetEffect("FireArrowEffect");

            washington.RightWeapon = washingtonSword;
            washington.LeftWeapon = washingtonSword;

            washington.Clothes.Add(
                "TABG carolean hat 00",
                "TABG carolean uniform 00",
                "TABG carolean pants 00",
                "TABG carolean shoes 01",
                "TABG wig 00"
            );

            washington.Moves.Add(
                "Pirate Kick",
                "Teacher Parry",
                "Quickdraw Projectile Dodge",
                "Knight Charge"
            );

            var colonist = colonial.NewUnit("Colonist");
            colonist.Health = 180f;
            colonist.TwoHanded = true;
            colonist.Icon = GetIcon("Musket");

            var colonistExplosion = CreateExplosion("Colonist Explosion", "ExplosionBombCannon");
            colonistExplosion.Radius *= 2;
            colonistExplosion.Size = 2;
            
            var colonistMusket = CreateWeapon("Colonist Musket", "Musket - Bayonet");
            colonistMusket.Explosion = colonistExplosion;
            colonistMusket.Cooldown /= 2f;
            colonistMusket.Ranged.Recoil = 100f;

            colonist.RightWeapon = colonistMusket;

            colonist.Clothes.Add(
                "TABG bicorne hat 00",
                "TABG carolean uniform 00",
                "TABG carolean pants 00",
                "TABG carolean shoes 01"
            );
        }
    }
}
