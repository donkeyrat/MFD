using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using DM;
using Landfall.TABS;
using Landfall.TABS.UnitEditor;
using ModdingForDummies.AssetManagement;
using ModdingForDummies.TABSSimp;
using UnityEngine;
using Object = UnityEngine.Object;

namespace ModdingForDummies
{
    public static class Utilities
    {
        private static readonly string[] sanitizationFilter = {"Icon_Legacy_", "Icons_128x128_", "Icons_", "Icon_",  "CP_", "P_", "E_", " Prefabs_VB", " Effects_VB", "_1", "_2", "_3", " (1)", " (2)", " (3)", " (4)", " (5)", " (6)", "B_", "Move_", "Leg_", "1_", "12_", "15_", "6__", "7_", "14_", "2_", "8_", "10_", "16_", "5_", "3_"};
        
        private static readonly Dictionary<UnitRig.GearType, string[]> boneDictionary = new Dictionary<UnitRig.GearType, string[]>
        {
            {UnitRig.GearType.HEAD, new[]{"M_Head"}},
            {UnitRig.GearType.NECK, new[]{"M_Neck"}},
            {UnitRig.GearType.SHOULDER, new[]{"M_Shoulder_Right","M_Shoulder_Left"}},
            {UnitRig.GearType.TORSO, new[]{"M_Torso"}},
            {UnitRig.GearType.ARMS, new[]{"M_Arm_Right","M_Arm_Left"}},
            {UnitRig.GearType.WRISTS, new[]{"M_Wrist_Right","M_Wrist_Left"}},
            {UnitRig.GearType.WAIST, new[]{"M_Waist"}},
            {UnitRig.GearType.LEGS, new[]{"M_Leg_Right","M_Leg_Left"}},
            {UnitRig.GearType.FEET, new[]{"M_Foot_Right","M_Foot_Left"}}
        };

        public static readonly Quaternion blenderToUnity = Quaternion.Euler(-90f, 0f, 0f);

        public static readonly string rootPath = Assembly.GetExecutingAssembly().Location;
        public static Dictionary<string, string> language = Localizer.GetLanguage(Localizer.Language.LANG_EN_US);
        
        public static readonly ContentDatabase fullDatabase = ContentDatabase.Instance();

        public static readonly LandfallContentDatabase database = ContentDatabase.Instance().LandfallContentDatabase;

        public static Explosion[] explosions = Resources.FindObjectsOfTypeAll<Explosion>();

        public static UnitEffectBase[] effects = Resources.FindObjectsOfTypeAll<UnitEffectBase>();

        public static Sprite[] sprites = Resources.FindObjectsOfTypeAll<Sprite>();

        public static readonly ModExplosion unitSpawner = new ModExplosion(Mod.PoolObject(new GameObject("UnitSpawner", typeof(UnitSpawner))));

        public static readonly ModFaction baseFaction;

        static Utilities()
        {
            var result = language
                .GroupBy(z => z.Value)
                .Where(z => z.Count() > 1)
                .SelectMany(z => z)
                .Select(z => z.Key)
                .ToList();
            var count = new Dictionary<string, int>();

            foreach (var v in result.Where(v => language[v] != null && language[v] != string.Empty))
            {
                if (count.ContainsKey(language[v]) == false)
                {
                    count.Add(language[v], 1);
                }
                else
                {
                    count[language[v]]++;
                    language[v] = language[v] + " " + count[language[v]];
                }
            }
        
            var factionObject = Object.Instantiate(GetFaction("Medieval"));
            factionObject.Entity.GenerateNewID();
            factionObject.Entity.Name = "Base Faction";
            factionObject.Units = Array.Empty<UnitBlueprint>();
            factionObject.index = database.GetFactions().ToList().Count;
            factionObject.m_FactionColor = HexColor("#888888");
            factionObject.m_displayFaction = false;
            AddFactionToDatabase(factionObject);
            baseFaction = new ModFaction(factionObject);
            
            if (Mod.DEV_MODE)
            {
                var printPath = Path.Combine(rootPath, "MFDPrints");

                if (!Directory.Exists(printPath)) Directory.CreateDirectory(printPath);
                
                var weapons = string.Join(Environment.NewLine, from GameObject weapon in database.GetWeapons() where weapon != null select GetProperName(weapon.GetComponentInChildren<WeaponItem>().Entity.Name));
                File.WriteAllText(Path.Combine(printPath, "weapons.txt"), weapons);
                
                var bases = string.Join(Environment.NewLine, from GameObject unitBase in database.GetUnitBases() where unitBase != null select GetProperName(unitBase.GetComponentInChildren<Unit>().Entity.Name));
                File.WriteAllText(Path.Combine(printPath, "bases.txt"), bases);

                var clothes = string.Join(Environment.NewLine, from GameObject clothing in database.GetCharacterProps() where clothing != null select GetProperName(clothing.GetComponentInChildren<PropItem>().Entity.Name));
                File.WriteAllText(Path.Combine(printPath, "clothing.txt"), clothes);

                var units = string.Join(Environment.NewLine, from IDatabaseEntity unit in database.GetUnitBlueprints() select GetProperName(unit.Entity.Name));
                File.WriteAllText(Path.Combine(printPath, "unit.txt"), units);

                var factions = string.Join(Environment.NewLine, from IDatabaseEntity faction in database.GetFactions() select GetProperName(faction.Entity.Name));
                File.WriteAllText(Path.Combine(printPath, "factions.txt"), factions);

                var explosionsS = string.Join(Environment.NewLine, from Explosion explosion in explosions where explosion != null select GetProperName(explosion.gameObject.name));
                File.WriteAllText(Path.Combine(printPath, "explosions.txt"), explosionsS);

                var effectsS = string.Join(Environment.NewLine, from UnitEffectBase effect in effects where effect != null select GetProperName(effect.gameObject.name));
                File.WriteAllText(Path.Combine(printPath, "effects.txt"), effectsS);

                var projectiles = string.Join(Environment.NewLine, from GameObject projectile in database.GetProjectiles() select GetProperName(projectile.GetComponentInChildren<ProjectileEntity>().Entity.Name));
                File.WriteAllText(Path.Combine(printPath, "projectiles.txt"), projectiles);
                
                var moves = string.Join(Environment.NewLine, from GameObject move in database.GetCombatMoves() select GetProperName(move.GetComponentInChildren<SpecialAbility>().Entity.Name));
                File.WriteAllText(Path.Combine(printPath, "moves.txt"), moves);

                var icons = string.Join(Environment.NewLine, from FactionIcon icon in fullDatabase.GetFactionIcons() select GetProperName(icon.Entity.Name));
                File.WriteAllText(Path.Combine(printPath, "icons.txt"), icons);

                var iconsExtra = string.Join(Environment.NewLine, from Sprite icon in Resources.FindObjectsOfTypeAll<Sprite>() select GetProperName(icon.name));
                File.WriteAllText(Path.Combine(printPath, "iconsExtra.txt"), iconsExtra);
            }
        }

        public static object CallMethod(this object o, string methodName, params object[] args)
        {
            var method = o.GetType().GetMethod(methodName, (BindingFlags)(-1));
            var flag = method != null;
            object result;
            if (flag)
            {
                result = method.Invoke(o, args);
            }
            else
            {
                Debug.Log("Call Method " + methodName + " nulled");
                result = null;
            }
            return result;
        }
        public static object GetField<T>(this T instance, string fieldName, BindingFlags flags = (BindingFlags)(-1))
        {
            var field = typeof(T).GetField(fieldName, flags);
            try
            {
                field.GetValue(instance);
            }
            catch
            {
                Debug.Log($"(GetField) Failed to get field '{fieldName}' for object '{typeof(T)}'. Try using manual BindingFlags.");
            }
            return field.GetValue(instance);
        }

        public static void SetField<T>(this T instance, string fieldName, object newValue, BindingFlags flags = (BindingFlags)(-1))
        {
            var field = typeof(T).GetField(fieldName, flags);
            Debug.Log(field);
            try
            {
                field.SetValue(instance, newValue);
            }
            catch
            {
                Debug.Log($"(SetField) Failed to set field '{fieldName}' for object '{typeof(T)}'. Try using manual BindingFlags.");
            }
        }


        public static Transform SetHideFlagsChildren(this Transform t, HideFlags hf = HideFlags.DontSave)
        {
            if (t.gameObject)
            {
                t.gameObject.hideFlags = hf;
            }
            if (t.childCount > 0)
            {
                for (var i = 0; i < t.childCount; i++)
                {
                    t.GetChild(i).SetHideFlagsChildren(hf);
                }
            }
            return t;
        }

        public static Sprite CreateSprite(string path)
        {
            if (string.IsNullOrEmpty(path))
            {
                return null;
            }

            if (!File.Exists(path))
            {
                return null;
            }
            var data = File.ReadAllBytes(path);
            var texture2D = new Texture2D(1, 1);
            texture2D.LoadImage(data);
            texture2D.filterMode = FilterMode.Point;
            return Sprite.Create(texture2D, new Rect(0f, 0f, texture2D.width, texture2D.height), new Vector2(0, 0), 1f);
        }
        public static bool NullCheck(this object self, string flag = "Object")
        {
            Debug.Log($"[NULLCHECK] {flag} is {(self == null ? "NULL" : "REAL")}!");
            return self == null;
        }

        public static T GetResourcesObj<T>(string obj)
        {
            var objs = Resources.FindObjectsOfTypeAll(typeof(T));
            return (T)(object)objs.FirstOrDefault(x => x.name.Contains(obj));
        }

        public static string IncrementName(string name)
        {
            return (name.Contains(";") ? name.Split(';')[0] + int.Parse(name.Split(';')[1]) + 1 : name + ";1");
        }


        public static void ForEach<T>(this IEnumerable<T> collection, Action<T> action)
        {
            foreach (var item in collection) action(item);
        }

        public static void ForEach<T>(this T[] array, Action<T> action)
        {
            foreach (var item in array) action(item);
        }

        public static Dictionary<V, U> Reverse<U, V>(this Dictionary<U, V> self)
        {
            var reverse = new Dictionary<V, U>();
            foreach(var key in self.Keys)
            {
                reverse[self[key]] = key;
            }

            return reverse;
        }

        public static Color HexColor(string hexCode)
        {
            hexCode = (hexCode.StartsWith("#") ? hexCode : "#" + hexCode).ToUpper();
            ColorUtility.TryParseHtmlString(hexCode, out var outColor);
            return outColor;
        }

        public static T FindVanillaObject<T>(IEnumerable<T> source, string name, Func<T, string> getName)
        {
            return source.FirstOrDefault(o => (name == GetProperName(getName(o))));
        }

        public static string GetProperName(string name)
        {
            string properName = null;

            if (language.ContainsKey(name))
            {
                properName = language[name];
            }
            else
            {
                properName = name;
                foreach (var filter in sanitizationFilter)
                {
                    properName = properName.Replace(filter, "");
                }
            }

            return properName;
        }

        public static string DeepString(this GameObject self)
        {
            var final = "\nGameObject '" + self.name + "':\n{\n\tComponents:\n\t{\n";
            final += String.Concat(from Component component in self.GetComponents<Component>() select ("\t\t" + component.GetType().Name + "\n"));
            final += "\t}\n";
            if (self.transform.childCount > 0)
            {
                final += "\tChildren:\n\t{\n";
                final += String.Concat(from Transform child in self.transform select (child.gameObject.DeepString().Replace("\n", "\n\t\t")));
                final += "\n\t}\n";
            }
            final += "}\n";
            return final;
        }

        public static void PlaceMeshOnBone(SkinnedMeshRenderer renderer, GameObject mesh, UnitRig.GearType gearType)
        {
            var selection = boneDictionary[gearType][0];

            foreach(var bone in renderer.bones)
            {
                if(bone.name == selection)
                {
                    mesh.transform.parent = bone.transform;
                    mesh.transform.localPosition = Vector3.zero;
                    mesh.transform.localRotation = Quaternion.Euler(-90f, 0f, 0f);
                }
            }
        }

        public static void SetMeshRenderers(GameObject gameObject, bool enabled)
        {
            var renderers = gameObject.GetComponentsInChildren<Renderer>();
            for (var i = 0; i < renderers.Length; i++)
            {
                var rendererType = renderers[i].GetType();
                if(rendererType == typeof(MeshRenderer) || rendererType == typeof(SkinnedMeshRenderer))
                {
                    renderers[i].enabled = enabled;
                }
            }
        }


        public static void SetObjectColor(GameObject colorObject, int index, Color color, float glow)
        {
            var renderers = GetBestRenderers(colorObject);
            var bestRenderer = GetBestRenderer(colorObject);
            var isLOD = colorObject.GetComponentInChildren<LODGroup>() != null;
            var lodDictionary = new Dictionary<Color, Material>();
            var rMaterials = bestRenderer.materials;
            var newMaterial = new Material(Shader.Find("Standard"));
            newMaterial.color = color;
            if (glow > 0f)
            {
                newMaterial.EnableKeyword("_EMISSION");
                newMaterial.SetColor("_EmissionColor", newMaterial.color * glow);
            }
            newMaterial.SetFloat("_Glossiness", 0f);

            if (bestRenderer)
            {
                for (var i = 0; i < rMaterials.Length; i++) if(i == index) lodDictionary[rMaterials[i].color] = newMaterial;
                if (renderers != null && renderers.Length > 0)
                {
                    if (isLOD)
                    {
                        foreach (var renderer in renderers)
                        {
                            var teamColors = renderer.GetComponents<TeamColor>().ToList();
                            var removed = new List<TeamColor>();
                            var thisMaterials = renderer.materials;
                            var dictMaterials = new List<Material>();
                            for (var i = 0; i < thisMaterials.Length; i++)
                            {
                                Material replacement = null;
                                if (lodDictionary.ContainsKey(thisMaterials[i].color)) replacement = lodDictionary[thisMaterials[i].color];
                                if (replacement != null)
                                {
                                    dictMaterials.Add(replacement);
                                    foreach (var tc in teamColors.Where(tc => !removed.Contains(tc) && tc.materialID == i))
                                    {
                                        removed.Add(tc);
                                        Object.DestroyImmediate(tc);
                                    }
                                }
                                else dictMaterials.Add(thisMaterials[i]);
                            }
                            renderer.materials = dictMaterials.ToArray();
                        }
                    }
                    else
                    {
                        var offset = 0;
                        foreach (var renderer in renderers)
                        {
                            var thisMaterials = renderer.materials;
                            var teamColors = renderer.GetComponents<TeamColor>().ToList();
                            for (var i = 0; i < thisMaterials.Length; i++)
                            {
                                if (offset + i == index)
                                {
                                    thisMaterials[i] = newMaterial;
                                    foreach (var tc in teamColors)
                                    {
                                        if (tc.materialID == i) Object.DestroyImmediate(tc);
                                    }
                                }
                            }
                            offset++;
                            renderer.materials = thisMaterials;
                        }
                        
                    }
                }
            }
        }

        public static void SetClothScale(ModClothing clothing, Vector3 scale)
        {
            var internalProp = clothing.internalObject;
            if (internalProp.GetComponentInChildren<MeshRenderer>() != null)
            {
                foreach (var rend in internalProp.GetComponentsInChildren<MeshRenderer>())
                {
                    rend.transform.localScale = scale;
                }
            }
            if (internalProp.GetComponentInChildren<SkinnedMeshRenderer>() != null)
            {
                var locScale = internalProp.transform.localScale;
                foreach (var v in internalProp.GetComponentsInChildren<SkinnedMeshRenderer>())
                {
                    v.sharedMesh = Object.Instantiate(v.sharedMesh);
                    var newMesh = v.sharedMesh;
                    newMesh.name = clothing.Name + "sharedmesh";
                    var baseVertices = newMesh.vertices;
                    if (baseVertices == null)
                        baseVertices = newMesh.vertices;

                    Debug.Log("baseVertices " + baseVertices.Length);
                    var vertices = new Vector3[baseVertices.Length];

                    for (var i = 0; i < vertices.Length; i++)
                    {
                        var vertex = baseVertices[i];
                        vertex.x /= locScale.x;
                        vertex.y /= locScale.y;
                        vertex.z /= locScale.z;

                        vertex.x *= scale.x;
                        vertex.y *= scale.y;
                        vertex.z *= scale.z;

                        vertices[i] = vertex;
                    }
                    internalProp.transform.localScale = scale;

                    newMesh.vertices = vertices;
                    newMesh.RecalculateBounds();
                    v.sharedMesh = newMesh;
                }
            }
        }

        public static Renderer GetBestRenderer(GameObject gameObject, Type rendererType = null)
        {
            if (rendererType == null) rendererType = typeof(Renderer);
            else if (!rendererType.IsSubclassOf(typeof(Renderer))) return null;
            Renderer renderer = null;
            var candidates = gameObject.GetComponentsInChildren<Renderer>();
            var superLocked = false;
            foreach (var candidate in candidates)
            {
                if (candidate.GetType() == rendererType || rendererType == typeof(Renderer))
                {
                    var name = candidate.gameObject.name;
                    if (name.Contains("LOD0"))
                    {
                        renderer = candidate;
                        superLocked = true;
                    }
                    else if (name.Contains("CP_") && !superLocked)
                    {
                        renderer = candidate;
                        superLocked = true;
                    }
                    else if (name.ToLower().Contains(gameObject.gameObject.name.ToLower()) && !superLocked) renderer = candidate;
                    else if (!renderer && (name != "RightHand" && name != "LeftHand") && !superLocked) renderer = candidate;
                }
            }
            return renderer;
        }

        public static Renderer[] GetBestRenderers(GameObject gameObject, Type rendererType = null)
        {
            if (rendererType == null) rendererType = typeof(Renderer);
            else if (!rendererType.IsSubclassOf(typeof(Renderer))) return null;
            var candidates = gameObject.GetComponentsInChildren<Renderer>();
            var chosen = new List<Renderer>();
            var superLocked = false;
            var lodGroup = gameObject.GetComponentInChildren<LODGroup>();
            if (lodGroup)
            {
                superLocked = true;
                chosen.AddRange(lodGroup.GetLODs().SelectMany(lod => lod.renderers));
            }
            foreach (var candidate in candidates)
            {
                if (!chosen.Contains(candidate) && (candidate.GetType() == rendererType || rendererType == typeof(Renderer)))
                {
                    var wName = gameObject.name;
                    if (wName.Contains("LOD") && candidate.transform.parent.name.ToLower().Contains("base"))
                    {
                        chosen.Add(candidate);
                        superLocked = true;
                    }
                    else if (wName.Contains("LOD"))
                    {
                        chosen.Add(candidate);
                        superLocked = true;
                    }
                    else if (wName.Contains("CP_") && !superLocked)
                    {
                        chosen.Add(candidate);
                        superLocked = true;
                    }
                    else if (wName.ToLower().Contains(gameObject.name.ToLower()) && !superLocked) chosen.Add(candidate);
                    else if (chosen.Count <= 0 && (wName != "RightHand" && wName != "LeftHand") && !superLocked) chosen.Add(candidate);
                }
            }
            return chosen.ToArray();
        }

        public static GameObject GetUnitBase(string name)
        {
            return FindVanillaObject(database.GetUnitBases(), name, ub => ub.GetComponentInChildren<Unit>().Entity.Name);
        }
        public static GameObject GetClothing(string name)
        {
            return FindVanillaObject(database.GetCharacterProps(), name, c => c.GetComponentInChildren<PropItem>().Entity.Name);
        }
        public static GameObject GetWeapon(string name)
        {
            return FindVanillaObject(database.GetWeapons(), name, w => w.GetComponentInChildren<WeaponItem>().Entity.Name);
        }
        public static GameObject GetMove(string name)
        {
            return FindVanillaObject(database.GetCombatMoves(), name, m => m.GetComponentInChildren<SpecialAbility>().Entity.Name);
        }
        public static GameObject GetProjectile(string name)
        {
            var result = FindVanillaObject(database.GetProjectiles(), name, p => p.GetComponent<ProjectileEntity>().Entity.Name).gameObject;

            return result != null ? result.gameObject : null;
        }
        public static GameObject GetExplosion(string name)
        {
            var result = FindVanillaObject(explosions, name, e => e.gameObject.name);

            return result != null ? result.gameObject : null;
        }
        public static UnitEffectBase GetEffect(string name)
        {
            var result = FindVanillaObject(effects, name, e => e.gameObject.name);
            
            return result;
        }
        public static Sprite GetIcon(string name)
        {
            var result = FindVanillaObject(fullDatabase.GetFactionIcons(), name, i => i.Entity.Name);
            if (result != null) return result.Entity.SpriteIcon;

            var deep = FindVanillaObject(AssetImporter.Sprites, name, s => s.name);
            if (deep != null) return deep;

            deep = FindVanillaObject(sprites, name, s => s.name);
            return deep;
        }
        public static UnitBlueprint GetUnit(string name)
        {
            var result = FindVanillaObject(database.GetUnitBlueprints(), name, u => u.Entity.Name);

            return result != null ? database.GetUnitBlueprint(result.Entity.GUID) : null;
        }
        public static Faction GetFaction(string name)
        {
            var result = FindVanillaObject(database.GetFactions(), name, f => f.Entity.Name);

            return result != null ? database.GetFaction(result.Entity.GUID) : null;
        }
        
        public static void AddUnitToDatabase(UnitBlueprint unit)
        {
            var nonStreamableAssets = (Dictionary<DatabaseID, Object>)typeof(AssetLoader).GetField("m_nonStreamableAssets", BindingFlags.NonPublic | BindingFlags.Instance).GetValue(fullDatabase.AssetLoader);
            var units = (Dictionary<DatabaseID, UnitBlueprint>)typeof(LandfallContentDatabase).GetField("m_unitBlueprints", BindingFlags.NonPublic | BindingFlags.Instance).GetValue(database);

            if (!units.ContainsKey(unit.Entity.GUID))
            {
                units.Add(unit.Entity.GUID, unit);
                nonStreamableAssets.Add(unit.Entity.GUID, unit);
            }
            
            typeof(LandfallContentDatabase).GetField("m_unitBlueprints", BindingFlags.NonPublic | BindingFlags.Instance).SetValue(database, units);
            typeof(AssetLoader).GetField("m_nonStreamableAssets", BindingFlags.NonPublic | BindingFlags.Instance).SetValue(fullDatabase.AssetLoader, nonStreamableAssets);
        }
        
        public static void AddFactionToDatabase(Faction faction)
        {
            var nonStreamableAssets = (Dictionary<DatabaseID, Object>)typeof(AssetLoader).GetField("m_nonStreamableAssets", BindingFlags.NonPublic | BindingFlags.Instance).GetValue(fullDatabase.AssetLoader);
            var factions = (Dictionary<DatabaseID, Faction>)typeof(LandfallContentDatabase).GetField("m_factions", BindingFlags.NonPublic | BindingFlags.Instance).GetValue(database);

            if (!factions.ContainsKey(faction.Entity.GUID))
            {
                factions.Add(faction.Entity.GUID, faction);
                nonStreamableAssets.Add(faction.Entity.GUID, faction);
            }
            
            typeof(LandfallContentDatabase).GetField("m_factions", BindingFlags.NonPublic | BindingFlags.Instance).SetValue(database, factions);
            typeof(AssetLoader).GetField("m_nonStreamableAssets", BindingFlags.NonPublic | BindingFlags.Instance).SetValue(fullDatabase.AssetLoader, nonStreamableAssets);
        }

        public static GameObject GetModel(string name)
        {
            var result = FindVanillaObject(AssetImporter.Models, name, m => m.gameObject.name);

            return (result != null ? result.gameObject : null);
        }

        public static void SetFlagsChildren(Transform child, HideFlags flag)
        {
            child.gameObject.hideFlags = flag;
            for (var i = 0; i < child.childCount; i++)
            {
                SetFlagsChildren(child.GetChild(i), flag);
            }
        }
    }
}