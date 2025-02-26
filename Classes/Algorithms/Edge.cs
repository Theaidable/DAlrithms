using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAlgorithms.Classes.Algorithms
{
    class Edge<T>
    {
        //Fields
        public Node<T> From { get; private set; }
        public Node<T> To { get; private set; }

        /// <summary>
        /// Constructor for at oprette en edge
        /// </summary>
        /// <param name="from"></param> Hvor der skal oprettes en edge fra
        /// <param name="to"></param> Hvor der skal oprettes en edge til
        public Edge(Node<T> from, Node<T> to)
        {
            this.From = from;
            this.To = to;
        }
    }
}
