using Qurre.API.Addons;
using System.ComponentModel;
using Qurre;
using Qurre.API;
using System.Collections.Generic;

namespace SCP1162
{
    public class Config : IConfig
    {
        [Description("Plugin Name")]
        public string Name { get; set; } = "SCP1162";

        [Description("Enable the plugin?")]
        public bool IsEnable { get; set; } = true;
        [Description("Kill players when using SCP-1162 without items in the inventory?")]
        public bool KillPlayerWithoutItems { get; set; } = true;
        public bool HintEnable { get; set; } = true;
        public ushort HintDuration { get; set; } = 6;
        public string HintMessage { get; set; } = "<size=50><color=#EC3535>You don't have the hands</color></size>";
        //public bool PlaceBloodEnable { get; set; } = true;
        //public string DieMessage { get; set; } = "<size=50><color=#EC3535>You died of blood loss from SCP-1162</color></size>";

        [Description("Items that can be obtained from SCP-1162.")]
        public List<ItemType> Items { get; set; } = new List<ItemType>()
        {
            ItemType.KeycardO5,
            ItemType.SCP500,
            ItemType.KeycardNTFCommander,
            ItemType.KeycardContainmentEngineer,
            ItemType.SCP268,
            ItemType.GrenadeHE,
            ItemType.SCP207,
            ItemType.Adrenaline,
            ItemType.KeycardFacilityManager,
            ItemType.Medkit,
            ItemType.KeycardNTFLieutenant,
            ItemType.KeycardGuard,
            ItemType.KeycardZoneManager,
            ItemType.KeycardScientist,
            ItemType.KeycardGuard,
            ItemType.Radio,
            ItemType.MicroHID,
            ItemType.GunCOM15,
            ItemType.GunCOM18,
            ItemType.GunCrossvec,
            ItemType.GunE11SR,
            ItemType.GunRevolver,
            ItemType.GrenadeFlash,
            ItemType.KeycardScientist,
            ItemType.KeycardJanitor,
            ItemType.Coin,
            ItemType.Flashlight
        };
    }
}
