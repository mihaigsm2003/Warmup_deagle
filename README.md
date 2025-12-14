# Warmup_deagle 🔫

**Warmup_deagle** is a simple **Counter-Strike 2** plugin built with **CounterStrikeSharp** that enforces **Deagle Only** gameplay during the **warmup round**.

The plugin is lightweight, automatic, and fully inactive once warmup ends.

---

## Features

- 🔫 Deagle only during warmup
- 🤖 Applies to **players and bots**
- 🧹 Removes all other weapons
- 🔁 Forces Deagle if another weapon is picked up
- 💬 Chat message on warmup start:
  > `[Warmup] Round is DEAGLE ONLY`
- 📴 Automatically disabled after warmup

---

## Requirements

- Counter-Strike 2
- CounterStrikeSharp `>= 1.0.348`

---

## Installation

1. Build or download the plugin `.dll`
2. Place it in:
   ```
   csgo/addons/counterstrikesharp/plugins/Warmup_deagle/
   ```
3. Restart the server! 

---

## How it works

- Detects warmup using **GameRules**
- Only active when `WarmupPeriod == true`
- On spawn: removes weapons and gives `weapon_deagle`
- On tick: enforces Deagle only for players and bots

---

## Author

- **GSM-RO** inspired from awp no scop plugin (https://github.com/phara1/awp_noscope)

---

## License

Open-source. Free to use and modify.

