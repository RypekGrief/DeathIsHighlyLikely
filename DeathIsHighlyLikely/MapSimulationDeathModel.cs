using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.GameComponents;
using TaleWorlds.CampaignSystem.MapEvents;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.CampaignSystem.CharacterDevelopment;
using TaleWorlds.Core;

namespace DeathIsHighlyLikely
{
    /// <summary>Overrides DefaultPartyHealingModel to inject custom hero and troop survival chance calculations for campaign map simulations (AI vs AI, Send Troops).</summary>
    public class MapSimulationDeathModel : DefaultPartyHealingModel
    {
        /// <summary>Calculates the chance a character survives a simulation battle. Applies MCM settings, surgeon skill, perks, and siege multiplier. Returns survival chance (1.0 minus death probability).</summary>
        public override float GetSurvivalChance(PartyBase party, CharacterObject character, DamageTypes damageType, bool canDamageKill, PartyBase enemyParty = null)
        {
            if (character != null)
            {
                var settings = DeathIsHighlyLikelySettings.Instance;
                if (settings == null) return base.GetSurvivalChance(party, character, damageType, canDamageKill, enemyParty);

                if (character.IsHero && settings.EnableHeroDeathMechanics)
                {
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

                    float deathProb = settings.HeroDeathProbability;
                    float reduction = medicineSkill * 0.0004f;
                    deathProb -= reduction;

                    if (settings.EnableSiegeDeathRateIncrease && party?.MapEvent?.IsSiegeAssault == true)
                    {
                        deathProb *= 1.05f;
                    }

                    if (deathProb < 0f) deathProb = 0f;
                    return 1f - deathProb;
                }
                else if (!character.IsHero && settings.EnableTroopDeathMechanics)
                {
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

                    float troopDeathProb = settings.TroopDeathProbability;
                    float reduction = medicineSkill * 0.001f;
                    troopDeathProb -= reduction;

                    if (troopDeathProb < 0f) troopDeathProb = 0f;

                    if (settings.EnableSiegeDeathRateIncrease && party?.MapEvent?.IsSiegeAssault == true)
                    {
                        troopDeathProb *= 1.15f;
                    }

                    if (hasMinisterOfHealth)
                    {
                        troopDeathProb *= 0.45f;
                    }

                    return 1f - troopDeathProb;
                }
            }

            return base.GetSurvivalChance(party, character, damageType, canDamageKill, enemyParty);
        }
    }
}