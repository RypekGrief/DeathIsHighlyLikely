using SandBox.GameComponents;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.CharacterDevelopment;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.Core;
using TaleWorlds.MountAndBlade;
using TaleWorlds.CampaignSystem.AgentOrigins;

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
                var settings = DeathIsHighlyLikelySettings.Instance;
                if (settings == null) return baseProbability;

                PartyBase party = (effectedAgent.Origin as PartyAgentOrigin)?.Party;
                PartyBase enemyParty = (affectorAgent?.Origin as PartyAgentOrigin)?.Party;

                Hero surgeon = party?.MobileParty?.EffectiveSurgeon;
                Hero enemySurgeon = enemyParty?.MobileParty?.EffectiveSurgeon;

                int medicineSkill = 0;
                bool hasMinisterOfHealth = false;

                if (surgeon != null)
                {
                    medicineSkill = surgeon.GetSkillValue(DefaultSkills.Medicine);
                    hasMinisterOfHealth = surgeon.GetPerkValue(DefaultPerks.Medicine.MinisterOfHealth);
                }
                else if (enemySurgeon != null && enemySurgeon.GetPerkValue(DefaultPerks.Medicine.DoctorsOath))
                {
                    medicineSkill = enemySurgeon.GetSkillValue(DefaultSkills.Medicine);
                    hasMinisterOfHealth = enemySurgeon.GetPerkValue(DefaultPerks.Medicine.MinisterOfHealth);
                }

                if (effectedAgent.IsHero)
                {
                    float customProbability = settings.HeroDeathProbability;
                    float multiplier = settings.LordVsLordMultiplier;

                    if (multiplier > 1.0f && affectorAgent != null && affectorAgent.IsHero)
                    {
                        customProbability *= multiplier;
                    }

                    float reduction = medicineSkill * 0.0004f;
                    customProbability -= reduction;

                    if (customProbability < 0f) customProbability = 0f;
                    if (customProbability > 1.0f) customProbability = 1.0f;

                    return customProbability;
                }
                else
                {
                    float troopProbability = settings.TroopDeathProbability;

                    float reduction = medicineSkill * 0.001f;
                    troopProbability -= reduction;

                    if (troopProbability < 0f) troopProbability = 0f;

                    if (hasMinisterOfHealth)
                    {
                        troopProbability *= 0.45f;
                    }

                    if (troopProbability > 1.0f) troopProbability = 1.0f;

                    return troopProbability;
                }
            }

            return baseProbability;
        }
    }
}