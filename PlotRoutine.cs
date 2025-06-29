using System.Drawing;

public static class GalaxyVisualizer
{
    public static void PlotGalaxyPositions(List<Galaxy> galaxies, string title = "Galaxy Positions")
    {
        var plt = new ScottPlot.Plot(800, 600);

        // Define buckets
        var lowMass = new List<(double x, double y)>();
        var midMass = new List<(double x, double y)>();
        var highMass = new List<(double x, double y)>();

        double min = galaxies.Min(g => g.Mass);
        double max = galaxies.Max(g => g.Mass);
        double range = max - min;

        foreach (var g in galaxies)
        {
            double norm = (g.Mass - min) / range;
            var point = (g.Position[0], g.Position[1]);

            if (norm < 0.33)
                lowMass.Add(point);
            else if (norm < 0.66)
                midMass.Add(point);
            else
                highMass.Add(point);
        }

        if (lowMass.Count > 0)
            plt.AddScatter(
                lowMass.Select(p => p.x).ToArray(),
                lowMass.Select(p => p.y).ToArray(),
                color: Color.Blue,
                markerSize: 5,
                label: "Low Mass");

        if (midMass.Count > 0)
            plt.AddScatter(
                midMass.Select(p => p.x).ToArray(),
                midMass.Select(p => p.y).ToArray(),
                color: Color.Green,
                markerSize: 5,
                label: "Medium Mass");

        if (highMass.Count > 0)
            plt.AddScatter(
                highMass.Select(p => p.x).ToArray(),
                highMass.Select(p => p.y).ToArray(),
                color: Color.Red,
                markerSize: 5,
                label: "High Mass");

        plt.Title(title);
        plt.XLabel("X Position");
        plt.YLabel("Y Position");
        plt.Legend();

        string filename = "galaxies_by_mass_bucketed.png";
        plt.SaveFig(filename);
        Console.WriteLine($"Saved mass-bucketed galaxy plot to {filename}");
    }
}
