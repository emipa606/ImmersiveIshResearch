using HarmonyLib;
using ImmersiveResearch;
using ResearchPal;

namespace ImmersiveIshResearch
{
    [HarmonyPatch(typeof(Node), "IsVisible")]
    public static class Node_IsVisible
    {
        public static void Postfix(Node __instance, ref bool __result)
        {
            if (!__result)
            {
                Log.Message($"{__instance} should now be shown anyway");
                return;
            }

            if (__instance is not ResearchNode node)
            {
                Log.Message($"{__instance} is not a research-node");
                return;
            }

            __result =
                LoreComputerHarmonyPatches.UndiscoveredResearchList.MainResearchDict
                    .ContainsKey(node.Research.defName) &&
                LoreComputerHarmonyPatches.UndiscoveredResearchList.MainResearchDict[node.Research.defName]
                    .IsDiscovered;
        }
    }
}