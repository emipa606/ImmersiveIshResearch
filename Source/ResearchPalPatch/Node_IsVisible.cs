using HarmonyLib;
using ImmersiveResearch;
using ResearchPowl;

namespace ImmersiveIshResearch;

[HarmonyPatch(typeof(Node), "IsVisible")]
public static class Node_IsVisible
{
    public static void Postfix(Node __instance, ref bool __result)
    {
        if (!__result)
        {
            return;
        }

        if (__instance is not ResearchNode node)
        {
            return;
        }

        __result =
            LoreComputerHarmonyPatches.UndiscoveredResearchList.MainResearchDict
                .ContainsKey(node.Research.defName) &&
            LoreComputerHarmonyPatches.UndiscoveredResearchList.MainResearchDict[node.Research.defName]
                .IsDiscovered;
    }
}