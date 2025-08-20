using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Il2Cpp;
using HarmonyLib;
using MelonLoader;
using Il2CppTLD.Gear;

namespace ImprovedFlasks.Patches
{
    [HarmonyPatch(typeof(InsulatedFlask), nameof(InsulatedFlask.IsNearFire))] // calls about 10 times a sec
    public static class HeatPatches
    {
        static void Postfix(InsulatedFlask __instance, ref bool __result)
        {
            if (__result) // flask is near fire
            {
                __instance.m_HeatPercent = 100;

                MelonLogger.Msg($"[HeatPatches] Heated flask {__instance.name} to {__instance.m_HeatPercent} near fire");
            }
        }
    }
}
