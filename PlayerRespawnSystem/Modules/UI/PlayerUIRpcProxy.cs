using UnityEngine.Networking;

namespace PlayerRespawnSystem
{
    class PlayerUIRpcProxy : NetworkBehaviour
    {
        [TargetRpc]
        public void TargetUpdateDeathTimer(NetworkConnection target, float respawnTime, bool canRespawn, bool canTimedRespawn, RespawnType activeRespawnType)
        {
            UnityEngine.Debug.Log($"PlayerRespawnSystem: [ClientRPC] Starting TargetUpdateDeathTimer()");

            var panel = UIDeathTimerClient.instance;
            if (!panel)
            {
                UnityEngine.Debug.Log($"PlayerRespawnSystem: [ClientRPC] Could not find singleton panel, exiting");
                return;
            }

            UnityEngine.Debug.Log($"PlayerRespawnSystem: [ClientRPC] Updating panel");
            panel.UpdateDeathTimer(respawnTime, canRespawn, canTimedRespawn, activeRespawnType);
        }
    }
}
