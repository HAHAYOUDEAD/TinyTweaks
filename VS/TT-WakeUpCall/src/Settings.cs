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
        [Section("Wake Up Call")]

        [Name("Show time widget")]
        [Description("Sense time while you sleep\n\n Default: false")]
        public bool showTime = false;

        [Name("Wake up from aurora")]
        [Description("Wake up from aurora sounds and lights like a normal person would\n\n Default: true")]
        public bool auroraSense = true;
        
        [Name("No pitch black")]
        [Description("Let some light through your eyelids\n\n Default: false")]
        public bool noPitchBlack = false;


        protected override void OnConfirm()
        {
            base.OnConfirm();
        }
    }

}
