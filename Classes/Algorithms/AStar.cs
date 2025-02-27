using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using DAlgorithms.Classes.World;

namespace DAlgorithms.Classes.Algorithms
{
    public class AStar
    {
        private Dictionary<Point, Tile> tiles;
        private readonly int[] directionsX = { 0, 1, 0, -1 };   // Firevejs (op, højre, ned, venstre)
        private readonly int[] directionsY = { -1, 0, 1, 0 };

        public AStar(Dictionary<Point, Tile> tiles)
        {
            this.tiles = tiles;
        }

        public List<Tile> FindPath(Point startPos, Point goalPos)
        {
            // Tjek at start/goal findes i dictionary
            if (!tiles.ContainsKey(startPos) || !tiles.ContainsKey(goalPos))
                return new List<Tile>();

            Tile startCell = tiles[startPos];
            Tile goalCell = tiles[goalPos];

            // Open/closed-lister
            var openList = new List<Tile>();
            var closedList = new HashSet<Tile>();

            // G og F-værdier for hver celle
            var gCost = new Dictionary<Tile, float>();
            var fCost = new Dictionary<Tile, float>();
            var parent = new Dictionary<Tile, Tile>();

            // Initér gCost, fCost
            foreach (var kvp in tiles)
            {
                gCost[kvp.Value] = float.PositiveInfinity;
                fCost[kvp.Value] = float.PositiveInfinity;
                parent[kvp.Value] = null;
            }

            // Startnode: gCost = 0, fCost = heuristik
            gCost[startCell] = 0;
            fCost[startCell] = Heuristic(startCell, goalCell);

            // Tilføj start til openList
            openList.Add(startCell);

            // A* loop
            while (openList.Count > 0)
            {
                // 1. Find cellen med laveste fCost i openList
                Tile current = GetCellWithLowestF(openList, fCost);
                if (current == goalCell)
                {
                    // Vi har fundet en sti - rekonstruér path
                    return ReconstructPath(parent, goalCell);
                }

                openList.Remove(current);
                closedList.Add(current);

                // 2. Undersøg naboer
                foreach (Tile neighbor in GetNeighbors(current))
                {
                    // Spring over, hvis nabo er lukket eller en mur
                    if (closedList.Contains(neighbor) || !neighbor.IsWalkable)
                        continue;

                    // Beregn ny gCost
                    float tentativeG = gCost[current] + Distance(current, neighbor);

                    // Hvis bedre gCost eller nabo ikke i openList
                    if (tentativeG < gCost[neighbor] || !openList.Contains(neighbor))
                    {
                        gCost[neighbor] = tentativeG;
                        fCost[neighbor] = tentativeG + Heuristic(neighbor, goalCell);
                        parent[neighbor] = current;

                        if (!openList.Contains(neighbor))
                            openList.Add(neighbor);
                    }
                }
            }

            // Ingen sti fundet
            return new List<Tile>();
        }

        // Manhattan-dist (uden diagonal). Hvis du vil have diagonal, brug fx en "octile" distance
        private float Heuristic(Tile a, Tile b)
        {
            // For en 2D grid: |x1 - x2| + |y1 - y2|
            // Gang evt. med en cost (f.eks. 10). Her bruger vi bare 1 pr. step.
            float dx = Math.Abs(a.IndexX - b.IndexX);
            float dy = Math.Abs(a.IndexY - b.IndexY);
            return dx + dy;
        }

        // Returnerer euklidisk eller manhattan-dist mellem to naboceller
        private float Distance(Tile a, Tile b)
        {
            // Hvis du kun bevæger dig ortogonalt (op/ned/venstre/højre), er dist = 1
            // Hvis du tillader diagonaler, kan du tjekke, om |dx|+|dy|=2 => sqrt(2).
            return 1;
        }

        private Tile GetCellWithLowestF(List<Tile> openList, Dictionary<Tile, float> fCost)
        {
            Tile best = openList[0];
            float bestF = fCost[best];

            for (int i = 1; i < openList.Count; i++)
            {
                Tile c = openList[i];
                if (fCost[c] < bestF)
                {
                    bestF = fCost[c];
                    best = c;
                }
            }
            return best;
        }

        private List<Tile> GetNeighbors(Tile tile)
        {
            var result = new List<Tile>();
            // Tjek 4 retninger (op, ned, venstre, højre)
            for (int i = 0; i < directionsX.Length; i++)
            {
                int nx = tile.IndexX + directionsX[i];
                int ny = tile.IndexY + directionsY[i];
                var neighborPos = new Point(nx, ny);

                // Tjek om neighbor er inden for grid
                if (tiles.ContainsKey(neighborPos))
                {
                    result.Add(tiles[neighborPos]);
                }
            }
            return result;
        }

        private List<Tile> ReconstructPath(Dictionary<Tile, Tile> parent, Tile goal)
        {
            var path = new List<Tile>();
            var current = goal;
            while (current != null)
            {
                path.Add(current);
                current = parent[current];
            }
            path.Reverse();
            return path;
        }
    }
}
