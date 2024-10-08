public class Tile
{
    private ITileblocker tileblocker;
    private readonly int xPosition;
    private readonly int yPosition;

    public bool IsBlocked { get; private set;}

    public Tile(int _xPosition, int _yPosition)
    {
        IsBlocked = false;
        tileblocker = null;
        xPosition = _xPosition;
        yPosition = _yPosition;
    }

    public Tile(ITileblocker _tileblocker, int _xPosition, int _yPosition) 
    {
        IsBlocked = true;
        tileblocker = _tileblocker;
        xPosition = _xPosition;
        yPosition = _yPosition;
    }

    public bool GetTileBlocker(out ITileblocker _tileblocker)
    {
        _tileblocker = tileblocker;
        return IsBlocked;
    }

    public bool Clear(out ITileblocker _tileblocker)
    {
        _tileblocker = tileblocker;
        bool result = IsBlocked;
        tileblocker = null;
        IsBlocked = false;
        return result;
    }

    public bool Set(out ITileblocker _tileblocker, ITileblocker newTileBlocker) 
    {
        _tileblocker = tileblocker;
        tileblocker = newTileBlocker;
        bool result = IsBlocked;
        IsBlocked = true;
        return result;
    }

    public override string ToString()
    {
        return "[" + xPosition + ", " + yPosition + "]";
    }


}
