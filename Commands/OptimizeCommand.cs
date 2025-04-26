using Rocket.API;
using Rocket.Unturned.Chat;
using Rocket.Unturned.Player;
using System.Collections.Generic;
using SDG.Unturned;
using UnityEngine;
using System;
using InvQoL.Utils;
using System.Linq;
using Rocket.Core.Logging;

namespace InvQoL.Commands
{
    public class OptimizeCommand : IRocketCommand
    {
        public AllowedCaller AllowedCaller => AllowedCaller.Player;

        public string Name => "optimize";
        public string Help => "Optimize your inventory.";
        public string Syntax => "/optimize";
        public List<string> Aliases => new List<string>();
        public List<string> Permissions => new List<string>() { "invqol.optimize.command" };

        public void Execute(IRocketPlayer caller, string[] command)
        {
            UnturnedPlayer player = (UnturnedPlayer)caller;
            InventoryHelper.OptimizeCommand(player);
        }

    }
}
