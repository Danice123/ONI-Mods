using HarmonyLib;
using UnityEngine;

namespace OrbitalScienceFix {
	public static class OrbitalScienceFixPatches {
        [HarmonyPatch(typeof(OrbitalResearchCenterConfig))]
        [HarmonyPatch(nameof(OrbitalResearchCenterConfig.ConfigureBuildingTemplate))]
        public static class OrbitalResearchCenterConfig_ConfigureBuildingTemplate_Patch {
            public static void Prefix(ref GameObject go, Tag prefab_tag) {
                go.AddOrGet<OrbitalScienceCenter>();
            }
        }
	}
}
