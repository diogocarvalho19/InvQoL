using Rocket.Unturned.Chat;
using Rocket.Unturned.Player;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using UnityEngine;

namespace InvQoL.Utils
{
    public class ChatHelper
    {

        // https://github.com/ShimmyMySherbet/RocketExtensions/blob/master/RocketExtensions/Utilities/ColorReformatter.cs

        public static readonly Regex ColorOpeningMatch = new Regex(@"\[color=[a-zA-Z0-9#]+\]");
        public static readonly Regex ColorClosing = new Regex(@"\[/color\]");

        public static string ReformatColor(string key)
        {
            var openings = ColorOpeningMatch.Matches(key);
            for (int i = 0; i < openings.Count; i++)
            {
                var opening = openings[i];
                if (!opening.Success)
                    continue;
                var color = opening.Value.Substring(7);
                color = color.Substring(0, color.Length - 1);

                key = key
                    .Remove(opening.Index, opening.Length)
                    .Insert(opening.Index, $"<color={color}>");
            }
            key = ColorClosing.Replace(key, "</color>", 20);
            return key;
        }

    }
}
