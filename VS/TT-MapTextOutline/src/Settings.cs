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
        [Section("Map Text Outline")]


        [Name("Outline style")]
        [Description("Default: Dark text w/ white outline")]
        [Choice(new string[]
        {
            "Dark text w/ beige outline",
            "Dark text w/ white outline",
            "Bright text w/ dark outline",
            "White text w/ dark outline"
        })]
        public int outlineColor = 1;

        /*
        [Name("Outline opacity")]
        [Description("Default = 70")]
        [Slider(0, 100)]
        public int outlineOpacity = 70;
        */

        protected override void OnConfirm()
        {
            InterfaceManager.GetPanel<Panel_Map>().CreateObjectPools();

            base.OnConfirm();
        }
    }

}
