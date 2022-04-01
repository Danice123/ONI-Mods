using HarmonyLib;
using STRINGS;
using TUNING;

namespace Fermenter {
	public static class FermenterPatches {
		[HarmonyPatch(typeof(GeneratedBuildings))]
		[HarmonyPatch(nameof(GeneratedBuildings.LoadGeneratedBuildings))]
		public static class GeneratedBuildings_LoadGeneratedBuildings_Patch{
			public static void Prefix() {
				Strings.Add($"STRINGS.BUILDINGS.PREFABS.{FermenterConfig.Id.ToUpperInvariant()}.NAME", UI.FormatAsLink(FermenterConfig.DisplayName, FermenterConfig.Id));
            	Strings.Add($"STRINGS.BUILDINGS.PREFABS.{FermenterConfig.Id.ToUpperInvariant()}.DESC", FermenterConfig.Description);
            	Strings.Add($"STRINGS.BUILDINGS.PREFABS.{FermenterConfig.Id.ToUpperInvariant()}.EFFECT", FermenterConfig.Effect);

				ModUtil.AddBuildingToPlanScreen("Refining", FermenterConfig.Id);
			}
		}

		[HarmonyPatch(typeof(Db))]
		[HarmonyPatch("Initialize")]
		public static class Db_Initialize_Patch{
			public static void Postfix() {
				Db.Get().Techs.Get("FoodRepurposing").unlockedItemIDs.Add(FermenterConfig.Id);
			}
		}
	}
}
