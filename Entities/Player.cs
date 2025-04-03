using Durak.Entities.Enum;

namespace Durak.Entities;

public class Player
{
    public string Name { get; set; }

    public List<Card> Hand { get; set; }

    public ActionPlayer Action { get; set; }
    
}