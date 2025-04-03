using Durak.Entities.Enum;

namespace Durak.Entities;

public class Deck
{
    List<Card> Kaloda = new List<Card>();

    public Card Kozer { get; set; }

    private Deck()
    {
        Kaloda = new List<Card>();
        
            Kaloda.Add(new Card { Suit = Suit.Club, Rank = Rank.Seven });
            Kaloda.Add(new Card { Suit = Suit.Club, Rank = Rank.Eight });
            Kaloda.Add(new Card { Suit = Suit.Club, Rank = Rank.Nine});
            Kaloda.Add(new Card { Suit = Suit.Club, Rank = Rank.Ten});
            Kaloda.Add(new Card { Suit = Suit.Club, Rank = Rank.Jack });
            Kaloda.Add(new Card { Suit = Suit.Club, Rank = Rank.Queen});
            Kaloda.Add(new Card { Suit = Suit.Club, Rank = Rank.King});
            Kaloda.Add(new Card { Suit = Suit.Club, Rank = Rank.Ace });
            
            Kaloda.Add(new Card { Suit = Suit.Diamond, Rank = Rank.Seven });
            Kaloda.Add(new Card { Suit = Suit.Diamond, Rank = Rank.Eight });
            Kaloda.Add(new Card { Suit = Suit.Diamond, Rank = Rank.Nine});
            Kaloda.Add(new Card { Suit = Suit.Diamond, Rank = Rank.Ten});
            Kaloda.Add(new Card { Suit = Suit.Diamond, Rank = Rank.Jack });
            Kaloda.Add(new Card { Suit = Suit.Diamond, Rank = Rank.Queen});
            Kaloda.Add(new Card { Suit = Suit.Diamond, Rank = Rank.King});
            Kaloda.Add(new Card { Suit = Suit.Diamond, Rank = Rank.Ace });
            
            Kaloda.Add(new Card { Suit = Suit.Heart, Rank = Rank.Seven });
            Kaloda.Add(new Card { Suit = Suit.Heart, Rank = Rank.Eight });
            Kaloda.Add(new Card { Suit = Suit.Heart, Rank = Rank.Nine});
            Kaloda.Add(new Card { Suit = Suit.Heart, Rank = Rank.Ten});
            Kaloda.Add(new Card { Suit = Suit.Heart, Rank = Rank.Jack });
            Kaloda.Add(new Card { Suit = Suit.Heart, Rank = Rank.Queen});
            Kaloda.Add(new Card { Suit = Suit.Heart, Rank = Rank.King});
            Kaloda.Add(new Card { Suit = Suit.Heart, Rank = Rank.Ace });
            
            Kaloda.Add(new Card { Suit = Suit.Spade, Rank = Rank.Seven });
            Kaloda.Add(new Card { Suit = Suit.Spade, Rank = Rank.Eight });
            Kaloda.Add(new Card { Suit = Suit.Spade, Rank = Rank.Nine});
            Kaloda.Add(new Card { Suit = Suit.Spade, Rank = Rank.Ten});
            Kaloda.Add(new Card { Suit = Suit.Spade, Rank = Rank.Jack });
            Kaloda.Add(new Card { Suit = Suit.Spade, Rank = Rank.Queen});
            Kaloda.Add(new Card { Suit = Suit.Spade, Rank = Rank.King});
            Kaloda.Add(new Card { Suit = Suit.Spade, Rank = Rank.Ace });

        if (Kaloda.Count > 0)
        {
            Kozer = Kaloda.First();

            Kaloda.RemoveAt(0);
        }
    }
}