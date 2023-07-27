using System;
using System.Collections.Generic;
using UnityEngine;
using Object = UnityEngine.Object;

namespace ModdingForDummies.TABSSimp
{
    public abstract class Mod //TODO: TEAM-COLORING + ABILITY COLORING + SAME-NAME FIXING
    {
        public static bool DEV_MODE { get; protected set; } = true;
        private const string KEY_BASES = "bases";
        private const string KEY_UNITS = "units";
        private const string KEY_FACTIONS = "factions";
        private const string KEY_WEAPONS = "weapons";
        private const string KEY_CLOTHES = "clothes";
        private const string KEY_PROJECTILES = "projectiles";
        private const string KEY_MOVES = "moves";
        private const string KEY_EXPLOSIONS = "explosions";
        private const string KEY_ICONS = "icons";
        private const string KEY_EFFECTS = "effects";
        private const string KEY_MODELS = "models";

        private static Dictionary<string, Dictionary<string, object>> vanilla = new Dictionary<string, Dictionary<string, object>>();

        private static Dictionary<string, Dictionary<string, object>> modified = new Dictionary<string, Dictionary<string, object>>();

        private static GameObject pool;

        static Mod()
        {
            vanilla[KEY_BASES] = new Dictionary<string, object>();
            vanilla[KEY_UNITS] = new Dictionary<string, object>();
            vanilla[KEY_FACTIONS] = new Dictionary<string, object>();
            vanilla[KEY_WEAPONS] = new Dictionary<string, object>();
            vanilla[KEY_CLOTHES] = new Dictionary<string, object>();
            vanilla[KEY_PROJECTILES] = new Dictionary<string, object>();
            vanilla[KEY_MOVES] = new Dictionary<string, object>();
            vanilla[KEY_EXPLOSIONS] = new Dictionary<string, object>();
            vanilla[KEY_ICONS] = new Dictionary<string, object>();
            vanilla[KEY_EFFECTS] = new Dictionary<string, object>();
            vanilla[KEY_MODELS] = new Dictionary<string, object>();
            
            modified[KEY_BASES] = new Dictionary<string, object>();
            modified[KEY_UNITS] = new Dictionary<string, object>();
            modified[KEY_FACTIONS] = new Dictionary<string, object>();
            modified[KEY_WEAPONS] = new Dictionary<string, object>();
            modified[KEY_CLOTHES] = new Dictionary<string, object>();
            modified[KEY_PROJECTILES] = new Dictionary<string, object>();
            modified[KEY_MOVES] = new Dictionary<string, object>();
            modified[KEY_EXPLOSIONS] = new Dictionary<string, object>();
            modified[KEY_ICONS] = new Dictionary<string, object>();
            modified[KEY_EFFECTS] = new Dictionary<string, object>();
            modified[KEY_MODELS] = new Dictionary<string, object>();

            pool = new GameObject("Pool")
            {
                hideFlags = HideFlags.HideAndDontSave,
                transform =
                {
                    position = Vector3.down * -1000f
                }
            };
            pool.SetActive(false);
        }
        public static GameObject PoolObject(GameObject gameObject)
        {
            gameObject.transform.parent = pool.transform;
            gameObject.transform.localPosition = Vector3.zero;
            pool.transform.SetHideFlagsChildren(HideFlags.HideAndDontSave);

            return gameObject;
        }
        
        public static GameObject CloneAndPoolObject(GameObject obj)
        {
            var newObj = Object.Instantiate(obj, pool.transform);
            PoolObject(newObj);
            return newObj;
        }

        private static M CreateObject<M>(string key, Func<string, M> query, Action<M> processClone, string name, string originalName) where M : ModdingClass<M>
        {
            M result = null;
            var original = query(originalName);

            if (original != null)
            {
                result = original.Clone();
                if (processClone != null) processClone(result);
                result.Name = name;
                modified[key][name] = result;
            }
            return result;
        }

        private static M GetObject<M, R>(string key, Func<string, R> query, Func<R, M> generate, string name)
        {
            var result = default(M);

            if (modified[key].ContainsKey(name))
            {
                result = (M)modified[key][name];
            }
            else if (vanilla[key].ContainsKey(name))
            {
                result = (M)vanilla[key][name];
            }   
            else
            {
                var queried = query(name);

                if(queried != null)
                {
                    result = generate(queried);
                    vanilla[key][name] = result;
                }
                
            }

            if(result == null) Debug.LogError($"[MFD] Could not find '{name}' in '{key}'.");

            return result;
        }

        public static ModUnit GetUnit(string name) => GetObject(KEY_UNITS, Utilities.GetUnit, u => new ModUnit(u), name);

        public static ModUnit CreateUnit(string name, string originalName = null) => CreateObject(KEY_UNITS, GetUnit, mu => { if(originalName == null) mu.Clothes.Clear();} , name, (originalName == null ? "Peasant" : originalName));


        public static ModFaction GetFaction(string name) => GetObject(KEY_FACTIONS, Utilities.GetFaction, f => new ModFaction(f), name);

        public static ModFaction CreateFaction(string name, string originalName = null) => CreateObject(KEY_FACTIONS, GetFaction, mf => { mf.Units.Clear(); mf.Visible = true; }, name, (originalName == null ? "Base Faction" : originalName));
        

        public static ModWeapon GetWeapon(string name) => GetObject(KEY_WEAPONS, Utilities.GetWeapon, w => new ModWeapon(w), name);

        public static ModWeapon CreateWeapon(string name, string originalName) => CreateObject(KEY_WEAPONS, GetWeapon, null, name, originalName);

        
        public static ModClothing GetClothing(string name) => GetObject(KEY_CLOTHES, Utilities.GetClothing, c => new ModClothing(c), name);

        public static ModClothing CreateClothing(string name, string originalName) => CreateObject(KEY_CLOTHES, GetClothing, null, name, originalName);


        public static ModMove GetMove(string name) => GetObject(KEY_MOVES, Utilities.GetMove, m => new ModMove(m), name);

        public static ModMove CreateMove(string name, string originalName) => CreateObject(KEY_MOVES, GetMove, null, name, originalName);


        public static ModProjectile GetProjectile(string name) => GetObject(KEY_PROJECTILES, Utilities.GetProjectile, p => new ModProjectile(p), name);

        public static ModProjectile CreateProjectile(string name, string originalName) => CreateObject(KEY_PROJECTILES, GetProjectile, null, name, originalName);


        public static ModExplosion GetExplosion(string name) => GetObject(KEY_EXPLOSIONS, Utilities.GetExplosion, e => new ModExplosion(e), name);

        public static ModExplosion CreateExplosion(string name, string originalName) => CreateObject(KEY_EXPLOSIONS, GetExplosion, null, name, originalName);


        public static ModEffect GetEffect(string name) => GetObject(KEY_EFFECTS, Utilities.GetEffect, e => new ModEffect(e), name);

        public static ModEffect CreateEffect(string name, string originalName) => CreateObject(KEY_EFFECTS, GetEffect, null, name, originalName);

        public static ModBase GetBase(string name) => GetObject(KEY_BASES, Utilities.GetUnitBase, b => new ModBase(b), name);

        public static ModBase CreateBase(string name, string originalName) => CreateObject(KEY_BASES, GetBase, null, name, originalName);

        public static ModModel GetModel(string name) => GetObject(KEY_MODELS, Utilities.GetModel, m => new ModModel(m), name);

        public static ModModel CreateModel(string name, string originalName) => CreateObject(KEY_MODELS, GetModel, null, name, originalName);

        public static Sprite GetIcon(string name) => GetObject(KEY_ICONS, Utilities.GetIcon, s => s, name);
    }
}