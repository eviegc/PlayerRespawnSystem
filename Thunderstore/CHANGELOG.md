# Changelog

## 2.2.0

- Update for Alloyed Collective DLC

## 2.1.5

- Fix faulty upload

## 2.1.4

- Small change to reduce logging and make the mod less noisy in console

## 2.1.3

- Use `nuget` for dependency management
- Fix `MethodAccessException` and some death timer UI `rpc` errors with a rebuild

## 2.1.2

- Make event respawns always getting deactivated on stage being advanced

## 2.1.1

- Fix UseTimedRespawn setting not working when set to false
- Fix timed respawn not using updated respawn position on events
- Fix timed respawn being blocked on tp event message being show when timed respawn is disabled

## 2.1.0

- Fix for Seekers of the Storm changes
- Added respawn handling for Voidling boss
- Added respawn handling for False Son boss
- Added Voidling and False Son maps to ignored Stages for timed respawn

## 2.0.4

- Update manifest + rebuild on new patch

## 2.0.3

- Fix for Survivors of the Void changes
- Fix bug where sometimes respawn system would not correctly destroy it's instance resulting in it's duplication
- Added new void boss stage to ignored by default
- Added option to turn off respawn on selected game modes

## 2.0.2

- Fix for Anniversary Update
- Fix issue where game would freeze if no proper repsawn point was found
- Fix missing R2Api "PrefabApi" reference
- Added new Moon stage to ignored Stages for timed respawn

## 2.0.1

- Reset respawn timer after RoR2 respawn method is called to eleminate conflicts with other respawn mods

## 2.0.0

- Update icon
- Added plentiful options to control respawning on certain events, regardless of timed respawn
- Added option to not use pods respawn on start of match
- Added Death Timer UI for dead players (client dependent)
