namespace TinyTweaks
{
    public class CapFeelsLike : MelonMod
    {
        public override void OnInitializeMelon()
        {
            Settings.OnLoad();
        }

        private static bool IsNearCampFire()
        {
            return GameManager.GetFireManagerComponent().GetDistanceToClosestFire(GameManager.GetPlayerTransform().position) < GameManager.GetBodyHarvestManagerComponent().m_RadiusToThawFromFire;
        }
        

        [HarmonyPatch(typeof(Freezing), "CalculateBodyTemperature")]
        private class FeelsLikeCap
        {
            
            public static void Postfix(ref float __result)
            {
                if ((GameManager.GetWeatherComponent() && GameManager.GetWeatherComponent().IsIndoorEnvironment()) || IsNearCampFire()) return;
                if (Settings.options.capHigh != 0 && __result > Settings.options.capHigh) __result = Settings.options.capHigh;
                if (Settings.options.capLow != 0 && __result < Settings.options.capLow) __result = Settings.options.capLow;
            }
        }

    }
}
