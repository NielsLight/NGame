using System;

public struct Vector2Int
{
    public int x;
    public int y;
    public Vector2Int(int x, int y)
    {
        this.x = x;
        this.y = y;
    }

    public override int GetHashCode()
    {
        return base.GetHashCode();
    }
    public override bool Equals(object obj)
    {
        if (obj == null)
            return false;
        Vector2Int b = (Vector2Int)obj;
        return x == b.x && y == b.y;
    }

    public string ToJson()
    {
        return LitJson.JsonMapper.ToJson(this);
    }
    public override string ToString()
    {
        return string.Format("x: {0} y:{1}",x,y);
    }

    public Vector2Int Abs()
    {
        return new Vector2Int(Math.Abs(x), Math.Abs(y));
    }

    public static Vector2Int Zero = new Vector2Int(0, 0);
    public  static Vector2Int operator-(Vector2Int a, Vector2Int b)
    {
        return new Vector2Int(a.x - b.x, a.y - b.y);
    }
    public static Vector2Int operator +(Vector2Int a, Vector2Int b)
    {
        return new Vector2Int(a.x + b.x, a.y + b.y);
    }
    public static bool operator ==(Vector2Int a, Vector2Int b)
    {
        return a.x == b.x&&a.y == b.y;
    }

    public static bool operator !=(Vector2Int a, Vector2Int b)
    {
        return !(a.x == b.x && a.y == b.y);
    }
}
