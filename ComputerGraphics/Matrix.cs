using System;
using System.Collections.Generic;

namespace ComputerGraphics
{
    class Matrix
    {
        public List<List<float>> MatrixList = new List<List<float>>();

        /// <summary>
        /// Конструктор матрицы пустой размерности
        /// </summary>
        public Matrix(int column,int row)
        {
            for (int i = 0; i < row; i++)
            {
                //создание пустого ряда
                MatrixList.Add(new List<float>());
                for (int j = 0; j < column; j++)
                {
                    //создания колонок в ряду
                    MatrixList[i].Add(0.0f);
                }
            }
        }
        /// <summary>
        /// Конструктор по умолчанию
        /// </summary>
        public Matrix()
        {

        }
        /// <summary>
        /// Вовзращает определитель матрицы
        /// </summary>
        /// <returns></returns>
        public float GetDeterminant(List<List<float>> matrixList=null)
        {
            if (matrixList == null)
                matrixList = this.MatrixList;
            //если получего алгеброическое дополнение 1х1
            if (matrixList.Count == 1 && matrixList[0].Count == 1)
                return matrixList[0][0];
            //если матрица 2x2
            if (matrixList[0].Count == 2 && matrixList.Count == 2)
                return matrixList[0][0] * matrixList[1][1] - matrixList[0][1] * matrixList[1][0];
            //в остальных случаях матрица разделяется на несколько других, до размера 2x2
            else
            {
                float result = 0.0f;
                int sign = 1;
                
                for (int column = 0; column < matrixList[0].Count; column++)
                {
                    //результат - сумма произведений чередующихся знаков 
                    //помноженных на значение каждого элемента верхнего ряда 
                    //помноженного на алгебраическому дополнение взятого по соответствующей вершине
                    result += sign * matrixList[0][column] * GetDeterminant(GetAlgebraicAddition(matrixList, 0, column));
                    sign *= -1;
                }
                return result;
            }
        }
        /// <summary>
        /// Возвращает произведение двух матриц
        /// </summary>
        /// <param name="matrixA"></param>
        /// <param name="matrixB"></param>
        /// <returns></returns>
        public List<List<float>> MultiplicationMatrix(List<List<float>> matrixA,List<List<float>> matrixB)
        {
            if (matrixA[0].Count != matrixB.Count)
                return null;
            List<List<float>> result = new List<List<float>>();
            for (int row = 0; row < matrixA.Count; row++)
            {
                result.Add(new List<float>());
                for (int column = 0; column < matrixB[0].Count; column++)
                {
                    result[row].Add(0.0f);
                    for (int index = 0; index < matrixB.Count; index++)
                        result[row][column] += matrixA[row][index] * matrixB[index][column];
                }
            }

            return result;
        }
        /// <summary>
        /// Определение алгебраического дополнения, относительно элемента Matrix[row][column]
        /// </summary>
        /// <param name="matrixList">Матрица</param>
        /// <param name="row">отсекающий ряд</param>
        /// <param name="column">отсекающий столбец</param>
        /// <returns></returns>
        public List<List<float>> GetAlgebraicAddition(List<List<float>> matrixList, int row, int column)
        {
            List<List<float>> resultMatrix = new List<List<float>>();
            for (int i = 0; i < matrixList.Count-1; i++)
            {
                resultMatrix.Add(new List<float>());
                for (int j = 0; j < matrixList[0].Count-1; j++)
                    resultMatrix[i].Add(0.0f);
            }
            int rowResult = 0;
            int columnResult = 0;

            for (int rowMatrix = 0; rowMatrix < matrixList.Count; rowMatrix++)
                for (int columnMatrix = 0; columnMatrix < matrixList[0].Count; columnMatrix++)
                {
                    if( rowMatrix!=row && columnMatrix != column)
                    {
                        resultMatrix[rowResult][columnResult] = matrixList[rowMatrix][columnMatrix];
                        columnResult++;
                        if(columnResult>=resultMatrix[0].Count)
                        {
                            columnResult = 0;
                            rowResult++;
                        }
                    }
                }
            return resultMatrix;
        }
        /// <summary>
        /// Возвращает обратную матрицу
        /// </summary>
        /// <param name="matrix"></param>
        /// <returns></returns>
        public List<List<float>> GetReverseMatrix(List<List<float>> matrix)
        {
            

            float determ = GetDeterminant(matrix);
            //получение матрицы алгеброических дополнений
            //определитель алгеброического дополнения Aij * (-1)^(i+j) и все деленное на определитель
            List<List<float>> resultMatrix = new List<List<float>>();
            for (int row = 0; row < matrix.Count; row++)
            {
                resultMatrix.Add(new List<float>());
                for (int column = 0; column < matrix[0].Count; column++)
                resultMatrix[row].Add((float)Math.Pow(-1,row+column+2)*GetDeterminant(GetAlgebraicAddition(matrix,row,column))/determ);
            }
            resultMatrix = GetTransparentMatrix(resultMatrix);
            return resultMatrix;
        }
        /// <summary>
        /// Возвращает транспорентную матрицу
        /// </summary>
        /// <param name="matrix"></param>
        /// <returns></returns>
        public List<List<float>> GetTransparentMatrix(List<List<float>> matrix)
        {
            //инициализация пустой инверсной матрицы
            List<List<float>> result = new List<List<float>>();
            for (int row = 0; row < matrix[0].Count; row++)
            {
                result.Add(new List<float>());
                for (int column = 0; column < matrix.Count; column++)
                {
                    result[row].Add(matrix[column][row]);
                }
            }
            return result;
        }
    }
}
