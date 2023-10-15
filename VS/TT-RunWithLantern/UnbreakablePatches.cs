namespace TinyTweaks
{

    class RunWithLantern : MelonMod
    {

        [HarmonyPatch(typeof(GameManager), nameof(GameManager.IsMovementLockedBecauseOfLantern))]
        private static class HarvestableAwake
        {
            internal static bool Prefix()
            {
                return false;
                
            }
        }

        [HarmonyPatch(typeof(CameraOverride), nameof(CameraOverride.OnStateUpdate))]
        private static class CameraLockIgnore1
        {
            internal static bool Prefix(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
            {
                if (stateInfo.IsName("Lantern_Prepare") || stateInfo.IsName("Lantern_Idle")) return false;
                else return true;
            }
        }

        [HarmonyPatch(typeof(CameraOverride), nameof(CameraOverride.OnStateEnter))]
        private static class CameraLockIgnore2
        {
            internal static bool Prefix(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
            {
                if (stateInfo.IsName("Lantern_Prepare") || stateInfo.IsName("Lantern_Idle")) return false;
                else return true;
            }
        }
    }

}
