using HarmonyLib;
using UnityEngine;

namespace EarlyGameNosh {
	public static class EarlyGameNoshPatches {
        [HarmonyPatch(typeof(BeanPlantConfig))]
        [HarmonyPatch(nameof(BeanPlantConfig.CreatePrefab))]
        public static class BeanPlantConfig_CreatePrefab_Patch {
            public static GameObject Postfix(GameObject template) {
                template.AddOrGet<TemperatureVulnerable>().Configure(198.15f, 248.15f, 293.15f, 343.15f);
                return template;
            }
        }
	}
}
