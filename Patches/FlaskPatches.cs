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
    internal class FlaskPatches
    {

        [HarmonyPatch(typeof(GearItem), nameof(GearItem.Awake))]

        public class AddFlaskToFoodFilter
        {

            public static void Postfix(GearItem __instance)
            {

                if (__instance.name.ToLowerInvariant().Contains("flask"))
                {
                    __instance.GearItemData.m_Type = GearType.Food | GearType.Tool;
                }
            }

        }

    }
}
