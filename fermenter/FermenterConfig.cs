using TUNING;
using UnityEngine;
using System.Collections.Generic;

// heat building until 25 C then turn off power E.G. fridge

namespace Fermenter {

    public class FermenterConfig : IBuildingConfig {
        public static string Id = "EthanolFermenter";
		public const string DisplayName = "Ethanol Fermenter";
		public const string Effect = "Turns food into ethanol.";
        public static string Description = "Now your food won't have to go to waste.";

        public override BuildingDef CreateBuildingDef() {
			var buildingDef = BuildingTemplates.CreateBuildingDef(
				id: Id,
				width: 3,
				height: 2,
				anim: "fermenter_kanim",
				hitpoints: BUILDINGS.HITPOINTS.TIER1,
				construction_time: BUILDINGS.CONSTRUCTION_TIME_SECONDS.TIER1,
				construction_mass: BUILDINGS.CONSTRUCTION_MASS_KG.TIER2,
				construction_materials: MATERIALS.ALL_METALS,
				melting_point: BUILDINGS.MELTING_POINT_KELVIN.TIER0,
				build_location_rule: BuildLocationRule.OnFloor,
				decor: BUILDINGS.DECOR.PENALTY.TIER3,
				noise: NOISE_POLLUTION.NONE);
            BuildingTemplates.CreateElectricalBuildingDef(buildingDef);
            buildingDef.AudioCategory = "HollowMetal";
            buildingDef.ViewMode = OverlayModes.LiquidConduits.ID;
            buildingDef.OutputConduitType = ConduitType.Liquid;
            buildingDef.UtilityOutputOffset = new CellOffset(1, 0);
            buildingDef.OverheatTemperature = 313.15f;
            buildingDef.EnergyConsumptionWhenActive = 60f;
            buildingDef.SelfHeatKilowattsWhenActive = 0.25f;
            buildingDef.LogicInputPorts = LogicOperationalController.CreateSingleInputPortList(new CellOffset(0, 1));
			return buildingDef;
		}

        public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag) {
            go.GetComponent<KPrefabID>().AddTag(RoomConstraints.ConstraintTags.IndustrialMachinery);

            go.AddOrGet<MinimumOperatingTemperature>().minimumTemperature = 298.15f;

            go.AddOrGet<BuildingComplete>().isManuallyOperated = false;

            ComplexFabricator fabricator = go.AddOrGet<ComplexFabricator>();
            fabricator.heatedTemperature = 313.15f;
            fabricator.duplicantOperated = false;
            fabricator.sideScreenStyle = ComplexFabricatorSideScreen.StyleSetting.ListQueueHybrid;
            go.AddOrGet<FabricatorIngredientStatusManager>();
            go.AddOrGet<CopyBuildingSettings>();
            BuildingTemplates.CreateComplexFabricatorStorage(go, fabricator);
            fabricator.outStorage.capacityKg = 100f;
            fabricator.storeProduced = true;

            ConduitDispenser conduitDispenser = go.AddOrGet<ConduitDispenser>();
            conduitDispenser.storage = fabricator.outStorage;
            conduitDispenser.conduitType = ConduitType.Liquid;
            conduitDispenser.alwaysDispense = true;
            conduitDispenser.elementFilter = new SimHashes[1]{SimHashes.Ethanol};

            this.ConfigureRecipes();

            Prioritizable.AddRef(go);
		}

        private void ConfigureRecipes() {
            ComplexRecipe.RecipeElement[] ethanolOutput = new ComplexRecipe.RecipeElement[1] {
                new ComplexRecipe.RecipeElement(ElementLoader.FindElementByHash(SimHashes.Ethanol).tag, 20f, ComplexRecipe.RecipeElement.TemperatureOperation.Heated, true)
            };

            (string id, string name, float val)[] recipes = new (string, string, float)[13] {
                (BasicPlantFoodConfig.ID, "Meal Lice", 0.5f),
                (MushroomConfig.ID, "Mushroom", 0.1333f),
                (PrickleFruitConfig.ID, "Bristle Berry", 0.1666f),
                (ColdWheatConfig.SEED_ID, "Sleet Wheat Grain", 1f),
                (LettuceConfig.ID, "Lettuce", 1f),
                (BeanPlantConfig.SEED_ID, "Nosh Bean", 1f),
                (BasicForagePlantConfig.ID, "Muckroot", 0.5f),
                (ForestForagePlantConfig.ID, "Hexalent Fruit", 0.0625f),

                (SwampFruitConfig.ID, "Bog Jelly", 0.1515f),
                (WormBasicFruitConfig.ID, "Spindly Grubfruit", 0.5f),
                (WormSuperFruitConfig.ID, "Grubfruit", 1f),
                (SwampForagePlantConfig.ID, "Swamp Chard Heart", 0.1666f),
                (PlantMeatConfig.ID, "Plant Meat", 0.333f),
            };

            foreach (var recipe in recipes) {
                ComplexRecipe.RecipeElement[] ingredients = new ComplexRecipe.RecipeElement[1] {
                    new ComplexRecipe.RecipeElement((Tag) recipe.id, recipe.val)
                };

                string obsolete_id = ComplexRecipeManager.MakeObsoleteRecipeID("EthanolFermenter", (Tag) recipe.id);
                string str = ComplexRecipeManager.MakeRecipeID("EthanolFermenter", (IList<ComplexRecipe.RecipeElement>) ingredients, (IList<ComplexRecipe.RecipeElement>) ethanolOutput);
                
                ComplexRecipe complexRecipe = new ComplexRecipe(str, ingredients, ethanolOutput) {
                    time = 20f,
                    description = $"Convert {recipe.name} into Ethanol",
                    nameDisplay = ComplexRecipe.RecipeNameDisplay.IngredientToResult,
                    fabricators = new List<Tag>() {
                        TagManager.Create("EthanolFermenter")
                    }
                };
                ComplexRecipeManager.Get().AddObsoleteIDMapping(obsolete_id, str);
            }
        }

        public override void DoPostConfigureComplete(GameObject go) {
            go.AddOrGet<LogicOperationalController>();
            go.AddOrGetDef<PoweredActiveController.Def>();
            SymbolOverrideControllerUtil.AddToPrefab(go);
		}
    }
}