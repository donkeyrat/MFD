using UnityEngine;

namespace ModdingForDummies.TABSSimp
{
    public class ModExplosion : ModdingClass<ModExplosion>
    {
        public GameObject internalObject { get; private set; }

        private Explosion explosion;

        private ExplosionAddEffect addEffect;

        public override string Name
        {
            get => internalObject.name;
            set => internalObject.name = value;
        }
        
        public float Damage
        {
            get
            {
                if (explosion) return explosion.damage;
                return 0f;
            }
            set
            {
                if (explosion) explosion.damage = value;
            }
        }
        
        public float Force
        {
            get
            {
                if (explosion) return explosion.force;
                return 0f;
            }
            set
            {
                if (explosion) explosion.force = value;
            }
        }
        
        public Explosion.ForceDirection ForceDirection
        {
            get
            {
                if (explosion) return explosion.forceDirection;
                return 0f;
            }
            set
            {
                if (explosion) explosion.forceDirection = value;
            }
        }
        
        public float Radius
        {
            get
            {
                if (explosion) return explosion.radius;
                return 0f;
            }
            set
            {
                if (explosion) explosion.radius = value;
            }
        }

        private ModEffect effect;
        public ModEffect Effect
        {
            get => effect;
            set
            {
                effect = value;
                
                if (!addEffect) addEffect = internalObject.AddComponent<ExplosionAddEffect>();
                
                if (value != null) addEffect.EffectPrefab = value.internalObject;
                else addEffect.EffectPrefab = null;
            }
        }
        
        public float Size
        {
            get
            {
                var scale = internalObject.transform.localScale;
                return ((scale.x + scale.y + scale.z) / 3f);
            }
            set => internalObject.transform.localScale = Vector3.one * value;
        }
        
        public Vector3 Scale
        {
            get => internalObject.transform.localScale;
            set => internalObject.transform.localScale = value;
        }

        public override void ColorInternal(int index, Color color, float glow = 0) => Utilities.SetObjectColor(internalObject, index, color, glow);

        public ModExplosion(GameObject explosionObject)
        {
            internalObject = explosionObject;
            explosion = internalObject.GetComponentInChildren<Explosion>();
            addEffect = internalObject.GetComponentInChildren<ExplosionAddEffect>();
        }

        public override ModExplosion Clone()
        {
            var newExplosion = Mod.CloneAndPoolObject(internalObject);
            var result = new ModExplosion(newExplosion);
            if (Effect != null) result.Effect = Effect;
            return result;
        }

        public override void Separate() 
        {
            if (Effect != null) Effect = Effect.Clone();
        }
    }
}
