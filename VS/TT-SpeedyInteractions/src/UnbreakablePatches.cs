using Il2Cpp;
using Il2CppTLD.Gear;
using Il2CppTLD.Interactions;

namespace TinyTweaks
{
    class SpeedyInteractions : MelonMod
    {
        private static float genericSliderTime = 3f;
        private static float craftingSliderTime = 5f;
        private static float shortSliderTime = 2f;
        private static float longSliderTime = 10f;


        public override void OnInitializeMelon()
        {
            Settings.OnLoad();
        }


        internal static void ApplySpeedsToLoadedPanels()
        {
            float g = Settings.options.globalSpeedMult;

            foreach (var p in Resources.FindObjectsOfTypeAll<Panel_BreakDown>())
                p.m_SecondsToBreakDown = genericSliderTime / Settings.options.breakdownSpeedMult;

            foreach (var p in Resources.FindObjectsOfTypeAll<Panel_Crafting>())
                p.m_CraftingDisplayTimeSeconds = craftingSliderTime / g;

            foreach (var p in Resources.FindObjectsOfTypeAll<Panel_Cooking>())
                p.m_RecipePreparationDisplayTimeSeconds = craftingSliderTime / g;

            foreach (var p in Resources.FindObjectsOfTypeAll<Panel_Milling>())
                p.m_RepairRealTimeSeconds = craftingSliderTime / g;

            foreach (var p in Resources.FindObjectsOfTypeAll<Panel_SnowShelterBuild>())
                p.m_RealtimeSecondsToBuild = genericSliderTime / g;

            foreach (var p in Resources.FindObjectsOfTypeAll<Panel_SnowShelterInteract>())
                p.m_RealtimeSecondsToRepairOrDismantle = genericSliderTime / g;

            foreach (var p in Resources.FindObjectsOfTypeAll<Panel_PickWater>())
                p.m_ProgressBarDurationSecondsBase = shortSliderTime / g;

            foreach (var p in Resources.FindObjectsOfTypeAll<Panel_IceFishingHoleClear>())
                p.m_ProgressBarSeconds = longSliderTime / g;

            foreach (var rc in Resources.FindObjectsOfTypeAll<RockCache>())
            {
                rc.m_BuildRealSecondsElapsed = Mathf.CeilToInt(shortSliderTime / g);
                rc.m_DismantleRealSecondsElapsed = Mathf.CeilToInt(shortSliderTime / g);
            }
        }


        [HarmonyPatch(typeof(PlayerManager), nameof(PlayerManager.UseFoodInventoryItem))]
        private static class EatingSpeed
        {
            internal static void Prefix(ref GearItem gi)
            {
                if (Settings.options.eatingSpeedMult != 1f)
                {
                    gi.m_FoodItem.m_TimeToEatSeconds /= Settings.options.eatingSpeedMult;
                    gi.m_FoodItem.m_TimeToOpenAndEatSeconds /= Settings.options.eatingSpeedMult;
                }
            }

            internal static void Postfix(ref GearItem gi)
            {
                if (Settings.options.eatingSpeedMult != 1f)
                {
                    gi.m_FoodItem.m_TimeToEatSeconds *= Settings.options.eatingSpeedMult;
                    gi.m_FoodItem.m_TimeToOpenAndEatSeconds *= Settings.options.eatingSpeedMult;
                }
            }
        }
        [HarmonyPatch(typeof(PlayerManager), nameof(PlayerManager.UseSmashableItem))]
        private static class EatingSpeed2
        {
            static int time = 6;
            internal static void Prefix(ref GearItem gi)
            {
                if (Settings.options.eatingSpeedMult != 1f)
                {
                    time = gi.m_SmashableItem.m_TimeToSmash;
                    gi.m_SmashableItem.m_TimeToSmash = Mathf.FloorToInt(gi.m_SmashableItem.m_TimeToSmash / Settings.options.eatingSpeedMult);
                }
            }

            internal static void Postfix(ref GearItem gi)
            {
                if (Settings.options.eatingSpeedMult != 1f)
                {
                    gi.m_SmashableItem.m_TimeToSmash = time;
                }
            }
        }        

        [HarmonyPatch(typeof(PlayerManager), nameof(PlayerManager.DrinkFromWaterSupply))]
        private static class EatingSpeed3
        {
            internal static void Prefix(ref WaterSupply ws)
            {
                if (Settings.options.eatingSpeedMult != 1f)
                {
                    ws.m_TimeToDrinkSeconds /= Settings.options.eatingSpeedMult;
                }
            }

            internal static void Postfix(ref WaterSupply ws)
            {
                if (Settings.options.eatingSpeedMult != 1f)
                {
                    ws.m_TimeToDrinkSeconds *= Settings.options.eatingSpeedMult;
                }
            }
        }


        [HarmonyPatch(typeof(Panel_Inventory_Examine), nameof(Panel_Inventory_Examine.OnRefuel))]
        private static class RefuelSpeed
        {
            internal static void Prefix(Panel_Inventory_Examine __instance)
            {
                if (Settings.options.globalSpeedMult != 1f)
                {
                    var kli = __instance.m_GearItem.GetComponent<KeroseneLampItem>();
                    if (kli)
                        kli.m_RefuelTimeSeconds /= Settings.options.globalSpeedMult;

                }
            }
            internal static void Postfix(Panel_Inventory_Examine __instance)
            {
                if (Settings.options.globalSpeedMult != 1f)
                {
                    var kli = __instance.m_GearItem.GetComponent<KeroseneLampItem>();
                    if (kli)
                        kli.m_RefuelTimeSeconds *= Settings.options.globalSpeedMult;
                }
            }
        }


        [HarmonyPatch(typeof(PlayerManager), nameof(PlayerManager.UseWaterPurificationItem))]
        private static class WaterPurificationSpeed
        {
            internal static void Prefix(ref GearItem gi)
            {
                if (Settings.options.globalSpeedMult != 1f)
                {
                    gi.m_PurifyWater.m_ProgressBarDurationSeconds /= Settings.options.globalSpeedMult;
                }
            }

            internal static void Postfix(ref GearItem gi)
            {
                if (Settings.options.globalSpeedMult != 1f)
                {
                    gi.m_PurifyWater.m_ProgressBarDurationSeconds *= Settings.options.globalSpeedMult;
                }
            }
        }





        [HarmonyPatch(typeof(Panel_Inventory_Examine), nameof(Panel_Inventory_Examine.AccelerateTimeOfDay))]
        private static class ExamineActionSpeed
        {
            internal static void Prefix(Panel_Inventory_Examine __instance)
            {
                if (__instance.IsResearchItem())
                {
                    if (Settings.options.readingSpeedMult != 1f)
                    {
                        __instance.m_ProgressBarTimeSeconds /= Settings.options.readingSpeedMult;
                    }
                }
                else if (Settings.options.globalSpeedMult != 1f)
                {
                    __instance.m_ProgressBarTimeSeconds /= Settings.options.globalSpeedMult;

                }
            }
        }

        [HarmonyPatch(typeof(RockCache), nameof(RockCache.OnBuild))]
        private static class RockCacheSpeed1
        {
            internal static void Prefix(RockCache __instance)
            {
                if (Settings.options.globalSpeedMult != 1f)
                {
                    __instance.m_BuildRealSecondsElapsed = Mathf.CeilToInt(shortSliderTime / Settings.options.globalSpeedMult);

                }
            }
        }
        [HarmonyPatch(typeof(RockCache), nameof(RockCache.OnDismantle))]
        private static class RockCacheSpeed2
        {
            internal static void Prefix(RockCache __instance)
            {
                if (Settings.options.globalSpeedMult != 1f)
                {
                    __instance.m_DismantleRealSecondsElapsed = Mathf.CeilToInt(shortSliderTime / Settings.options.globalSpeedMult);

                }
            }
        }

        [HarmonyPatch(typeof(Panel_BreakDown), nameof(Panel_BreakDown.OnBreakDown))]
        private static class GlobalSpeed2
        {
            internal static void Prefix(Panel_BreakDown __instance)
            {
                if (Settings.options.breakdownSpeedMult != 1f)
                {
                    __instance.m_SecondsToBreakDown = genericSliderTime / Settings.options.breakdownSpeedMult;

                }
            }
        }

        [HarmonyPatch(typeof(Panel_Crafting), nameof(Panel_Crafting.CraftingStart))]
        private static class GlobalSpeed3
        {
            internal static void Prefix(Panel_Crafting __instance)
            {
                if (Settings.options.globalSpeedMult != 1f)
                {
                    __instance.m_CraftingDisplayTimeSeconds = craftingSliderTime / Settings.options.globalSpeedMult;

                }
            }
        }

        [HarmonyPatch(typeof(Panel_Cooking), nameof(Panel_Cooking.OnCook))]
        private static class GlobalSpeed4
        {
            internal static void Prefix(Panel_Cooking __instance)
            {
                if (Settings.options.globalSpeedMult != 1f)
                {
                    __instance.m_RecipePreparationDisplayTimeSeconds = craftingSliderTime / Settings.options.globalSpeedMult;

                }
            }
        }

        [HarmonyPatch(typeof(Panel_Cooking), nameof(Panel_Cooking.OnCookRecipe))]
        private static class GlobalSpeed5
        {
            internal static void Prefix(Panel_Cooking __instance)
            {
                if (Settings.options.globalSpeedMult != 1f)
                {
                    __instance.m_RecipePreparationDisplayTimeSeconds = craftingSliderTime / Settings.options.globalSpeedMult;

                }
            }
        }

        [HarmonyPatch(typeof(Panel_Milling), nameof(Panel_Milling.BeginRepair))]
        private static class GlobalSpeed6
        {
            internal static void Prefix(Panel_Milling __instance)
            {
                if (Settings.options.globalSpeedMult != 1f)
                {
                    __instance.m_RepairRealTimeSeconds = craftingSliderTime / Settings.options.globalSpeedMult;

                }
            }
        }

        [HarmonyPatch(typeof(Panel_SnowShelterBuild), nameof(Panel_SnowShelterBuild.OnBuild))]
        private static class GlobalSpeed7
        {
            internal static void Prefix(Panel_SnowShelterBuild __instance)
            {
                if (Settings.options.globalSpeedMult != 1f)
                {
                    __instance.m_RealtimeSecondsToBuild = genericSliderTime / Settings.options.globalSpeedMult;

                }
            }
        }

        [HarmonyPatch(typeof(Panel_SnowShelterInteract), nameof(Panel_SnowShelterInteract.OnInteractionCommon))]
        private static class GlobalSpeed8
        {
            internal static void Prefix(Panel_SnowShelterInteract __instance)
            {
                if (Settings.options.globalSpeedMult != 1f)
                {
                    __instance.m_RealtimeSecondsToRepairOrDismantle = genericSliderTime / Settings.options.globalSpeedMult;

                }
            }
        }

        [HarmonyPatch(typeof(Panel_PickWater), nameof(Panel_PickWater.TakeWater))]
        private static class GlobalSpeed9
        {
            internal static void Prefix(Panel_PickWater __instance)
            {
                if (Settings.options.globalSpeedMult != 1f)
                {
                    __instance.m_ProgressBarDurationSecondsBase = shortSliderTime / Settings.options.globalSpeedMult;

                }
            }
        }

        [HarmonyPatch(typeof(Panel_IceFishingHoleClear), nameof(Panel_IceFishingHoleClear.UseTool))]
        private static class GlobalSpeed10
        {
            internal static void Prefix(Panel_IceFishingHoleClear __instance)
            {
                if (Settings.options.globalSpeedMult != 1f)
                {
                    __instance.m_ProgressBarSeconds = longSliderTime / Settings.options.globalSpeedMult;

                }
            }
        }        
        

        [HarmonyPatch(typeof(AfflictionDefinition), nameof(AfflictionDefinition.GetStandardDurationdUsed))]
        private static class AfflictionTreatmentSpeed1
        {
            internal static void Postfix(AfflictionDefinition __instance, ref float __result)
            {
                if (Settings.options.globalSpeedMult != 1f)
                {
                    __result /= Settings.options.globalSpeedMult;
                }
            }
        }

        [HarmonyPatch(typeof(AfflictionDefinition), nameof(AfflictionDefinition.GetAlternateDurationUsed))]
        private static class AfflictionTreatmentSpeed2
        {
            internal static void Postfix(AfflictionDefinition __instance, ref float __result)
            {
                if (Settings.options.globalSpeedMult != 1f)
                {
                    __result /= Settings.options.globalSpeedMult;
                }
            }
        }
        
        /*
        [HarmonyPatch(typeof(Panel_HUD), nameof(Panel_HUD.StartItemProgressBar))]
        private static class ProgressBarSpeed
        {
            internal static void Postfix(Panel_HUD __instance, ref float duration)
            {
                if (Settings.options.globalSpeedMult != 1f)
                {
                    __instance.m_ItemProgressBarDuration = duration / Settings.options.globalSpeedMult;
                }
            }
        }
        */

        [HarmonyPatch(typeof(TimedHoldInteraction), nameof(TimedHoldInteraction.BeginHold))]
        private static class HoldInteractionTime
        {
            public static Dictionary<TimedHoldInteraction, (float baseValue, float lastValue, float lastModifier)> data = new();
            internal static void Prefix(TimedHoldInteraction __instance)
            {
                if (Settings.options.interactionSpeedMult != 1f)
                {
                    float currentValue = __instance.HoldTime;
                    float currentModifier = Settings.options.interactionSpeedMult;

                    if (!data.ContainsKey(__instance)) // init key
                    {
                        __instance.HoldTime = currentValue / currentModifier;

                        data.Add(__instance, (currentValue, __instance.HoldTime, currentModifier));

                        return;
                    }
                    else
                    {
                        var entry = data[__instance];

                        if (Math.Abs(currentValue - entry.lastValue) > 0.0001f) // changed externally, update base value and recalculate
                        {
                            __instance.HoldTime /= currentModifier;
                            entry.baseValue = currentValue;

                        }
                        else if (entry.lastModifier != currentModifier) // unchanged but modifier changed, recalculate
                        {
                            __instance.HoldTime = entry.baseValue / currentModifier;
                        }

                        entry.lastValue = __instance.HoldTime;
                        entry.lastModifier = currentModifier;
                        data[__instance] = entry;
                    }
                }
            }
        }

        [HarmonyPatch(typeof(GameManager), nameof(GameManager.ResetLists))]
        private static class ResetDict
        {
            internal static void Postfix()
            {
                HoldInteractionTime.data.Clear();
            }
        }

        /*
        [HarmonyPatch(typeof(Panel_Inventory_Examine), nameof(Panel_Inventory_Examine.StartRepair))]
        private static class GlobalSpeed
        {
            internal static void Postfix(Panel_Inventory_Examine __instance)
            {
                if (Settings.options.globalSpeedMult != 1f)
                {
                    __instance.m_ProgressBarTimeSeconds = __instance.m_RepairTimeSeconds / Settings.options.globalSpeedMult;

                }
            }
        }

        [HarmonyPatch(typeof(Panel_Inventory_Examine), nameof(Panel_Inventory_Examine.StartSharpen))]
        private static class GlobalSpeed2
        {
            internal static void Prefix(Panel_Inventory_Examine __instance)
            {
                if (Settings.options.globalSpeedMult != 1f)
                {
                    __instance.m_ProgressBarTimeSeconds = __instance.m_SharpenTimeSeconds / Settings.options.globalSpeedMult;

                }
            }
        }

        [HarmonyPatch(typeof(Panel_Inventory_Examine), nameof(Panel_Inventory_Examine.StartClean))]
        private static class GlobalSpeed3
        {
            internal static void Prefix(Panel_Inventory_Examine __instance)
            {
                if (Settings.options.globalSpeedMult != 1f)
                {
                    __instance.m_ProgressBarTimeSeconds = __instance.m_CleanTimeSeconds / Settings.options.globalSpeedMult;

                }
            }
        }

        [HarmonyPatch(typeof(Panel_Inventory_Examine), nameof(Panel_Inventory_Examine.StartHarvest))]
        private static class GlobalSpeed4
        {
            internal static void Prefix(Panel_Inventory_Examine __instance)
            {
                if (Settings.options.globalSpeedMult != 1f)
                {
                    __instance.m_ProgressBarTimeSeconds = __instance.m_HarvestTimeSeconds / Settings.options.globalSpeedMult;

                }
            }
        }

        [HarmonyPatch(typeof(Panel_Inventory_Examine), nameof(Panel_Inventory_Examine.StartRead))]
        private static class GlobalSpeed5
        {
            internal static void Prefix(Panel_Inventory_Examine __instance)
            {
                if (Settings.options.globalSpeedMult != 1f)
                {
                    __instance.m_ProgressBarTimeSeconds = __instance.m_ReadTimeSeconds / Settings.options.globalSpeedMult;

                }
            }
        }
        */
    }
}
