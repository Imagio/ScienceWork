using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ru.Imagio.Science.Math
{
    public struct Matrix : ICloneable
    {
        private readonly int _rowCount;
        private readonly int _columnCount;
        private readonly double[,] _data;

        public Matrix(int columnCount, int rowCount)
        {
            _rowCount = rowCount;
            _columnCount = columnCount;

            _data = new double[_rowCount, _columnCount];
        }

        public void Clear()
        {
            for (var i = 0; i < _rowCount; i++)
            {
                for (var j = 0; j < _columnCount; j++)
                {
                    _data[i, j] = 0;
                }
            }
        }

        public double this[int column, int row]
        {
            get
            {
                return _data[row, column];
            }
            set
            {
                _data[row, column] = value;
            }
        }

        public int RowCount { get { return _rowCount; } }
        public int ColumnCount { get { return _columnCount; } }

        public object Clone()
        {
            var matrix = new Matrix(_rowCount, _columnCount);
            for (var i = 0; i < _rowCount; i++)
            {
                for (var j = 0; j < _columnCount; j++)
                {
                    matrix._data[i, j] = _data[i, j];
                }
            }
            return matrix;
        }

        public void CopyFrom(Matrix matrix)
        {
            if (matrix.RowCount != _rowCount)
            {
                return;
            }
            if (matrix.ColumnCount != _columnCount)
            {
                return;
            }

            for (var i = 0; i < _rowCount; i++)
            {
                for (var j = 0; j < _columnCount; j++)
                {
                    _data[i, j] = matrix._data[i, j];
                }
            }
        }
    }
}
