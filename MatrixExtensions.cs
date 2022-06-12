using System;
using System.Collections.Generic;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SteganographyGraduate
{
    public static class MatrixExtensions
    {
        public static T[,] Transpose<T> (this T[,] matrix)
        {
            var rows = matrix.RowsCount();
            var columns = matrix.ColumnsCount();

            var transposedMatrix = new T[rows, columns];

            for(var i = 0; i < rows; i++)
            {
                for (var j = 0; j < columns; j++)
                {
                    transposedMatrix[i, j] = matrix[j, i];
                }
            }

            return transposedMatrix;
        }


        public static R[,] Multiply<T, R>(this T[,] matrixA, R[,] matrixB)
        {
            var aRows = matrixA.RowsCount();
            var aColumns = matrixA.ColumnsCount();
            var bColumns = matrixB.ColumnsCount();

            var matrixC = new R[aRows, bColumns];

            for (var i = 0; i < aRows; i++)
            {
                for (var j = 0; j < bColumns; j++)
                {
                    matrixC[i, j] = default;

                    for (var k = 0; k < aColumns; k++)
                    {
                        dynamic a = matrixA[i, k];
                        dynamic b = matrixB[k, j];
                        matrixC[i, j] += a * b;
                    }
                }
            }

            return matrixC;
        }

        private static int RowsCount<T>(this T[,] matrix) => matrix.GetUpperBound(0) + 1;

        private static int ColumnsCount<T>(this T[,] matrix) => matrix.GetUpperBound(1) + 1;
    }
}
