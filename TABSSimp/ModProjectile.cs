using System.Collections.Generic;
using System.Linq;
using Landfall.TABS;
using UnityEngine;

namespace ModdingForDummies.TABSSimp
{
    public enum ProjectileType
    {
        Regular,
        Physical,
        Other
    }

    public class ModProjectile : ModdingClass<ModProjectile>
    {
        public GameObject internalObject { get; private set; }
        
        private ProjectileEntity projectileEntity;

        private ProjectileHit rHit;
        
        private MoveTransform rMoveTransform;
        
        private ProjectileHitAddEffect rAddEffect;

        private CollisionWeapon pCollision;

        private AddForce pAddForce;
        
        private MeleeWeaponAddEffect pAddEffect;
        
        private MeleeWeaponSpawn pSpawner;

        public override string Name
        {
            get => projectileEntity.Entity.Name;
            set => projectileEntity.Entity.Name = value;
        }
        public ProjectileType Type { get; private set; }

        private ModModel model;
        public ModModel Model
        {
            get => model;
            set
            {
                if(value != null)
                {
                    Utilities.SetMeshRenderers(internalObject, false);
                    model = value.Clone();

                    model.internalObject.transform.parent = internalObject.transform;
                    model.internalObject.transform.localPosition = Vector3.zero;
                    model.internalObject.transform.localRotation = Quaternion.Euler(-90f, 0f, 0f);
                }
                else
                {
                    if(model != null) Utilities.SetMeshRenderers(internalObject, true);
                    model = null;
                }
            }
        }


        private ModEffect effect;
        public ModEffect Effect
        {
            get => effect;
            set
            {
                effect = value;
                
                if (Type == ProjectileType.Regular)
                {
                    if (!rAddEffect) rAddEffect = rHit.gameObject.AddComponent<ProjectileHitAddEffect>();
                
                    rAddEffect.EffectPrefab = value?.internalObject;
                }
                else if (Type == ProjectileType.Physical)
                {
                    if (!pAddEffect) pAddEffect = pCollision.gameObject.AddComponent<MeleeWeaponAddEffect>();

                    pAddEffect.EffectPrefab = value?.internalObject;
                }
            }
        }

        public WrapperDelineation<ModExplosion> Explosions { get; private set; }
        public ModExplosion Explosion
        {
            get
            {
                ModExplosion result = null;
                if(Explosions.List.Count > 0) result = Explosions.List[0];
                return result;
            }
            set
            {
                Explosions.Clear();

                if (value != null) Explosions.Add(value);
            }
        }

        private ModUnit unit;
        public ModUnit Unit
        {
            get => unit;
            set
            {
                unit = value;
                Explosions.Clear();
                var unitSpawner = Utilities.unitSpawner.Clone();
                unitSpawner.internalObject.GetComponent<UnitSpawner>().unitBlueprint = unit.internalObject;

                Explosions.Add(unitSpawner);
            }
        }

        public float Damage
        {
            get
            {
                if (Type == ProjectileType.Regular) return rHit.damage;
                if (Type == ProjectileType.Physical) return pCollision.damage;
                return 0f;
            }
            set
            {
                if (Type == ProjectileType.Regular) rHit.damage = value;
                else if (Type == ProjectileType.Physical) pCollision.damage = value;
            }
        }

        public float Force
        {
            get
            {
                if (Type == ProjectileType.Regular) return rHit.force;
                if (Type == ProjectileType.Physical) return pCollision.onImpactForce;
                return 0f;
            }
            set
            {
                if (Type == ProjectileType.Regular) rHit.force = value;
                else if (Type == ProjectileType.Physical) pCollision.onImpactForce = value;
            }
        }
        
        public float Speed
        {
            get
            {
                if (Type == ProjectileType.Regular) return rMoveTransform.selfImpulse.z;
                if (Type == ProjectileType.Physical) return pAddForce.force.z;
                return 0f;
            }
            set
            {
                if (Type == ProjectileType.Regular) rMoveTransform.selfImpulse.z = value;
                else if (Type == ProjectileType.Physical) pAddForce.force.z = value;
            }
        }

        public float Size
        {
            get
            {
                Vector3 scale = internalObject.transform.localScale;
                return ((scale.x + scale.y + scale.z) / 3f);
            }
            set => internalObject.transform.localScale = Vector3.one * value;
        }

        public Vector3 Scale
        {
            get => internalObject.transform.localScale;
            set => internalObject.transform.localScale = value;
        }
        
        public void UpdateImpact(List<ModExplosion> explosions) 
        {
            if (Type == ProjectileType.Regular)
            {
                rHit.objectsToSpawn = (from ModExplosion explosion in explosions select new ObjectToSpawn {objectToSpawn = explosion.internalObject}).ToArray();
            }
            else if (Type == ProjectileType.Physical && explosions.Count > 0)
            {
                if (pSpawner) pSpawner.objectToSpawn = explosions[0].internalObject;
                else
                {
                    pSpawner = pCollision.gameObject.AddComponent<MeleeWeaponSpawn>();
                    pSpawner.objectToSpawn = explosions[0].internalObject;
                }
            }
        } 
        
        public override void ColorInternal(int index, Color color, float glow = 0f) => Utilities.SetObjectColor(internalObject, index, color, glow);

        public ModProjectile(GameObject projectile, bool isClone = false)
        {
            internalObject = projectile;
            projectileEntity = internalObject.GetComponentInChildren<ProjectileEntity>();
            
            rHit = internalObject.GetComponentInChildren<ProjectileHit>();
            rMoveTransform = internalObject.GetComponentInChildren<MoveTransform>();
            rAddEffect = internalObject.GetComponentInChildren<ProjectileHitAddEffect>();
            
            pCollision = internalObject.GetComponentInChildren<CollisionWeapon>();
            pAddForce = internalObject.GetComponentInChildren<AddForce>();
            pAddEffect = internalObject.GetComponentInChildren<MeleeWeaponAddEffect>();
            pSpawner = internalObject.GetComponentInChildren<MeleeWeaponSpawn>();
            
            Explosions = new WrapperDelineation<ModExplosion>(UpdateImpact, Mod.GetExplosion);

            if (rHit) Type = ProjectileType.Regular;
            else if (pCollision) Type = ProjectileType.Physical;
            else Type = ProjectileType.Other;

            if (Type == ProjectileType.Regular)
            {
                foreach (var spawnable in rHit.objectsToSpawn)
                {
                    if (spawnable != null) Explosions.Add(new ModExplosion(spawnable.objectToSpawn));
                }

                if (rAddEffect && rAddEffect.EffectPrefab && !isClone)
                {
                    effect = new ModEffect(rAddEffect.EffectPrefab);
                }
            }
            else if (Type == ProjectileType.Physical)
            {
                if (pSpawner && pSpawner.objectToSpawn)
                {
                    Explosions.Add(new ModExplosion(pSpawner.objectToSpawn));
                }
                
                if (pAddEffect && pAddEffect.EffectPrefab && !isClone)
                {
                    effect = new ModEffect(pAddEffect.EffectPrefab);
                }
            }
        }

        public override ModProjectile Clone()
        {
            var projectile = Mod.CloneAndPoolObject(internalObject);
            
            var result = new ModProjectile(projectile, true)
            {
                Explosions =
                {
                    List = Explosions.List
                },
                Model = Model
            };
            var incName = Utilities.IncrementName(result.projectileEntity.Entity.Name);
            result.projectileEntity.Entity.Name = incName;
            result.internalObject.name = incName;
            if (Effect != null) result.Effect = Effect;
            
            result.Separate();
            return result;
        }

        public override void Separate() 
        {
            if (Effect != null) Effect = Effect.Clone();
            Explosions.List = (from ModExplosion explosion in Explosions.List select explosion.Clone()).ToList();
        }
    }
}
