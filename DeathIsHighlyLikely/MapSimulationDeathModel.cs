using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.GameComponents;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.Core;

namespace DeathIsHighlyLikely
{
    public class MapSimulationDeathModel : DefaultPartyHealingModel
    {
        public override float GetSurvivalChance(PartyBase party, CharacterObject character, DamageTypes damageType, bool canDamageKill, PartyBase enemyParty = null)
        {
            // "&& canDamageKill" şartını sildik.
            // Artık simülasyon bayılma bile dese, lordlar bizim ölüm testimize girecek.
            if (character != null && character.IsHero)
            {
                float deathProb = DeathIsHighlyLikelySettings.Instance?.HeroDeathProbability ?? 0.20f;
                return 1f - deathProb; // Ölüm ihtimalini yaşama ihtimaline çevirip oyuna veriyoruz.
            }

            return base.GetSurvivalChance(party, character, damageType, canDamageKill, enemyParty);
        }
    }
}