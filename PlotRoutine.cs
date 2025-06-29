using ScottPlot;
using System.Drawing; // Needed for Color

public static class GalaxyVisualizer
{
    public static void PlotGalaxyPositions(List<Galaxy> galaxies, string title = "Galaxy Positions")
    {
        var plt = new ScottPlot.Plot(800, 600);
        int count = galaxies.Count;
        double[] xs = new double[count];
        double[] ys = new double[count];

        for (int i = 0; i < count; i++)
        {
            xs[i] = galaxies[i].Position[0];
            ys[i] = galaxies[i].Position[1];
        }

        plt.AddScatter(xs, ys, color: Color.Cyan, markerSize: 5, label: "Galaxies");
        plt.Title(title);
        plt.XLabel("X Position (light years)");
        plt.YLabel("Y Position (light years)");
        plt.Legend();

        string filename = "galaxy_plot.png";
        plt.SaveFig(filename);
        Console.WriteLine($"Saved plot to {filename}");
    }
}
