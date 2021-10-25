using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Ikc5.TypeLibrary;
using ImageResearchNew.Model;
using ImageResearchNew.ViewModel.ImageModule;
using ImageResearchNew.ViewModel.ImageModule.ImageCharts;
using System.Linq;
using ImageResearchNew.Utilities;

namespace ImageResearchNew.ViewModel
{
    public class DynamicGridViewModel : BaseNotifyPropertyChanged
    {
        private readonly ObservableCollection<ImagePlaceViewModel> _unformattedCells;

        public DynamicGridViewModel(ObservableCollection<ImagePlaceViewModel> cells)
        {
            this.SetDefaultValues();
            _unformattedCells = cells;
            GridCount = _unformattedCells.Count;
            SelectedComparedType = Model.CompareType.Image;
        }

        private IReadOnlyCollection<CompareType> _compareType = new List<CompareType>()
        {
            Model.CompareType.Image,
            Model.CompareType.Vectorscope,
            Model.CompareType.Histogram,
        };
        public IReadOnlyCollection<CompareType> CompareType => _compareType;

        private CompareType _selectedComparedType;
        public CompareType SelectedComparedType
        {
            get => _selectedComparedType;
            set
            {
                SetProperty(ref _selectedComparedType, value);
                UpdateCompareList();
            }
        }

        private void UpdateCompareList()
        {
            switch(SelectedComparedType)
            {
                case Model.CompareType.Image:
                    ObjectViewModels = CreateCellsStructure(_unformattedCells.Cast<object>().ToObservableCollection());
                    break;
                case Model.CompareType.Histogram:
                    ObjectViewModels = CreateCellsStructure(_unformattedCells.Select(u => new HistogramChartViewModel(u.imageInfo)).Cast<object>().ToObservableCollection());
                    break;
                case Model.CompareType.Vectorscope:
                    ObjectViewModels = CreateCellsStructure(_unformattedCells.Select(u => new VectorscopeChartViewModel(u.imageInfo)).Cast<object>().ToObservableCollection());
                    break;
                default: throw new ArgumentException("Incorrect Compare type.");
            }
        }

        private ObservableCollection<ObservableCollection<object>> _objectViewModels;
        public ObservableCollection<ObservableCollection<object>> ObjectViewModels
        {
            get => _objectViewModels;
            set => SetProperty(ref _objectViewModels, value);
        }

        private ObservableCollection<ObservableCollection<T>> CreateCellsStructure<T>(ObservableCollection<T> unformatedCells)
        {
            if (GridCount != 0)
            {
                int counter = 0;
                GridWidth = (int)Math.Ceiling(Math.Sqrt(GridCount));
                GridHeight = (int)Math.Ceiling(GridCount / (double)GridWidth);

                var cells = new ObservableCollection<ObservableCollection<T>>();
                for (var posRow = 0; posRow < GridHeight; posRow++)
                {
                    var row = new ObservableCollection<T>();
                    for (var posCol = 0; posCol < GridWidth && counter < GridCount; posCol++, counter++)
                    {
                        var cellViewModel = unformatedCells[counter];
                        row.Add(cellViewModel);
                    }
                    cells.Add(row);
                }
                return cells;
            }
            return null;
        }

        #region DynamicGridViewModel

        private int _gridHeight;
        private int _gridWidth;
        private int _gridCount;

        public int GridWidth
        {
            get { return _gridWidth; }
            set { SetProperty(ref _gridWidth, value); }
        }

        public int GridHeight
        {
            get { return _gridHeight; }
            set { SetProperty(ref _gridHeight, value); }
        }

        public int GridCount
        {
            get { return _gridCount; }
            set
            {
                SetProperty(ref _gridCount, value);
            }
        }

        #endregion
    }
}
