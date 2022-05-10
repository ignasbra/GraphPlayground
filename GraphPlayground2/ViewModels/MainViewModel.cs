using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Windows.Input;
using System.Windows.Media;
using GraphPlayground2.Core.Models;
using GraphPlayground2.Models;
using GraphPlayground2.Services;
using Microsoft.Toolkit.Mvvm.ComponentModel;
using Microsoft.Toolkit.Mvvm.Input;

namespace GraphPlayground2.ViewModels
{
    public class MainViewModel : ObservableObject
    {

        public MainViewModel()
        {
            CanvasObjects = new ObservableCollection<ICanvasObject>();
            OnLeftCickCommand = new RelayCommand<System.Windows.Point>(x => HandleLeftClick(x));
            OnRightCickCommand = new RelayCommand<System.Windows.Point>(x => HandleRightClick(x));
            OnEdgeStateClickCommand = new RelayCommand(ChangeStateToEdgeModification);
            OnNodeStateClickCommand = new RelayCommand(ChangeStateToNodeModification);
            OnPointAClickCommand = new RelayCommand(ChangeStateToAPointSelection);
            OnPointBClickCommand = new RelayCommand(ChangeStateToBPointSelection);
            OnImportOSMClickCommand = new RelayCommand(ImportOSMFile);
            OnClearClickCommand = new RelayCommand(ClearNodes);
            OnClusterClickCommand = new RelayCommand(Cluster);
        }

        public ObservableCollection<ICanvasObject> CanvasObjects { get; set; }

        public CanvasStateEnum CanvasState { get; set; } = CanvasStateEnum.NodeModification;

        public ICommand OnLeftCickCommand { get; }
        public ICommand OnRightCickCommand { get; }
        public ICommand OnEdgeStateClickCommand { get; }
        public ICommand OnNodeStateClickCommand { get; }
        public ICommand OnPointAClickCommand { get; }
        public ICommand OnPointBClickCommand { get; }
        public ICommand OnImportOSMClickCommand { get; }
        public ICommand OnClearClickCommand { get; }
        public ICommand OnClusterClickCommand { get; }

        public NodeViewModel FirstSelectedNodeItem { get; set; }
        public NodeViewModel SecondSelectedNodeItem { get; set; }

        public NodeViewModel APointNodeItem { get; set; }
        public NodeViewModel BPointNodeItem { get; set; }

        public double ES { get; set; }
        public double ED { get; set; }

        private void HandleLeftClick(System.Windows.Point point)
        {
            switch (CanvasState)
            {
                case CanvasStateEnum.NodeModification:
                    AddNode(point);
                    break;

                case CanvasStateEnum.EdgeModificatgion:
                    AddEdge(point);
                    break;

                case CanvasStateEnum.APointSelection:
                    SelectAPoint(point);
                    break;

                case CanvasStateEnum.BPointSelection:
                    SelectBPoint(point);
                    break;
            }
        }

        private void HandleRightClick(System.Windows.Point point)
        {
            switch (CanvasState)
            {
                case CanvasStateEnum.NodeModification:
                    DeleteNode(point);
                    break;

                case CanvasStateEnum.EdgeModificatgion:
                    DeleteEdge(point);
                    break;
            }
        }

        private void Cluster()
        {
            var clusteringService = new ClusteringService();

            var graph = new Graph();

            foreach (var nodeVM in CanvasObjects.Where(x => x is NodeViewModel).Select(x => x as NodeViewModel))
            {
                var node = new Node();
                node.x = nodeVM.X;
                node.y = nodeVM.Y;

                graph.Nodes.Add(node);
            }

            foreach (var edgeVM in CanvasObjects.Where(x => x is EdgeViewModel).Select(x => x as EdgeViewModel))
            {
                var edge = new Edge();
                edge.node1 = graph.Nodes.First(x => (Math.Abs(x.x - edgeVM.A_X) < 15) && (Math.Abs(x.y - edgeVM.A_Y) < 15));
                edge.node2 = graph.Nodes.First(x => (Math.Abs(x.x - edgeVM.B_X) < 15) && (Math.Abs(x.y - edgeVM.B_Y) < 15));

                graph.Edges.Add(edge);
            }

            clusteringService.G = graph;

            var groups = clusteringService.FindClusters(ES, ED);


            foreach (var nodeVm in CanvasObjects.Where(x => x is NodeViewModel).Select(x => x as NodeViewModel))
            {
                if (groups[0].Any(x => x.x == nodeVm.X && x.y == nodeVm.Y))
                {
                    nodeVm.Color = Brushes.Yellow;
                }

                if (groups[1].Any(x => x.x == nodeVm.X && x.y == nodeVm.Y))
                {
                    nodeVm.Color = Brushes.Green;
                }
            }
        }

        private void AddNode(System.Windows.Point point) => 
            CanvasObjects.Add(new NodeViewModel(point.X, point.Y, 10, 10));

        private void AddEdge(System.Windows.Point point)
        {
            var clickedNodeItem = GetClickedNodeItem(point);
            if (clickedNodeItem == null) return;

            if (FirstSelectedNodeItem == null)
            {
                FirstSelectedNodeItem = clickedNodeItem;
                clickedNodeItem.Color = Brushes.CadetBlue;

            }
            else if (SecondSelectedNodeItem == null)
            {
                SecondSelectedNodeItem = clickedNodeItem;

                CanvasObjects.Add(new EdgeViewModel(FirstSelectedNodeItem, SecondSelectedNodeItem, 1, 5));

                FirstSelectedNodeItem.Color = Brushes.Black;

                FirstSelectedNodeItem = null;
                SecondSelectedNodeItem = null;
            }
        }

        private void AddEdge(System.Windows.Point x, System.Windows.Point y)
        {
            CanvasObjects.Add(new EdgeViewModel(x, y, 1, 5));
        }

        private void SelectAPoint(System.Windows.Point point)
        {
            var clickedNodeItem = GetClickedNodeItem(point);
            if (clickedNodeItem == null) return;

            APointNodeItem = clickedNodeItem;
            clickedNodeItem.Color = Brushes.Red;
        }


        private void SelectBPoint(System.Windows.Point point)
        {
            var clickedNodeItem = GetClickedNodeItem(point);
            if (clickedNodeItem == null) return;

            BPointNodeItem = clickedNodeItem;
            clickedNodeItem.Color = Brushes.Red;
        }

        private void DeleteNode(System.Windows.Point point)
        {
            var nodeToRemove = GetClickedNodeItem(point);
            if (nodeToRemove != null)
            {
                CanvasObjects.Remove(nodeToRemove);
            }
        }

        private void DeleteEdge(System.Windows.Point point)
        {
            throw new NotImplementedException();
        }

        private void ChangeStateToEdgeModification()
        {
            CanvasState = CanvasStateEnum.EdgeModificatgion;
        }

        private void ChangeStateToNodeModification()
        {
            CanvasState = CanvasStateEnum.NodeModification;
        }

        private void ChangeStateToAPointSelection()
        {
            CanvasState = CanvasStateEnum.APointSelection;
        }

        private void ChangeStateToBPointSelection()
        {
            CanvasState = CanvasStateEnum.BPointSelection;
        }

        private void ImportOSMFile()
        {
            // TODO: I need to have a better canvas, one I could zoom into and pan. Then I could display real coordinates.

            var graphImporter = new GraphImporter();

            var ofd = new Microsoft.Win32.OpenFileDialog();
            var result = ofd.ShowDialog();
            if (result == false) return;

            var graphInRAM = graphImporter.ImportOSMFile(ofd.FileName);

            foreach (var vertex in graphInRAM.Vertices)
            {
                AddNode(new System.Windows.Point(vertex.x * 2, vertex.y * 2));
            }

            foreach (var edge in graphInRAM.Edges)
            {
                AddEdge(new System.Windows.Point(edge.Source.x * 2, edge.Source.y * 2),
                    new System.Windows.Point(edge.Target.x * 2, edge.Target.y * 2));
            }
        }

        private void ClearNodes()
        {
            CanvasObjects.Clear();
        }

        private NodeViewModel GetClickedNodeItem(System.Windows.Point point)
        {
            foreach (var canvasObject in CanvasObjects)
            {
                if (canvasObject is NodeViewModel)
                {
                    var nodeItem = canvasObject as NodeViewModel;
                    if (IsPointWithinNode(point, nodeItem))
                    {
                        return nodeItem;
                    }
                }
            }
            return null;
        }

        private bool IsPointWithinNode(System.Windows.Point point, NodeViewModel node) => 
            point.X < (node.X + node.Width) && point.X > (node.X - node.Width) && 
            point.Y < (node.Y + node.Width) && point.Y > (node.Y - node.Width);
    }
}
