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
        [Section("Speedy Interactions")]

        [Name("Actions")]
        [Description("Multiplier. Higher = faster" +
            "\n\nIngame time passed is unaffected" +
            "\n\nAffects: " +
            "\n  - repairing, " +
            "\n  - harvesting gear, " +
            "\n  - sharpening, " +
            "\n  - cleaning, " +
            "\n  - refueling, " +
            "\n  - applying medicine, " +
            "\n  - crafting/milling," +
            "\n  - cooking," +
            "\n  - taking water," +
            "\n  - making ice hole," +
            "\n  - snow shelter/rock cache actions")]
        [Slider(0.2f, 6f, 30, NumberFormat = "x{0:0.#}")]
        public float globalSpeedMult = 1f;

        [Name("Object interactions")]
        [Description("Multiplier. Higher = faster"+
            "\n\nCompatible with other mods that change interaction time, the final time will be multiplied" +
            "\n\nAffects: " +
            "\n  - opening containers, " +
            "\n  - harvesting plants, " +
            "\n  - entering vehicles, " +
            "\n  - opening doors ")]
        [Slider(0.2f, 6f, 30, NumberFormat = "x{0:0.#}")]
        public float interactionSpeedMult = 1f;

        [Name("Consumption")]
        [Description("Multiplier. Higher = faster")]
        [Slider(0.2f, 6f, 30, NumberFormat = "x{0:0.#}")]
        public float eatingSpeedMult = 1f;

        [Name("Breaking down")]
        [Description("Multiplier. Higher = faster")]
        [Slider(0.2f, 6f, 30, NumberFormat = "x{0:0.#}")]
        public float breakdownSpeedMult = 1f;
        
        [Name("Reading")]
        [Description("Multiplier. Higher = faster")]
        [Slider(0.2f, 6f, 30, NumberFormat = "x{0:0.#}")]
        public float readingSpeedMult = 1f;



        protected override void OnConfirm()
        {
            base.OnConfirm();
            SpeedyInteractions.ApplySpeedsToLoadedPanels();
        }
    }

}
