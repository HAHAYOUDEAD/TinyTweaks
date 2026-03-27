namespace TinyTweaks
{
    class NoSaveOnSprain : MelonMod
    {
        private static bool fromFall;

        public override void OnInitializeMelon()
        {
            Settings.OnLoad();
        }

        private static void PreventSaveFromAffliction(ref AfflictionOptions options)
        {
            if ((options & AfflictionOptions.DoAutoSave) != 0)
            {
                if ((Settings.options.alsoFromFalls && fromFall) || !fromFall)
                    options &= ~AfflictionOptions.DoAutoSave;
                fromFall = false;
            }
        }

        [HarmonyPatch(typeof(FallDamage), nameof(FallDamage.MaybeSprainAnkle))]
        private static class TrackSprainFall1
        {
            internal static void Postfix(bool __result)
            {
                fromFall = true;
            }
        }
        [HarmonyPatch(typeof(FallDamage), nameof(FallDamage.MaybeSprainWrist))]
        private static class TrackSprainFall2
        {
            internal static void Postfix(bool __result)
            {
                fromFall = true;
            }
        }

        [HarmonyPatch(typeof(SprainedWrist), nameof(SprainedWrist.SprainedWristStart))]
        private static class WristPreventSave
        {
            internal static void Prefix(ref AfflictionOptions options) => PreventSaveFromAffliction(ref options);
        }
                
        [HarmonyPatch(typeof(SprainedAnkle), nameof(SprainedAnkle.SprainedAnkleStart))]
        private static class AnklePreventSave
        {
            internal static void Prefix(ref AfflictionOptions options) => PreventSaveFromAffliction(ref options);
        }
    }
}
