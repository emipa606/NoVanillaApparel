using System.Collections.Generic;
using System.Linq;
using RimWorld;
using Verse;

namespace NoVanillaApparel
{
    // Token: 0x02000004 RID: 4
    [StaticConstructorOnStartup]
    internal static class NoVanillaApparel
    {
        // Token: 0x0600000A RID: 10 RVA: 0x00002678 File Offset: 0x00000878
        static NoVanillaApparel()
        {
            var vanillaApparelDefNames = new List<string>
            {
                "Apparel_ShieldBelt",
                "Apparel_CowboyHat",
                "Apparel_BowlerHat",
                "Apparel_TribalHeaddress",
                "Apparel_Tuque",
                "Apparel_WarMask",
                "Apparel_WarVeil",
                "Apparel_SimpleHelmet",
                "Apparel_AdvancedHelmet",
                "Apparel_PowerArmorHelmet",
                "Apparel_ArmorHelmetRecon",
                "Apparel_PsychicFoilHelmet",
                "Apparel_SmokepopBelt",
                "Apparel_TribalA",
                "Apparel_Parka",
                "Apparel_Pants",
                "Apparel_BasicShirt",
                "Apparel_CollarShirt",
                "Apparel_Duster",
                "Apparel_Jacket",
                "Apparel_PlateArmor",
                "Apparel_FlakVest",
                "Apparel_FlakPants",
                "Apparel_FlakJacket",
                "Apparel_PowerArmor",
                "Apparel_ArmorRecon"
            };
            var vanillaApparel = new List<ThingDef>();

            DefDatabase<ThingDef>.AllDefsListForReading.ForEach(delegate(ThingDef thing)
            {
                if (!vanillaApparelDefNames.Contains(thing.defName))
                {
                    return;
                }

                thing.destroyOnDrop = true;
                thing.generateCommonality = 0;
                thing.apparel.defaultOutfitTags?.Clear();

                thing.apparel.tags?.Clear();

                thing.generateAllowChance = 0;
                thing.recipeMaker = null;
                thing.scatterableOnMapGen = false;
                thing.tradeability = Tradeability.None;
                thing.tradeTags?.Clear();

                vanillaApparel.Add(thing);
            });
            foreach (var apparel in vanillaApparel)
            {
                GenGeneric.InvokeStaticMethodOnGenericType(typeof(DefDatabase<>), typeof(ThingDef), "Remove", apparel);
            }

            DefDatabase<RecipeDef>.AllDefsListForReading.ForEach(delegate(RecipeDef recipe)
            {
                if ((recipe.ProducedThingDef == null ||
                     !vanillaApparelDefNames.Contains(recipe.ProducedThingDef.defName)) &&
                    !(from ThingDefCountClass thing in recipe.products
                        where vanillaApparelDefNames.Contains(thing.thingDef.defName)
                        select thing).Any())
                {
                    return;
                }

                Log.Message($"factionPrerequisiteTags {recipe.label}");
                recipe.factionPrerequisiteTags = new List<string> {"NotForYou"};
            });
        }
    }
}