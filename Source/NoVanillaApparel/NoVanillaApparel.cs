using System.Collections.Generic;
using System.Linq;
using RimWorld;
using Verse;

namespace NoVanillaApparel;

[StaticConstructorOnStartup]
internal static class NoVanillaApparel
{
    private const float ArmorLimit = 0.4f;

    static NoVanillaApparel()
    {
        var vanillaApparel = (from ThingDef apparel in DefDatabase<ThingDef>.AllDefsListForReading
            where apparel is { IsApparel: true, modContentPack.IsOfficialMod: true, destroyOnDrop: false }
            select apparel).ToList();

        var apparelToRemove = new List<ThingDef>();
        var removeAll = NoVanillaApparelMod.Instance.Settings.RemoveUpperBody &&
                        NoVanillaApparelMod.Instance.Settings.RemoveHeadgear &&
                        NoVanillaApparelMod.Instance.Settings.RemoveLowerBody &&
                        NoVanillaApparelMod.Instance.Settings.RemoveArmor;
        foreach (var thingDef in vanillaApparel)
        {
            if (removeAll)
            {
                apparelToRemove.Add(thingDef);
                continue;
            }

            var remove = thingDef.apparel.CoversBodyPartGroup(BodyPartGroupDefOf.Legs) &&
                         NoVanillaApparelMod.Instance.Settings.RemoveLowerBody;

            if (!remove && thingDef.apparel.CoversBodyPartGroup(BodyPartGroupDefOf.Torso) &&
                NoVanillaApparelMod.Instance.Settings.RemoveUpperBody)
            {
                remove = true;
            }

            if (!remove && thingDef.apparel.CoversBodyPartGroup(BodyPartGroupDefOf.FullHead) &&
                NoVanillaApparelMod.Instance.Settings.RemoveHeadgear)
            {
                remove = true;
            }

            if (!remove && NoVanillaApparelMod.Instance.Settings.RemoveArmor)
            {
                if (thingDef.StatBaseDefined(StatDefOf.ArmorRating_Blunt) &&
                    thingDef.GetStatValueAbstract(StatDefOf.ArmorRating_Blunt) > ArmorLimit ||
                    thingDef.StatBaseDefined(StatDefOf.ArmorRating_Sharp) &&
                    thingDef.GetStatValueAbstract(StatDefOf.ArmorRating_Sharp) > ArmorLimit ||
                    thingDef.StatBaseDefined(StatDefOf.StuffEffectMultiplierArmor) &&
                    thingDef.GetStatValueAbstract(StatDefOf.StuffEffectMultiplierArmor) > ArmorLimit)
                {
                    remove = true;
                }
            }

            if (remove)
            {
                apparelToRemove.Add(thingDef);
            }
        }

        for (var i = apparelToRemove.Count - 1; i > 0; i--)
        {
            apparelToRemove[i].generateCommonality = 0;
            apparelToRemove[i].weaponTags = [];
            apparelToRemove[i].tradeTags = [];
            apparelToRemove[i].apparel.tags = [];
            apparelToRemove[i].apparel.canBeGeneratedToSatisfyWarmth = false;
            apparelToRemove[i].apparel.canBeDesiredForIdeo = false;
            apparelToRemove[i].apparel.canBeGeneratedToSatisfyToxicEnvironmentResistance = false;

            GenGeneric.InvokeStaticMethodOnGenericType(typeof(DefDatabase<>), typeof(ThingDef), "Remove",
                apparelToRemove[i]);
        }

        DefDatabase<ThingDef>.ResolveAllReferences();

        var apparelRecipes = from recipe in DefDatabase<RecipeDef>.AllDefsListForReading
            where apparelToRemove.Contains(recipe.ProducedThingDef) || (from product in recipe.products
                where apparelToRemove.Contains(product.thingDef)
                select product).Any()
            select recipe;

        foreach (var apparelRecipe in apparelRecipes)
        {
            apparelRecipe.factionPrerequisiteTags = ["NotForYou"];
        }

        DefDatabase<RecipeDef>.ResolveAllReferences();

        var vanillaNames = new List<string>();
        apparelToRemove.ForEach(def => vanillaNames.Add(def.label));
        Log.Message(
            $"[NoVanillaApparel]: Removed {apparelToRemove.Count} vanilla apparel: {string.Join(",", vanillaNames)}");
    }
}