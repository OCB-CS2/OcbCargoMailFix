@echo off

call MC-CS2 OcbCargoMailFix.dll Harmony\*.cs ^
  /reference:"%PATH_CS2_MANAGED%\Game.dll" && ^
echo Successfully compiled OcbCargoMailFix.dll

pause