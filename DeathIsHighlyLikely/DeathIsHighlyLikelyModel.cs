using SandBox.GameComponents;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.CharacterDevelopment;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.Core;
using TaleWorlds.MountAndBlade;
using TaleWorlds.CampaignSystem.AgentOrigins;

namespace DeathIsHighlyLikely
{
    /// <summary>Overrides SandboxAgentDecideKilledOrUnconsciousModel to inject custom hero and troop death probability calculations for 3D battles.</summary>
    public class DeathIsHighlyLikelyModel : SandboxAgentDecideKilledOrUnconsciousModel
    {
        /// <summary>Calculates the probability that an agent is killed (rather than wounded) after receiving damage. Applies MCM settings, surgeon skill, perks, age factor, and siege multiplier. Tournaments are excluded.</summary>
        public override float GetAgentStateProbability(Agent affectorAgent, Agent effectedAgent, DamageTypes damageType, WeaponFlags weaponFlags, out float useSurgeryProbability)
        {
            float baseProbability = base.GetAgentStateProbability(affectorAgent, effectedAgent, damageType, weaponFlags, out useSurgeryProbability);

            if (effectedAgent == null) return baseProbability;

            bool isTournament = Mission.Current != null && Mission.Current.Mode == MissionMode.Tournament;

            if (!isTournament)
            {
                var settings = DeathIsHighlyLikelySettings.Instance;
                if (settings == null) return baseProbability;

                if (effectedAgent.IsHero && settings.EnableHeroDeathMechanics)
                {
                    PartyBase party = (effectedAgent.Origin as PartyAgentOrigin)?.Party;
                    PartyBase enemyParty = (affectorAgent?.Origin as PartyAgentOrigin)?.Party;

                    Hero surgeon = party?.MobileParty?.EffectiveSurgeon;
                    Hero enemySurgeon = enemyParty?.MobileParty?.EffectiveSurgeon;

                    int medicineSkill = 0;

                    if (surgeon != null)
                    {
                        medicineSkill = surgeon.GetSkillValue(DefaultSkills.Medicine);
                    }

                    if (enemySurgeon != null && enemySurgeon.GetPerkValue(DefaultPerks.Medicine.DoctorsOath))
                    {
                        int enemyMed = enemySurgeon.GetSkillValue(DefaultSkills.Medicine);
                        if (enemyMed > medicineSkill)
                        {
                            medicineSkill = enemyMed;
                        }
                    }

                    float customProbability = settings.HeroDeathProbability;
                    float multiplier = settings.LordVsLordMultiplier;

                    if (multiplier > 1.0f && affectorAgent != null && affectorAgent.IsHero)
                    {
                        customProbability *= multiplier;
                    }

                    float reduction = medicineSkill * 0.0004f;
                    customProbability -= reduction;

                    // YAŞ FAKTÖRÜ (v3.2.0)
                    if (settings.EnableAgeFactor && effectedAgent.Character is CharacterObject character)
                    {
                        Hero victimHero = character.HeroObject;
                        if (victimHero != null)
                        {
                            float ageMultiplier = 1.0f;
                            if (victimHero.Age >= 70f)
                                ageMultiplier = 1.5f;
                            else if (victimHero.Age >= 60f)
                                ageMultiplier = 1.2f;
                            else if (victimHero.Age >= 50f)
                                ageMultiplier = 1.1f;

                            // HealthAdvise perk'ü: Klan liderinde varsa çarpanları azalt
                            if (ageMultiplier > 1.0f && victimHero.Clan?.Leader != null
                                && victimHero.Clan.Leader.GetPerkValue(DefaultPerks.Medicine.HealthAdvise))
                            {
                                if (ageMultiplier == 1.5f)
                                    ageMultiplier = 1.35f;
                                else if (ageMultiplier == 1.2f)
                                    ageMultiplier = 1.1f;
                                else if (ageMultiplier == 1.1f)
                                    ageMultiplier = 1.05f;
                            }

                            customProbability *= ageMultiplier;
                        }
                    }

                    if (settings.EnableSiegeDeathRateIncrease && Mission.Current != null && Mission.Current.IsSiegeBattle)
                    {
                        customProbability *= 1.05f;
                    }

                    if (customProbability < 0f) customProbability = 0f;
                    if (customProbability > 1.0f) customProbability = 1.0f;

                    return customProbability;
                }
                else if (!effectedAgent.IsHero && settings.EnableTroopDeathMechanics)
                {
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

                    if (enemySurgeon != null && enemySurgeon.GetPerkValue(DefaultPerks.Medicine.DoctorsOath))
                    {
                        int enemyMed = enemySurgeon.GetSkillValue(DefaultSkills.Medicine);
                        if (enemyMed > medicineSkill)
                        {
                            medicineSkill = enemyMed;
                            hasMinisterOfHealth = enemySurgeon.GetPerkValue(DefaultPerks.Medicine.MinisterOfHealth);
                        }
                    }

                    float troopProbability = settings.TroopDeathProbability;
                    float reduction = medicineSkill * 0.001f;
                    troopProbability -= reduction;

                    if (troopProbability < 0f) troopProbability = 0f;

                    if (settings.EnableSiegeDeathRateIncrease && Mission.Current != null && Mission.Current.IsSiegeBattle)
                    {
                        troopProbability *= 1.15f;
                    }

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