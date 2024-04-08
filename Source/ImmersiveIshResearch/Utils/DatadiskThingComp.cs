using Verse;

namespace ImmersiveResearch;

internal class DatadiskThingComp : ThingComp
{
    public string datadiskDescription;
    public DatadiskCompProperties Props => (DatadiskCompProperties)props;
}