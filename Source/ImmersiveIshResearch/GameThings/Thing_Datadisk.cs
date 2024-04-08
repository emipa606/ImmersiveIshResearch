using System.Text;
using Verse;

namespace ImmersiveResearch;

internal class Thing_Datadisk : ThingWithComps
{
    private string _uniqueDescription;

    public string UniqueDescription => _uniqueDescription;

    public override void PostMake()
    {
        base.PostMake();
        GetComp<DatadiskThingComp>().datadiskDescription =
            LoreComputerHarmonyPatches.GenerateRandomDatadiskDescription(this);
        _uniqueDescription = GetComp<DatadiskThingComp>().datadiskDescription;
    }

    public override string GetInspectString()
    {
        if (this.TryGetComp<DatadiskThingComp>() == null)
        {
            Log.Error("datadisk thing comp is null.");
            return "";
        }

        var sBuilder = new StringBuilder();

        sBuilder.Append("Datadisk Contents: \n");
        sBuilder.Append(_uniqueDescription);
        return sBuilder.ToString();
    }

    public override void ExposeData()
    {
        base.ExposeData();
        Scribe_Values.Look(ref _uniqueDescription, "datadiskDescription");
    }
}