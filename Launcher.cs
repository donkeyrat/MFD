using System.Collections;
using BepInEx;
using ModdingForDummies.AssetManagement;
using UnityEngine;

namespace ModdingForDummies
{
	[BepInPlugin("myname.myfirstmod", "My First Mod", "1.0.0")]
    public class Launcher : BaseUnityPlugin
    {
		public Launcher()
		{
			Debug.Log("Starting My First Mod!");

            AssetImporter.Initialize();
            new Main();
        }
    }
}