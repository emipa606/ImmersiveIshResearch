using RimWorld;
using Verse;

namespace ImmersiveResearch;

public class Bill_Experiment(RecipeDef recipe) : Experiment(recipe)
{
    public FloatRange hpRange = FloatRange.ZeroToOne;

    public Zone_Stockpile includeFromZone;

    public bool limitToAllowedStuff;

    public bool paused;

    public QualityRange qualityRange = QualityRange.All;
    private BillStoreModeDef storeMode = BillStoreModeDefOf.BestStockpile;

    private Zone_Stockpile storeZone;
}