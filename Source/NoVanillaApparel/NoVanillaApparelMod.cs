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
    public static NoVanillaApparelMod instance;

    private static string currentVersion;

    /// <summary>
    ///     Constructor
    /// </summary>
    /// <param name="content"></param>
    public NoVanillaApparelMod(ModContentPack content) : base(content)
    {
        instance = this;
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
        var listing_Standard = new Listing_Standard();
        listing_Standard.Begin(rect);
        listing_Standard.Label("NVA.ChangeRequiresRestart".Translate());
        listing_Standard.Gap();
        listing_Standard.CheckboxLabeled("NVA.RemoveHeadgear".Translate(), ref Settings.RemoveHeadgear);
        listing_Standard.CheckboxLabeled("NVA.RemoveUpperBody".Translate(), ref Settings.RemoveUpperBody);
        listing_Standard.CheckboxLabeled("NVA.RemoveLowerBody".Translate(), ref Settings.RemoveLowerBody);
        listing_Standard.CheckboxLabeled("NVA.RemoveArmor".Translate(), ref Settings.RemoveArmor);

        if (currentVersion != null)
        {
            listing_Standard.Gap();
            GUI.contentColor = Color.gray;
            listing_Standard.Label("NVA.ModVersion".Translate(currentVersion));
            GUI.contentColor = Color.white;
        }

        listing_Standard.End();
    }
}