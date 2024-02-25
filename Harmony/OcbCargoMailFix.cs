using Game.Economy;

using BepInEx;
using HarmonyLib;
using System.Reflection;
using BepInEx.Logging;
using System.Linq;

namespace OcbCargoMailFix
{

    [BepInPlugin("ch.ocbnet.cmf", "OcbCargoMailFix", "0.0.1")]
    public class Plugin : BaseUnityPlugin
    {
        internal static ManualLogSource Log;
        private void Awake()
        {
            Log = base.Logger;
            // Unity.Burst.BurstCompiler.Options.EnableBurstCompilation = false;
            var harmony = Harmony.CreateAndPatchAll(Assembly.GetExecutingAssembly(), "ch.ocbnet.cmf");
            var patchedMethods = harmony.GetPatchedMethods().ToArray();
            log.info("Plugin OcbCargoMailFix is loaded! Patched {0} methods", patchedMethods.Length);
            foreach (var patchedMethod in patchedMethods)
            {
                log.info("Patched method: {0}:{1}", patchedMethod.Module.Name, patchedMethod.Name);
            }
        }
    }

    // Little logging helper
    public static class log
    {
        public static void fatal(string msg) =>
            Plugin.Log.Log(LogLevel.Fatal, msg);
        public static void error(string msg) =>
            Plugin.Log.Log(LogLevel.Error, msg);
        public static void warn(string msg) =>
            Plugin.Log.Log(LogLevel.Warning, msg);
        public static void message(string msg) =>
            Plugin.Log.Log(LogLevel.Message, msg);
        public static void info(string msg) =>
            Plugin.Log.Log(LogLevel.Info, msg);
        public static void debug(string msg) =>
            Plugin.Log.Log(LogLevel.Debug, msg);
        public static void fatal(string fmt, params object[] args) =>
            Plugin.Log.Log(LogLevel.Fatal, string.Format(fmt, args));
        public static void error(string fmt, params object[] args) =>
            Plugin.Log.Log(LogLevel.Error, string.Format(fmt, args));
        public static void warn(string fmt, params object[] args) =>
            Plugin.Log.Log(LogLevel.Warning, string.Format(fmt, args));
        public static void message(string fmt, params object[] args) =>
            Plugin.Log.Log(LogLevel.Message, string.Format(fmt, args));
        public static void info(string fmt, params object[] args) =>
            Plugin.Log.Log(LogLevel.Info, string.Format(fmt, args));
        public static void debug(string fmt, params object[] args) =>
            Plugin.Log.Log(LogLevel.Debug, string.Format(fmt, args));
    }

    // Harmony patch to change post vans to trucks and vice-versa

    [HarmonyPatch(typeof(Game.Prefabs.PostFacility), "Initialize")]
    static class PostFacility_Patch
    {
        static void Prefix(Game.Prefabs.PostFacility __instance)
        {
            // Extend regular post office
            // Convert 10% of vans to cargo trucks
            // To ship unsorted mail independently
            if (__instance.m_PostTruckCapacity == 0)
            {
                int delta = __instance.m_PostVanCapacity / 10;
                __instance.m_PostTruckCapacity += delta;
                __instance.m_PostVanCapacity -= delta;
            }
            // Add a single truck to ship unsorted mail
            else if(__instance.m_PostVanCapacity == 0)
            {
                // Convert 20% of cargo trucks to post vans
                // To deliver sorted (local) mail independently
                int delta = __instance.m_PostTruckCapacity / 5;
                __instance.m_PostTruckCapacity -= delta;
                __instance.m_PostVanCapacity += delta;
                // Alternatively just add vans to sorter
                // Making sorting facility also a post office
                // __instance.m_PostVanCapacity =
                //     __instance.m_PostTruckCapacity;
            }
        }
    }

    // Harmony patch to avoid trading local mail at transport depots
    [HarmonyPatch(typeof(Game.Prefabs.CargoTransportStation), "Initialize")]
    static class CargoTransportStation_Patch
    {
        static bool IsMail(ResourceInEditor resource)
        {
            if (resource == ResourceInEditor.LocalMail) return true;
            // if (resource == ResourceInEditor.OutgoingMail) return true;
            // if (resource == ResourceInEditor.UnsortedMail) return true;
            return false;
        }

        static void Prefix(Game.Prefabs.CargoTransportStation __instance)
        {
            if (__instance.name == "CargoTransportStation")
            {
                // Since changed in prefix, this will be passed on to
                // StorageCompanyData.m_StoredResources in base method
                // We don't trade nor store it, which seems to enable/fix
                // LocalMail to be sent via outside trucks to post offices again
                __instance.m_TradedResources = __instance.m_TradedResources
                    .Where(resource => !IsMail(resource)).ToArray();
            }
        }
    }

}
