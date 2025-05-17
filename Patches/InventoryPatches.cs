using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;
using Il2Cpp;
using HarmonyLib;
using ImprovedFlasks.Utilities;
using Il2CppTLD.Gear;

namespace ImprovedFlasks.Patches
{
    internal class InventoryPatches
    {

        [HarmonyPatch(typeof(ItemDescriptionPage), nameof(ItemDescriptionPage.UpdateButtons))]

        public class FlaskButtonsChange
        {
            public static void Postfix(ItemDescriptionPage __instance, ref GearItem gi)
            {
                if (gi == null) return;

                if (gi.m_InsulatedFlask)
                {
                    //drink button is enabled
                    __instance.m_Label_MouseButtonEquip.text = "饮用";

                    //transfer button is enabled
                    __instance.m_MouseButtonExamine.SetActive(true);
                    __instance.m_Label_MouseButtonExamine.text = "转移";

                    //swap button functionality
                    __instance.m_OnActionsDelegate = __instance.m_OnEquipDelegate;

                    UIButton[] components = __instance.m_MouseButtonExamine.GetComponents<UIButton>();
                    for (int i = 0; i < components.Length; i++)
                    {
                        components[i].isEnabled = true;
                    }
                }
            }
        }

        [HarmonyPatch(typeof(ItemDescriptionPage), nameof(ItemDescriptionPage.OnEquip))]

        public class OverrideDrinkButtonOnClickForFlask
        {

            public static bool Prefix(ItemDescriptionPage __instance)
            {

                if (InterfaceManager.GetPanel<Panel_Inventory>().isActiveAndEnabled)
                {
                    if (InterfaceManager.GetPanel<Panel_Inventory>().GetCurrentlySelectedItem().m_GearItem.m_InsulatedFlask)
                    {
                        FlaskUtils.ConsumeFromFlask(InterfaceManager.GetPanel<Panel_Inventory>().GetCurrentlySelectedItem().m_GearItem.m_InsulatedFlask);
                        return false;
                    }
                    return true;
                }

                return true;
            }

        }
    }
}
