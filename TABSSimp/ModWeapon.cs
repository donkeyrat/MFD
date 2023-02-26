using Landfall.TABS;
using UnityEngine;

namespace ModdingForDummies.TABSSimp
{
    public enum WeaponType
    {
        Melee,
        Ranged
    }

    public class ModWeapon : ModdingClass<ModWeapon>
    {
        public GameObject internalObject { get; private set; }

        private WeaponItem item;
        
        private Weapon weapon;

        private RangeWeapon internalRanged;
        
        private MeleeWeapon internalMelee;

        private CollisionWeapon mCollision;

        private MeleeWeaponSpawn mSpawner;

        private MeleeWeaponAddEffect mAddEffect;
        

        public override string Name
        {
            get => item.Entity.Name;
            set => item.Entity.Name = value;
        }

        public WeaponType Type { get; private set; }

        public float Cooldown
        {
            get => weapon.internalCooldown;
            set => weapon.internalCooldown = value;
        }

        public float AttackSpeed
        {
            get => 1f / weapon.internalCooldown;
            set => weapon.internalCooldown = 1f / value;
        }
        

        public float AttackRange
        {
            get => weapon.maxRange;
            set => weapon.maxRange = value;
        }

        public float AttackAngle
        {
            get => weapon.maxAngle;
            set => weapon.maxAngle = value;
        }

        public bool StartCooldown
        {
            get => weapon.startOnCooldown;
            set => weapon.startOnCooldown = value;
        }
        
        public float Damage
        {
            get
            {
                if (Type == WeaponType.Melee) return mCollision.damage;
                if (Type == WeaponType.Ranged) return Ranged.Projectile.Damage;
                return 0f;
            }
            set
            {
                if (Type == WeaponType.Melee) mCollision.damage = value;
                else if (Type == WeaponType.Ranged) Ranged.Projectile.Damage = value;
            }
        }
        
        public float Force
        {
            get
            {
                if (Type == WeaponType.Melee) return mCollision.onImpactForce;
                if (Type == WeaponType.Ranged) return projectile.Force;
                return 0f;
            }
            set
            {
                if (Type == WeaponType.Melee) mCollision.onImpactForce = value;
                else if (Type == WeaponType.Ranged) projectile.Force = value;
            }
        }
        
        private ModEffect effect;
        public ModEffect Effect
        {
            get
            {
                if (Type == WeaponType.Melee)
                {
                    return effect;
                }
                if (Type == WeaponType.Ranged)
                {
                    return Ranged.Projectile.Effect;
                }

                return null;
            }
            set
            {
                effect = value;
                if (Type == WeaponType.Melee)
                {
                    effect = value;
                
                    if (!mAddEffect) mAddEffect = internalObject.AddComponent<MeleeWeaponAddEffect>();
                    
                    bool effectValid = (effect != null && effect.internalObject);

                    if (effectValid) mAddEffect.EffectPrefab = effect.internalObject;
                    else mAddEffect.EffectPrefab = null;

                    mAddEffect.enabled = effectValid;
                }
                else if (Type == WeaponType.Ranged)
                {
                    Ranged.Projectile.Effect = value;
                }
            }
        }


        private ModExplosion explosion;

        public ModExplosion Explosion
        {
            get
            {
                if(Type == WeaponType.Melee)
                {
                    return explosion;
                }

                if (Type == WeaponType.Ranged)
                {
                    if (projectile != null) return projectile.Explosion;
                }

                return null;
            }
            set
            {
                explosion = value;
                if (Type == WeaponType.Melee)
                {
                    if (!mSpawner)
                    {
                        mSpawner = internalObject.AddComponent<MeleeWeaponSpawn>();
                        mSpawner.pos = MeleeWeaponSpawn.Pos.ContactPoint;
                    }
                    
                    bool explosionValid = explosion != null && explosion.internalObject;

                    if (explosionValid) mSpawner.objectToSpawn = explosion.internalObject;
                    else mSpawner.objectToSpawn = null;

                    mSpawner.enabled = explosionValid;
                }
                else if (Type == WeaponType.Ranged)
                {
                    if (projectile != null) projectile.Explosion = value;
                }
            }
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

                    model.internalObject.transform.parent = internalObject.transform;
                    model.internalObject.transform.localPosition = Vector3.zero;
                    model.internalObject.transform.localRotation = Quaternion.Euler(-90f, 0f, 0f);
                }
                else
                {
                    if (model != null) Utilities.SetMeshRenderers(internalObject, true);
                    model = null;
                }
            }
        }

        public class ModMelee
        {
            private readonly ModWeapon internalWeapon;
            
            public ModMelee(ModWeapon weapon)
            {
                internalWeapon = weapon;
            }
            
            public float SwingSpeed
            {
                get => internalWeapon.Type == WeaponType.Melee ? internalWeapon.internalMelee.curveForce : 0f;
                set
                {
                    if (internalWeapon.Type == WeaponType.Melee) internalWeapon.internalMelee.curveForce = value;
                }
            }
            
            public Vector3 SwingDirection
            {
                get => internalWeapon.Type == WeaponType.Melee ? internalWeapon.internalMelee.swingDirection : Vector3.zero;
                set
                {
                    if (internalWeapon.Type == WeaponType.Melee) internalWeapon.internalMelee.swingDirection = value;
                }
            }
            
            public float Impact
            {
                get => internalWeapon.Type == WeaponType.Melee ? internalWeapon.mCollision.impactMultiplier : 0f;
                set
                {
                    if (internalWeapon.Type == WeaponType.Melee) internalWeapon.mCollision.impactMultiplier = value;
                }
            }
        }

        public ModMelee Melee;
        
        private ModProjectile projectile;
        
        private ModUnit unit;
        
        public class ModRanged
        {
            private readonly ModWeapon internalWeapon;
            
            public ModRanged(ModWeapon weapon)
            {
                internalWeapon = weapon;
            }
            
            public ModProjectile Projectile
            {
                get => internalWeapon.projectile;
                set
                {
                    if (internalWeapon.Type == WeaponType.Ranged)
                    {
                        internalWeapon.projectile = value;
                        internalWeapon.internalRanged.ObjectToSpawn = internalWeapon.projectile.internalObject;
                    }
                }
            }
            
            public ModUnit UnitProjectile
            {
                get => internalWeapon.unit;
                set
                {
                    internalWeapon.unit = value;
                    if (internalWeapon.Type == WeaponType.Ranged)
                    {
                        Projectile = null;
                        internalWeapon.internalRanged.unitToSpawn = internalWeapon.unit.internalObject;
                    }
                }
            }
            
            public int Ammo
            {
                get
                {
                    if (internalWeapon.Type == WeaponType.Ranged) return internalWeapon.internalRanged.magSize;
                    return 0;
                }
                set
                {
                    if (internalWeapon.Type == WeaponType.Ranged) internalWeapon.internalRanged.magSize = value;
                }
            }
            
            public int ProjectileAmount
            {
                get
                {
                    if (internalWeapon.Type == WeaponType.Ranged) return internalWeapon.internalRanged.numberOfObjects;
                    return 0;
                }
                set
                {
                    if (internalWeapon.Type == WeaponType.Ranged) internalWeapon.internalRanged.numberOfObjects = value;
                }
            }
            
            public float Spread
            {
                get
                {
                    if (internalWeapon.Type == WeaponType.Ranged) return internalWeapon.internalRanged.spread;
                    return 0;
                }
                set
                {
                    if (internalWeapon.Type == WeaponType.Ranged) internalWeapon.internalRanged.spread = value;
                }
            }
            
            public float Recoil
            {
                get
                {
                    if (internalWeapon.Type == WeaponType.Ranged) return internalWeapon.internalRanged.shootRecoil;
                    return 0;
                }
                set
                {
                    if (internalWeapon.Type == WeaponType.Ranged)
                    {
                        internalWeapon.internalRanged.shootRecoil = value;
                        internalWeapon.internalRanged.torsoRecoil = value * 0.1f;
                    }
                }
            }
        }

        public ModRanged Ranged;
        
        public float Size
        {
            get
            {
                Vector3 scale = internalObject.transform.localScale;
                return (scale.x + scale.y + scale.z) / 3f;
            }
            set => internalObject.transform.localScale = Vector3.one * value;
        }
        
        public Vector3 Scale
        {
            get => internalObject.transform.localScale;
            set => internalObject.transform.localScale = value;
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

        public ModWeapon(GameObject weaponObject, bool isClone = false)
        {
            internalObject = weaponObject;
            weapon = internalObject.GetComponentInChildren<Weapon>();
            item = internalObject.GetComponentInChildren<WeaponItem>();
            
            mCollision = internalObject.GetComponentInChildren<CollisionWeapon>();
            mAddEffect = internalObject.GetComponentInChildren<MeleeWeaponAddEffect>();
            mSpawner = internalObject.GetComponentInChildren<MeleeWeaponSpawn>();
            
            if (mAddEffect && mAddEffect.EffectPrefab && !isClone)
            {
                effect = new ModEffect(mAddEffect.EffectPrefab);
            }
            
            Melee = new ModMelee(this);
            Ranged = new ModRanged(this);

            Type = weapon.GetType() == typeof(RangeWeapon) ? WeaponType.Ranged : WeaponType.Melee;

            if (Type == WeaponType.Melee)
            {
                internalMelee = (MeleeWeapon)weapon;
            }
            else if (Type == WeaponType.Ranged)
            {
                internalRanged = (RangeWeapon)weapon;
                if (internalRanged.ObjectToSpawn != null) Ranged.Projectile = new ModProjectile(internalRanged.ObjectToSpawn);
            }
        }

        public override ModWeapon Clone()
        {
            var weapon = Mod.CloneAndPoolObject(internalObject);
            weapon.name = Utilities.IncrementName(internalObject.name);
            var result = new ModWeapon(weapon, true)
            {
                Model = Model
            };

            result.Separate();
            return result;
        }

        public override void Separate()
        {
            if (Type == WeaponType.Ranged) Ranged.Projectile = Ranged.Projectile.Clone(); 
            else if (Type == WeaponType.Melee)
            {
                if (Effect != null) Effect = Effect.Clone();
                if (Explosion != null) Explosion = Explosion.Clone();
            }
        }
    }
}
