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
        [Section("No Save On Sprain")]

        [Name("Disable on fall")]
        [Description("Disable save on sprains aquired from fall damage, otherwise only affects sprains from slopes\n\n Default: false")]
        public bool alsoFromFalls = false;


        protected override void OnConfirm()
        {
            base.OnConfirm();
        }
    }

}
