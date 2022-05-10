using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GraphPlayground2.Services
{
    public class ClusteringService
    {
        public Graph G { get; set; }

        public double r_s { get; set; }
        
        public double r_d { get; set; }

        public double E_S { get; set; }
        
        public double E_D { get; set; }



        public List<List<Node>> FindClusters(double e_s, double e_d)
        {
            E_S = e_s;
            E_D = e_d;

            var V1 = new List<Node>();
            V1.Add(G.Nodes[0]);

            var V2 = new List<Node>();

            while (G.Nodes.Count() != V1.Union(V2).Count())
            {
                var e = G.Edges.FirstOrDefault(x => V1.Union(V2).Contains(x.node1) && G.Nodes.Except(V1.Union(V2)).Contains(x.node2));

                if(e == null)
                {
                    break;
                }

                if (Process(e.node1, e.node2, 0, 0))
                {
                    if (V1.Contains(e.node1))
                    {
                        V1.Add(e.node2);
                    }
                    else
                    {
                        V2.Add(e.node2);
                    }
                }
                else
                {
                    if (V1.Contains(e.node1))
                    {
                        V2.Add(e.node2);
                    }
                    else
                    {
                        V1.Add(e.node2);
                    }
                }
            }

            return new List<List<Node>> { V1, V2 };
        }

        public bool Process(Node u, Node v, double r_s, double r_d) // Is in same cluster?
        {
            var count = (double)CountNumberOfCommonNeighbors(u, v);
            var n = (double)G.Nodes.Count;

            if (Math.Abs((count / n) - E_S) < Math.Abs((count / n) - E_D)) // santykis kaimynų su visais nodes atimta tikimybė turėti kaimyną clusteryje vs santkis ir tikimybė turėti kaimyną ne klusteryje.
            {
                return true;
            }

            return false;
        }

        //private double E_S(double r_d, double r_s)
        //{

        //    return 0;
        //}

        //private double E_D(double r_d, double r_s)
        //{
        //    //var b = 1; // skaičius su kuriuo generuotas turėtų būti grafas?
        //    //var n = G.Nodes.Count;

        //    //return (2 * b + Math.Sqrt(6 * b)) * (Math.Log(n) / n);
        //}


        private int CountNumberOfCommonNeighbors(Node u, Node v)
        {
            var uNeighbors = G.Edges.Where(x => x.node1 == u).Select(x => x.node2).ToList();
            uNeighbors.AddRange(G.Edges.Where(x => x.node2 == u).Select(x => x.node1));

            var vNeighbors = G.Edges.Where(x => x.node1 == v).Select(x => x.node2).ToList();
            vNeighbors.AddRange(G.Edges.Where(x => x.node2 == v).Select(x => x.node1));


            var commonNeightbors = uNeighbors.Intersect(vNeighbors).ToList();

            return commonNeightbors.Count;
        }

    }

    public class Graph
    {
        public List<Node> Nodes { get; set; } = new List<Node>();

        public List<Edge> Edges { get; set; } = new List<Edge>();
    }

    public class Edge
    {
        public Node node1 { get; set; }

        public Node node2 { get; set; }
    }

    public class Node
    {
        public double x { get; set; }

        public double y { get; set; }
    }

}
