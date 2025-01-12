namespace TinyTweaks
{
    class WakeUpCall : MelonMod
    {

        private static float fadeAlpha = 0.9f;
        private static bool wentSleepDuringAurora;
        public override void OnInitializeMelon()
        {
            Settings.OnLoad();
        }

        public override void OnUpdate()
        {
            if (InputManager.GetPauseMenuTogglePressed(InputManager.m_CurrentContext))
            {
                if (GameManager.GetRestComponent().IsSleeping())
                {
                    WakeUp();
                }
            }
        }

        public static void WakeUp() => GameManager.GetRestComponent().m_InterruptionAfterSecondsSleeping = 1;

        public static void PostWakeUp()
        {
            Panel_HUD ph = InterfaceManager.GetPanel<Panel_HUD>();
            ph.m_Sprite_SystemFadeOverlay.transform.SetParent(ph.transform);
            ph.m_Sprite_SystemFadeOverlay.depth = 50; // not important
            ph.HideHudElements(false);

            if (Settings.options.noPitchBlack) GameManager.GetCameraEffects().DepthOfFieldTurnOff(false);
            CameraFade.Fade(Settings.options.noPitchBlack ? fadeAlpha : 1f, 0f, 0.5f, 0f, null);

            if (Settings.options.showTime) InterfaceManager.GetInstance().SetTimeWidgetActive(false);
        }


        [HarmonyPatch(typeof(Rest), nameof(Rest.BeginSleeping), new Type[] { typeof(Bed), typeof(int), typeof(int), typeof(float), typeof(Rest.PassTimeOptions), typeof(Il2CppSystem.Action) })]
        private static class ManualWakeUp
        {
            internal static void Prefix(ref Rest.PassTimeOptions options, ref Il2CppSystem.Action onSleepEnd)
            {
                wentSleepDuringAurora = GameManager.GetAuroraManager().IsFullyActive();

                options = Rest.PassTimeOptions.DisplayCancelButton;
                Action action = () => WakeUp();
                InterfaceManager.GetPanel<Panel_HUD>().m_AccelTimePopup.m_CancelCallback = action;

                Panel_HUD ph = InterfaceManager.GetPanel<Panel_HUD>();
                ph.m_Sprite_SystemFadeOverlay.transform.SetParent(ph.m_NonEssentialHud.transform);
                ph.m_Sprite_SystemFadeOverlay.depth = 0;
                ph.m_Sprite_SystemFadeOverlay.OnEnable();
                ph.HideHudElements(true);

                Action postAction = () => PostWakeUp();
                onSleepEnd = postAction;

                //if (Settings.options.noPitchBlack) 
                CameraFade.Fade(0f, Settings.options.noPitchBlack ? fadeAlpha : 1f, 0.5f, 0f, null);
            }
        } 

        [HarmonyPatch(typeof(AuroraManager), nameof(AuroraManager.UpdateAuroraValue))]
        private static class AuroraWakeUp
        {
            internal static void Postfix(ref AuroraManager __instance)
            {
                if (Settings.options.auroraSense && GameManager.GetRestComponent().IsSleeping() && __instance.IsFullyActive() && !wentSleepDuringAurora)
                { 
                    WakeUp();
                }
            }
        }
        
        [HarmonyPatch(typeof(InterfaceManager), nameof(InterfaceManager.SetTimeWidgetActive))]
        private static class EnableTimeWidget
        {
            internal static void Prefix(ref bool active)
            {
                if (Settings.options.showTime && GameManager.GetRestComponent().IsSleeping() && !active)// && !wakingUp)
                { 
                    active = true;
                }
            }
        }
        [HarmonyPatch(typeof(Panel_Rest), nameof(Panel_Rest.Enable), new Type[] { typeof(bool), typeof(bool) })]
        private static class EnableDOF
        {
            internal static void Postfix(ref bool enable)
            {
                if (Settings.options.noPitchBlack && GameManager.GetRestComponent().IsSleeping() && !enable)// && !wakingUp)
                {
                    GameManager.GetCameraEffects().DepthOfFieldTurnOn();
                }
            }
        }
    }
}
