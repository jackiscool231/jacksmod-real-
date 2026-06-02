using MegaCrit.Sts2.Core.Logging;
using MegaCrit.Sts2.Core.Modding;
using HarmonyLib;
using ModSmith.Util;
using ModSmith.Registry;

namespace JacksMod;

[ModInitializer(nameof(Initialize))]
public static class JacksModMain
{
    public const string ModId = "JacksMod"; // Must match the id in `JacksMod.json`

    public static Logger Logger { get; } = new(ModId, LogType.Generic);

    public static ResourcePaths Res { get; } = new(ModId);

    public static void Initialize()
    {
        Logger.Info("Initializing... 108" );
        Harmony harmony = new(ModId);
        harmony.PatchAll();

        // Register your content...
        StarterContent.RegisterStarterContent();
        // Registry.RegisterStarterContent<GoldArmor>();
        Registry.RegisterRelic<GoldArmor>();


        Logger.Info("Initialized.");
    }
}
