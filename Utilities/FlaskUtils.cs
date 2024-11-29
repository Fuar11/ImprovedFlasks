using Il2Cpp;
using Il2CppTLD.Gear;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Il2CppAK.STATES;

namespace ImprovedFlasks.Utilities
{
    internal class FlaskUtils 
    {
        public static void ConsumeFromFlask(InsulatedFlask flaskItem)
        {

            Il2CppSystem.Collections.Generic.List<GearItem> items = new Il2CppSystem.Collections.Generic.List<GearItem>();

            flaskItem.GetAllItems(items);

            //no items in flask
            if (items.Count == 0)
            {
                GameAudioManager.PlayGUIError();
                HUDMessage.AddMessage("Flask is empty.");
                return;
            }

            FoodItem consumable = items[0].m_FoodItem;

            if (consumable != null) 
            {
                if (consumable.m_IsDrink && GameManager.GetThirstComponent().m_CurrentThirst / GameManager.GetThirstComponent().m_MaxThirst < 0.01f)
                {
                    HUDMessage.AddMessage(Localization.Get("GAMEPLAY_Youarenotthirsty"), false, true);
                    GameAudioManager.PlayGUIError();
                    return;
                }
                else if (!consumable.m_IsDrink && GameManager.GetHungerComponent().m_MaxReserveCalories - GameManager.GetHungerComponent().GetCalorieReserves() < 10f)
                {
                    GameAudioManager.PlayGUIError();
                    HUDMessage.AddMessage(Localization.Get("GAMEPLAY_Youaretoofulltoeat"), false, true);
                    return;
                }

                if (flaskItem.TryRemoveItem(consumable.m_GearItem))
                {
                    PlayerManager pm = GameManager.GetPlayerManagerComponent();

                    pm.UseFoodInventoryItem(consumable.m_GearItem);
                    Main.Logger.Log($"{consumable.name} consumed.", ComplexLogger.FlaggedLoggingLevel.Debug);

                    if (!pm.ShouldDestroyFoodAfterEating(pm.m_FoodItemEaten))
                    {
                        if (flaskItem.TryAddItem(pm.m_FoodItemEaten))
                        {
                            Main.Logger.Log($"{consumable.name} not fully consumed, added back into flask.", ComplexLogger.FlaggedLoggingLevel.Debug);
                        }
                    }
                }
            }
            else
            {
                Main.Logger.Log("Consumable is null!", ComplexLogger.FlaggedLoggingLevel.Error);
            }
        }
    }
}
