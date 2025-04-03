using Durak.Entities;
 
 namespace Durak.Service;
 
 public interface IGameService
 {
     void Start(Player player1, Player player2);
     
     void RazdachaCards(Deck deck);
     
     bool IsGameOver();
     
     void Attack();
     
     void Defend();
     
 }