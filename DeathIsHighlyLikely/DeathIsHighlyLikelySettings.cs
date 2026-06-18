// DeathIsHighlyLikelySettings.cs
using MCM.Abstractions.Attributes;
using MCM.Abstractions.Attributes.v2;
using MCM.Abstractions.Base.Global;

namespace DeathIsHighlyLikely
{
    public class DeathIsHighlyLikelySettings : AttributeGlobalSettings<DeathIsHighlyLikelySettings>
    {
        public override string Id => "DeathIsHighlyLikely_v3_2_0";
        public override string DisplayName => "Death Is Highly Likely";
        public override string FolderName => "DeathIsHighlyLikely";
        public override string FormatType => "json2";

        [SettingPropertyBool("Enable Hero Death Mechanics", Order = 0, RequireRestart = false, HintText = "Turn on/off custom death mechanics for heroes. If off, reverts to Vanilla.")]
        [SettingPropertyGroup("Hero Settings", GroupOrder = 1)]
        public bool EnableHeroDeathMechanics { get; set; } = true;

        [SettingPropertyFloatingInteger("Hero Death Probability", 0.0f, 1.0f, "0.00", Order = 1, RequireRestart = false, HintText = "Determines the base probability of heroes/lords dying in battle. (Set to 1.0 for 100%)")]
        [SettingPropertyGroup("Hero Settings", GroupOrder = 1)]
        public float HeroDeathProbability { get; set; } = 0.15f;

        [SettingPropertyFloatingInteger("Lord vs. Lord Multiplier", 1.0f, 5.0f, "0.0", Order = 2, RequireRestart = false, HintText = "Multiplies the death chance when two heroes clash. (Set to 1.0 to disable this feature)")]
        [SettingPropertyGroup("Hero Settings", GroupOrder = 1)]
        public float LordVsLordMultiplier { get; set; } = 1.5f;

        [SettingPropertyBool("Enable Age Factor", Order = 3, RequireRestart = false, HintText = "Older heroes have increased death probability. Health Advice perk on clan leader reduces this effect.")]
        [SettingPropertyGroup("Hero Settings", GroupOrder = 1)]
        public bool EnableAgeFactor { get; set; } = true;

        [SettingPropertyBool("Enable Troop Death Mechanics", Order = 0, RequireRestart = false, HintText = "Turn on/off custom death mechanics for regular troops. If off, reverts to Vanilla.")]
        [SettingPropertyGroup("Troop Settings", GroupOrder = 2)]
        public bool EnableTroopDeathMechanics { get; set; } = true;

        [SettingPropertyFloatingInteger("Troop Death Probability", 0.0f, 1.0f, "0.00", Order = 1, RequireRestart = false, HintText = "Determines the base probability of regular troops dying instead of being wounded. (Set to 1.0 for 100%)")]
        [SettingPropertyGroup("Troop Settings", GroupOrder = 2)]
        public float TroopDeathProbability { get; set; } = 0.60f;
    }
}