using CounterStrikeSharp.API;
using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Modules.Utils;
using Microsoft.Extensions.Logging;

namespace deagle_only
{
    public class deagle_only : BasePlugin
    {
        public override string ModuleAuthor => "GSM-RO";
        public override string ModuleName => "Warmup_deagle";
        public override string ModuleVersion => "1.0.1";
        public override string ModuleDescription => "Warmup Deagle Only with config";

        private bool _warmupMessageSent = false;
        private static HashSet<string> AllowedWeapons = new();

        public override void Load(bool hotReload)
        {
            LoadConfig();

            RegisterEventHandler<EventPlayerSpawn>(OnPlayerSpawn);
            RegisterEventHandler<EventRoundStart>(OnRoundStart);
            RegisterListener<Listeners.OnTick>(OnTick);
        }

        private void LoadConfig()
        {
            var path = Path.Combine(ModuleDirectory, "config.cfg");

            if (!File.Exists(path))
            {
                File.WriteAllText(path,
                    "allowed_weapons = weapon_deagle, weapon_knife\n");
            }

            var lines = File.ReadAllLines(path);

            foreach (var line in lines)
            {
                if (string.IsNullOrWhiteSpace(line) || line.StartsWith("#"))
                    continue;

                if (!line.StartsWith("allowed_weapons"))
                    continue;

                var parts = line.Split('=');
                if (parts.Length < 2)
                    continue;

                AllowedWeapons = parts[1]
                    .Split(',')
                    .Select(w => w.Trim())
                    .Where(w => !string.IsNullOrEmpty(w))
                    .ToHashSet();
            }

            Logger.LogInformation(
                $"[Warmup_deagle] Allowed weapons: {string.Join(", ", AllowedWeapons)}"
            );
        }

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

        private HookResult OnRoundStart(EventRoundStart @event, GameEventInfo info)
        {
            if (!IsWarmupActive())
            {
                _warmupMessageSent = false;
                return HookResult.Continue;
            }

            if (_warmupMessageSent)
                return HookResult.Continue;

            Server.PrintToChatAll($" {ChatColors.Green}[Warmup]{ChatColors.Default} Round is {ChatColors.Red}DEAGLE ONLY");
            _warmupMessageSent = true;
            return HookResult.Continue;
        }

        private static void OnTick()
        {
            if (!IsWarmupActive())
                return;

            foreach (var player in Utilities.GetPlayers())
            {
                if (player == null || !player.IsValid || !player.PawnIsAlive)
                    continue;

                var weaponServices = player.PlayerPawn?.Value?.WeaponServices;
                if (weaponServices?.MyWeapons == null)
                    continue;

                foreach (var weapon in weaponServices.MyWeapons)
                {
                    if (weapon?.IsValid != true || weapon.Value == null)
                        continue;

                    var name = weapon.Value.DesignerName;

                    if (!AllowedWeapons.Contains(name))
                    {
                        weapon.Value.AddEntityIOEvent(
                            "Kill",
                            weapon.Value,
                            null,
                            "",
                            0.0f
                        );
                    }
                }
            }
        }

        private HookResult OnPlayerSpawn(EventPlayerSpawn @event, GameEventInfo info)
{
    if (!IsWarmupActive())
        return HookResult.Continue;

    var player = @event.Userid;
    if (player == null || !player.IsValid)
        return HookResult.Continue;

    // 🔹 Amânăm execuția până când Pawn-ul este complet respawnat
    Server.NextFrame(() =>
    {
        if (!player.PawnIsAlive || player.PlayerPawn?.Value == null)
            return;

        RemoveAllWeapons(player);

        // Dă armele din config
        foreach (var weapon in AllowedWeapons)
        {
            player.GiveNamedItem(weapon);
        }
    });

    return HookResult.Continue;
}


        private static void RemoveAllWeapons(CCSPlayerController player)
        {
            var weaponServices = player.PlayerPawn?.Value?.WeaponServices;
            if (weaponServices?.MyWeapons == null)
                return;

            foreach (var weapon in weaponServices.MyWeapons)
            {
                if (weapon?.IsValid != true || weapon.Value == null)
                    continue;

                var name = weapon.Value.DesignerName;

                if (AllowedWeapons.Contains(name))
                    continue;

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
