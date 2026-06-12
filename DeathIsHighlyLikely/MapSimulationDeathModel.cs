using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.GameComponents;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.CampaignSystem.CharacterDevelopment;
using TaleWorlds.Core;

namespace DeathIsHighlyLikely
{
    public class MapSimulationDeathModel : DefaultPartyHealingModel
    {
        public override float GetSurvivalChance(PartyBase party, CharacterObject character, DamageTypes damageType, bool canDamageKill, PartyBase enemyParty = null)
        {
            if (character != null)
            {
                var settings = DeathIsHighlyLikelySettings.Instance;
                if (settings == null) return base.GetSurvivalChance(party, character, damageType, canDamageKill, enemyParty);

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

                if (character.IsHero)
                {
                    float deathProb = settings.HeroDeathProbability;

                    float reduction = medicineSkill * 0.0004f;
                    deathProb -= reduction;

                    if (deathProb < 0f) deathProb = 0f;

                    return 1f - deathProb;
                }
                else
                {
                    float troopDeathProb = settings.TroopDeathProbability;

                    float reduction = medicineSkill * 0.001f;
                    troopDeathProb -= reduction;

                    if (troopDeathProb < 0f) troopDeathProb = 0f;

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