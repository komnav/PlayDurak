using Durak.Entities;
using Durak.Service;

Game game = new Game();
IGameService gameService = new GameService(game);

if (!gameService.IsGameOver())
{
    gameService.Start(game.Player1, game.Player2);
    
    gameService.RazdachaCards(game.Deck);
    
    // Attack
    
    // 

    // if (currentAttacker == game.Player1)
    // {
    //     gameService.AttackerAction();
    // }
    //
    // else if (currentDefender == game.Player2)
    // {
    //     gameService.DefenderAction();
    // }
    //
    // else if (currentDefender == game.Player1)
    // {
    //     gameService.DefenderAction();
    // }
    //
    // else if (currentAttacker == game.Player2)
    // {
    //     gameService.AttackerAction();
    // }
}