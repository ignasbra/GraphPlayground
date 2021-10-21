namespace GraphPlayground2.Models
{
    public class EdgeItem : ICanvasObject
    {
        public double A_X { get; set; }
        public double A_Y { get; set; }
        public double B_X { get; set; }
        public double B_Y { get; set; }
        public int StrokeThickness;
        public double Weight;

        public EdgeItem(NodeItem a, NodeItem b, double weight, int strokeThickness)
        {
            A_X = a.X;
            A_Y = a.Y;
            B_X = b.X;
            B_Y = b.Y;
            Weight = weight;
            StrokeThickness = strokeThickness;
        }
    }
}
