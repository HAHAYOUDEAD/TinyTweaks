using src;

namespace TinyTweaks
{
    class FallDeathGoat : MelonMod
    {
        public override void OnInitializeMelon()
        {
            Settings.OnLoad();
        }

        [HarmonyPatch(typeof(FallDeathTrigger), nameof(FallDeathTrigger.OnTriggerEnter))]
        private static class FallTriggerDisable
        {
            internal static void Postfix(ref Collider c)
            {
                if (c.gameObject.CompareTag("Player"))
                {
                    if (Settings.options.notify) HUDMessage.AddMessage("Fall death trigger passed");
                    GameManager.GetFallDamageComponent().m_DieOnNextFall = false;
                }
            }
        }
        [HarmonyPatch(typeof(GameManager), nameof(GameManager.Awake))]
        private static class ApplyFallDamageMultipler
        {
            internal static void Postfix()
            {
                GameManager.GetFallDamageComponent().m_DamagePerMeter = Settings.options.fallDamageMult;
            }
        }
    }
}
