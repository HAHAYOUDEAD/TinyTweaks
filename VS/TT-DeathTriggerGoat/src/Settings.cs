using ModSettings;

namespace src
{
    internal static class Settings
    {
        public static TTSettings options;

        public static void OnLoad()
        {
            options = new TTSettings();
            options.AddToModSettings("[Tiny Tweaks]");
        }
    }

    internal class TTSettings : JsonModSettings
    {
        [Section("Fall Death Goat")]

        [Name("Fall Damage")]
        [Description("Fall damage multiplier, applied per meter of free fall.\n\n Vanilla: 3")]
        [Slider(1, 12)]
        public int fallDamageMult = 6;

        [Name("Notify")]
        [Description("Show notification when death wall is entered")]
        public bool notify = false;

        protected override void OnConfirm()
        {
            GameManager.GetFallDamageComponent().m_DamagePerMeter = fallDamageMult;
            base.OnConfirm();
        }
    }

}
