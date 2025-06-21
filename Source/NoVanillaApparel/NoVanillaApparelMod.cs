using Mlie;
using UnityEngine;
using Verse;

namespace NoVanillaApparel;

[StaticConstructorOnStartup]
internal class NoVanillaApparelMod : Mod
{
    /// <summary>
    ///     The instance of the settings to be read by the mod
    /// </summary>
    public static NoVanillaApparelMod Instance;

    private static string currentVersion;

    /// <summary>
    ///     Constructor
    /// </summary>
    /// <param name="content"></param>
    public NoVanillaApparelMod(ModContentPack content) : base(content)
    {
        Instance = this;
        Settings = GetSettings<NoVanillaApparelSettings>();
        currentVersion = VersionFromManifest.GetVersionFromModMetaData(content.ModMetaData);
    }

    /// <summary>
    ///     The instance-settings for the mod
    /// </summary>
    internal NoVanillaApparelSettings Settings { get; }

    /// <summary>
    ///     The title for the mod-settings
    /// </summary>
    /// <returns></returns>
    public override string SettingsCategory()
    {
        return "No Vanilla Apparel";
    }

    /// <summary>
    ///     The settings-window
    ///     For more info: https://rimworldwiki.com/wiki/Modding_Tutorials/ModSettings
    /// </summary>
    /// <param name="rect"></param>
    public override void DoSettingsWindowContents(Rect rect)
    {
        var listingStandard = new Listing_Standard();
        listingStandard.Begin(rect);
        listingStandard.Label("NVA.ChangeRequiresRestart".Translate());
        listingStandard.Gap();
        listingStandard.CheckboxLabeled("NVA.RemoveHeadgear".Translate(), ref Settings.RemoveHeadgear);
        listingStandard.CheckboxLabeled("NVA.RemoveUpperBody".Translate(), ref Settings.RemoveUpperBody);
        listingStandard.CheckboxLabeled("NVA.RemoveLowerBody".Translate(), ref Settings.RemoveLowerBody);
        listingStandard.CheckboxLabeled("NVA.RemoveArmor".Translate(), ref Settings.RemoveArmor);

        if (currentVersion != null)
        {
            listingStandard.Gap();
            GUI.contentColor = Color.gray;
            listingStandard.Label("NVA.ModVersion".Translate(currentVersion));
            GUI.contentColor = Color.white;
        }

        listingStandard.End();
    }
}