using Rocket.API;
using Rocket.Unturned.Chat;
using Rocket.Unturned.Player;
using System.Collections.Generic;
using SDG.Unturned;
using UnityEngine;
using System;
using InvQoL.Utils;
using System.Linq;
using Rocket.Core.Assets;

namespace InvQoL.Commands
{
    public class PutAllCommand : IRocketCommand
    {
        public AllowedCaller AllowedCaller => AllowedCaller.Player;

        public string Name => "putall";
        public string Help => "Puts all inventory items into the nearest container.";
        public string Syntax => "/putall";
        public List<string> Aliases => new List<string>();
        public List<string> Permissions => new List<string>() { "inv.putall" };

        public void Execute(IRocketPlayer caller, string[] command)
        {
            UnturnedPlayer player = (UnturnedPlayer)caller;
            InventoryHelper.PutAll(player);
        }

    }
}
