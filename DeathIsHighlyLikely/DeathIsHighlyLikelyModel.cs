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

            if (effectedAgent.IsHero)
            {
                bool isTournament = Mission.Current != null && Mission.Current.Mode == MissionMode.Tournament;

                if (!isTournament)
                {
                    float customProbability = DeathIsHighlyLikelySettings.Instance?.HeroDeathProbability ?? 0.20f;

                    return customProbability;
                }
            }

            return baseProbability;
        }
    }
}