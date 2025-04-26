using Rocket.API;
using Rocket.Core.Logging;
using Rocket.Unturned.Chat;
using Rocket.Unturned.Player;
using SDG.Unturned;
using System.Collections.Generic;
using System.Linq;

namespace InvQoL.Utils
{
    public class PermissionsUtils
    {

        public static bool PlayerHavePermission(UnturnedPlayer player, string Permission)
        {
            List<string> permissions = new List<string>();
            foreach (var permission in player.GetPermissions())
            {
                permissions.Add(permission.Name);
            }
            if (permissions.Contains(Permission))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public static void DebugPermissions(UnturnedPlayer player)
        {
            int i = 1;
            foreach (var permission in player.GetPermissions())
            {
                Logger.Log($"{i} | Permission Name: {permission.Name} | Permission Cooldown: {permission.Cooldown}");
                i++;
            }
        }
    }
}