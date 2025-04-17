using Durak.Entities;
using Durak.Entities.Enum;
using Durak.Requests;
using Durak.Service;
using FluentAssertions;

namespace Durak.Tests;

public class DurakTests
{
    private GameService _gameService;

    public DurakTests()
    {
        _gameService = new GameService();
    }

    [Test]
    public void Durak_BeatScenario_E2E_Test()
    {
        var player1 = new Player { Name = "Player 1", };
        var player2 = new Player { Name = "Player 2" };

        _gameService.Start(player1, player2);

        var game = _gameService.GetGame();
        game.Should().NotBeNull();

        var gamePlayer1 = game.Player1;
        var gamePlayer2 = game.Player2;

        gamePlayer1.Player.Should().Be(player1);
        gamePlayer2.Player.Should().Be(player2);

        gamePlayer1.Hand.Count.Should().Be(6);
        gamePlayer2.Hand.Count.Should().Be(6);

        game.Deck.Should().NotBeNull();
        game.Deck.Cards.Count.Should().Be(24);
        game.CurrentAction.Should().Be(GameAction.AttackerAction);
        game.Attacker.Should().NotBeNull();
        game.Defender.Should().NotBeNull();

        var cardToAttack = gamePlayer1.Hand[0];

        var previousAttacker = game.Attacker;
        var previousDefender = game.Defender;

        var request = new AttackerActionRequest()
        {
            Action = AttackerActionType.Attack,
            Cards = [cardToAttack]
        };
        _gameService.AttackerAction(request);

        gamePlayer1.Hand.Count.Should().Be(5);
        gamePlayer2.Hand.Count.Should().Be(6);
        game.Deck.Cards.Count.Should().Be(24);
        game.FieldCards.Count.Should().Be(1);
        game.Attacker.Should().Be(previousAttacker);
        game.Defender.Should().Be(previousDefender);
        game.FieldCards
            .FirstOrDefault
            (x => x.Rank == cardToAttack.Rank
                  && x.Suit == cardToAttack.Suit)
            .Should().NotBeNull();

        //Defend
        game.Attacker.Should().Be(previousAttacker);
        game.Defender.Should().Be(previousDefender);
        game.CurrentAction.Should().Be(GameAction.DefendAction);
        game.Attacker.Should().NotBeNull();
        game.Defender.Should().NotBeNull();

        var cardToDefend = gamePlayer2.Hand[0];

        var requestDefending = new DefendingActionRequest
        {
            Action = DefendingActionType.Defend,
            Cards = [cardToDefend]
        };
        _gameService.DefenderAction(requestDefending);

        gamePlayer1.Hand.Count.Should().Be(5);
        gamePlayer2.Hand.Count.Should().Be(5);
        game.Deck.Cards.Count.Should().Be(24);
        game.FieldCards.Count.Should().Be(2);
        game.Attacker.Should().Be(previousAttacker);
        game.Defender.Should().Be(previousDefender);
        game.CurrentAction.Should().Be(GameAction.AttackerAction);
        game.FieldCards
            .FirstOrDefault
            (x => x.Rank == cardToDefend.Rank
                  && x.Suit == cardToDefend.Suit)
            .Should().NotBeNull();

        //Attack
        var cardToAttack2 = gamePlayer1.Hand[0];

        var requestAttacker = new AttackerActionRequest
        {
            Action = AttackerActionType.Attack,
            Cards = [cardToAttack2]
        };

        _gameService.AttackerAction(requestAttacker);

        gamePlayer1.Hand.Count.Should().Be(4);
        gamePlayer2.Hand.Count.Should().Be(5);
        game.Deck.Cards.Count.Should().Be(24);
        game.FieldCards.Count.Should().Be(3);
        game.Attacker.Should().Be(previousAttacker);
        game.Defender.Should().Be(previousDefender);
        game.CurrentAction.Should().Be(GameAction.DefendAction);
        game.FieldCards
            .FirstOrDefault
            (x => x.Rank == cardToAttack2.Rank
                  && x.Suit == cardToAttack2.Suit)
            .Should().NotBeNull();

        //Defend
        var cardToDefend2 = gamePlayer2.Hand[0];

        var requestDefend = new DefendingActionRequest
        {
            Cards = [cardToDefend2],
            Action = DefendingActionType.Defend
        };

        _gameService.DefenderAction(requestDefend);

        gamePlayer1.Hand.Count.Should().Be(4);
        gamePlayer2.Hand.Count.Should().Be(4);
        game.Deck.Cards.Count.Should().Be(24);
        game.FieldCards.Count.Should().Be(4);
        game.Attacker.Should().Be(previousAttacker);
        game.Defender.Should().Be(previousDefender);
        game.CurrentAction.Should().Be(GameAction.AttackerAction);
        game.FieldCards
            .FirstOrDefault
            (x => x.Rank == cardToDefend2.Rank
                  && x.Suit == cardToDefend2.Suit)
            .Should().NotBeNull();

        //Attack

        var requestAttacker2 = new AttackerActionRequest
        {
            Cards = [],
            Action = AttackerActionType.Beat
        };

        _gameService.AttackerAction(requestAttacker2);

        game.FieldCards.Count.Should().Be(0);
        game.Attacker.Should().Be(previousDefender);
        game.Defender.Should().Be(previousAttacker);
        gamePlayer1.Hand.Count.Should().Be(6);
        gamePlayer2.Hand.Count.Should().Be(6);
        game.Deck.Cards.Count.Should().Be(20);
        game.CurrentAction.Should().Be(GameAction.AttackerAction);


        // Attack 

        var cardToAttacker3 = game.Attacker.Hand[0];
        var cardToAttacker4 = game.Attacker.Hand[1];

        var requestNewAttacker = new AttackerActionRequest
        {
            Cards = [cardToAttacker3, cardToAttacker4],
            Action = AttackerActionType.Attack
        };

        _gameService.AttackerAction(requestNewAttacker);

        game.FieldCards.Count.Should().Be(2);
        game.CurrentAction.Should().Be(GameAction.DefendAction);
        game.Attacker.Hand.Count.Should().Be(4);
        game.Defender.Hand.Count.Should().Be(6);
        game.Deck.Cards.Count.Should().Be(20);
        game.Attacker.Should().Be(previousDefender);
        game.Defender.Should().Be(previousAttacker);
        game.FieldCards
            .FirstOrDefault
            (x => x.Rank == cardToAttacker3.Rank
                  && x.Suit == cardToAttacker3.Suit)
            .Should().NotBeNull();
        game.FieldCards
            .FirstOrDefault
            (x => x.Rank == cardToAttacker4.Rank
                  && x.Suit == cardToAttacker4.Suit)
            .Should().NotBeNull();


        //Defend
        var cardToDefend3 = game.Defender.Hand[0];
        var cardToDefend4 = game.Defender.Hand[1];

        var requestNewtDefend = new DefendingActionRequest
        {
            Cards = [cardToDefend3, cardToDefend4],
            Action = DefendingActionType.Defend
        };

        _gameService.DefenderAction(requestNewtDefend);

        game.Attacker.Hand.Count.Should().Be(4);
        game.Defender.Hand.Count.Should().Be(4);
        game.Deck.Cards.Count.Should().Be(20);
        game.FieldCards.Count.Should().Be(4);
        game.Attacker.Should().Be(previousDefender);
        game.Defender.Should().Be(previousAttacker);
        game.CurrentAction.Should().Be(GameAction.AttackerAction);
        game.FieldCards
            .FirstOrDefault
            (x => x.Rank == cardToDefend3.Rank
                  && x.Suit == cardToDefend3.Suit)
            .Should().NotBeNull();
        game.FieldCards
            .FirstOrDefault
            (x => x.Rank == cardToDefend4.Rank
                  && x.Suit == cardToDefend4.Suit)
            .Should().NotBeNull();


        // Attack 
        var cardToAttacker5 = game.Attacker.Hand[0];
        var cardToAttacker6 = game.Attacker.Hand[1];

        var requestNewAttacker2 = new AttackerActionRequest
        {
            Cards = [cardToAttacker5, cardToAttacker6],
            Action = AttackerActionType.Attack
        };

        _gameService.AttackerAction(requestNewAttacker2);

        game.FieldCards.Count.Should().Be(6);
        game.CurrentAction.Should().Be(GameAction.DefendAction);
        game.Attacker.Hand.Count.Should().Be(2);
        game.Defender.Hand.Count.Should().Be(4);
        game.Deck.Cards.Count.Should().Be(20);
        game.Attacker.Should().Be(previousDefender);
        game.Defender.Should().Be(previousAttacker);
        game.FieldCards
            .FirstOrDefault
            (x => x.Rank == cardToAttacker5.Rank
                  && x.Suit == cardToAttacker5.Suit)
            .Should().NotBeNull();
        game.FieldCards
            .FirstOrDefault
            (x => x.Rank == cardToAttacker6.Rank
                  && x.Suit == cardToAttacker6.Suit)
            .Should().NotBeNull();


        //Defend
        var cardToDefend5 = game.Defender.Hand[0];
        var cardToDefend6 = game.Defender.Hand[1];

        var requestNewtDefend2 = new DefendingActionRequest
        {
            Cards = [cardToDefend5, cardToDefend6],
            Action = DefendingActionType.Defend
        };

        _gameService.DefenderAction(requestNewtDefend2);

        game.Attacker.Hand.Count.Should().Be(2);
        game.Defender.Hand.Count.Should().Be(2);
        game.Deck.Cards.Count.Should().Be(20);
        game.FieldCards.Count.Should().Be(8);
        game.Attacker.Should().Be(previousDefender);
        game.Defender.Should().Be(previousAttacker);
        game.CurrentAction.Should().Be(GameAction.AttackerAction);
        game.FieldCards
            .FirstOrDefault
            (x => x.Rank == cardToDefend5.Rank
                  && x.Suit == cardToDefend5.Suit)
            .Should().NotBeNull();
        game.FieldCards
            .FirstOrDefault
            (x => x.Rank == cardToDefend6.Rank
                  && x.Suit == cardToDefend6.Suit)
            .Should().NotBeNull();


        // Attack 
        var cardToAttacker7 = game.Attacker.Hand[0];
        var cardToAttacker8 = game.Attacker.Hand[1];

        var requestNewAttacker3 = new AttackerActionRequest
        {
            Cards = [cardToAttacker7, cardToAttacker8],
            Action = AttackerActionType.Attack
        };

        _gameService.AttackerAction(requestNewAttacker3);

        game.FieldCards.Count.Should().Be(10);
        game.CurrentAction.Should().Be(GameAction.DefendAction);
        game.Attacker.Hand.Count.Should().Be(0);
        game.Defender.Hand.Count.Should().Be(2);
        game.Deck.Cards.Count.Should().Be(20);
        game.Attacker.Should().Be(previousDefender);
        game.Defender.Should().Be(previousAttacker);
        game.FieldCards
            .FirstOrDefault
            (x => x.Rank == cardToAttacker7.Rank
                  && x.Suit == cardToAttacker7.Suit)
            .Should().NotBeNull();
        game.FieldCards
            .FirstOrDefault
            (x => x.Rank == cardToAttacker8.Rank
                  && x.Suit == cardToAttacker8.Suit)
            .Should().NotBeNull();


        //Defend
        var cardToDefend7 = game.Defender.Hand[0];
        var cardToDefend8 = game.Defender.Hand[1];

        var requestNewtDefend3 = new DefendingActionRequest
        {
            Cards = [cardToDefend7, cardToDefend8],
            Action = DefendingActionType.Defend
        };

        _gameService.DefenderAction(requestNewtDefend3);

        game.Attacker.Hand.Count.Should().Be(0);
        game.Defender.Hand.Count.Should().Be(0);
        game.Deck.Cards.Count.Should().Be(20);
        game.FieldCards.Count.Should().Be(12);
        game.Attacker.Should().Be(previousDefender);
        game.Defender.Should().Be(previousAttacker);
        game.CurrentAction.Should().Be(GameAction.AttackerAction);
        game.FieldCards
            .FirstOrDefault
            (x => x.Rank == cardToDefend7.Rank
                  && x.Suit == cardToDefend7.Suit)
            .Should().NotBeNull();
        game.FieldCards
            .FirstOrDefault
            (x => x.Rank == cardToDefend8.Rank
                  && x.Suit == cardToDefend8.Suit)
            .Should().NotBeNull();


        //Attack

        var requestAttacker4 = new AttackerActionRequest
        {
            Cards = [],
            Action = AttackerActionType.Beat
        };

        _gameService.AttackerAction(requestAttacker4);

        game.FieldCards.Count.Should().Be(0);
        game.Attacker.Should().Be(previousAttacker);
        game.Defender.Should().Be(previousDefender);
        gamePlayer1.Hand.Count.Should().Be(6);
        gamePlayer2.Hand.Count.Should().Be(6);
        game.Deck.Cards.Count.Should().Be(8);
        game.CurrentAction.Should().Be(GameAction.AttackerAction);


        // Attack 
        var cardToAttacker9 = game.Attacker.Hand[0];
        var cardToAttacker10 = game.Attacker.Hand[1];
        var cardToAttacker11 = game.Attacker.Hand[2];
        var cardToAttacker12 = game.Attacker.Hand[3];

        var requestNewAttacker4 = new AttackerActionRequest
        {
            Cards = [cardToAttacker9, cardToAttacker10, cardToAttacker11, cardToAttacker12],
            Action = AttackerActionType.Attack
        };

        _gameService.AttackerAction(requestNewAttacker4);

        game.FieldCards.Count.Should().Be(4);
        game.CurrentAction.Should().Be(GameAction.DefendAction);
        game.Attacker.Hand.Count.Should().Be(2);
        game.Defender.Hand.Count.Should().Be(6);
        game.Deck.Cards.Count.Should().Be(8);
        game.Attacker.Should().Be(previousAttacker);
        game.Defender.Should().Be(previousDefender);
        game.FieldCards
            .FirstOrDefault
            (x => x.Rank == cardToAttacker9.Rank
                  && x.Suit == cardToAttacker9.Suit)
            .Should().NotBeNull();
        game.FieldCards
            .FirstOrDefault
            (x => x.Rank == cardToAttacker10.Rank
                  && x.Suit == cardToAttacker10.Suit)
            .Should().NotBeNull();
        game.FieldCards
            .FirstOrDefault
            (x => x.Rank == cardToAttacker11.Rank
                  && x.Suit == cardToAttacker11.Suit)
            .Should().NotBeNull();
        game.FieldCards
            .FirstOrDefault
            (x => x.Rank == cardToAttacker12.Rank
                  && x.Suit == cardToAttacker12.Suit)
            .Should().NotBeNull();


        //Defend
        var requestNewtDefend4 = new DefendingActionRequest
        {
            Cards = [],
            Action = DefendingActionType.Take
        };

        _gameService.DefenderAction(requestNewtDefend4);

        game.Attacker.Hand.Count.Should().Be(6);
        game.Defender.Hand.Count.Should().Be(10);
        game.Deck.Cards.Count.Should().Be(4);
        game.FieldCards.Count.Should().Be(0);
        game.Attacker.Should().Be(previousAttacker);
        game.Defender.Should().Be(previousDefender);
        game.CurrentAction.Should().Be(GameAction.AttackerAction);

        // Attack 
        var cardToAttacker13 = game.Attacker.Hand[0];
        var cardToAttacker14 = game.Attacker.Hand[1];
        var cardToAttacker15 = game.Attacker.Hand[2];
        var cardToAttacker16 = game.Attacker.Hand[3];

        var requestNewAttacker5 = new AttackerActionRequest
        {
            Cards = [cardToAttacker13, cardToAttacker14, cardToAttacker15, cardToAttacker16],
            Action = AttackerActionType.Attack
        };

        _gameService.AttackerAction(requestNewAttacker5);

        game.FieldCards.Count.Should().Be(4);
        game.CurrentAction.Should().Be(GameAction.DefendAction);
        game.Attacker.Hand.Count.Should().Be(2);
        game.Defender.Hand.Count.Should().Be(10);
        game.Deck.Cards.Count.Should().Be(4);
        game.Attacker.Should().Be(previousAttacker);
        game.Defender.Should().Be(previousDefender);
        game.FieldCards
            .FirstOrDefault
            (x => x.Rank == cardToAttacker13.Rank
                  && x.Suit == cardToAttacker13.Suit)
            .Should().NotBeNull();
        game.FieldCards
            .FirstOrDefault
            (x => x.Rank == cardToAttacker14.Rank
                  && x.Suit == cardToAttacker14.Suit)
            .Should().NotBeNull();
        game.FieldCards
            .FirstOrDefault
            (x => x.Rank == cardToAttacker15.Rank
                  && x.Suit == cardToAttacker15.Suit)
            .Should().NotBeNull();
        game.FieldCards
            .FirstOrDefault
            (x => x.Rank == cardToAttacker16.Rank
                  && x.Suit == cardToAttacker16.Suit)
            .Should().NotBeNull();


        //Defend
        var cardToDefend9 = game.Defender.Hand[0];
        var cardToDefend10 = game.Defender.Hand[1];
        var cardToDefend11 = game.Defender.Hand[2];
        var cardToDefend12 = game.Defender.Hand[3];

        var requestNewtDefend5 = new DefendingActionRequest
        {
            Cards = [cardToDefend9, cardToDefend10, cardToDefend11, cardToDefend12],
            Action = DefendingActionType.Defend
        };

        _gameService.DefenderAction(requestNewtDefend5);

        game.Attacker.Hand.Count.Should().Be(2);
        game.Defender.Hand.Count.Should().Be(6);
        game.Deck.Cards.Count.Should().Be(4);
        game.FieldCards.Count.Should().Be(8);
        game.Attacker.Should().Be(previousAttacker);
        game.Defender.Should().Be(previousDefender);
        game.CurrentAction.Should().Be(GameAction.AttackerAction);
        game.FieldCards
            .FirstOrDefault
            (x => x.Rank == cardToDefend9.Rank
                  && x.Suit == cardToDefend9.Suit)
            .Should().NotBeNull();
        game.FieldCards
            .FirstOrDefault
            (x => x.Rank == cardToDefend10.Rank
                  && x.Suit == cardToDefend10.Suit)
            .Should().NotBeNull();
        game.FieldCards
            .FirstOrDefault
            (x => x.Rank == cardToDefend11.Rank
                  && x.Suit == cardToDefend11.Suit)
            .Should().NotBeNull();
        game.FieldCards
            .FirstOrDefault
            (x => x.Rank == cardToDefend12.Rank
                  && x.Suit == cardToDefend12.Suit)
            .Should().NotBeNull();

        //Attack
        var requestAttacker5 = new AttackerActionRequest
        {
            Cards = [],
            Action = AttackerActionType.Beat
        };

        _gameService.AttackerAction(requestAttacker5);

        game.FieldCards.Count.Should().Be(0);
        game.Attacker.Should().Be(previousDefender);
        game.Defender.Should().Be(previousAttacker);
        gamePlayer1.Hand.Count.Should().Be(6);
        gamePlayer2.Hand.Count.Should().Be(6);
        game.Deck.Cards.Count.Should().Be(0);
        game.CurrentAction.Should().Be(GameAction.AttackerAction);


        //Attack
        var cardToAttacker17 = game.Attacker.Hand[0];
        var cardToAttacker18 = game.Attacker.Hand[1];
        var cardToAttacker19 = game.Attacker.Hand[2];
        var cardToAttacker20 = game.Attacker.Hand[3];
        var cardToAttacker21 = game.Attacker.Hand[4];
        var cardToAttacker22 = game.Attacker.Hand[5];

        var requestNewAttacker6 = new AttackerActionRequest
        {
            Cards =
            [
                cardToAttacker17,
                cardToAttacker18,
                cardToAttacker19,
                cardToAttacker20,
                cardToAttacker21,
                cardToAttacker22
            ],
            Action = AttackerActionType.Attack
        };

        _gameService.AttackerAction(requestNewAttacker6);

        game.FieldCards.Count.Should().Be(6);
        game.CurrentAction.Should().Be(GameAction.DefendAction);
        game.Attacker.Hand.Count.Should().Be(0);
        game.Defender.Hand.Count.Should().Be(6);
        game.Deck.Cards.Count.Should().Be(0);
        game.Attacker.Should().Be(previousDefender);
        game.Defender.Should().Be(previousAttacker);
        game.FieldCards
            .FirstOrDefault
            (x => x.Rank == cardToAttacker17.Rank
                  && x.Suit == cardToAttacker17.Suit)
            .Should().NotBeNull();
        game.FieldCards
            .FirstOrDefault
            (x => x.Rank == cardToAttacker18.Rank
                  && x.Suit == cardToAttacker18.Suit)
            .Should().NotBeNull();
        game.FieldCards
            .FirstOrDefault
            (x => x.Rank == cardToAttacker19.Rank
                  && x.Suit == cardToAttacker19.Suit)
            .Should().NotBeNull();
        game.FieldCards
            .FirstOrDefault
            (x => x.Rank == cardToAttacker20.Rank
                  && x.Suit == cardToAttacker20.Suit)
            .Should().NotBeNull();
        game.FieldCards
            .FirstOrDefault
            (x => x.Rank == cardToAttacker21.Rank
                  && x.Suit == cardToAttacker21.Suit)
            .Should().NotBeNull();
        game.FieldCards
            .FirstOrDefault
            (x => x.Rank == cardToAttacker22.Rank
                  && x.Suit == cardToAttacker22.Suit)
            .Should().NotBeNull();


        //Defend

        var requestNewtDefend6 = new DefendingActionRequest
        {
            Cards = [],
            Action = DefendingActionType.Take
        };

        _gameService.DefenderAction(requestNewtDefend6);

        game.Attacker.Hand.Count.Should().Be(0);
        game.Defender.Hand.Count.Should().Be(12);
        game.Deck.Cards.Count.Should().Be(0);
        game.FieldCards.Count.Should().Be(0);
        game.Attacker.Should().Be(previousDefender);
        game.Defender.Should().Be(previousAttacker);
        game.CurrentAction.Should().Be(GameAction.AttackerAction);

        //Attack
        var requestAttacker7 = new AttackerActionRequest
        {
            Cards = [],
            Action = AttackerActionType.Beat
        };

        _gameService.AttackerAction(requestAttacker7);

        game.FieldCards.Count.Should().Be(0);
        game.Attacker.Should().Be(previousAttacker);
        game.Defender.Should().Be(previousDefender);
        game.Deck.Cards.Count.Should().Be(0);
        game.CurrentAction.Should().Be(GameAction.AttackerAction);
        _gameService.IsGameOver().Should().BeTrue();
        _gameService.Winner().Should().Be(game.Player2.Player);
    }
}