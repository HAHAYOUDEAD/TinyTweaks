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
        [Description("In days, per individual plant")]
        [Slider(1, 365)]
        public int respawnTime = 45;

        [Name("Randomize respawn time")]
        [Description("Controlled random considers chosen respawn time")]
        [Choice(new string[]
        {
            "No random",
            "Controlled random",
            "Wild random"
        })]
        public int randomizeRespawnTime;

        [Name("Respawn all plants")]
        [Description("Respawn every possible plant spawn, even the ones that were rolled harvested at the start of the run\n\nUseful when adding mod mid-save since it will respawn everything that player harvested as well")]
        public bool respawnAll = false;

        protected override void OnConfirm()
        {

            base.OnConfirm();
        }
    }

}
