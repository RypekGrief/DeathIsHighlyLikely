using MCM.Abstractions.Attributes;
using MCM.Abstractions.Attributes.v2;
using MCM.Abstractions.Base.Global;

namespace DeathIsHighlyLikely
{
    public class DeathIsHighlyLikelySettings : AttributeGlobalSettings<DeathIsHighlyLikelySettings>
    {
        public override string Id => "DeathIsHighlyLikely_v1";
        public override string DisplayName => "Death Is Highly Likely";
        public override string FolderName => "DeathIsHighlyLikely";
        public override string FormatType => "json2";

        [SettingPropertyFloatingInteger("Probability of Death", 0.0f, 1.0f, "0.00", Order = 1, RequireRestart = false, HintText = "Determines the probability of heroes dying in battle. (Set to 1.0 for 100%)")]
        [SettingPropertyGroup("General Settings")]
        public float HeroDeathProbability { get; set; } = 0.20f;
    }
}
