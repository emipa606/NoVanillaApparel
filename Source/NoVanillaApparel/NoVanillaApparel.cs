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
        var settings = NoVanillaApparelMod.Instance.Settings;
        var removeAll = settings.RemoveUpperBody &&
                        settings.RemoveHeadgear &&
                        settings.RemoveLowerBody &&
                        settings.RemoveUtility &&
                        settings.RemoveArmor;
        foreach (var thingDef in vanillaApparel)
        {
            var isUtility = thingDef.apparel.layers?.Contains(ApparelLayerDefOf.Belt) == true;
            if (isUtility && !settings.RemoveUtility)
            {
                continue;
            }

            if (removeAll)
            {
                apparelToRemove.Add(thingDef);
                continue;
            }

            var remove = thingDef.apparel.CoversBodyPartGroup(BodyPartGroupDefOf.Legs) && settings.RemoveLowerBody;

            if (!remove && thingDef.apparel.CoversBodyPartGroup(BodyPartGroupDefOf.Torso) && settings.RemoveUpperBody)
            {
                remove = true;
            }

            if (!remove && thingDef.apparel.CoversBodyPartGroup(BodyPartGroupDefOf.FullHead) && settings.RemoveHeadgear)
            {
                remove = true;
            }

            if (!remove && isUtility && settings.RemoveUtility)
            {
                remove = true;
            }

            if (!remove && settings.RemoveArmor)
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