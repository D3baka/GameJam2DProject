public interface IStash
{
    public void addCard(Card.Type type);
    void changeCard(Card.Type type, Card.Type t);
    public void removeCard(Card.Type type);
}
