using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAlgorithms.Classes.Algorithms
{
    class Node<T>
    {
        //Fields
        public T Data { get; private set; }
        public List<Edge<T>> Edges { get; private set; } = new List<Edge<T>>();

        /// <summary>
        /// Constructor for at oprettelse af en node
        /// </summary>
        /// <param name="data"></param>
        public Node(T data)
        {
            this.Data = data;
        }

        /// <summary>
        /// Metode til at lave en edge mellem to noder
        /// </summary>
        /// <param name="other"></param>
        public void AddEdge(Node<T> other)
        {
            // Tilføj kanten til listen af kanter
            Edges.Add(new Edge<T>(this, other));
        }

        public override string ToString()
        {
            return Data.ToString();
        }
    }
}
