using TUNING;
using UnityEngine;
using System.Collections.Generic;

namespace Fermenter {

    public class FermenterConfig : IBuildingConfig {
        public static string Id = "EthanolFermenter";
		public const string DisplayName = "Ethanol Fermenter";
		public static string Description = "Some sorta description";
		public const string Effect = "Can make the ethanols.";

        public override BuildingDef CreateBuildingDef() {
			var buildingDef = BuildingTemplates.CreateBuildingDef(
				id: Id,
				width: 2,
				height: 2,
				anim: "compost_kanim",
				hitpoints: BUILDINGS.HITPOINTS.TIER1,
				construction_time: BUILDINGS.CONSTRUCTION_TIME_SECONDS.TIER1,
				construction_mass: BUILDINGS.CONSTRUCTION_MASS_KG.TIER2,
				construction_materials: MATERIALS.ALL_METALS,
				melting_point: BUILDINGS.MELTING_POINT_KELVIN.TIER0,
				build_location_rule: BuildLocationRule.OnFloor,
				decor: BUILDINGS.DECOR.PENALTY.TIER3,
				noise: NOISE_POLLUTION.NONE);

            buildingDef.ExhaustKilowattsWhenActive = 0.125f;
            buildingDef.SelfHeatKilowattsWhenActive = 1f;
            buildingDef.Overheatable = false;
            buildingDef.AudioCategory = "HollowMetal";
            buildingDef.ViewMode = OverlayModes.LiquidConduits.ID;
            buildingDef.OutputConduitType = ConduitType.Liquid;
            buildingDef.UtilityOutputOffset = new CellOffset(1, 0);
            SoundEventVolumeCache.instance.AddVolume("anim_interacts_compost_kanim", "Compost_shovel_in", NOISE_POLLUTION.NOISY.TIER2);
            SoundEventVolumeCache.instance.AddVolume("anim_interacts_compost_kanim", "Compost_shovel_out", NOISE_POLLUTION.NOISY.TIER2);

			return buildingDef;
		}

        public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag) {
            go.GetComponent<KPrefabID>().AddTag(RoomConstraints.ConstraintTags.IndustrialMachinery);
            Prioritizable.AddRef(go);
            go.AddOrGet<BuildingComplete>().isManuallyOperated = true;

            ComplexFabricator fabricator = go.AddOrGet<ComplexFabricator>();
            fabricator.sideScreenStyle = ComplexFabricatorSideScreen.StyleSetting.ListQueueHybrid;
            fabricator.duplicantOperated = true;
            go.AddOrGet<FabricatorIngredientStatusManager>();
            go.AddOrGet<CopyBuildingSettings>();

            ComplexFabricatorWorkable fabricatorWorkable = go.AddOrGet<ComplexFabricatorWorkable>();
            BuildingTemplates.CreateComplexFabricatorStorage(go, fabricator);
            fabricator.outStorage.capacityKg = 2000f;
            fabricator.storeProduced = true;

            fabricatorWorkable.overrideAnims = new KAnimFile[1] {
                Assets.GetAnim((HashedString) "anim_interacts_compost_kanim")
            };
            fabricatorWorkable.workingPstComplete = new HashedString[1]
            {
            (HashedString) "working_pst_complete"
            };

            ConduitDispenser conduitDispenser = go.AddOrGet<ConduitDispenser>();
            conduitDispenser.storage = fabricator.outStorage;
            conduitDispenser.conduitType = ConduitType.Liquid;
            conduitDispenser.alwaysDispense = true;
            conduitDispenser.elementFilter = (SimHashes[]) null;

            this.ConfigureRecipes();
		}

        private void ConfigureRecipes() {
            ComplexRecipe.RecipeElement[] recipeElementArray1 = new ComplexRecipe.RecipeElement[1] {
                new ComplexRecipe.RecipeElement((Tag) "BasicPlantFood", 2f)
            };

            ComplexRecipe.RecipeElement[] recipeElementArray2 = new ComplexRecipe.RecipeElement[1] {
                new ComplexRecipe.RecipeElement(ElementLoader.FindElementByHash(SimHashes.Ethanol).tag, 2f)
            };
            
            string obsolete_id = ComplexRecipeManager.MakeObsoleteRecipeID("EthanolFermenter", (Tag) "BasicPlantFood");
            string str = ComplexRecipeManager.MakeRecipeID("EthanolFermenter", (IList<ComplexRecipe.RecipeElement>) recipeElementArray1, (IList<ComplexRecipe.RecipeElement>) recipeElementArray2);
            
            ComplexRecipe complexRecipe = new ComplexRecipe(str, recipeElementArray1, recipeElementArray2)
            {
                time = 20f,
                description = "Convert Mealwood into Ethanol",
                nameDisplay = ComplexRecipe.RecipeNameDisplay.IngredientToResult,
                fabricators = new List<Tag>() {
                    TagManager.Create("EthanolFermenter")
                }
            };
            ComplexRecipeManager.Get().AddObsoleteIDMapping(obsolete_id, str);
        }

        public override void DoPostConfigureComplete(GameObject go)
		{
		}
    }
}