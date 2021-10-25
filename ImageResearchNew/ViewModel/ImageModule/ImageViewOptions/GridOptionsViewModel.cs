using Ikc5.TypeLibrary;
using ImageResearchNew.Model.ViewOptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageResearchNew.ViewModel.ImageModule.ImageViewOptions
{
    public class GridOptionsViewModel : BaseNotifyPropertyChanged
    {
        private readonly GridOptions gridOptions;

        public GridOptionsViewModel(GridOptions gridOptions)
        {
            this.gridOptions = gridOptions;
            _isHidden = gridOptions.IsHidden;
            _gridSize = gridOptions.GridSize;
        }

        private IReadOnlyCollection<int> _gridValues = new List<int>()
        {
            1,
            8,
            16,
            32,
            64,
            128,
            256
        };
        public IReadOnlyCollection<int> GridValues => _gridValues;

        private bool _isHidden;
        public bool IsHidden
        {
            get => _isHidden;
            set
            {
                SetProperty(ref _isHidden, value);
                UpdateGridOptions();
            }
        }

        private int _gridSize;
        public int GridSize
        {
            get => _gridSize;
            set
            {
                SetProperty(ref _gridSize, value);
                UpdateGridOptions();
            }
        }

        private void UpdateGridOptions()
        {
            gridOptions.IsHidden = IsHidden;
            gridOptions.GridSize = GridSize;
        }
    }
}
