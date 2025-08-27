using UnityEngine.Networking;

namespace PlayerRespawnSystem
{
    class PlayerUIRpcProxy : NetworkBehaviour
    {
        [TargetRpc]
        public void TargetUpdateDeathTimer(NetworkConnection target, float respawnTime, bool canRespawn, bool canTimedRespawn, RespawnType activeRespawnType)
        {
            PlayerRespawnSystemPlugin.Log.LogDebug($"[ClientRPC] Starting TargetUpdateDeathTimer()");

            var panel = UIDeathTimerClient.instance;
            if (!panel)
            {
                PlayerRespawnSystemPlugin.Log.LogDebug($"[ClientRPC] Could not find singleton panel, exiting");
                return;
            }

            PlayerRespawnSystemPlugin.Log.LogDebug($"[ClientRPC] Updating panel");
            panel.UpdateDeathTimer(respawnTime, canRespawn, canTimedRespawn, activeRespawnType);
        }
    }
}
