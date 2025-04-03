using Durak.Entities.Enum;

namespace Durak.Entities;

public class Game
{
    public List<Card> FieldCards { get; set; }
    public Player Player1 { get; set; }
    public Player Player2 { get; set; }
    public List<Card> Beat { get; set; }
    public Deck Deck { get; set; }
    
}