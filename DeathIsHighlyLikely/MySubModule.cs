using TaleWorlds.CampaignSystem;
using TaleWorlds.Core;
using TaleWorlds.MountAndBlade;

namespace DeathIsHighlyLikely
{
    public class MySubModule : MBSubModuleBase
    {
        protected override void OnGameStart(Game game, IGameStarter gameStarterObject)
        {
            base.OnGameStart(game, gameStarterObject);

            if (game.GameType is Campaign)
            {
                // 1. Senin kusursuz çalışan 3D savaş modelin
                gameStarterObject.AddModel(new DeathIsHighlyLikelyModel());

                // 2. Sadece harita simülasyonlarında çalışacak olan yeni modelimiz
                gameStarterObject.AddModel(new MapSimulationDeathModel());
            }
        }
    }
}