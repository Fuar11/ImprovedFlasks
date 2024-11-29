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

            static Vector3 defaultPosition1 = new Vector3(0.48f, -0.5925f, 0f);
            static Vector3 defaultPosition2 = new Vector3(0.8393f, -0.5925f, 0f);
            static bool buttonSwapped = false;
            public static void Postfix(ItemDescriptionPage __instance, ref GearItem gi)
            {

                if (gi.m_InsulatedFlask)
                {
                    __instance.m_MouseButtonEquip.gameObject.GetComponent<UIButtonOffset>().enabled = false;

                    if (!buttonSwapped)
                    {
                        Transform ButtonPosition1 = __instance.m_MouseButtonEquip.gameObject.transform;
                        Transform ButtonPosition2 = __instance.m_MouseButtonExamine.gameObject.transform;

                        Vector3 tempPosition = ButtonPosition1.position;

                        ButtonPosition1.position = ButtonPosition2.position;
                        ButtonPosition2.position = tempPosition;
                        buttonSwapped = true;
                    }

                    //drink button is enabled and moves to the first position
                    __instance.m_MouseButtonExamine.SetActive(true);
                    __instance.m_Label_MouseButtonExamine.text = "Drink";

                    UIButton[] components = __instance.m_MouseButtonExamine.GetComponents<UIButton>();
                    for (int i = 0; i < components.Length; i++)
                    {
                        components[i].isEnabled = true;
                    }
                }
                else
                {
                    
                    Transform ButtonPosition1 = __instance.m_MouseButtonEquip.gameObject.transform;
                    Transform ButtonPosition2 = __instance.m_MouseButtonExamine.gameObject.transform;

                    ButtonPosition1.position = defaultPosition1;
                    ButtonPosition2.position = defaultPosition2;
                    buttonSwapped = false;

                    __instance.m_MouseButtonExamine.gameObject.GetComponent<UIButtonOffset>().enabled = false;
                }
            }
        }

        [HarmonyPatch(typeof(ItemDescriptionPage), nameof(ItemDescriptionPage.OnActions))]

        public class OverrideButtonOnClickForFlask
        {

            public static bool Prefix(ItemDescriptionPage __instance)
            {

                //did i go the long way around?
                if (InterfaceManager.GetPanel<Panel_Inventory>().GetCurrentlySelectedGearItem().m_InsulatedFlask)
                {
                    FlaskUtils.ConsumeFromFlask(InterfaceManager.GetPanel<Panel_Inventory>().GetCurrentlySelectedGearItem().m_InsulatedFlask);
                    return false;
                }

                return true;
            }

        }

    }
}
