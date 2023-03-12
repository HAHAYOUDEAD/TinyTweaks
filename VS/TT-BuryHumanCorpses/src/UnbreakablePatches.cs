using ModData;
using MelonLoader.TinyJSON;
using System.Reflection.Metadata;


namespace TinyTweaks
{ 
    class BuryHumanCorpses : MelonMod
    {
        public static readonly int hoursToBury = 1;
        private static readonly float secondsToInteract = 3f;

        private static bool interrupted;
        public static bool inProgress;

        public static IEnumerator BuryCorpse(GameObject corpse)
        {
            GameManager.GetPlayerVoiceComponent().BlockNonCriticalVoiceForDuration(10f);

            interrupted = false;
            inProgress = true;
            InterfaceManager.GetPanel<Panel_GenericProgressBar>().Launch("Bury a friend", secondsToInteract, hoursToBury * 60f, 0, true, null);
            while (inProgress) yield return new WaitForEndOfFrame();
            if (!interrupted) corpse.active = false;

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
        
        [HarmonyPatch(typeof(PlayerManager), nameof(PlayerManager.InteractiveObjectsProcessAltFire))] 
        public class CatchAltInteractionWithCorpse
        {
            private static GameObject corpse;

            public static void Prefix()
            {
                corpse = Utility.GetGameObjectUnderCrosshair();

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



        /*
        [HarmonyPatch(typeof(TimeOfDay), nameof(TimeOfDay.Accelerate))]
        public class CatchAltInteractionWithCorpse1
        {

            public static void Prefix(ref float realTimeSeconds, ref float gameTimeHours, ref bool doFadeToBlack)
            {

                if (inProgress)
                {
                    InterfaceManager.GetPanel<Panel_HUD>().m_AccelTimePopup.m_NonFullFadeValue = desiredAlpha;
                    doFadeToBlack = true;
                }

            }

            public static void Postfix()
            {
                if (inProgress)
                {
                    InterfaceManager.GetPanel<Panel_HUD>().m_AccelTimePopup.m_NonFullFadeValue = defaultAlpha;
                    inProgress = false;
                }
                
            }
        }
        */




    }

}
