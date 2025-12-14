# Warmup_deagle 🔫

**Warmup_deagle** is a simple **Counter-Strike 2** plugin built with **CounterStrikeSharp** that enforces **Deagle Only** gameplay during the **warmup round**.

The plugin is lightweight, automatic, and fully inactive once warmup ends.

---

## Features

- 🔫 Deagle Only during warmup
- 🤖 Works for **players and bots**
- 🧹 Automatically removes any disallowed weapons
- 💬 Chat message at round start: `[Warmup] Round is DEAGLE ONLY`
- ✅ Allowed weapons are configurable via **config file**

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

