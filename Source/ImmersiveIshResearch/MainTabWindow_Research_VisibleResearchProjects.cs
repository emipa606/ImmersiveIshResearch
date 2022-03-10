using System.Collections.Generic;
using HarmonyLib;
using RimWorld;
using Verse;

namespace ImmersiveResearch;

[HarmonyPatch(typeof(MainTabWindow_Research), "VisibleResearchProjects", MethodType.Getter)]
public static class MainTabWindow_Research_VisibleResearchProjects
{
    public static void Postfix(ref List<ResearchProjectDef> __result)
    {
        for (var index = 0; index < __result.Count; ++index)
        {
            var project = __result[index].defName;
            if (LoreComputerHarmonyPatches.UndiscoveredResearchList.MainResearchDict[project]?.IsDiscovered != true)
            {
                __result.RemoveAt(index);
            }
        }
    }
}