using ModData;
using MelonLoader.TinyJSON;
using TinyTweaks;

namespace TinyTweaks
{
    class RegrowSaveData
    {
        public Dictionary<string, Dictionary<string, float>> dictionarySaveProxy;
    }

    public class RespawnablePlants : MelonMod
    {
        public static readonly string saveDataTag = "regrowPlants";
        public static Dictionary<string, Dictionary<string, float>> harvestedPlants = new Dictionary<string, Dictionary<string, float>>(); // scene, dict<guid, time of harvest> 
        private static float coroutineUpdateInterval = 30f;// in seconds;
        public static object routine;

        public static ModDataManager dataManager = new ModDataManager("TinyTweaks");

        public override void OnInitializeMelon()
        {
            Settings.OnLoad();
        }

        public static IEnumerator CheckHarvestablesForRespawn()
        {
            while (Utility.IsScenePlayable())
            {
                for (float t = 0f; t < coroutineUpdateInterval; t += Time.deltaTime)
                {
                    if (!Utility.IsScenePlayable()) yield break;
                    yield return new WaitForEndOfFrame();
                }

                if (harvestedPlants == null || harvestedPlants.Count < 1) continue;

                string scene = GameManager.m_ActiveScene;

                if (harvestedPlants.ContainsKey(scene))
                {
                    foreach (KeyValuePair<string, float> entry in harvestedPlants[scene])
                    {
                        float randomized = Settings.options.respawnTime * 24f;
                        if (Settings.options.randomizeRespawnTime != 0)
                        {
                            if (Settings.options.randomizeRespawnTime == 1) // controlled random
                            {
                                int range = Mathf.CeilToInt(randomized * 0.2f);
                                randomized = Mathf.Clamp(randomized + new System.Random(entry.Key.GetHashCode()).Next(-range, range), 1f, 365f * 24f);
                            }
                            if (Settings.options.randomizeRespawnTime == 2) // wild random
                            {
                                randomized = new System.Random(entry.Key.GetHashCode()).Next(0, 365 * 24);
                            }

                        }
                        float hoursToRespawn = randomized;

                        if (entry.Value + hoursToRespawn < GameManager.GetTimeOfDayComponent().GetHoursPlayedNotPaused())
                        {
                            HarvestableManager.FindHarvestableByGuid(entry.Key).Respawn();
                            harvestedPlants[scene].Remove(entry.Key);
                        }
                    }
                }
            }

            yield break;
        }



        [HarmonyPatch(typeof(Harvestable), nameof(Harvestable.Awake))]
        private static class HarvestableAwake
        {
            internal static void Prefix(ref Harvestable __instance)
            {
                __instance.m_DestroyObjectOnHarvest = false;
            }
        }

        [HarmonyPatch(typeof(Harvestable), nameof(Harvestable.Harvest))]
        private static class SaveTimeOfHarvest
        {
            internal static void Postfix(ref Harvestable __instance)
            {
              bool respawnAllowed = false;
                if (__instance.m_Harvested && __instance.RegisterAsPlantsHaversted)
                {
                    string scene = GameManager.m_ActiveScene;
                    if (Settings.options.limitRegions == 0 )
                    {
                      respawnAllowed = true;
                    }
                    else if (Settings.options.limitRegions == 1 && (scene == "AshCanyonRegion" || scene == "RiverValleyRegion" || scene == "MiningRegion"))
                    {
                      respawnAllowed = true;
                    }
                    if (respawnAllowed) {
                      if (!harvestedPlants.ContainsKey(scene))
                      {
                          harvestedPlants.Add(scene, new Dictionary<string, float>());
                      }

                      string guid = ObjectGuid.GetGuidFromGameObject(__instance.gameObject);
                      harvestedPlants[scene][guid] = GameManager.GetTimeOfDayComponent().GetHoursPlayedNotPaused();
                    }
                }
            }
        }


        [HarmonyPatch(typeof(SaveGameSystem), nameof(SaveGameSystem.SaveSceneData))]
        private static class SaveHarvestTimes
        {
            internal static void Postfix(ref SlotData slot)
            {
                RegrowSaveData data = new RegrowSaveData() { dictionarySaveProxy = harvestedPlants };
                string serializedSaveData = JSON.Dump(data);

                dataManager.Save(serializedSaveData, saveDataTag);
            }
        }


        [HarmonyPatch(typeof(SaveGameSystem), nameof(SaveGameSystem.LoadSceneData))]
        private static class LoadHarvestTimes
        {
            internal static void Postfix(ref string name)
            {
                string? serializedSaveData = dataManager.Load(saveDataTag);
                RegrowSaveData? data = null;

                if (!string.IsNullOrEmpty(serializedSaveData)) JSON.MakeInto(JSON.Load(serializedSaveData), out data);

                if (data != null && data.dictionarySaveProxy != null)
                {
                    harvestedPlants = data.dictionarySaveProxy;
                }

                if (routine != null) MelonCoroutines.Stop(routine);
                routine = MelonCoroutines.Start(CheckHarvestablesForRespawn());

            }
        }


    }

    public static class Extensions
    {
        public static void Respawn(this Harvestable h)
        {
            h.gameObject.SetActive(true);
            h.m_Harvested = false;
        }
    }
}
