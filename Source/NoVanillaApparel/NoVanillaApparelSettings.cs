using Verse;

namespace NoVanillaApparel;

/// <summary>
///     Definition of the settings for the mod
/// </summary>
internal class NoVanillaApparelSettings : ModSettings
{
    public bool RemoveArmor = true;
    public bool RemoveHeadgear = true;
    public bool RemoveLowerBody = true;
    public bool RemoveUpperBody = true;

    /// <summary>
    ///     Saving and loading the values
    /// </summary>
    public override void ExposeData()
    {
        base.ExposeData();
        Scribe_Values.Look(ref RemoveHeadgear, "RemoveHeadgear", true);
        Scribe_Values.Look(ref RemoveArmor, "RemoveArmor", true);
        Scribe_Values.Look(ref RemoveUpperBody, "RemoveUpperBody", true);
        Scribe_Values.Look(ref RemoveLowerBody, "RemoveLowerBody", true);
    }
}