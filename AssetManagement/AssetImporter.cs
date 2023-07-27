using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading;
using System.Xml;
using ModdingForDummies.TABSSimp;
using Sirenix.Serialization;
using UnityEngine;

namespace ModdingForDummies.AssetManagement
{
    public static class AssetImporter 
    {
        public static List<GameObject> Models { get; private set; } = new List<GameObject>();

        public static List<Sprite> Sprites { get; private set; } = new List<Sprite>();

        private static int[] GenerateRange(int floor, int ceiling)
        {
            var range = new List<int>();
            for (var i = floor; i < ceiling; i++)
            {
                range.Add(i);
            }
            return range.ToArray();
        }

        public static string GetAttr(this XmlNode self, string name)
        {
            return (from XmlAttribute attr in self.Attributes where attr.Name == name select attr.Value).ToArray()[0];
        }

        public static float[] GetFloats(string floats)
        {
            var numbers = floats.Split(' ');
            return (from string number in numbers select float.Parse(number, CultureInfo.InvariantCulture)).ToArray();
        }

        public static int[] GetInts(string ints)
        {
            var numbers = ints.Split(' ');
            return (from string number in numbers select int.Parse(number, CultureInfo.InvariantCulture)).ToArray();
        }
        public static Color GetColor(string floats)
        {
            var casted = (from float flt in GetFloats(floats) select Mathf.Pow(flt, 2.2f)).ToArray();
            return new Color(casted[0], casted[1], casted[2], casted[3]);
        }

        public static Vector3[] GetVector3(string floats)
        {
            var casted = GetFloats(floats);
            var vectors = new Vector3[casted.Length / 3];
            for (var i = 0; i < (casted.Length / 3); i++)
            {
                vectors[i] = new Vector3(casted[(3 * i)], casted[(3 * i) + 1], casted[(3 * i) + 2]);
            }
            return vectors;
        }

        public static Vector2[] GetVector2(string floats)
        {
            var casted = GetFloats(floats);
            var vectors = new Vector2[casted.Length / 2];
            for (var i = 0; i < (casted.Length / 2); i++)
            {
                vectors[i] = new Vector2(casted[(2 * i)], casted[(2 * i) + 1]);
            }
            return vectors;
        }
        public static SpriteAsset ImportSprite(string filePath)
        {
            var data = File.ReadAllBytes(filePath);
            var name = Path.GetFileNameWithoutExtension(filePath);

            var created = new SpriteAsset(name, data);

            return created;
        }


        public static ModelAsset ImportModel(string filePath)
        {
            var collada = new XmlDocument();

            collada.Load(filePath);
            XmlNode root = collada.DocumentElement;
            var nsmgr = new XmlNamespaceManager(collada.NameTable);
            nsmgr.AddNamespace("c", root.GetAttr("xmlns"));

            
            var allMaterials = new Dictionary<string, Color>();
            var materials = root.SelectNodes("//c:library_effects/c:effect", nsmgr);
            foreach(XmlNode materialNode in materials)
            {
                string materialName = materialNode.GetAttr("id").Split('-')[0];

                var colorText = materialNode.SelectNodes("c:profile_COMMON/c:technique/c:lambert/c:diffuse/c:color", nsmgr).Item(0).InnerText;

                var color = GetColor(colorText);

                allMaterials[materialName] = color;
            }

            var geometries = root.SelectNodes("//c:library_geometries/c:geometry", nsmgr);
            var geometry = geometries.Item(0);

            var geometryName = geometry.GetAttr("name").Replace('.', '_');
            var verticesText = geometry.SelectSingleNode($"c:mesh/c:source[@id='{geometryName + "-mesh-positions"}']/c:float_array", nsmgr).InnerText;
            var normalsText = geometry.SelectSingleNode($"c:mesh/c:source[@id='{geometryName + "-mesh-normals"}']/c:float_array", nsmgr).InnerText;
            var uvmapText = geometry.SelectSingleNode($"c:mesh/c:source[@id='{geometryName + "-mesh-map-0"}']/c:float_array", nsmgr).InnerText;

            var uniqueVertices = GetVector3(verticesText);
            var uniqueNormals = GetVector3(normalsText);
            var uniqueTextures = GetVector2(uvmapText);

            var submeshNodes = geometry.SelectNodes("c:mesh/c:triangles", nsmgr);

            var submeshes = new Dictionary<string, int[]>();
            foreach(XmlNode submeshNode in submeshNodes)
            {
                string materialName = submeshNode.GetAttr("material").Split('-')[0];
                var indicesText = submeshNode.SelectSingleNode("c:p", nsmgr).InnerText;

                var indices = GetInts(indicesText);
                submeshes[materialName] = indices;
            }
            IEnumerable<int> buffer = new int[0];
            foreach (var array in submeshes.Values) buffer = buffer.Concat(array);
            var allIndices = buffer.ToArray();
            var vertIndices = new int[allIndices.Length / 3];
            var normIndices = new int[allIndices.Length / 3];
            var textIndices = new int[allIndices.Length / 3];
            for (var j = 0; j < (allIndices.Length / 3); j++)
            {
                vertIndices[j] = allIndices[(j * 3)];
                normIndices[j] = allIndices[(j * 3) + 1];
                textIndices[j] = allIndices[(j * 3) + 2];
            }
            var nvtBuffer = new int[vertIndices.Length][];
            var nvtUnique = new List<int[]>();
            for (var k = 0; k < vertIndices.Length; k++)
            {
                nvtBuffer[k] = new[] { vertIndices[k], normIndices[k], textIndices[k] };
            }


            for (var l = 0; l < nvtBuffer.Length; l++)
            {
                if (!nvtUnique.Exists(el => el == nvtBuffer[l]))
                {
                    nvtUnique.Add(nvtBuffer[l]);
                }
            }
            var nvtTriplets = nvtUnique.ToArray();

            var vertices = new Vector3[nvtTriplets.Length];
            var normals = new Vector3[nvtTriplets.Length];
            var textures = new Vector2[nvtTriplets.Length];

            for (var n = 0; n < nvtTriplets.Length; n++)
            {
                vertices[n] = uniqueVertices[nvtTriplets[n][0]];
                normals[n] = uniqueNormals[nvtTriplets[n][1]];
                textures[n] = uniqueTextures[nvtTriplets[n][2]];
            }

            var model = new ModelAsset();
            model.name = Path.GetFileNameWithoutExtension(filePath);
            model.vertices = vertices;
            model.normals = normals;
            model.textures = textures;
            model.submeshes = new int[submeshes.Count][];
            model.materials = new Color[submeshes.Count];

            var o = 0;
            var progress = 0;
            foreach (var key in submeshes.Keys)
            {
                var tripletCount = submeshes[key].Length / 3;
                model.submeshes[0] = GenerateRange(progress, progress + tripletCount);
                progress += tripletCount;
                model.materials[o] = allMaterials[key];
                o++;
            }

            return model;
        }

        public static void Initialize()
        {
            var assetPath = Path.Combine(Utilities.rootPath, "Assets");
            if (Directory.Exists(assetPath))
            {
                if (Mod.DEV_MODE)
                {
                    var bundle = new ModAssetBundle();
                    var spritePath = Path.Combine(assetPath, "Sprites");
                    if (Directory.Exists(spritePath))
                    {
                        var files = Directory.GetFiles(spritePath);
                        var sprites = new List<SpriteAsset>();

                        foreach (var file in files)
                        {
                            if (file.EndsWith(".png"))
                            {
                                sprites.Add(ImportSprite(file));
                            }
                        }

                        bundle.sprites = sprites.ToArray();
                    }

                    var modelPath = Path.Combine(assetPath, "Models");
                    if (Directory.Exists(modelPath))
                    {
                        var files = Directory.GetFiles(modelPath);
                        var models = new List<ModelAsset>();

                        foreach (var file in files)
                        {
                            if (file.EndsWith(".dae"))
                            {
                                models.Add(ImportModel(file));
                            }
                        }

                        bundle.models = models.ToArray();
                    }

                    var bundleData = SerializationUtility.SerializeValue(bundle, DataFormat.Binary);
                    File.WriteAllBytes(Path.Combine(assetPath, "assets.bin"), bundleData);
                }

                var bundlePath = Path.Combine(assetPath, "assets.bin");
                if (File.Exists(bundlePath))
                {
                    var data = File.ReadAllBytes(bundlePath);
                    var bundle = SerializationUtility.DeserializeValue<ModAssetBundle>(data, DataFormat.Binary);

                    if(bundle.models != null)
                    {
                        foreach (var model in bundle.models)
                        {
                            var mesh = new Mesh();
                            mesh.Clear();
                            mesh.vertices = model.vertices;
                            mesh.normals = model.normals;
                            mesh.uv = model.textures;
                            mesh.subMeshCount = model.submeshes.Length;
                            
                            var modelContainer = new GameObject(model.name);
                            var meshFilter = modelContainer.AddComponent<MeshFilter>();
                            var meshRenderer = modelContainer.AddComponent<MeshRenderer>();

                            meshFilter.mesh = mesh;

                            var realMaterials = new Material[model.submeshes.Length];

                            for(var i = 0; i < model.submeshes.Length; i++)
                            {
                                mesh.SetTriangles(model.submeshes[i], i);
                            }

                            for(var i = 0; i < model.materials.Length; i++)
                            {
                                var material = new Material(Shader.Find("Standard"));
                                material.EnableKeyword("_EMISSION");
                                material.color = model.materials[i];
                                material.SetFloat("_Glossiness", 0f);
                                realMaterials[i] = material;
                            }

                            meshRenderer.materials = realMaterials;
                            Mod.PoolObject(modelContainer);
                            Models.Add(modelContainer);
                        }
                    }

                    if (bundle.sprites != null)
                    {
                        foreach(var sprite in bundle.sprites)
                        {
                            var texture = new Texture2D(1, 1, TextureFormat.ARGB32, false);
                            texture.LoadImage(sprite.data);
                            texture.name = sprite.name;
                            texture.filterMode = FilterMode.Point;

                            var realSprite = Sprite.Create(texture, new Rect(0f, 0f, texture.width, texture.height), Vector2.one/2f);
                            realSprite.name = sprite.name;

                            Sprites.Add(realSprite);
                        }
                    }
                }
            }
        }
    }
}