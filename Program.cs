using System;
using System.Collections.Generic;

public class Galaxy
{
    public BlackHole CenterBlackHole { get; private set; }
    public List<Star> Stars { get; private set; }
    public double[] Position { get; private set; } = new double[3]; // 3D position of the galaxy
    public double[] Velocity { get; private set; } = new double[3]; // Velocity of the galaxy
    public double[] Acceleration { get; private set; } = new double[3]; // Acceleration due to intergalactic gravity
    public double Mass { get; private set; } // Total mass of the galaxy

    public Galaxy(double blackHoleMass, int starCount, double[] position)
    {
        CenterBlackHole = new BlackHole(blackHoleMass);
        Position = position;
        Stars = GenerateStars(starCount);
        Mass = CalculateTotalMass();
    }

    private List<Star> GenerateStars(int count)
    {
        List<Star> stars = new List<Star>();
        string[] starTypes = { "Blue Giant", "Red Giant", "Yellow Dwarf", "White Dwarf" };
        Random rand = new Random();

        for (int i = 0; i < count; i++)
        {
            string starType = starTypes[rand.Next(starTypes.Length)];
            double mass = rand.NextDouble() * (50 - 0.8) + 0.8; // Random mass
            double distanceFromCenter = rand.NextDouble() * (100000 - 100) + 100; // Distance from 100 to 100,000 light years
            double[] position = GenerateRandomPosition(distanceFromCenter);
            stars.Add(new Star(starType, mass, position));
        }

        return stars;
    }

    private double CalculateTotalMass()
    {
        double totalMass = CenterBlackHole.Mass;
        foreach (var star in Stars)
        {
            totalMass += star.Mass;
        }
        return totalMass;
    }

    private static double[] GenerateRandomPosition(double distance)
    {
        Random rand = new Random();
        double theta = rand.NextDouble() * 2 * Math.PI;
        double phi = Math.Acos(2 * rand.NextDouble() - 1);
        double x = distance * Math.Sin(phi) * Math.Cos(theta);
        double y = distance * Math.Sin(phi) * Math.Sin(theta);
        double z = distance * Math.Cos(phi);
        return new[] { x, y, z };
    }

    public void ResetAcceleration()
    {
        for (int i = 0; i < 3; i++)
        {
            Acceleration[i] = 0;
        }
    }

    public void ApplyGravitationalForce(double[] force)
    {
        for (int i = 0; i < 3; i++)
        {
            Acceleration[i] += force[i] / Mass;
        }
    }

    public void UpdateVelocity(double timeStep)
    {
        for (int i = 0; i < 3; i++)
        {
            Velocity[i] += Acceleration[i] * timeStep;
        }
    }

    public void UpdatePosition(double timeStep)
    {
        for (int i = 0; i < 3; i++)
        {
            Position[i] += Velocity[i] * timeStep;
        }
    }
}

public class Universe
{
    public List<Galaxy> Galaxies { get; private set; }

    public Universe(int galaxyCount)
    {
        Galaxies = GenerateGalaxies(galaxyCount);
    }

    private List<Galaxy> GenerateGalaxies(int count)
    {
        List<Galaxy> galaxies = new List<Galaxy>();
        Random rand = new Random();

        for (int i = 0; i < count; i++)
        {
            double blackHoleMass = rand.NextDouble() * (1e10 - 1e6) + 1e6; // Random mass for the supermassive black hole
            int starCount = rand.Next(1000, 100000); // Random number of stars in each galaxy
            double[] position = GenerateRandomPosition(1e6); // Random position in the universe, distance in light-years
            galaxies.Add(new Galaxy(blackHoleMass, starCount, position));
        }

        return galaxies;
    }

    private static double[] GenerateRandomPosition(double distance)
    {
        Random rand = new Random();
        double theta = rand.NextDouble() * 2 * Math.PI;
        double phi = Math.Acos(2 * rand.NextDouble() - 1);
        double x = distance * Math.Sin(phi) * Math.Cos(theta);
        double y = distance * Math.Sin(phi) * Math.Sin(theta);
        double z = distance * Math.Cos(phi);
        return new[] { x, y, z };
    }

    public void SimulateUniverse(double timeStep, int steps)
    {
        for (int step = 0; step < steps; step++)
        {
            // Reset acceleration for each galaxy
            foreach (var galaxy in Galaxies)
            {
                galaxy.ResetAcceleration();
            }

            // Calculate gravitational forces between galaxies
            for (int i = 0; i < Galaxies.Count; i++)
            {
                for (int j = i + 1; j < Galaxies.Count; j++)
                {
                    double[] force = CalculateGravitationalForce(Galaxies[i], Galaxies[j]);
                    Galaxies[i].ApplyGravitationalForce(force);
                    for (int k = 0; k < 3; k++) force[k] *= -1; // Equal and opposite force
                    Galaxies[j].ApplyGravitationalForce(force);
                }
            }

            // Update velocity and position for each galaxy
            foreach (var galaxy in Galaxies)
            {
                galaxy.UpdateVelocity(timeStep);
                galaxy.UpdatePosition(timeStep);
            }
        }
    }

    private static double[] CalculateGravitationalForce(Galaxy g1, Galaxy g2)
    {
        double G = 6.67430e-11; // Gravitational constant
        double[] force = new double[3];
        double distanceSquared = 0;

        for (int i = 0; i < 3; i++)
        {
            distanceSquared += Math.Pow(g2.Position[i] - g1.Position[i], 2);
        }

        double distance = Math.Sqrt(distanceSquared);
        double forceMagnitude = (G * g1.Mass * g2.Mass) / distanceSquared;

        for (int i = 0; i < 3; i++)
        {
            force[i] = forceMagnitude * (g2.Position[i] - g1.Position[i]) / distance;
        }

        return force;
    }
}

class Program
{
    static void Main()
    {
        // Generate a universe with 100 galaxies
        Universe universe = new Universe(100);

   // Print a few galaxies for inspection
        for (int i = 0; i < Math.Min(5, universe.Galaxies.Count); i++)
        {
            var galaxy = universe.Galaxies[i];
            Console.WriteLine($"Galaxy {i + 1}:");
            Console.WriteLine($"  Position: ({galaxy.Position[0]:F2}, {galaxy.Position[1]:F2}, {galaxy.Position[2]:F2})");
            Console.WriteLine($"  Mass: {galaxy.Mass:E2}");
            Console.WriteLine($"  Stars: {galaxy.Stars.Count}");
            Console.WriteLine($"  Black Hole Mass: {galaxy.CenterBlackHole.Mass:E2}");
            Console.WriteLine();
        }


        // Simulate universe dynamics (e.g., 1 time step = 10 million years, 1000 steps)
        universe.SimulateUniverse(1e7, 1000);

        GalaxyVisualizer.PlotGalaxyPositions(universe.Galaxies, "Galaxies After Simulation");

        Console.WriteLine("Universe simulation completed.");
    }
}
