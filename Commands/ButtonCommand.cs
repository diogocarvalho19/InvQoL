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
    public class ButtonCommand : IRocketCommand
    {
        public AllowedCaller AllowedCaller => AllowedCaller.Player;

        public string Name => "button";
        public string Help => "Show/hide button.";
        public string Syntax => "/button";
        public List<string> Aliases => new List<string>();
        public List<string> Permissions => new List<string>() { "invqol.button" };

        public void Execute(IRocketPlayer caller, string[] command)
        {
            UnturnedPlayer player = (UnturnedPlayer)caller;
            
            if(!Core.Instance.playersWithUI.Contains(player.CSteamID))
            {
                UIHelper.ShowButton(player.Player);
                Core.Instance.playersWithUI.Add(player.CSteamID);
                UnturnedChat.Say(player, ChatHelper.ReformatColor(Core.Instance.Translate("Show_Button")), Color.yellow, true);
                return;
            }
            else
            {
                UIHelper.HideButton(player.Player);
                Core.Instance.playersWithUI.Remove(player.CSteamID);
                UnturnedChat.Say(player, ChatHelper.ReformatColor(Core.Instance.Translate("Hide_Button")), Color.yellow, true);
                return;
            }

        }

    }
}
