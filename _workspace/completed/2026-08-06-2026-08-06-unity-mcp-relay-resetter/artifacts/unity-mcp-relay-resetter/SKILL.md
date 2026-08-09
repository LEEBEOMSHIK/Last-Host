---
name: unity-mcp-relay-resetter
description: Inspect and safely stop Codex-owned Unity MCP client relay processes on Windows while preserving Codex, Unity Editor, and Unity Editor relay processes. Use when Unity MCP connection approvals persist, Remove All Connections immediately repopulates, direct-connection limits are exhausted, stale Codex sessions keep reconnecting, or the user asks to disconnect, clear, clean up, or reset all Codex Unity MCP relays.
---

# Unity MCP Relay Resetter

Reset Unity MCP client relay state without terminating Codex or Unity Editor.

`reset` means stopping existing Codex client relays so a later Unity MCP call can start a fresh relay. The script itself does not reconnect, approve Unity dialogs, change Unity settings, or remove connection-history files. A still-running Codex session can immediately create a new relay on its own.

## Safety boundary

- Target only `relay_win.exe --mcp` from the per-user `.unity\relay` directory whose live parent is `codex.exe`.
- Never stop `codex.exe`, `Unity.exe`, Unity Hub, or `relay_win.exe --relay` owned by Unity Editor.
- Never delete Unity MCP connection registries or edit Codex/Unity configuration.
- Never call a Unity MCP tool after cleanup merely to verify the result; that call can create a new relay.
- Do not repeatedly kill respawned processes. Report their parent Codex PIDs so the user can close unwanted sessions.

## Workflow

1. Run `scripts/Stop-UnityMcpRelays.ps1` without `-Apply` to inspect exact targets.
2. Report target count, relay PIDs, and parent Codex PIDs.
3. If the user only asked for status or diagnosis, stop without changing processes.
4. If the user explicitly asked to disconnect, clear, clean up, or reset the relays, run:

   ```powershell
   & '<skill-dir>\scripts\Stop-UnityMcpRelays.ps1' -Apply -Confirm:$false
   ```

5. Use the script's post-check result as verification. Do not invoke Unity MCP for verification.
6. Explain that the next intentional Unity MCP call can start a fresh relay and may show a Unity approval dialog.

## Result interpretation

- `Success = true`, `RemainingCount = 0`: all discovered Codex Unity MCP client relays stopped.
- `RespawnedCount > 0`: one or more running Codex sessions recreated relays; report their parent PIDs.
- `SkippedCount > 0`: relay-like processes failed the strict Codex ownership predicate and were preserved.
- `TargetCount = 0`: no eligible Codex Unity MCP client relays were running.
