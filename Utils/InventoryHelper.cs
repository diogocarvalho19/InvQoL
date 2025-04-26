using Rocket.Unturned.Chat;
using Rocket.Unturned.Player;
using SDG.Unturned;
using Steamworks;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace InvQoL.Utils
{
    public class InventoryHelper
    {

        public static void OptimizeCommand(UnturnedPlayer player)
        {
            PlayerInventory inventory = player.Inventory;

            List<InventoryItem> items = new List<InventoryItem>();
            for (byte page = 2; page < PlayerInventory.PAGES - 2; page++)
            {

                if (inventory.items[page] == null)
                {
                    continue;
                }

                byte itemCount = inventory.getItemCount(page);
                for (byte index = 0; index < itemCount; index++)
                {
                    ItemJar itemJar = inventory.items[page].getItem(index);
                    if (itemJar == null) continue;

                    ItemAsset itemAsset = (ItemAsset)Assets.find(EAssetType.ITEM, itemJar.item.id);
                    if (itemAsset == null) continue;

                    if (Core.Instance.Configuration.Instance.Debug)
                    {
                        Rocket.Core.Logging.Logger.Log($"Collecting item '{itemAsset.itemName}' from page {page} at position ({itemJar.x}, {itemJar.y})");
                    }

                    items.Add(new InventoryItem
                    {
                        Item = itemJar.item,
                        SizeX = itemAsset.size_x,
                        SizeY = itemAsset.size_y,
                        Rot = itemJar.rot
                    });
                }
            }

            items = items.OrderByDescending(i => i.SizeX * i.SizeY).ToList();

            for (byte page = 2; page < PlayerInventory.PAGES - 2; page++)
            {
                if (inventory.items[page] == null)
                {
                    continue;
                }

                while (inventory.getItemCount(page) > 0)
                {
                    inventory.items[page].removeItem(0);
                }

            }

            inventory.sendStorage();

            bool itemDropped = false;
            foreach (var item in items)
            {
                bool placed = false;

                for (byte page = 2; page < PlayerInventory.PAGES - 2 && !placed; page++)
                {

                    if (inventory.items[page] == null)
                    {
                        continue;
                    }

                    byte width = inventory.getWidth(page);
                    byte height = inventory.getHeight(page);

                    for (byte y = 0; y < height && !placed; y++)
                    {
                        for (byte x = 0; x < width && !placed; x++)
                        {
                            /*if (CanPlaceItem(inventory, page, x, y, item.SizeX, item.SizeY, item.Rot)) {
                                inventory.items[page].addItem(x, y, item.Rot, item.Item);
                                placed = true;
                            }*/
                            int bestScore = -1;
                            byte bestRot = item.Rot;
                            byte bestX = x;
                            byte bestY = y;

                            if (CanPlaceItem(inventory, page, x, y, item.SizeX, item.SizeY, item.Rot))
                            {
                                int score = CalculateFreeSpaceScore(inventory, page, x, y, item.SizeX, item.SizeY, item.Rot);
                                if (score > bestScore)
                                {
                                    bestScore = score;
                                    bestRot = item.Rot;
                                    bestX = x;
                                    bestY = y;
                                }
                            }

                            byte altRot = (byte)((item.Rot + 1) % 4);
                            if (CanPlaceItem(inventory, page, x, y, item.SizeX, item.SizeY, altRot))
                            {
                                int score = CalculateFreeSpaceScore(inventory, page, x, y, item.SizeX, item.SizeY, altRot);
                                if (score > bestScore)
                                {
                                    bestScore = score;
                                    bestRot = altRot;
                                    bestX = x;
                                    bestY = y;
                                }
                            }

                            if (bestScore >= 0)
                            {
                                inventory.items[page].addItem(bestX, bestY, bestRot, item.Item);
                                placed = true;
                            }
                        }
                    }
                }

                if (!placed)
                {
                    itemDropped = true;
                    ItemManager.dropItem(item.Item, player.Position, true, true, true);
                }
            }

            inventory.sendStorage();

            if (itemDropped)
            {
                UnturnedChat.Say(player, ChatHelper.ReformatColor(Core.Instance.Translate("Dropped_Items")), Color.green, true);
            }

            UnturnedChat.Say(player, ChatHelper.ReformatColor(Core.Instance.Translate("Optimized_Inv")), Color.green, true);
        }

        public static void OptimizeButton(Player player)
        {
            UnturnedPlayer UPlayer = UnturnedPlayer.FromPlayer(player);
            if (IsPossibleOptimize(player))
            {
                PlayerInventory inventory = UPlayer.Inventory;

                List<InventoryItem> items = new List<InventoryItem>();
                for (byte page = 2; page < PlayerInventory.PAGES - 2; page++)
                {

                    if (inventory.items[page] == null)
                    {
                        continue;
                    }

                    byte itemCount = inventory.getItemCount(page);
                    for (byte index = 0; index < itemCount; index++)
                    {
                        ItemJar itemJar = inventory.items[page].getItem(index);
                        if (itemJar == null) continue;

                        ItemAsset itemAsset = (ItemAsset)Assets.find(EAssetType.ITEM, itemJar.item.id);
                        if (itemAsset == null) continue;

                        if (Core.Instance.Configuration.Instance.Debug)
                        {
                            Rocket.Core.Logging.Logger.Log($"Collecting item '{itemAsset.itemName}' from page {page} at position ({itemJar.x}, {itemJar.y})");
                        }

                        items.Add(new InventoryItem
                        {
                            Item = itemJar.item,
                            SizeX = itemAsset.size_x,
                            SizeY = itemAsset.size_y,
                            Rot = itemJar.rot
                        });
                    }
                }

                items = items.OrderByDescending(i => i.SizeX * i.SizeY).ToList();

                for (byte page = 2; page < PlayerInventory.PAGES - 2; page++)
                {
                    if (inventory.items[page] == null)
                    {
                        continue;
                    }

                    while (inventory.getItemCount(page) > 0)
                    {
                        inventory.items[page].removeItem(0);
                    }

                }

                inventory.sendStorage();

                bool itemDropped = false;
                foreach (var item in items)
                {
                    bool placed = false;

                    for (byte page = 2; page < PlayerInventory.PAGES - 2 && !placed; page++)
                    {

                        if (inventory.items[page] == null)
                        {
                            continue;
                        }

                        byte width = inventory.getWidth(page);
                        byte height = inventory.getHeight(page);

                        for (byte y = 0; y < height && !placed; y++)
                        {
                            for (byte x = 0; x < width && !placed; x++)
                            {
                                /*if (CanPlaceItem(inventory, page, x, y, item.SizeX, item.SizeY, item.Rot)) {
                                    inventory.items[page].addItem(x, y, item.Rot, item.Item);
                                    placed = true;
                                }*/
                                int bestScore = -1;
                                byte bestRot = item.Rot;
                                byte bestX = x;
                                byte bestY = y;

                                if (CanPlaceItem(inventory, page, x, y, item.SizeX, item.SizeY, item.Rot))
                                {
                                    int score = CalculateFreeSpaceScore(inventory, page, x, y, item.SizeX, item.SizeY, item.Rot);
                                    if (score > bestScore)
                                    {
                                        bestScore = score;
                                        bestRot = item.Rot;
                                        bestX = x;
                                        bestY = y;
                                    }
                                }

                                byte altRot = (byte)((item.Rot + 1) % 4);
                                if (CanPlaceItem(inventory, page, x, y, item.SizeX, item.SizeY, altRot))
                                {
                                    int score = CalculateFreeSpaceScore(inventory, page, x, y, item.SizeX, item.SizeY, altRot);
                                    if (score > bestScore)
                                    {
                                        bestScore = score;
                                        bestRot = altRot;
                                        bestX = x;
                                        bestY = y;
                                    }
                                }

                                if (bestScore >= 0)
                                {
                                    inventory.items[page].addItem(bestX, bestY, bestRot, item.Item);
                                    placed = true;
                                }
                            }
                        }
                    }

                    if (!placed)
                    {
                        itemDropped = true;
                        ItemManager.dropItem(item.Item, UPlayer.Position, true, true, true);
                    }
                }

                inventory.sendStorage();

                if (itemDropped)
                {
                    UnturnedChat.Say(UPlayer, ChatHelper.ReformatColor(Core.Instance.Translate("Dropped_Items")), Color.green, true);
                }

                UnturnedChat.Say(UPlayer, ChatHelper.ReformatColor(Core.Instance.Translate("Optimized_Inv")), Color.green, true);
            }
        }
        
        public static void TakeAll(UnturnedPlayer player)
        {
            Ray ray = new Ray(player.Player.look.aim.position, player.Player.look.aim.forward);
            RaycastHit hit;
            InteractableStorage targetStorage;

            int layerMask = LayerMask.GetMask("Barricade", "Structure");
            if (Physics.Raycast(ray, out hit, Core.Instance.Configuration.Instance.PutTakeAllDistance, layerMask))
            {
                targetStorage = hit.transform.GetComponent<InteractableStorage>();
                if (targetStorage == null)
                {
                    targetStorage = hit.transform.GetComponentInParent<InteractableStorage>();
                }
                if (targetStorage == null)
                {
                    targetStorage = hit.transform.GetComponentInChildren<InteractableStorage>();
                }

                if (targetStorage == null)
                {
                    UnturnedChat.Say(player, ChatHelper.ReformatColor(Core.Instance.Translate("No_Storage_Found")), Color.red, true);
                    return;
                }
            }
            else
            {
                if (Core.Debug)
                {
                    UnturnedChat.Say(player, ChatHelper.ReformatColor(Core.Instance.Translate("No_Storage_Found")), Color.red, true);
                }
                return;
            }
            
            var storage = BarricadeManager.FindBarricadeByRootTransform(targetStorage.transform);
            if(storage == null)
            {
                UnturnedChat.Say(player, ChatHelper.ReformatColor(Core.Instance.Translate("No_Storage_Found")), Color.red, true);
                return;
            }

            if (storage.asset.isLocked)
            {
                if (targetStorage.owner != player.CSteamID && (targetStorage.group == CSteamID.Nil || targetStorage.group != player.Player.quests.groupID))
                {
                    UnturnedChat.Say(player, ChatHelper.ReformatColor(Core.Instance.Translate("Access_Denied")), Color.red, true);
                    return;
                }
            }

            if (Core.Instance.Configuration.Instance.BlacklistStorage.Any(b => b.StorageID == storage.asset?.id))
            {
                UnturnedChat.Say(player, ChatHelper.ReformatColor(Core.Instance.Translate("Storage_Blacklist")), Color.red, true);
                return;
            }

            if (targetStorage.isOpen)
            {
                UnturnedChat.Say(player, ChatHelper.ReformatColor(Core.Instance.Translate("Storage_Open")), Color.red, true);
                return;
            }

            var inventory = player.Inventory;
            var items = targetStorage.items.items;

            for (int i = items.Count - 1; i >= 0; i--)
            {
                var item = items[i];

                if (inventory.tryAddItem(item.item, false))
                {
                    targetStorage.items.removeItem((byte)i);
                }
            }

            UnturnedChat.Say(player, ChatHelper.ReformatColor(Core.Instance.Translate("Take_All")), Color.green, true);
        }

        public static void PutAll(UnturnedPlayer player)
        {
            Ray ray = new Ray(player.Player.look.aim.position, player.Player.look.aim.forward);
            RaycastHit hit;
            InteractableStorage targetStorage = null;

            int layerMask = LayerMask.GetMask("Barricade", "Structure");
            if (Physics.Raycast(ray, out hit, Core.Instance.Configuration.Instance.PutTakeAllDistance, layerMask))
            {
                targetStorage = hit.transform.GetComponent<InteractableStorage>();
                if (targetStorage == null)
                {
                    targetStorage = hit.transform.GetComponentInParent<InteractableStorage>();
                }
                if (targetStorage == null)
                {
                    targetStorage = hit.transform.GetComponentInChildren<InteractableStorage>();
                }

                if (targetStorage == null)
                {
                    UnturnedChat.Say(player, ChatHelper.ReformatColor(Core.Instance.Translate("No_Storage_Found")), Color.red, true);
                    return;
                }
            }
            else
            {
                if (Core.Instance.Configuration.Instance.Debug)
                {
                    UnturnedChat.Say(player, ChatHelper.ReformatColor(Core.Instance.Translate("No_Storage_Found")), Color.red, true);
                    if(Core.Instance.Configuration.Instance.Debug)
                    {
                        Rocket.Core.Logging.Logger.Log($"No object hit within {Core.Instance.Configuration.Instance.PutTakeAllDistance}m.");
                    }
                }
                return;
            }

            var storage = BarricadeManager.FindBarricadeByRootTransform(targetStorage.transform);
            if (storage == null)
            {
                UnturnedChat.Say(player, ChatHelper.ReformatColor(Core.Instance.Translate("No_Storage_Found")), Color.red, true);
                return;
            }

            if(storage.asset.isLocked)
            {
                if (targetStorage.owner != player.CSteamID && (targetStorage.group == CSteamID.Nil || targetStorage.group != player.Player.quests.groupID))
                {
                    UnturnedChat.Say(player, ChatHelper.ReformatColor(Core.Instance.Translate("Access_Denied")), Color.red, true);
                    return;
                }
            }

            if (Core.Instance.Configuration.Instance.BlacklistStorage.Any(b => b.StorageID == storage.asset?.id))
            {
                UnturnedChat.Say(player, ChatHelper.ReformatColor(Core.Instance.Translate("Storage_Blacklist")), Color.red, true);
                return;
            }

            if (targetStorage.isOpen)
            {
                UnturnedChat.Say(player, ChatHelper.ReformatColor(Core.Instance.Translate("Storage_Open")), Color.red, true);
                return;
            }

            var inventory = player.Inventory;

            for (byte page = 2; page < PlayerInventory.PAGES - 2; page++)
            {
                int count = inventory.getItemCount(page);
                for (int x = count - 1; x >= 0; x--)
                {
                    var item = inventory.getItem(page, (byte)x);
                    if (item != null)
                    {
                        if (targetStorage.items.tryAddItem(item.item))
                        {
                            inventory.removeItem(page, (byte)x);
                        }
                    }
                }
            }

            UnturnedChat.Say(player, ChatHelper.ReformatColor(Core.Instance.Translate("Put_All")), Color.green, true);
        }

        public static bool IsPossibleOptimize(Player player)
        {
            UnturnedPlayer UPlayer = UnturnedPlayer.FromPlayer(player);
            if (!player.life.IsAlive)
            {
                if (Core.Instance.Configuration.Instance.Debug)
                {
                    Rocket.Core.Logging.Logger.Log($"Inventory optimize was canceled. {player.name} is dead.");
                }
                return false;
            }
            if (player == null || UPlayer == null)
            {
                if (Core.Instance.Configuration.Instance.Debug)
                {
                    Rocket.Core.Logging.Logger.Log($"Inventory optimize was canceled. {player.name} was not found.");
                }
                return false;
            }
            if (UPlayer.Inventory == null)
            {
                if (Core.Instance.Configuration.Instance.Debug)
                {
                    Rocket.Core.Logging.Logger.Log($"Inventory optimize was canceled. Inventory from {player.name} was not found.");
                }
                return false;
            }
            if (Player.isLoading)
            {
                if (Core.Instance.Configuration.Instance.Debug)
                {
                    Rocket.Core.Logging.Logger.Log($"Inventory optimize was canceled. {player.name} is still loading the game.");
                }
                return false;
            }
            if (Core.Instance.Configuration.Instance.ButtonNeedPermission)
            {
                if (!PermissionsUtils.PlayerHavePermission(UPlayer, Core.Instance.Configuration.Instance.ButtonPermission))
                {
                    if (Core.Instance.Configuration.Instance.Debug)
                    {
                        Rocket.Core.Logging.Logger.Log($"Inventory optimize was canceled. {player.name} dont have permission to use button.");
                    }
                    return false;
                }
            }
            if (Core.Instance.Configuration.Instance.EnableCooldownToUseButton)
            {
                if (!CooldownHelper.IsCooldownReady(UPlayer.CSteamID, out double secondsLeft))
                {
                    UnturnedChat.Say(UPlayer, ChatHelper.ReformatColor(Core.Instance.Translate("Cooldown", Math.Ceiling(secondsLeft))), Color.red, true);
                    if (Core.Instance.Configuration.Instance.Debug)
                    {
                        Rocket.Core.Logging.Logger.Log($"Inventory optimize was canceled. {player.name} have cooldown active.");
                    }
                    return false;
                }
            }
            return true;
        }

        public static bool CanPlaceItem(PlayerInventory inventory, byte page, byte x, byte y, byte sizeX, byte sizeY, byte rot)
        {

            if (page == 0)
            {
                return false;
            }

            byte width = inventory.getWidth(page);
            byte height = inventory.getHeight(page);

            byte effectiveSizeX = (rot % 2 == 0) ? sizeX : sizeY;
            byte effectiveSizeY = (rot % 2 == 0) ? sizeY : sizeX;

            if (x + effectiveSizeX > width || y + effectiveSizeY > height)
            {
                return false;
            }

            byte itemCount = inventory.getItemCount(page);
            for (byte index = 0; index < itemCount; index++)
            {
                ItemJar existingItem = inventory.items[page].getItem(index);
                if (existingItem == null) continue;

                ItemAsset existingAsset = (ItemAsset)Assets.find(EAssetType.ITEM, existingItem.item.id);
                if (existingAsset == null) continue;

                byte existingSizeX = (existingItem.rot % 2 == 0) ? existingAsset.size_x : existingAsset.size_y;
                byte existingSizeY = (existingItem.rot % 2 == 0) ? existingAsset.size_y : existingAsset.size_x;

                bool overlapX = x < existingItem.x + existingSizeX && x + effectiveSizeX > existingItem.x;
                bool overlapY = y < existingItem.y + existingSizeY && y + effectiveSizeY > existingItem.y;

                if (overlapX && overlapY)
                {
                    return false;
                }
            }

            return true;
        }

        public static int CalculateFreeSpaceScore(PlayerInventory inventory, byte page, byte x, byte y, byte sizeX, byte sizeY, byte rot)
        {
            byte width = inventory.getWidth(page);
            byte height = inventory.getHeight(page);

            byte effectiveSizeX = (rot % 2 == 0) ? sizeX : sizeY;
            byte effectiveSizeY = (rot % 2 == 0) ? sizeY : sizeX;

            int freeCells = 0;

            for (int dy = -1; dy <= effectiveSizeY; dy++)
            {
                for (int dx = -1; dx <= effectiveSizeX; dx++)
                {
                    if (dx >= 0 && dx < effectiveSizeX && dy >= 0 && dy < effectiveSizeY)
                    {
                        continue;
                    }

                    int checkX = x + dx;
                    int checkY = y + dy;

                    if (checkX < 0 || checkX >= width || checkY < 0 || checkY >= height)
                    {
                        continue;
                    }

                    bool occupied = false;
                    byte itemCount = inventory.getItemCount(page);
                    for (byte index = 0; index < itemCount; index++)
                    {
                        ItemJar existingItem = inventory.items[page].getItem(index);
                        if (existingItem == null) continue;

                        ItemAsset existingAsset = (ItemAsset)Assets.find(EAssetType.ITEM, existingItem.item.id);
                        if (existingAsset == null) continue;

                        byte existingSizeX = (existingItem.rot % 2 == 0) ? existingAsset.size_x : existingAsset.size_y;
                        byte existingSizeY = (existingItem.rot % 2 == 0) ? existingAsset.size_y : existingAsset.size_x;

                        if (checkX >= existingItem.x && checkX < existingItem.x + existingSizeX &&
                            checkY >= existingItem.y && checkY < existingItem.y + existingSizeY)
                        {
                            occupied = true;
                            break;
                        }
                    }

                    if (!occupied)
                    {
                        freeCells++;
                    }
                }
            }

            return freeCells;
        }

    }
}