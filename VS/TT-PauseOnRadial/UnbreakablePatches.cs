namespace TinyTweaks
{

    class PauseOnRadial : MelonMod
    {
        public static bool isTimeSlowedDown = false;

        public static object timescaleCoroutine;

        public static IEnumerator LerpTimescaleToValue(float value, float speed)
        {
            float currentTimeScale = Time.timeScale;
            float lerp = 0f;

            while (!Mathf.Approximately(value, Time.timeScale))
            {
                Time.timeScale = GameManager.m_GlobalTimeScale = Mathf.Lerp(currentTimeScale, value, lerp += Time.unscaledDeltaTime * speed);
                yield return new WaitForEndOfFrame();
            }
            
            yield break;
        }

        [HarmonyPatch(typeof(Panel_ActionsRadial), nameof(Panel_ActionsRadial.Enable), new Type[] { typeof(bool), typeof(bool) })]
        private static class SlowDownOnOpen
        {
            internal static void Prefix(bool enable)
            {
                if (enable && !isTimeSlowedDown)
                {
                    if (timescaleCoroutine != null) MelonCoroutines.Stop(timescaleCoroutine);
                    timescaleCoroutine = MelonCoroutines.Start(LerpTimescaleToValue(0.1f, 1f));
                    isTimeSlowedDown = true;
                }
                if (!enable && isTimeSlowedDown)
                {
                    if (timescaleCoroutine != null) MelonCoroutines.Stop(timescaleCoroutine);
                    timescaleCoroutine = MelonCoroutines.Start(LerpTimescaleToValue(1f, 3f));
                    isTimeSlowedDown = false;
                }
            }
        }

        [HarmonyPatch(typeof(GameManager), nameof(GameManager.Update))]
        private static class FoolProof
        {
            internal static void Postfix()
            {
                if (!InterfaceManager.GetPanel<Panel_ActionsRadial>().IsEnabled() && isTimeSlowedDown)
                {
                    MelonCoroutines.Start(LerpTimescaleToValue(1f, 3f));
                    isTimeSlowedDown = false;
                }
            }
        }
    }
}
