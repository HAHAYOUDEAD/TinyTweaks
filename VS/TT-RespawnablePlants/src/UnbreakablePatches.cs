using ModData;
using MelonLoader.TinyJSON;
using TinyTweaks;

namespace TinyTweaks
{
    class RegrowSaveData
    {
        public Dictionary<string, Dictionary<string, float>> dictionarySaveProxy;
    }

    class RespawnablePlants : MelonMod
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
                        float hoursToRespawn = Settings.options.respawnTime * 24f;

                        if (entry.Value + hoursToRespawn < GameManager.GetTimeOfDayComponent().GetHoursPlayedNotPaused())
                        {
                            /*
                            Harvestable h = HarvestableManager.FindHarvestableByGuid(entry.Key);
                            if (h.gameObject.GetComponent<Renderer>().isVisible)
                            {
                                MelonLogger.Msg("respawn time for " + entry.Key + ", but player can see it");
                                continue;
                            }
                            */
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
                if (__instance.m_Harvested)
                {
                    string scene = GameManager.m_ActiveScene;
                    if (!harvestedPlants.ContainsKey(scene))
                    {
                        harvestedPlants.Add(scene, new Dictionary<string, float>());
                    }

                    string guid = ObjectGuid.GetGuidFromGameObject(__instance.gameObject);
                    harvestedPlants[scene][guid] = GameManager.GetTimeOfDayComponent().GetHoursPlayedNotPaused();
                }
            }
        }


        [HarmonyPatch(typeof(SaveGameSystem), nameof(SaveGameSystem.SaveSceneData))]
        private static class SaveHarvestTimes
        {
            internal static void Prefix(ref SlotData slot)
            {
                RegrowSaveData data = new RegrowSaveData() { dictionarySaveProxy = harvestedPlants };
                string serializedSaveData = JSON.Dump(data);

                dataManager.Save(serializedSaveData, saveDataTag);


                //Utility.SaveData(slot, saveDataTag, data);


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

                //Utility.TryLoadData(name, saveDataTag, out RegrowSaveData data);

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
