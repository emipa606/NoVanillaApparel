using System.Collections.Generic;
using System.Linq;
using RimWorld;
using Verse;

namespace NoVanillaApparel;

[StaticConstructorOnStartup]
internal static class NoVanillaApparel
{
    static NoVanillaApparel()
    {
        var vanillaApparel = (from ThingDef apparel in DefDatabase<ThingDef>.AllDefsListForReading
            where apparel is { IsApparel: true, modContentPack: { IsOfficialMod: true }, destroyOnDrop: false }
            select apparel).ToList();

        foreach (var thingDef in vanillaApparel)
        {
            thingDef.destroyOnDrop = true;
            thingDef.generateCommonality = 0;
            thingDef.apparel.defaultOutfitTags?.Clear();

            thingDef.apparel.tags?.Clear();

            thingDef.generateAllowChance = 0;
            thingDef.recipeMaker = null;
            thingDef.scatterableOnMapGen = false;
            thingDef.tradeability = Tradeability.None;
            thingDef.tradeTags?.Clear();
        }

        for (var i = vanillaApparel.Count - 1; i > 0; i--)
        {
            GenGeneric.InvokeStaticMethodOnGenericType(typeof(DefDatabase<>), typeof(ThingDef), "Remove",
                vanillaApparel[i]);
        }

        DefDatabase<ThingDef>.ResolveAllReferences();

        var apparelRecipes = from recipe in DefDatabase<RecipeDef>.AllDefsListForReading
            where vanillaApparel.Contains(recipe.ProducedThingDef) || (from product in recipe.products
                where vanillaApparel.Contains(product.thingDef)
                select product).Any()
            select recipe;

        foreach (var apparelRecipe in apparelRecipes)
        {
            apparelRecipe.factionPrerequisiteTags = new List<string> { "NotForYou" };
        }

        DefDatabase<RecipeDef>.ResolveAllReferences();

        var vanillaNames = new List<string>();
        vanillaApparel.ForEach(def => vanillaNames.Add(def.label));
        Log.Message(
            $"[NoVanillaApparel]: Removed {vanillaApparel.Count} vanilla apparel: {string.Join(",", vanillaNames)}");
    }
}