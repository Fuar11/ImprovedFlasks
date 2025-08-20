using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Il2Cpp;
using HarmonyLib;
using Il2CppTLD.IntBackedUnit;
using ImprovedFlasks.Utilities;

namespace ImprovedFlasks.Patches
{
    internal class RadialPatches
    {

        [HarmonyPatch(typeof(Panel_ActionsRadial), nameof(Panel_ActionsRadial.GetDrinkItemsInInventory))]

        public class AddFlaskToRadial
        {

            public static bool Prefix => false;

            public static void Postfix(Panel_ActionsRadial __instance, ref Il2CppSystem.Collections.Generic.List<GearItem> __result)
            {

                __instance.m_TempGearItemListNormal.Clear();
                __instance.m_TempGearItemListFavorites.Clear();
                GearItem potableWaterSupply = GameManager.GetInventoryComponent().GetPotableWaterSupply();
                if (potableWaterSupply != null && potableWaterSupply.m_WaterSupply.m_VolumeInLiters > ItemLiquidVolume.Zero)
                {
                    __instance.m_TempGearItemListFavorites.Add(potableWaterSupply);
                }
                for (int i = 0; i < GameManager.GetInventoryComponent().m_Items.Count; i++)
                {
                    GearItem gearItem = GameManager.GetInventoryComponent().m_Items[i];
                    if (gearItem && !gearItem.m_EnergyBoost && gearItem.m_FoodItem && gearItem.m_FoodItem.m_IsDrink && (!gearItem.IsWornOut() || gearItem.m_IsInSatchel))
                    {
                        if (gearItem.m_IsInSatchel)
                        {
                            __instance.m_TempGearItemListFavorites.Add(gearItem);
                        }
                        else
                        {
                            __instance.m_TempGearItemListNormal.Add(gearItem);
                        }
                    }
                    else if(gearItem && gearItem.m_InsulatedFlask && gearItem.m_InsulatedFlask.m_VolumeLitres > ItemLiquidVolume.Zero)
                    {
                        __instance.m_TempGearItemListFavorites.Add(gearItem);
                    }
                }

                foreach (GearItem gearItem in __instance.m_TempGearItemListNormal)
                {
                    __instance.m_TempGearItemListFavorites.Add(gearItem);
                }

                __result = __instance.m_TempGearItemListFavorites;
            }

        }

        [HarmonyPatch(typeof(Panel_ActionsRadial), nameof(Panel_ActionsRadial.UseItem))]

        public class OverrideDefaultAction
        {

            public static bool Prefix(ref GearItem gi)
            {
                return gi.m_InsulatedFlask ? false : true;
            }

            public static void Postfix(ref GearItem gi)
            {
                if (gi.m_InsulatedFlask)
                {
                    InterfaceManager.GetPanel<Panel_Inventory>().UpdateFilteredInventoryList();
                    FlaskUtils.ConsumeFromFlask(gi.m_InsulatedFlask);
                }
            }
        }

        [HarmonyPatch(typeof(RadialMenuArm), nameof(RadialMenuArm.SetRadialInfoGear))]

        public class AddDrinkNameToRadialItem
        {

            public static void Postfix(RadialMenuArm __instance)
            {
                if (__instance.m_GearItem.m_InsulatedFlask)
                {
                    __instance.m_NameWhenHoveredOver = __instance.m_GearItem.m_InsulatedFlask.m_Items[0].m_GearItem.DisplayName.Replace("cup of", "");

                    if (__instance.m_GearItem.m_InsulatedFlask.m_HeatPercent > 0) { __instance.m_NameWhenHoveredOver = "Hot " + __instance.m_NameWhenHoveredOver; }
                    else { __instance.m_NameWhenHoveredOver = "Cold " + __instance.m_NameWhenHoveredOver; }
                }
            }
        }

    [HarmonyPatch(typeof(Panel_ActionsRadial), nameof(Panel_ActionsRadial.CanPlaceFromRadial), new Type[] { typeof(GearItem) })]
        public class AddPlacementActionToRadial
        {
            public static void Postfix(GearItem gi, ref bool __result)
            {
                if (gi.m_InsulatedFlask)
                {
                    __result = true;
                }
                
            }
        }
    }
}
