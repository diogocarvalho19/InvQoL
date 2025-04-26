using Rocket.API;
using System.Collections.Generic;
using System.Xml.Serialization;
using UnityEngine;

namespace InvQoL
{
    public class PluginConfig : IRocketPluginConfiguration
    {
        public bool Debug;
        public List<Blacklist> BlacklistStorage;
        public bool ButtonNeedPermission;
        public string ButtonPermission;
        public bool EnableCooldownToUseButton;
        public int ButtonCooldown;
        public float PutTakeAllDistance;
        public bool ShowButtonWhenJoinServer;
        public bool AllowUseKeys;
        public int CooldownKeys;

        public class Blacklist
        {
            public Blacklist() { }
            internal Blacklist(ushort id)
            {
                StorageID = id;
            }
            [XmlAttribute]
            public ushort StorageID;
        }

        public void LoadDefaults()
        {
            Debug = false;
            BlacklistStorage = new List<Blacklist>
            {
                new Blacklist(1249)
            };
            ButtonNeedPermission = true;
            ButtonPermission = "invqol.optimize.button";
            EnableCooldownToUseButton = true;
            ButtonCooldown = 5;
            PutTakeAllDistance = 5f;
            ShowButtonWhenJoinServer = true;
            AllowUseKeys = true;
            CooldownKeys = 2;
        }
    }
}