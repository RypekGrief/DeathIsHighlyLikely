using TaleWorlds.CampaignSystem;
using TaleWorlds.Core;
using TaleWorlds.MountAndBlade;

namespace DeathIsHighlyLikely
{
    /// <summary>Entry point for the DeathIsHighlyLikely mod. Registers custom game models on campaign start.</summary>
    public class MySubModule : MBSubModuleBase
    {
        /// <summary>Registers DeathIsHighlyLikelyModel and MapSimulationDeathModel when a campaign game starts.</summary>
        protected override void OnGameStart(Game game, IGameStarter gameStarterObject)
        {
            base.OnGameStart(game, gameStarterObject);

            if (game.GameType is Campaign)
            {
                gameStarterObject.AddModel(new DeathIsHighlyLikelyModel());

                gameStarterObject.AddModel(new MapSimulationDeathModel());
            }
        }
    }
}