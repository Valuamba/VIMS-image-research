using ImageResearch.Engine;
using ImageResearchNew.Model;
using ImageResearchNew.Model.ImageProcessors;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageResearchNew.ViewModel.ImageModule.ImageProcessors
{
    public class ImageConvolutionViewModel  : ImageProcessorViewModel
    {
        private float[,] _kernel;
        public float[,] Kernel
        {
            get => _kernel;
            set
            {
                SetProperty(ref _kernel, value);
                UpdateImageFactory();
            }
        }

        private IReadOnlyCollection<int> _kernelSizeValues = new List<int>()
        {
            3,
            5,
            7,
            9
        };
        public IReadOnlyCollection<int> KernelSizeValues => _kernelSizeValues;

        private int _selectedKernelSize;
        public int SelectedKernelSize
        {
            get => _selectedKernelSize;
            set
            {
                SetProperty(ref _selectedKernelSize, value);
                Kernel = new float[value, value];
                for (int i = 0; i < value; i++)
                {
                    for (int j = 0; j < value; j++)
                    {
                        Kernel[i, j] = 1;
                    }
                }
                UpdateFilter();
            }
        }

        public override string Header => "Correlation";

        public override ImageProcessor Model { get; set; }

        public DelegateCommand CurrentCellChangedCommand { get; set; }
        public DelegateCommand NormalizeKernelCommand { get; set; }

        public ImageConvolutionViewModel()
        {
            CurrentCellChangedCommand = new DelegateCommand((obj) => OnCurrentCellChangedCommand());
            NormalizeKernelCommand = new DelegateCommand((obj) => NormalizeKernel());
            Model = new ImageConvolution();
            SelectedKernelSize = KernelSizeValues.ElementAt(0);
        }

        private void OnCurrentCellChangedCommand()
        {
            var table = DataView.Table;
            for (int i = 0; i < SelectedKernelSize; i++)
            {
                for (int j = 0; j < SelectedKernelSize; j++)
                {
                    Kernel[i, j] = Convert.ToSingle(table.Rows[i][j]);
                }
            }
            UpdateImageFactory();
        }

        private IReadOnlyCollection<string> _kernelTypes = new List<string>()
        {
            "Первитта (вериткальный)",
            "Первитта (горизонтальный)",
            "Собеля (вертикальный)",
            "Собеля (горизонтальный)",
            "Повышения резкости",
            "ФВЧ Лапласа",
            "ФНЧ Гаусса"
        };
        public IReadOnlyCollection<string> KernelTypes => _kernelTypes;

        private string _kernelType;
        public string KernelType
        {
            get => _kernelType;
            set
            {
                SetProperty(ref _kernelType, value);
                ApplyKernelType();
            }
        }

        private DataView _dataView;
        public DataView DataView
        {
            get => _dataView;
            set
            {
                SetProperty(ref _dataView, value);
            }
        }

        public void ApplyKernelType()
        {
            SelectedKernelSize = 3;
            switch (KernelType)
            {
                case "Первитта (вериткальный)":
                    Kernel = new float[3, 3]
                    {
                        { -1, 0, 1 },
                        { -1, 0, 1 },
                        { -1, 0, 1 },
                    };
                    break;
                case "Первитта (горизонтальный)":
                    Kernel = new float[3, 3]
                    {
                        { 1, 1, 1 },
                        { 0, 0, 0 },
                        { -1, -1, -1 },
                    };
                    break;
                case "Собеля (вертикальный)":
                    Kernel = new float[3, 3]
                    {
                        { -1, 0, 1 },
                        { -2, 0, 2 },
                        { -1, 0, 1 },
                    };
                    break;
                case "Собеля (горизонтальный)":
                    Kernel = new float[3, 3]
                    {
                        { 1, 2, 1 },
                        { 0, 0, 0 },
                        { -1, -2, -1 },
                    };
                    break;
                case "Повышения резкости":
                    Kernel = new float[3, 3]
                    {
                        { -1, -1, -1 },
                        { -1, 16, -1 },
                        { -1, -1, -1 },
                    };
                    break;
                case "ФВЧ Лапласа":
                    Kernel = new float[3, 3]
                    {
                        { -1, -1, -1 },
                        { -1, 8, -1 },
                        { -1, -1, -1 },
                    };
                    break;
                case "ФНЧ Гаусса":
                    Kernel = new float[3, 3]
                    {
                        { 1, 2, 1 },
                        { 2, 4, 2 },
                        { 1, 2, 1 },
                    };
                    break;

                default: throw new ArgumentException("Incorrect kernel type");
            }
            UpdateFilter();
        }

        public void UpdateFilter()
        {
            DataTable dt = new DataTable();

            for (int i = 0; i < SelectedKernelSize; i++)
            {
                dt.Columns.Add(i.ToString(), typeof(double));
            }

            for (int row = 0; row < SelectedKernelSize; row++)
            {
                DataRow dr = dt.NewRow();
                for (int col = 0; col < SelectedKernelSize; col++)
                {
                    dr[col] = Kernel[row, col].ToString("F3");
                }
                dt.Rows.Add(dr);
            }

            DataView = dt.DefaultView;
        }

        public void NormalizeKernel()
        {
            float sum = 0;
            for (int i = 0; i < SelectedKernelSize; i++)
            {
                for (int j = 0; j < SelectedKernelSize; j++)
                {
                    sum += Kernel[i, j];
                }
            }

            if (sum != 0)
            {
                for (int i = 0; i < SelectedKernelSize; i++)
                {
                    for (int j = 0; j < SelectedKernelSize; j++)
                    {
                        Kernel[i, j] = Kernel[i, j] / sum;
                    }
                }
            }
            UpdateFilter();
            UpdateImageFactory();
        }

        public void UpdateImageFactory()
        {
            (Model as ImageConvolution).Kernel = Kernel;
        }
    }
}
