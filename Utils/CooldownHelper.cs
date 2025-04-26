using Steamworks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InvQoL.Utils
{
    public class CooldownHelper
    {

        private static readonly Dictionary<CSteamID, DateTime> cooldowns = new Dictionary<CSteamID, DateTime>();

        public static bool IsCooldownReady(CSteamID cSteamID, out double secondsLeft)
        {
            if(cooldowns.TryGetValue(cSteamID, out DateTime lastUse))
            {
                double elapsed = (DateTime.Now - lastUse).TotalSeconds;
                if(elapsed < Core.Instance.Configuration.Instance.ButtonCooldown)
                {
                    secondsLeft = Core.Instance.Configuration.Instance.ButtonCooldown - elapsed;
                    return false;
                }
            }
            cooldowns[cSteamID] = DateTime.Now;
            secondsLeft = 0;
            return true;
        }

    }
}
