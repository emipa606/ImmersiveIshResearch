using System.Reflection;
using HarmonyLib;
using Verse;

namespace ImmersiveIshResearch;

[StaticConstructorOnStartup]
public class ResearchTreePatch
{
    static ResearchTreePatch()
    {
        new Harmony("Mlie.ImmersiveIshResearch.ResearchTree").PatchAll(Assembly.GetExecutingAssembly());
        Log.Message("[ImmersiveishResearch]: Patched for ResearchTree");
    }
}