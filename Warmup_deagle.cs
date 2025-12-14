using CounterStrikeSharp.API;
using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Modules.Utils;
using System.Linq;

namespace deagle_only
{
    public class deagle_only : BasePlugin
    {
        public override string ModuleAuthor => "GSM-RO";
        public override string ModuleName => "Warmup_deagle";
        public override string ModuleVersion => "1.0";
        public override string ModuleDescription => "Deagle Only - Warmup";

        private bool _warmupMessageSent = false;

        public override void Load(bool hotReload)
        {
            RegisterEventHandler<EventPlayerSpawn>(OnPlayerSpawn);
            RegisterEventHandler<EventRoundStart>(OnRoundStart);
            RegisterListener<Listeners.OnTick>(OnTick);
        }

        // 🔥 Detectare warmup corectă
        private static bool IsWarmupActive()
        {
            var gameRulesEnt = Utilities
                .FindAllEntitiesByDesignerName<CBaseEntity>("cs_gamerules")
                .SingleOrDefault();

            if (gameRulesEnt == null)
                return false;

            var proxy = gameRulesEnt.As<CCSGameRulesProxy>();
            return proxy?.GameRules?.WarmupPeriod == true;
        }

        // 📢 Mesaj la început de rundă (o singură dată)
        private HookResult OnRoundStart(EventRoundStart @event, GameEventInfo info)
        {
            if (!IsWarmupActive())
            {
                _warmupMessageSent = false;
                return HookResult.Continue;
            }

            if (_warmupMessageSent)
                return HookResult.Continue;

            Server.PrintToChatAll($" {ChatColors.Green}[Warmup]{ChatColors.Default} Runda este {ChatColors.Red}DEAGLE ONLY");
            Server.PrintToChatAll($" {ChatColors.Green}[Warmup]{ChatColors.Default} Runda este {ChatColors.Red}DEAGLE ONLY");
            Server.PrintToChatAll($" {ChatColors.Green}[Warmup]{ChatColors.Default} Runda este {ChatColors.Red}DEAGLE ONLY");

            _warmupMessageSent = true;
            return HookResult.Continue;
        }

        // 🔁 Forțare Deagle
        private static void OnTick()
        {
            if (!IsWarmupActive())
                return;

            foreach (var player in Utilities.GetPlayers())
            {
                if (player == null || !player.IsValid || !player.PawnIsAlive)
                    continue;

                var activeWeapon = player.PlayerPawn?.Value?
                    .WeaponServices?.ActiveWeapon.Value;

                if (activeWeapon == null)
                    continue;

                if (activeWeapon.DesignerName != "weapon_deagle")
                {
                    Server.NextFrame(() =>
                    {
                        RemoveAllWeapons(player);
                        player.GiveNamedItem("weapon_deagle");
                    });
                }
            }
        }

        // 🎯 Deagle la spawn
        private HookResult OnPlayerSpawn(EventPlayerSpawn @event, GameEventInfo info)
        {
            if (!IsWarmupActive())
                return HookResult.Continue;

            var player = @event.Userid;
            if (player == null || !player.IsValid || !player.PawnIsAlive)
                return HookResult.Continue;

            Server.NextFrame(() =>
            {
                RemoveAllWeapons(player);
                player.GiveNamedItem("weapon_deagle");
            });

            return HookResult.Continue;
        }

        // 🧹 Ștergere arme
        private static void RemoveAllWeapons(CCSPlayerController player)
        {
            var weaponServices = player.PlayerPawn?.Value?.WeaponServices;
            if (weaponServices?.MyWeapons == null)
                return;

            foreach (var weapon in weaponServices.MyWeapons)
            {
                if (weapon?.IsValid == true && weapon.Value != null)
                {
                    weapon.Value.AddEntityIOEvent(
                        "Kill",
                        weapon.Value,
                        null,
                        "",
                        0.1f
                    );
                }
            }
        }
    }
}