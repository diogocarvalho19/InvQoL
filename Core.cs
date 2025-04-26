using InvQoL.Utils;
using Rocket.API;
using Rocket.API.Collections;
using Rocket.Core.Logging;
using Rocket.Core.Plugins;
using Rocket.Unturned;
using Rocket.Unturned.Chat;
using Rocket.Unturned.Events;
using Rocket.Unturned.Player;
using SDG.Unturned;
using Steamworks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using UnityEngine;

namespace InvQoL
{
    public class Core : RocketPlugin<PluginConfig>
    {
        public static Core Instance;
        public static bool Debug = false;
        public readonly List<CSteamID> playersWithUI = new List<CSteamID>();

        protected override void Load()
        {

            Rocket.Core.Logging.Logger.Log($"Loading InvQoL...");

            Instance = this;

            Debug = Configuration.Instance.Debug;

            U.Events.OnPlayerConnected += OnPlayerConnected;
            U.Events.OnPlayerDisconnected += OnPlayerDisconnected;
            EffectManager.onEffectButtonClicked += OnButtonClick;
            if(Configuration.Instance.AllowUseKeys) PlayerInput.onPluginKeyTick += OnKeyPress;

            Rocket.Core.Logging.Logger.Log("InvQoL loaded!");

        }

        protected override void Unload()
        {
            U.Events.OnPlayerConnected -= OnPlayerConnected;
            U.Events.OnPlayerDisconnected -= OnPlayerDisconnected;
            EffectManager.onEffectButtonClicked -= OnButtonClick;
            if (Configuration.Instance.AllowUseKeys) PlayerInput.onPluginKeyTick -= OnKeyPress;
        }

        public Dictionary<CSteamID, long> KeyCooldown = new Dictionary<CSteamID, long>();
        private void OnKeyPress(Player player, uint simulation, byte key, bool state)
        {
            if (!player.life.IsAlive) return;
            UnturnedPlayer UPlayer = UnturnedPlayer.FromPlayer(player);
            switch(key)
            {
                case 0 when state:
                    if (KeyCooldown.TryGetValue(UPlayer.CSteamID, out var lastSentTicks1) && DateTime.UtcNow.Ticks - lastSentTicks1 < TimeSpan.FromSeconds(Configuration.Instance.CooldownKeys).Ticks)
                        return;
                    KeyCooldown[UPlayer.CSteamID] = DateTime.UtcNow.Ticks;
                    InventoryHelper.PutAll(UPlayer);
                    if (Debug)
                    {
                        Rocket.Core.Logging.Logger.Log($"Key {key} as been pressed!");
                    }
                    break;
                case 1 when state:
                    if (KeyCooldown.TryGetValue(UPlayer.CSteamID, out var lastSentTicks2) && DateTime.UtcNow.Ticks - lastSentTicks2 < TimeSpan.FromSeconds(Configuration.Instance.CooldownKeys).Ticks)
                        return;
                    KeyCooldown[UPlayer.CSteamID] = DateTime.UtcNow.Ticks;
                    InventoryHelper.TakeAll(UPlayer);
                    if (Debug)
                    {
                        Rocket.Core.Logging.Logger.Log($"Key {key} as been pressed!");
                    }
                    break;
                default:
                    return;
            }
        }

        private void OnPlayerConnected(UnturnedPlayer player)
        {
            if(Configuration.Instance.ShowButtonWhenJoinServer)
            {
                if (!playersWithUI.Contains(player.CSteamID))
                {
                    EffectManager.sendUIEffect(37001, 370, player.CSteamID, true);
                    playersWithUI.Add(player.CSteamID);
                }
            }
        }

        private void OnPlayerDisconnected(UnturnedPlayer player)
        {
            if(Configuration.Instance.ShowButtonWhenJoinServer)
            {
                playersWithUI.Remove(player.CSteamID);
            }
        }

        private void OnButtonClick(Player player, string buttonName)
        {
            if (Debug)
                Rocket.Core.Logging.Logger.Log($"Button click detected by {player.name}, button name {buttonName}.");

            if (buttonName == "OptimizeButton")
            {
                InventoryHelper.OptimizeButton(player);
            }
        }

        public override TranslationList DefaultTranslations => new TranslationList()
        {
            { "Dropped_Items", "[color=red]Could not place all items. Some items may have been dropped.[/color]" },
            { "Optimized_Inv", "[color=green]Your inventory has been optimized.[/color]" },
            { "No_Storage_Found", "[color=red]No storage found in your line of sight.[/color]" },
            { "Storage_Open", "[color=red]This storage is currently being accessed by another player![/color]" },
            { "Put_All", "[color=green]All possible items have been moved to the targeted container.[/color]" },
            { "Take_All", "[color=green]All possible items have been moved to your inventory.[/color]" },
            { "Cooldown", "[color=red]You have an active cooldown, wait another {0} seconds.[/color]" },
            { "Storage_Blacklist", "[color=red]This storage is blacklisted and cannot be used for this.[/color]" },
            { "Show_Button", "[color=yellow]The optimize button is visible.[/color]" },
            { "Hide_Button", "[color=yellow]The optimize button has been hidden.[/color]" },
            { "Access_Denied", "[color=red]You are neither the owner nor a member of the group to use this command on this storage.[/color]" },
        };

    }
}