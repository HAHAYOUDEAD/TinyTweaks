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
        [Section("Cap Feels Like Temperature")]

        [Name("Cap high")]
        [Description("Does what it says (in Celsius). \n\n0 = no cap")]
        [Slider(-10, 50)]
        public int capHigh = 0;

        [Name("Cap low")]
        [Description("Does what it says (in Celsius). \n\n0 = no cap")]
        [Slider(-50, 10)] 
        public int capLow = 0;

        protected override void OnConfirm()
        {
            base.OnConfirm();
        }
    }

}
