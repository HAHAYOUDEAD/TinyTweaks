using ModSettings;

namespace TinyTweaks
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
        [Section("Respawnable Plants")]

        [Name("Respawn time")]
        [Description("In days.")]
        [Slider(1, 365)]
        public int respawnTime = 45;


        protected override void OnConfirm()
        {

            base.OnConfirm();
        }
    }

}
