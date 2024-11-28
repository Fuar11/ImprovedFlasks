using ComplexLogger;
using MelonLoader;

namespace ImprovedFlasks
{
    public class Main : MelonMod
    {
        internal static ComplexLogger<Main> Logger = new();
        public override void OnInitializeMelon()
        {
            Logger.Log("Improved Flasks is online.", FlaggedLoggingLevel.None);   
        }
    }
}
