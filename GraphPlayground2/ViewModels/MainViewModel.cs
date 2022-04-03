using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Windows.Input;
using System.Windows.Media;
using GraphPlayground2.Core.Models;
using GraphPlayground2.Models;
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

        public NodeViewModel FirstSelectedNodeItem { get; set; }
        public NodeViewModel SecondSelectedNodeItem { get; set; }

        public NodeViewModel APointNodeItem { get; set; }
        public NodeViewModel BPointNodeItem { get; set; }

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

        private void AddNode(System.Windows.Point point) => 
            CanvasObjects.Add(new NodeViewModel(point.X, point.Y, 2, 2));

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
            var graphImporter = new GraphImporter();

            var ofd = new Microsoft.Win32.OpenFileDialog();
            var result = ofd.ShowDialog();
            if (result == false) return;

            var graphInRAM = graphImporter.ImportOSMFile(ofd.FileName);

            foreach (var vertex in graphInRAM.Vertices)
            {
                AddNode(new System.Windows.Point(vertex.x % 0.1 * 10000, vertex.y % 0.1 * 10000));
            }

            foreach (var edge in graphInRAM.Edges)
            {
                AddEdge(new System.Windows.Point(edge.Source.x % 0.1 * 10000, edge.Source.y % 0.1 * 10000),
                    new System.Windows.Point(edge.Target.x % 0.1 * 10000, edge.Target.y % 0.1 * 10000));
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
