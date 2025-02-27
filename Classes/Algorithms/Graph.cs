using DAlgorithms.Classes.Objects;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using SharpDX.MediaFoundation;
using System;
using System.Collections.Generic;

namespace DAlgorithms.Classes.Algorithms
{
    class Graph<T>
    {
        public List<Node<T>> Nodes { get; private set; } = new List<Node<T>>();

        public void AddNode(T value)
        {
            Nodes.Add(new Node<T>(value));
        }

        public void AddDirectedEdge(T from, T to)
        {
            Node<T> fromNode = Nodes.Find(x => x.Data.Equals(from));
            Node<T> toNode = Nodes.Find(x => x.Data.Equals(to));

            if (fromNode == null || toNode == null)
            {
                throw new ArgumentException("Begge noder skal allerede være tilføjet i grafen.");
            }

            fromNode.AddEdge(toNode);
        }

        public void AddEdge(T from, T to)
        {
            Node<T> fromNode = Nodes.Find(x => x.Data.Equals(from));
            Node<T> toNode = Nodes.Find(x => x.Data.Equals(to));

            if (fromNode == null || toNode == null)
            {
                throw new ArgumentException("Begge noder skal allerede være tilføjet i grafen.");
            }

            fromNode.AddEdge(toNode);
            toNode.AddEdge(fromNode);
        }

        /// <summary>
        /// Finder en sti fra start til target ved hjælp af DFS (iterativt med en stack).
        /// Returnerer en liste af noder, der udgør stien, eller en tom liste hvis ingen sti findes.
        /// </summary>
        public List<Node<T>> FindPathDFS(T start, T target)
        {
            Node<T> startNode = Nodes.Find(n => n.Data.Equals(start));
            Node<T> targetNode = Nodes.Find(n => n.Data.Equals(target));
            if (startNode == null || targetNode == null)
                throw new ArgumentException("Start- eller targetnode findes ikke.");

            Stack<Node<T>> stack = new Stack<Node<T>>();
            Dictionary<Node<T>, Node<T>> parent = new Dictionary<Node<T>, Node<T>>();
            HashSet<Node<T>> visited = new HashSet<Node<T>>();

            stack.Push(startNode);
            visited.Add(startNode);
            parent[startNode] = null;

            while (stack.Count > 0)
            {
                Node<T> current = stack.Pop();
                if (current.Data.Equals(target))
                {
                    return ReconstructPath(parent, current);
                }

                foreach (Edge<T> edge in current.Edges)
                {
                    Node<T> neighbor = edge.To;
                    if (!visited.Contains(neighbor))
                    {
                        visited.Add(neighbor);
                        parent[neighbor] = current;
                        stack.Push(neighbor);
                    }
                }
            }
            return new List<Node<T>>();
        }

        // Hjælpefunktion til at rekonstruere stien ud fra parent-pegeren
        private List<Node<T>> ReconstructPath(Dictionary<Node<T>, Node<T>> parent, Node<T> target)
        {
            List<Node<T>> path = new List<Node<T>>();
            Node<T> current = target;
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
