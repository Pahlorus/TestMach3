
namespace MatchCore
{
    public enum SwitchType { On, Off}

    public enum GameState { Menu = 1, Game = 2, GameOver = 3}

    public enum State
    {
        Init = 0,
        Wait = 1,
        Change = 2,
        Matching = 3
    }

    public enum RangeType { Hor, Vert }

    public enum BitType
    {
        Circle = 0,
        Triangle = 1,
        Square = 2,
        Rhombus = 3,
        Cross = 4
    }

    public enum ModifiType
    {
        None = 0,
        HLine = 1,
        VLine = 2,
        Bomb = 3,
    }
    public enum DestroyerType
    {
        Top = 0,
        Bottom = 1,
        Right = 2,
        Left = 3,
    }
}