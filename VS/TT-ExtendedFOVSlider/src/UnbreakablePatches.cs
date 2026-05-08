namespace TinyTweaks
{
    public class ExtendedFOV : MelonMod
    {
        public static readonly float maxFOV = 150f;
        public static readonly float minFOV = 30f;

        [HarmonyPatch(typeof(Panel_OptionsMenu), "ApplyGraphicsModeAndResolution")]
        private static class ChangeMaxFOV
        {
            internal static void Postfix(ref Panel_OptionsMenu __instance)
            {
                __instance.m_FieldOfViewMax = maxFOV;
                __instance.m_FieldOfViewMin = minFOV;
                __instance.m_FieldOfViewSlider.m_Slider.numberOfSteps = (int)(maxFOV - minFOV) + 1;

            }
        }
        
        [HarmonyPatch(typeof(Panel_OptionsMenu), "OnDisplayTab")]
        private static class ChangeMaxFOV2
        {
            internal static void Postfix(ref Panel_OptionsMenu __instance)
            {
                __instance.m_FieldOfViewMax = maxFOV;
                __instance.m_FieldOfViewMin = minFOV;
                __instance.m_FieldOfViewSlider.m_Slider.numberOfSteps = (int)(maxFOV - minFOV) + 1;
            }
        }
        
        
    }
}
