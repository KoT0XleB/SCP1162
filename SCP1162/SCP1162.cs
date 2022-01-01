using System;
using System.Collections.Generic;
using Mirror;
using Qurre;
using Qurre.API;
using Qurre.API.Controllers;
using Qurre.API.Controllers.Items;
using Qurre.API.Events;
using UnityEngine;
using MEC;

namespace SCP1162
{
    public class SCP1162 : Plugin
    {
        public override string Developer => "KoToXleB#4663";
        public override string Name => "SCP1162";
        public override System.Version Version => new System.Version(1, 0, 0);
        public override void Enable() => RegisterEvents();
        public override void Disable() => UnregisterEvents();

        private static GameObject SCP_1162;
        public Config CustomConfig { get; private set; }

        private static List<string> PlayersWithoutHands = new List<string>();

        public void RegisterEvents()
        {
            CustomConfig = new Config();
            CustomConfigs.Add(CustomConfig);
            if (!CustomConfig.IsEnable) return;

            Qurre.Events.Round.Waiting += Waiting;
            Qurre.Events.Round.Restart += Restart;
            Qurre.Events.Player.PickupItem += PickingUp;
            Qurre.Events.Player.InteractDoor += OnInteractDoor;
            Qurre.Events.Player.RoleChange += OnChangeRole;
        }
        public void UnregisterEvents()
        {
            CustomConfig = new Config();
            CustomConfigs.Add(CustomConfig);
            if (!CustomConfig.IsEnable) return;

            Qurre.Events.Round.Waiting -= Waiting;
            Qurre.Events.Round.Restart -= Restart;
            Qurre.Events.Player.PickupItem -= PickingUp;
            Qurre.Events.Player.InteractDoor -= OnInteractDoor;
            Qurre.Events.Player.RoleChange -= OnChangeRole;
        }
        public static void Waiting()
        {
            Pickup pickup = new Item(ItemType.Adrenaline).Spawn(new Vector3(0, 0, 0), new Quaternion(0, 0, 0, 0));
            SCP_1162 = pickup.Base.gameObject;
            NetworkServer.UnSpawn(SCP_1162);
            SCP_1162.GetComponent<Rigidbody>().isKinematic = true;
            SCP_1162.AddComponent<SCP1162Pickup>();

            var Transform = Qurre.API.Objects.RoomType.Lcz330.GetRoom().Transform;

            SCP_1162.transform.parent = Transform;
            SCP_1162.transform.localPosition = new Vector3(0.97f, 2.0f, -6f);
            SCP_1162.transform.localRotation = Quaternion.Euler(0f, 0f, 0f);
            SCP_1162.transform.localScale = new Vector3(73.5f, 47.8f, 2.83f);
            NetworkServer.Spawn(SCP_1162);
        }
        public static void Restart()
        {
            if (Round.Ended)
            {
                NetworkServer.Destroy(SCP_1162);
            }
        }
        public void PickingUp(PickupItemEvent ev)
        {
            if (PlayersWithoutHands.Contains(ev.Player.UserId))
            {
                ev.Allowed = false;
                if (!CustomConfig.HintEnable) return;
                ev.Player.ClearBroadcasts();
                ev.Player.Broadcast(CustomConfig.HintMessage, CustomConfig.HintDuration);
                return;
            }

            if (ev.Pickup.Base.gameObject.GetComponent<SCP1162Pickup>() == null) return;

            ev.Allowed = false;
            PickingUpSCP1162(ev.Player);
        }
        public void PickingUpSCP1162(Player player)
        {
            if (player.AllItems.Count < 1 && CustomConfig.KillPlayerWithoutItems)
            {
                player.EnableEffect(Qurre.API.Objects.EffectType.Poisoned, 999f, false);
                player.EnableEffect(Qurre.API.Objects.EffectType.Hemorrhage, 999f, false);
                player.PlaceBlood(player.Position, 1, 0.8f);
                //if (CustomConfig.HintEnable) player.Broadcast(CustomConfig.HintMessage, CustomConfig.HintDuration);
                PlayersWithoutHands.Add(player.UserId);
                //Timing.RunCoroutine(BloodPlaceKill(player));
                return;
            }
            List<ItemType> newItems = new List<ItemType>();
            foreach (Item item in player.AllItems)
            {
                newItems.Add(CalculateItem());
            }
            player.ResetInventory(newItems);
        }
        /*
        public IEnumerator<float> BloodPlaceKill(Player player)
        {
            while (player.Hp > 0)
            {
                if (CustomConfig.PlaceBloodEnable) player.PlaceBlood(player.Position, 1, 0.8f);
                if (player.Hp < 1)
                {
                    player.Kill(CustomConfig.DieMessage);
                    break;
                }
            }
            yield return Timing.WaitForSeconds(1f);
        }*/
        public ItemType CalculateItem()
        {
            return CustomConfig.Items[UnityEngine.Random.Range(0, CustomConfig.Items.Count - 1)];
        }
        public void OnInteractDoor(InteractDoorEvent ev)
        {
            if (PlayersWithoutHands.Contains(ev.Player.UserId)) ev.Allowed = false;
        }
        public void OnChangeRole(RoleChangeEvent ev)
        {
            if (PlayersWithoutHands.Contains(ev.Player.UserId)) PlayersWithoutHands.Remove(ev.Player.UserId);
        }
        internal class SCP1162Pickup : MonoBehaviour { }
    }
}
