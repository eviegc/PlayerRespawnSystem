using UnityEngine;

namespace PlayerRespawnSystem
{
    class UIDeathTimerClient : MonoBehaviour
    {
        public static UIDeathTimerClient instance { get; private set; }

        private UIDeathTimerPanel deathTimerPanel;

        public void Awake()
        {
            On.RoR2.UI.HUD.Awake += HUD_Awake;
            On.RoR2.Run.OnDestroy += Run_OnDestroy;

            PlayerRespawnSystemPlugin.Log.LogDebug(
                $"[Client] UIDeathTimerClient enabled, assigning singleton"
            );
            UIDeathTimerClient.instance = SingletonHelper.Assign<UIDeathTimerClient>(
                UIDeathTimerClient.instance,
                this
            );
        }

        public void OnDestroy()
        {
            On.RoR2.UI.HUD.Awake -= HUD_Awake;
            On.RoR2.Run.OnDestroy -= Run_OnDestroy;

            PlayerRespawnSystemPlugin.Log.LogDebug(
                $"[Client] UIDeathTimerClient destroyed, unassigning singleton"
            );
            UIDeathTimerClient.instance = SingletonHelper.Unassign<UIDeathTimerClient>(
                UIDeathTimerClient.instance,
                this
            );
        }

        private void HUD_Awake(On.RoR2.UI.HUD.orig_Awake orig, RoR2.UI.HUD self)
        {
            orig(self);

            PlayerRespawnSystemPlugin.Log.LogDebug($"[Client] HUD is awake, constructing panel");

            var go = new GameObject("death_timer_box");
            go.transform.SetParent(self.mainContainer.transform, false);
            go.transform.SetAsFirstSibling();
            deathTimerPanel = go.AddComponent<UIDeathTimerPanel>();
        }

        private void Run_OnDestroy(On.RoR2.Run.orig_OnDestroy orig, RoR2.Run self)
        {
            orig(self);

            PlayerRespawnSystemPlugin.Log.LogDebug($"[Client] HUD is destroyed, removing panel");

            Destroy(deathTimerPanel);
        }

        public void UpdateDeathTimer(
            float respawnTime,
            bool canRespawn,
            bool canTimedRespawn,
            RespawnType activeRespawnType
        )
        {
            PlayerRespawnSystemPlugin.Log.LogDebug($"[Client] Start UpdateDeathTimer()");

            if (!deathTimerPanel || !PluginConfig.UseDeathTimerUI.Value)
            {
                PlayerRespawnSystemPlugin.Log.LogDebug(
                    $"[Client] deathTimerPanel cannot be updated"
                );
                return;
            }

            if (canRespawn)
            {
                if (canTimedRespawn)
                {
                    deathTimerPanel.textContext2.text =
                        $"in <color=red>{Mathf.CeilToInt(respawnTime)}</color> seconds";
                    deathTimerPanel.show = true;
                }
                else
                {
                    switch (activeRespawnType)
                    {
                        case RespawnType.Teleporter:
                            if (PluginConfig.RespawnOnTPEnd.Value)
                            {
                                deathTimerPanel.textContext2.text =
                                    $"after <color=red>teleporter</color> event";
                                deathTimerPanel.show = true;
                            }
                            break;

                        case RespawnType.Mithrix:
                            if (PluginConfig.RespawnOnMithrixEnd.Value)
                            {
                                deathTimerPanel.textContext2.text =
                                    $"after <color=red>Mithrix</color> fight";
                                deathTimerPanel.show = true;
                            }
                            break;

                        case RespawnType.Artifact:
                            if (PluginConfig.RespawnOnArtifactTrialEnd.Value)
                            {
                                deathTimerPanel.textContext2.text =
                                    $"after <color=red>artifact trial</color> ends";
                                deathTimerPanel.show = true;
                            }
                            break;
                        default:
                            deathTimerPanel.show = false;
                            break;
                    }
                }
            }
            else
            {
                deathTimerPanel.show = false;
            }

            PlayerRespawnSystemPlugin.Log.LogDebug(
                $"[Client] deathTimerPanel.show = {deathTimerPanel.show}"
            );
        }
    }
}
