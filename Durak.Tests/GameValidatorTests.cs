using Durak.Entities;
using Durak.Entities.Enum;
using Durak.Exceptions;
using Durak.Requests;
using Durak.Service;
using FluentAssertions;

namespace Durak.Tests;

public class GameValidatorTests
{
    private GameValidator _gameValidator;
    private GameService _gameService;

    [SetUp]
    public void Setup()
    {
        _gameValidator = new GameValidator();
        _gameService = new GameService();
    }

    [Test]
    public void GameValidator_AttackerRequest_StatusGame_NotAttackTest()
    {
        // Arrange 
        var game = new Game
        {
            CurrentAction = GameAction.DefendAction
        };
        var request = new AttackerActionRequest
        {
            Action = AttackerActionType.Attack,
        };

        // Act, Assert
        var exception = Assert.Throws<PlayerInvalidRequestException>(
            () => _gameValidator.ValidateAttackerRequest(request, game));
        exception.Code.Should().Be(PlayerInvalidRequestExceptionCodes.GameActionNotValid);
    }

    [Test]
    public void GameValidator_AttackerRequest_CheckRequestCards_FromHandAttacker_ToAttackTest()
    {
        var player1 = new Player { Name = "Player 1", };
        var player2 = new Player { Name = "Player 2" };

        _gameService.Start(player1, player2);
        var game = _gameService.GetGame();
        game.Should().NotBeNull();

        var newCards = new Card
        {
            Rank = (Rank)1,
            Suit = (Suit)1
        };

        var requestNewAttacker = new AttackerActionRequest
        {
            Cards = [newCards],
            Action = AttackerActionType.Attack
        };

        var exception = Assert.Throws<PlayerInvalidRequestException>(
            () => _gameValidator.ValidateAttackerRequest(requestNewAttacker, game)
        );
        exception.Code.Should().Be(PlayerInvalidRequestExceptionCodes.CardsNotValid);
    }

    [Test]
    public void GameValidate_DefendingRequest_StatusGame_NotDefendingTest()
    {
        var game = new Game
        {
            CurrentAction = GameAction.AttackerAction
        };
        var request = new DefendingActionRequest
        {
            Action = DefendingActionType.Defend
        };

        var exception = Assert.Throws<PlayerInvalidRequestException>(
            () => _gameValidator.ValidateDefenderRequest(request, game));
        exception.Code.Should().Be(PlayerInvalidRequestExceptionCodes.GameActionNotValid);
    }

    [Test]
    public void GameValidator_DefendingRequest_CheckRequestCards_FromHandAttacker_ToDefendingTest()
    {
        var player1 = new Player { Name = "Player 1", };
        var player2 = new Player { Name = "Player 2" };

        _gameService.Start(player1, player2);
        var game = _gameService.GetGame();
        game.Should().NotBeNull();
        game.CurrentAction = GameAction.DefendAction;

        var newCards = new Card
        {
            Rank = (Rank)1,
            Suit = (Suit)1
        };

        var request = new DefendingActionRequest
        {
            Cards = [newCards],
            Action = DefendingActionType.Defend
        };

        var exception = Assert.Throws<PlayerInvalidRequestException>(
            () => _gameValidator.ValidateDefenderRequest(request, game)
        );
        exception.Code.Should().Be(PlayerInvalidRequestExceptionCodes.CardsNotValid);
    }

    [Test]
    public void GameValidator_CheckFields_IsNotNull()
    {
        var player1 = new Player { Name = "Player 1", };
        var player2 = new Player { Name = "Player 2" };

        _gameService.Start(player1, player2);
        var game = _gameService.GetGame();
        game.Should().NotBeNull();
        game.CurrentAction = GameAction.DefendAction;

        var requestToDefend = game.Defender.Hand[0];
        var request = new DefendingActionRequest
        {
            Cards = [requestToDefend],
            Action = DefendingActionType.Defend
        };
        var exception = Assert.Throws<PlayerInvalidRequestException>(
            () => _gameValidator.ValidateDefenderRequest(request, game)
        );
        exception.Code.Should().Be(PlayerInvalidRequestExceptionCodes.NotCardsInField);
    }

    [Test]
    public void GameValidator_DefendingRequest_CheckRequestCardsRank_ForValidToDefend()
    {
        var player1 = new Player { Name = "Player 1", };
        var player2 = new Player { Name = "Player 2" };

        _gameService.Start(player1, player2);
        var game = _gameService.GetGame();
        game.Should().NotBeNull();
        game.CurrentAction = GameAction.DefendAction;

        var newCards = new Card
        {
            Rank = (Rank)8,
            Suit = (Suit)8
        };
        game.FieldCards.Add(newCards);

        var requestToDefend = game.Defender.Hand[0];
        var requestDefending = new DefendingActionRequest
        {
            Cards = [requestToDefend],
            Action = DefendingActionType.Defend
        };
        var exception = Assert.Throws<PlayerInvalidRequestException>(
            () => _gameValidator.ValidateDefenderRequest(requestDefending, game)
        );
        exception.Code.Should().Be(PlayerInvalidRequestExceptionCodes.CardsIsNotValidToDefend);
    }

    [Test]
    public void GameValidator_DefendingRequest_CheckRequestCardsSuitAndTrump_ForValidToDefend()
    {
        var player1 = new Player { Name = "Player 1", };
        var player2 = new Player { Name = "Player 2" };

        _gameService.Start(player1, player2);
        var game = _gameService.GetGame();
        game.Should().NotBeNull();
        game.CurrentAction = GameAction.DefendAction;

        var newCards = new Card
        {
            Rank = (Rank)3,
            Suit = (Suit)4
        };
        game.FieldCards.Add(newCards);

        var requestToDefend = game.Defender.Hand[0];
        var requestDefending = new DefendingActionRequest
        {
            Cards = [requestToDefend],
            Action = DefendingActionType.Defend
        };
        var exception = Assert.Throws<PlayerInvalidRequestException>(
            () => _gameValidator.ValidateDefenderRequest(requestDefending, game)
        );
        exception.Code.Should().Be(PlayerInvalidRequestExceptionCodes.CardsIsNotValidToDefend);
    }
}