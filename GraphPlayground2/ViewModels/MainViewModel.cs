using System;
using System.Collections.ObjectModel;
using System.Windows.Input;
using System.Windows.Media;
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
        }

        public ObservableCollection<ICanvasObject> CanvasObjects { get; set; }

        public CanvasStateEnum CanvasState { get; set; } = CanvasStateEnum.NodeModification;

        public ICommand OnLeftCickCommand { get; }
        public ICommand OnRightCickCommand { get; }
        public ICommand OnEdgeStateClickCommand { get; }
        public ICommand OnNodeStateClickCommand { get; }

        public NodeViewModel FirstSelectedNodeItem { get; set; }
        public NodeViewModel SecondSelectedNodeItem { get; set; }

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
