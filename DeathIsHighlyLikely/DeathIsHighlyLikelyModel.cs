using SandBox.GameComponents;
using TaleWorlds.Core;
using TaleWorlds.MountAndBlade;

namespace DeathIsHighlyLikely
{
    public class DeathIsHighlyLikelyModel : SandboxAgentDecideKilledOrUnconsciousModel
    {
        public override float GetAgentStateProbability(Agent affectorAgent, Agent effectedAgent, DamageTypes damageType, WeaponFlags weaponFlags, out float useSurgeryProbability)
        {
            float baseProbability = base.GetAgentStateProbability(affectorAgent, effectedAgent, damageType, weaponFlags, out useSurgeryProbability);

            if (effectedAgent == null) return baseProbability;

            bool isTournament = Mission.Current != null && Mission.Current.Mode == MissionMode.Tournament;

            if (!isTournament)
            {
                if (effectedAgent.IsHero)
                {
                    float customProbability = DeathIsHighlyLikelySettings.Instance?.HeroDeathProbability ?? 0.20f;
                    float multiplier = DeathIsHighlyLikelySettings.Instance?.LordVsLordMultiplier ?? 1.0f;

                    if (multiplier > 1.0f && affectorAgent != null && affectorAgent.IsHero)
                    {
                        customProbability *= multiplier;

                        if (customProbability > 1.0f) customProbability = 1.0f;
                    }

                    return customProbability;
                }
                else if (!effectedAgent.IsHero)
                {
                    float troopProbability = DeathIsHighlyLikelySettings.Instance?.TroopDeathProbability ?? 0.40f;
                    return troopProbability;
                }
            }

            return baseProbability;
        }
    }
}