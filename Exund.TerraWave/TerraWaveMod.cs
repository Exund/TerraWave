using System;
using System.Collections.Generic;
using System.Linq;
using Harmony;
using UnityEngine;
using System.Reflection;
using System.IO;

namespace Exund.TerraWave
{
	public class TerraWaveMod
	{
		public static string assets_path;
		public static void Load()
		{
			var harmony = HarmonyInstance.Create("Exund.ModOptionsTab");
			harmony.PatchAll(Assembly.GetExecutingAssembly());

			var materials = Resources.FindObjectsOfTypeAll<Material>();
			var Skydome_Planets = materials.First(m => m.name == "Skydome_Planets");
			var Moon = materials.First(m => m.name == "Moon");

			assets_path = Path.Combine(Directory.GetParent(Assembly.GetExecutingAssembly().Location).FullName, "Assets");

			var Skydome_Planets_Albedo = new Texture2D(0, 0);
			Skydome_Planets_Albedo.LoadImage(File.ReadAllBytes(Path.Combine(assets_path, "Skydome_Planets_Albedo.png")));
			Skydome_Planets.mainTexture = Skydome_Planets_Albedo;

			var moon_texture = new Texture2D(0, 0);
			moon_texture.LoadImage(File.ReadAllBytes(Path.Combine(assets_path, "moon_texture.png")));
			Moon.mainTexture = moon_texture;
			Moon.SetTexture("_MainTex", moon_texture);

			var handler = new GameObject();
			handler.AddComponent<AudioSource>();
			handler.AddComponent<MusicLoader>();
			GameObject.DontDestroyOnLoad(handler);
		}
	}

	static class Patches
	{
		static FieldInfo m_Trail;

		static Patches()
		{
			var privateFlags = BindingFlags.Instance | BindingFlags.NonPublic;
			m_Trail = typeof(FanJet).GetField("m_Trail", privateFlags);
		}

		[HarmonyPatch(typeof(FanJet), "OnSpawn")]
		static class FanJetOnSpawn
		{

			private static void Postfix(ref FanJet __instance)
			{
				var trail = (TrailRenderer)m_Trail.GetValue(__instance);
				if (trail != null)
				{
					trail.startColor = new Color32(254, 80, 32, 127);
					trail.endColor = new Color32(255, 5, 180, 127);
				}
			}
		}


		static class TOD_SkyPatches
		{
			[HarmonyPatch(typeof(TOD_Sky), "LateUpdate")]
			static class LateUpdate
			{
				private static void Prefix(ref TOD_Sky __instance)
				{
					__instance.m_UseTerraTechBiomeData = false;
				}
			}

			[HarmonyPatch(typeof(TOD_Sky), "OnEnable")]
			static class OnEnable
			{
				private static void Prefix(ref TOD_Sky __instance)
				{
					__instance.Day.LightColor = new Gradient
					{
						alphaKeys = new GradientAlphaKey[]
						{
							new GradientAlphaKey(1f, 0f),
							new GradientAlphaKey(1f, 0.5f),
							new GradientAlphaKey(1f, 1f)
						},
						colorKeys = new GradientColorKey[]
						{
							new GradientColorKey(new Color32(240, 40, 130, byte.MaxValue), 0f),
							new GradientColorKey(new Color32(240, 40, 130, byte.MaxValue), 0.5f),
							new GradientColorKey(new Color32(240, 40, 130, byte.MaxValue), 1f)
						}
					};
					__instance.Day.RayColor = new Gradient
					{
						alphaKeys = new GradientAlphaKey[]
						{
							new GradientAlphaKey(1f, 0f),
							new GradientAlphaKey(1f, 0.5f),
							new GradientAlphaKey(1f, 1f)
						},
						colorKeys = new GradientColorKey[]
						{
							new GradientColorKey(new Color32(240, 40, 130, byte.MaxValue), 0f),
							new GradientColorKey(new Color32(240, 40, 130, byte.MaxValue), 0.5f),
							new GradientColorKey(new Color32(240, 40, 130, byte.MaxValue), 1f)
						}
					};
					__instance.Day.SkyColor = new Gradient
					{
						alphaKeys = new GradientAlphaKey[]
						{
							new GradientAlphaKey(1f, 0f),
							new GradientAlphaKey(1f, 0.5f),
							new GradientAlphaKey(1f, 1f)
						},
						colorKeys = new GradientColorKey[]
						{
							new GradientColorKey(new Color32(18, 10, 29, byte.MaxValue), 0f),
							new GradientColorKey(new Color32(18, 10, 29, byte.MaxValue), 0.5f),
							new GradientColorKey(new Color32(18, 10, 29, byte.MaxValue), 1f)
						}
					};
					__instance.Day.SunColor = new Gradient()
					{
						alphaKeys = new GradientAlphaKey[]
						{
							new GradientAlphaKey(1f, 0f),
							new GradientAlphaKey(1f, 0.5f),
							new GradientAlphaKey(1f, 1f)
						},
						colorKeys = new GradientColorKey[]
						{
							new GradientColorKey(new Color32(245, 230, 30, byte.MaxValue), 0f),
							new GradientColorKey(new Color32(245, 230, 30, byte.MaxValue), 0.5f),
							new GradientColorKey(new Color32(245, 230, 30, byte.MaxValue), 1f)
						}
					};
					__instance.Day.CloudColor = new Gradient
					{
						alphaKeys = new GradientAlphaKey[]
						{
							new GradientAlphaKey(1f, 0f),
							new GradientAlphaKey(1f, 0.5f),
							new GradientAlphaKey(1f, 1f)
						},
						colorKeys = new GradientColorKey[]
						{
							new GradientColorKey(new Color32(0, 255,255,255), 0f),
							new GradientColorKey(new Color32(0, 255,255,255), 0.5f),
							new GradientColorKey(new Color32(0, 255,255,255), 1f)
						}
					};
					__instance.Day.FogColor = new Gradient
					{
						alphaKeys = new GradientAlphaKey[]
						{
							new GradientAlphaKey(1f, 0f),
							new GradientAlphaKey(1f, 0.5f),
							new GradientAlphaKey(1f, 1f)
						},
						colorKeys = new GradientColorKey[]
						{
							new GradientColorKey(new Color32(255, 5, 180, byte.MaxValue), 0f),
							new GradientColorKey(new Color32(255, 5, 180, byte.MaxValue), 0.5f),
							new GradientColorKey(new Color32(255, 5, 180, byte.MaxValue), 1f)
						}
					};
					__instance.Day.AmbientColor = new Gradient
					{
						alphaKeys = new GradientAlphaKey[]
						{
							new GradientAlphaKey(1f, 0f),
							new GradientAlphaKey(1f, 0.5f),
							new GradientAlphaKey(1f, 1f)
						},
						colorKeys = new GradientColorKey[]
						{
							new GradientColorKey(new Color32(75, 0, 65, byte.MaxValue), 0f),
							new GradientColorKey(new Color32(75, 0, 65, byte.MaxValue), 0.5f),
							new GradientColorKey(new Color32(75, 0, 65, byte.MaxValue), 1f)
						}
					};
					__instance.Night.LightColor = new Gradient
					{
						alphaKeys = new GradientAlphaKey[]
						{
							new GradientAlphaKey(1f, 0f),
							new GradientAlphaKey(1f, 0.5f),
							new GradientAlphaKey(1f, 1f)
						},
						colorKeys = new GradientColorKey[]
						{
							new GradientColorKey(new Color32(240, 40, 130, byte.MaxValue), 0f),
							new GradientColorKey(new Color32(240, 40, 130, byte.MaxValue), 0.5f),
							new GradientColorKey(new Color32(240, 40, 130, byte.MaxValue), 1f)
						}
					};
					__instance.Night.RayColor = new Gradient
					{
						alphaKeys = new GradientAlphaKey[]
						{
							new GradientAlphaKey(1f, 0f),
							new GradientAlphaKey(0.2f, 0.5f),
							new GradientAlphaKey(0.2f, 1f)
						},
						colorKeys = new GradientColorKey[]
						{
							new GradientColorKey(new Color32(245, 40, 130, byte.MaxValue), 0f),
							new GradientColorKey(new Color32(245, 40, 130, byte.MaxValue), 0.5f),
							new GradientColorKey(new Color32(245, 40, 130, byte.MaxValue), 1f)
						}
					};
					__instance.Night.SkyColor = new Gradient
					{
						alphaKeys = new GradientAlphaKey[]
						{
							new GradientAlphaKey(1f, 0f),
							new GradientAlphaKey(0.2f, 0.5f),
							new GradientAlphaKey(0.2f, 1f)
						},
						colorKeys = new GradientColorKey[]
						{
							new GradientColorKey(new Color32(0, 0, 0, byte.MaxValue), 0f),
							new GradientColorKey(new Color32(0, 0, 0, byte.MaxValue), 0.5f),
							new GradientColorKey(new Color32(0, 0, 0, byte.MaxValue), 1f)
						}
					};
					__instance.Night.MoonColor = new Gradient()
					{
						alphaKeys = new GradientAlphaKey[]
						{
							new GradientAlphaKey(1f, 0f),
							new GradientAlphaKey(1f, 0.5f),
							new GradientAlphaKey(1f, 1f)
						},
						colorKeys = new GradientColorKey[]
						{
							new GradientColorKey(new Color32(245, 230, 30, byte.MaxValue), 0f),
							new GradientColorKey(new Color32(245, 230, 30, byte.MaxValue), 0.5f),
							new GradientColorKey(new Color32(245, 230, 30, byte.MaxValue), 1f)
						}
					};
					__instance.Night.CloudColor = new Gradient
					{
						alphaKeys = new GradientAlphaKey[]
						{
							new GradientAlphaKey(1f, 0f),
							new GradientAlphaKey(0.1f, 0.5f),
							new GradientAlphaKey(0.1f, 1f)
						},
						colorKeys = new GradientColorKey[]
						{
							new GradientColorKey(new Color32(200, 0, 45,255), 0f),
							new GradientColorKey(new Color32(200, 0, 45,255), 0.5f),
							new GradientColorKey(new Color32(200, 0, 45,255), 1f)
						}
					};
					__instance.Night.FogColor = new Gradient
					{
						alphaKeys = new GradientAlphaKey[]
						{
							new GradientAlphaKey(1f, 0f),
							new GradientAlphaKey(0.2f, 0.5f),
							new GradientAlphaKey(0.2f, 1f)
						},
						colorKeys = new GradientColorKey[]
						{
							new GradientColorKey(new Color32(255, 5, 180, byte.MaxValue), 0f),
							new GradientColorKey(new Color32(255, 5, 180, byte.MaxValue), 0.5f),
							new GradientColorKey(new Color32(255, 5, 180, byte.MaxValue), 1f)
						}
					};
					__instance.Night.AmbientColor = new Gradient
					{
						alphaKeys = new GradientAlphaKey[]
						{
							new GradientAlphaKey(1f, 0f),
							new GradientAlphaKey(0.2f, 0.5f),
							new GradientAlphaKey(0.2f, 1f)
						},
						colorKeys = new GradientColorKey[]
						{
							new GradientColorKey(new Color32(75, 0, 65, byte.MaxValue), 0f),
							new GradientColorKey(new Color32(75, 0, 65, byte.MaxValue), 0.5f),
							new GradientColorKey(new Color32(75, 0, 65, byte.MaxValue), 1f)
						}
					};
				}
			}
		}
	}
}
/*
this.SunSkyColor = new Color32(18, 10, 29, byte.MaxValue);
this.SunLightColor = new Color32(240, 40, 130, byte.MaxValue);
this.SunRayColor = new Color32(240, 40, 130, byte.MaxValue);
this.SunMeshColor = new Color32(245, 230, 30, byte.MaxValue);
this.SunCloudColor = new Color32(0, byte.MaxValue, byte.MaxValue, byte.MaxValue);

this.MoonSkyColor = new Color32(18, 10, 29, byte.MaxValue);
this.MoonLightColor = new Color32(240, 40, 130, byte.MaxValue);
this.MoonRayColor = new Color32(245, 230, 30, byte.MaxValue);
this.MoonMeshColor = new Color32(245, 230, 30, byte.MaxValue);
this.MoonCloudColor = new Color32(200, 0, 45, byte.MaxValue);

this.MoonHaloColor = TOD_Util.MulRGB(this.MoonSkyColor, this.Moon.HaloBrightness * num109);

this.AmbientColor = new Color32(50, 200, 80, byte.MaxValue);
this.FogColor = new Color32(150, 150, 250, byte.MaxValue);
this.GroundColor = new Color32(75, 0, 65, byte.MaxValue);
*/
