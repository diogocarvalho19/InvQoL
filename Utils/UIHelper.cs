using SDG.Unturned;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InvQoL.Utils
{
    public static class UIHelper
    {

        public static void ShowButton(Player player)
        {
            EffectAsset effectAsset = Assets.find<EffectAsset>(new Guid("b8f9725977154ec39ee384f2e2fc6cc8"));
            EffectManager.SendUIEffect(effectAsset, 370, player.channel.owner.transportConnection, true);
        }

        public static void HideButton(Player player) 
        {
            EffectManager.ClearEffectByGuid(new Guid("b8f9725977154ec39ee384f2e2fc6cc8"), player.channel.owner.transportConnection);
        }

    }
}
