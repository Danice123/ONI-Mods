using CaiLib.Utils;
using HarmonyLib;
using static CaiLib.Utils.BuildingUtils;
using static CaiLib.Utils.StringUtils;

namespace Fermenter
{
	public static class FermenterPatches
	{
		[HarmonyPatch(typeof(GeneratedBuildings))]
		[HarmonyPatch(nameof(GeneratedBuildings.LoadGeneratedBuildings))]
		public static class GeneratedBuildings_LoadGeneratedBuildings_Patch
		{
			public static void Prefix()
			{
				AddBuildingStrings(FermenterConfig.Id, FermenterConfig.DisplayName, FermenterConfig.Description, FermenterConfig.Effect);
				AddBuildingToPlanScreen(GameStrings.PlanMenuCategory.Refinement, FermenterConfig.Id);
			}
		}

		[HarmonyPatch(typeof(Db))]
		[HarmonyPatch("Initialize")]
		public static class Db_Initialize_Patch
		{
			public static void Postfix()
			{
				AddBuildingToTechnology(GameStrings.Technology.Food.FoodRepurposing, FermenterConfig.Id);
			}
		}
	}
}
