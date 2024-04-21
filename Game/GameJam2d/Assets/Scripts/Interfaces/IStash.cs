public interface IStash
{
    bool acceptCard(Card.Type t);
    public void addCard(Card.Type type);
    void cardClicked(Card.Type type);
    void changeCard(Card.Type type, Card.Type t);
    public void removeCard(Card.Type type);
}
