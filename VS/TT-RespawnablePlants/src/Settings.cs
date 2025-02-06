﻿using ModSettings;

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

        [Name("Respawn regions")]
        [Description("Limit respawn to certian regions.\n Enabling regions mid-game will not respawn already harvested plants.")]
        [Choice(new string[] {
          "All",
          "Limited"
        })]
        public int limitRegions;

        protected override void OnConfirm()
        {

            base.OnConfirm();
        }
    }

}
