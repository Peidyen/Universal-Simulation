public class Star
{
    public string Type { get; private set; }
    public double Mass { get; private set; }
    public double[] Position { get; private set; }

    public Star(string type, double mass, double[] position)
    {
        Type = type;
        Mass = mass;
        Position = position;
    }
}
