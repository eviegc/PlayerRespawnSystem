using RoR2;
using UnityEngine;
using UnityEngine.Networking;

namespace PlayerRespawnSystem
{
    class UIDeathTimerServer : NetworkBehaviour
    {
        public readonly float UpdateUIEveryXSeconds = 0.5f;

        [ServerCallback]
        public void Awake()
        {
            On.RoR2.Run.Start += Run_OnStart;
            On.RoR2.Run.OnDestroy += Run_OnDestroy;
        }

        [ServerCallback]
        public void OnDestroy()
        {
            On.RoR2.Run.Start -= Run_OnStart;
            On.RoR2.Run.OnDestroy -= Run_OnDestroy;
        }

        public void Run_OnStart(On.RoR2.Run.orig_Start orig, RoR2.Run self)
        {
            orig(self);

            InvokeRepeating("UpdateAllDeathTimers", 2.0f, UpdateUIEveryXSeconds);
        }

        private void Run_OnDestroy(On.RoR2.Run.orig_OnDestroy orig, RoR2.Run self)
        {
            orig(self);

            CancelInvoke("UpdateAllDeathTimers");
        }

        public void UpdateAllDeathTimers()
        {
            if (
                PlayerRespawnSystem.instance
                && PlayerRespawnSystem.instance.RespawnControllers.ContainsKey(RespawnType.Timed)
            )
            {
                RespawnType activeRespawnType = RespawnType.Timed;
                TimedRespawnController timedRespawnController = null;

                foreach (
                    var (respawnType, respawnController) in PlayerRespawnSystem
                        .instance
                        .RespawnControllers
                )
                {
                    if (respawnController is TimedRespawnController)
                    {
                        timedRespawnController = respawnController as TimedRespawnController;
                    }
                    else if (respawnController.IsActive)
                    {
                        activeRespawnType = respawnType;
                    }
                }

                if (timedRespawnController)
                {
                    foreach (var user in NetworkUser.readOnlyInstancesList)
                    {
                        var conn = user.connectionToClient;
                        if (conn == null || !conn.isReady)
                            continue;

                        if (
                            timedRespawnController.UserRespawnTimers.TryGetValue(
                                user.id,
                                out var userTimer
                            )
                        )
                        {
                            float respawnTime = userTimer.TimeRemaining;
                            bool canRespawn =
                                PlayerRespawnSystem.instance.CheckIfUserCanBeRespawned(user);
                            bool canTimedRespawn = canRespawn && timedRespawnController.IsActive;

                            user.GetComponent<PlayerUIRpcProxy>()
                                .TargetUpdateDeathTimer(
                                    conn,
                                    respawnTime,
                                    canRespawn,
                                    canTimedRespawn,
                                    activeRespawnType
                                );
                        }
                    }
                }
            }
        }
    }
}
