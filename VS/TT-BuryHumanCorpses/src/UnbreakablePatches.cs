using ModData;
using MelonLoader.TinyJSON;

namespace TinyTweaks
{ 
    class BuryHumanCorpses : MelonMod
    {
        public static readonly string saveDataTag = "buryCorpses";
        public static ModDataManager dataManager = new ModDataManager("TinyTweaks");

        public static readonly int hoursToBury = 1;
        private static readonly float secondsToInteract = 3f;

        public static Dictionary<string, List<string>> buriedCorpses = new Dictionary<string, List<string>>();

        private static bool interrupted;
        public static bool inProgress;

        public static IEnumerator BuryCorpse(GameObject corpse)
        {
            GameManager.GetPlayerVoiceComponent().BlockNonCriticalVoiceForDuration(10f);

            interrupted = false;
            inProgress = true;
            InterfaceManager.GetPanel<Panel_GenericProgressBar>().Launch("Bury a friend", secondsToInteract, hoursToBury * 60f, 0, true, null);
            while (inProgress) yield return new WaitForEndOfFrame();
            if (!interrupted)
            {
                corpse.active = false;
                Carrion crows = corpse.GetComponent<Carrion>();
                if (crows != null) crows.Destroy();
                string guid = ObjectGuid.GetGuidFromGameObject(corpse.gameObject);
                if (buriedCorpses.ContainsKey(GameManager.m_ActiveScene)) buriedCorpses[GameManager.m_ActiveScene].Add(guid);
                else buriedCorpses[GameManager.m_ActiveScene] = new List<string> { guid };

            }
            yield break;
        }

        public static bool IsCorpse(GameObject corpse)
        {
            if (corpse != null && corpse.GetComponent<Container>() == null)
            {
                corpse = corpse.transform.GetParent()?.gameObject;
            }

            if (!corpse || corpse.GetComponent<Container>() == null || !corpse.GetComponent<Container>().m_IsCorpse)
            {
                return false;
            }
            return true;
        }


        [HarmonyPatch(typeof(InputManager), nameof(InputManager.ExecuteAltFire))] 
        public class CatchAltInteractionWithCorpse
        {
            public static void Prefix()
            {
                if (!GameManager.GetPlayerManagerComponent()) return;

                GameObject corpse = Utility.GetGameObjectUnderCrosshair();
                if (!IsCorpse(corpse)) return;
                MelonCoroutines.Start(BuryCorpse(corpse));
            }
        }

        [HarmonyPatch(typeof(Panel_HUD), nameof(Panel_HUD.SetHoverText))] 
        public class ShowButtonPrompts
        {
            public static void Prefix(ref GameObject itemUnderCrosshairs)
            {
                if (!IsCorpse(itemUnderCrosshairs)) return;

                InterfaceManager.GetPanel<Panel_HUD>().m_EquipItemPopup.enabled = true;
                InterfaceManager.GetPanel<Panel_HUD>().m_EquipItemPopup.ShowGenericPopupWithDefaultActions("Search", "Bury");
            }
        }


        [HarmonyPatch(typeof(Panel_GenericProgressBar), nameof(Panel_GenericProgressBar.ProgressBarEnded))] 
        public class ProgressBarCallback
        {
            public static void Prefix(ref bool success, ref bool playerCancel)
            {
                if (inProgress)
                {
                    if (!success) interrupted = true;
                    inProgress = false;
                }
            }
        }


        [HarmonyPatch(typeof(SaveGameSystem), nameof(SaveGameSystem.SaveSceneData))]
        private static class SaveHarvestTimes
        {
            internal static void Prefix(ref SlotData slot)
            {
                string serializedSaveData = JSON.Dump(buriedCorpses);

                dataManager.Save(serializedSaveData, saveDataTag);
            }
        }


        [HarmonyPatch(typeof(SaveGameSystem), nameof(SaveGameSystem.LoadSceneData))]
        private static class LoadHarvestTimes
        {
            internal static void Postfix(ref string name)
            {
                string? serializedSaveData = dataManager.Load(saveDataTag);

                if (!string.IsNullOrEmpty(serializedSaveData)) JSON.MakeInto(JSON.Load(serializedSaveData), out buriedCorpses);

                if (buriedCorpses.ContainsKey(GameManager.m_ActiveScene))
                {
                    foreach (string s in buriedCorpses[GameManager.m_ActiveScene])
                    {
                        ContainerManager.FindContainerByGuid(s)?.gameObject.SetActive(false);
                    }
                }
            }
        }
    }
}
