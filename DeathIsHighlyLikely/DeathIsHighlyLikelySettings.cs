using MCM.Abstractions.Attributes;
using MCM.Abstractions.Attributes.v2;
using MCM.Abstractions.Base.Global;

namespace DeathIsHighlyLikely
{
    /// <summary>MCM global settings for DeathIsHighlyLikely. Exposes hero death mechanics, troop death mechanics, age factor, Lord vs Lord multiplier, and siege death rate increase.</summary>
    public class DeathIsHighlyLikelySettings : AttributeGlobalSettings<DeathIsHighlyLikelySettings>
    {
        /// <summary>Unique identifier for this settings version.</summary>
        public override string Id => "DeathIsHighlyLikely_v3_3_0";

        /// <summary>Display name shown in the MCM menu.</summary>
        public override string DisplayName => "Death Is Highly Likely";

        /// <summary>Folder name for the settings JSON file.</summary>
        public override string FolderName => "DeathIsHighlyLikely";

        /// <summary>Serialization format type. JSON2 supports nested objects and arrays.</summary>
        public override string FormatType => "json2";

        /// <summary>Toggles custom death mechanics for heroes. When off, vanilla rules apply.</summary>
        [SettingPropertyBool("Enable Hero Death Mechanics", Order = 0, RequireRestart = false, HintText = "Turn on/off custom death mechanics for heroes. If off, reverts to Vanilla.")]
        [SettingPropertyGroup("Hero Settings", GroupOrder = 1)]
        public bool EnableHeroDeathMechanics { get; set; } = true;

        /// <summary>Base probability of heroes/lords dying in battle. Range: 0.0 to 1.0 (100%).</summary>
        [SettingPropertyFloatingInteger("Hero Death Probability", 0.0f, 1.0f, "0.00", Order = 1, RequireRestart = false, HintText = "Determines the base probability of heroes/lords dying in battle. (Set to 1.0 for 100%)")]
        [SettingPropertyGroup("Hero Settings", GroupOrder = 1)]
        public float HeroDeathProbability { get; set; } = 0.15f;

        /// <summary>Multiplies hero death chance when two heroes clash in 3D battles. Set to 1.0 to disable.</summary>
        [SettingPropertyFloatingInteger("Lord vs. Lord Multiplier", 1.0f, 5.0f, "0.0", Order = 2, RequireRestart = false, HintText = "Multiplies the death chance when two heroes clash. (Set to 1.0 to disable this feature)")]
        [SettingPropertyGroup("Hero Settings", GroupOrder = 1)]
        public float LordVsLordMultiplier { get; set; } = 1.5f;

        /// <summary>Toggles age factor. Older heroes (50+, 60+, 70+) have increased death probability.</summary>
        [SettingPropertyBool("Enable Age Factor", Order = 3, RequireRestart = false, HintText = "Older heroes have increased death probability. Health Advice perk on clan leader reduces this effect.")]
        [SettingPropertyGroup("Hero Settings", GroupOrder = 1)]
        public bool EnableAgeFactor { get; set; } = true;

        /// <summary>Toggles custom death mechanics for regular troops. When off, vanilla rules apply.</summary>
        [SettingPropertyBool("Enable Troop Death Mechanics", Order = 0, RequireRestart = false, HintText = "Turn on/off custom death mechanics for regular troops. If off, reverts to Vanilla.")]
        [SettingPropertyGroup("Troop Settings", GroupOrder = 2)]
        public bool EnableTroopDeathMechanics { get; set; } = true;

        /// <summary>Base probability of regular troops dying instead of being wounded. Range: 0.0 to 1.0 (100%).</summary>
        [SettingPropertyFloatingInteger("Troop Death Probability", 0.0f, 1.0f, "0.00", Order = 1, RequireRestart = false, HintText = "Determines the base probability of regular troops dying instead of being wounded. (Set to 1.0 for 100%)")]
        [SettingPropertyGroup("Troop Settings", GroupOrder = 2)]
        public float TroopDeathProbability { get; set; } = 0.60f;

        /// <summary>Toggles siege death rate increase. In wall assaults, hero death rate +5%, troop death rate +15%.</summary>
        [SettingPropertyBool("Enable Siege Death Rate Increase", Order = 0, RequireRestart = false, HintText = "Increases death probability during siege assaults. Hero +5%, Troop +15%.")]
        [SettingPropertyGroup("Siege Settings", GroupOrder = 3)]
        public bool EnableSiegeDeathRateIncrease { get; set; } = true;
    }
}