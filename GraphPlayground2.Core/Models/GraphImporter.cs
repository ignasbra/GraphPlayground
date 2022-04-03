using OsmSharp;
using OsmSharp.Complete;
using OsmSharp.Streams;
using QuickGraph;
using System.IO;
using System.Linq;

namespace GraphPlayground2.Core.Models
{
    public class GraphImporter
    {
        public AdjacencyGraph<(double x, double y), Edge<(double x, double y)>> ImportOSMFile(string path)
        {
            var result = new AdjacencyGraph<(double, double), Edge<(double, double)>>();
            using (var fileStream = File.OpenRead(path))
            {
                // create source stream.
                var source = new XmlOsmStreamSource(fileStream);

                // filter all powerlines and keep all nodes.
                var filtered = from osmGeo in source
                               select osmGeo;

                // convert to complete stream.
                // WARNING: nodes that are partof powerlines will be kept in-memory.
                //          it's important to filter only the objects you need **before** 
                //          you convert to a complete stream otherwise all objects will 
                //          be kept in-memory.
                var complete = filtered.ToComplete();

                foreach (var geoObject in complete)
                {
                    if (geoObject.Type == OsmGeoType.Way && (geoObject?.Tags?.Any(x => x.Key == "highway") ?? true))
                    {
                        var way = (CompleteWay) geoObject;

                        (double, double)? x = null;
                        (double, double)? y = null;

                        foreach (var node in way.Nodes)
                        {
                            result.AddVertex((node.Latitude.Value, node.Longitude.Value));

                            if (x == null) 
                            {
                                x = (node.Latitude.Value, node.Longitude.Value);
                                if (y != null)
                                {
                                    result.AddEdge(new Edge<(double, double)>(x.Value, y.Value));
                                }
                            }
                            else if (y == null)
                            {
                                y = (node.Latitude.Value, node.Longitude.Value);
                                result.AddEdge(new Edge<(double, double)>(x.Value, y.Value));
                                x = y;
                                y = null;
                            }
                        }
                    }
                }
            }

            return result;
        }
    }
}
