using System.Collections.Generic;
using System.Linq;
using RimWorld;
using Verse;

namespace NoVanillaApparel;

[StaticConstructorOnStartup]
internal static class NoVanillaApparel
{
    private static readonly float armorLimit = 0.4f;

    static NoVanillaApparel()
    {
        var vanillaApparel = (from ThingDef apparel in DefDatabase<ThingDef>.AllDefsListForReading
            where apparel is { IsApparel: true, modContentPack.IsOfficialMod: true, destroyOnDrop: false }
            select apparel).ToList();

        var apparelToRemove = new List<ThingDef>();

        foreach (var thingDef in vanillaApparel)
        {
            var remove = thingDef.apparel.CoversBodyPartGroup(BodyPartGroupDefOf.Legs) &&
                         NoVanillaApparelMod.instance.Settings.RemoveLowerBody;

            if (!remove && thingDef.apparel.CoversBodyPartGroup(BodyPartGroupDefOf.Torso) &&
                NoVanillaApparelMod.instance.Settings.RemoveUpperBody)
            {
                remove = true;
            }

            if (!remove && thingDef.apparel.CoversBodyPartGroup(BodyPartGroupDefOf.FullHead) &&
                NoVanillaApparelMod.instance.Settings.RemoveHeadgear)
            {
                remove = true;
            }

            if (!remove && NoVanillaApparelMod.instance.Settings.RemoveArmor)
            {
                if (thingDef.StatBaseDefined(StatDefOf.ArmorRating_Blunt) &&
                    thingDef.GetStatValueAbstract(StatDefOf.ArmorRating_Blunt) > armorLimit ||
                    thingDef.StatBaseDefined(StatDefOf.ArmorRating_Sharp) &&
                    thingDef.GetStatValueAbstract(StatDefOf.ArmorRating_Sharp) > armorLimit ||
                    thingDef.StatBaseDefined(StatDefOf.StuffEffectMultiplierArmor) &&
                    thingDef.GetStatValueAbstract(StatDefOf.StuffEffectMultiplierArmor) > armorLimit)
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