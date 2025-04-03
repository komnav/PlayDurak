using Durak.Entities;
using Durak.Entities.Enum;

namespace Durak.Service;

public class GameService(Game game) : IGameService
{
    private Game _game = game;

    public void Start(Player player1, Player player2)
    {
        player1.Name = "Player 1";
        player2.Name = "Player 2";
    }

    public void RazdachaCards(Deck deck)
    {
        for (int i = 0; i <= 6; i++)
        {
            _game.Player1.Hand.Add(deck.Kaloda.First());
        }

        for (int i = 0; i <= 6; i++)
        {
            _game.Player2.Hand.AddRange(deck.Kaloda);
        }
    }

    public bool IsGameOver()
    {
        if (_game.Player1.Hand.Count == 0 || _game.Player2.Hand.Count == 0)
        {
            return true;
        }

        return false;
    }

    //specifyAttcker

    public void Attack(Player player)
    {
        var firstAttacker = TryGetFirstAttacker();

        if (firstAttacker != null && firstAttacker.Name != player.Name)
        {
            throw new Exception($"Incorrect player is moving. {firstAttacker.Name} should move 1st.");
        }

        if (player.Action != ActionPlayer.Attacker)
        {
            throw new Exception("Incorrect player is attacking ");   
        }
    }

    public Player? TryGetFirstAttacker()
    {
        if (_game.FieldCards.Count == 0 && _game.Player1.Hand.Count == 6 && _game.Player2.Hand.Count == 6 &&
            _game.Beat.Count == 0)
        {
            var kozer = _game.Deck.Kozer;

            var smallKozerPlayer1 = _game.Player1.Hand.Where(x => x.Suit == kozer.Suit).Min(x => x.Rank);

            var smallKozerPlayer2 = _game.Player2.Hand.Where(x => x.Suit == kozer.Suit).Min(x => x.Rank);


            return smallKozerPlayer1 > smallKozerPlayer2 ? _game.Player2 : _game.Player1;
        }

        return null;
    }

    public void Defend()
    {
        if (game.Player1.Action == ActionPlayer.Defender)
        {
            var firstDefend = _game.Player1.Hand.First();

            _game.FieldCards.Add(firstDefend);

            _game.Player1.Hand.Remove(firstDefend);

            var changeAction = _game.Player1.Action == ActionPlayer.Attacker;
        }

        else
        {
            var firstDefend = _game.Player2.Hand.First();

            _game.FieldCards.Add(firstDefend);

            _game.Player2.Hand.Remove(firstDefend);

            var changeAction = _game.Player2.Action == ActionPlayer.Attacker;
        }
    }
}