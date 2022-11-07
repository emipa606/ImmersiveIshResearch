using RimWorld;
using Verse;

namespace ImmersiveResearch;

public class Bill_Experiment : Experiment, IExposable
{
    public FloatRange hpRange = FloatRange.ZeroToOne;

    public Zone_Stockpile includeFromZone;

    public bool limitToAllowedStuff;

    public bool paused;

    public QualityRange qualityRange = QualityRange.All;
    private BillStoreModeDef storeMode = BillStoreModeDefOf.BestStockpile;

    private Zone_Stockpile storeZone;

    public Bill_Experiment(RecipeDef recipe)
        : base(recipe)
    {
    }
}