using Rocket.API;
using Rocket.Unturned.Chat;
using Rocket.Unturned.Player;
using System.Collections.Generic;
using SDG.Unturned;
using UnityEngine;
using System;
using InvQoL.Utils;
using System.Linq;

namespace InvQoL.Commands
{
    public class TakeAllCommand : IRocketCommand
    {
        public AllowedCaller AllowedCaller => AllowedCaller.Player;

        public string Name => "takeall";
        public string Help => "Take all inventory items from the nearest container.";
        public string Syntax => "/takeall";
        public List<string> Aliases => new List<string>();
        public List<string> Permissions => new List<string>() { "invqol.takeall" };

        public void Execute(IRocketPlayer caller, string[] command)
        {
            UnturnedPlayer player = (UnturnedPlayer)caller;
            InventoryHelper.TakeAll(player);
        }
    }
}
