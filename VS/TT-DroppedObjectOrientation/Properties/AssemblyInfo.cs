using MelonLoader;
using System.Reflection;
using System.Runtime.InteropServices;
using TinyTweaks;

//This is a C# comment. Comments have no impact on compilation.

//ModName, ModVersion, ModAuthor, and ModNamespace.ModClassInheritingFromMelonMod all need changed.

[assembly: AssemblyTitle("TinyTweaks-FixDroppedOrientation")]
[assembly: AssemblyCopyright("Created by Waltz")]

//Version numbers in C# are a set of 1 to 4 positive integers separated by periods.
//Mods typically use 3 numbers. For example: 1.2.1
//The mod version need specified in three places.
[assembly: AssemblyVersion(Utility.globalModVersion)]
[assembly: AssemblyFileVersion(Utility.globalModVersion)]
[assembly: MelonInfo(typeof(RotateStuff), "TinyTweaks-FixDroppedOrientation", Utility.globalModVersion, "Waltz")]

//This tells MelonLoader that the mod is only for The Long Dark.
[assembly: MelonGame("Hinterland", "TheLongDark")]