using System;

namespace dm
{
    /// <summary>
    /// Класс Отношения для Натуральных чисел.
    /// Реализованный в виде, квадратной матрицы.
    /// Где пара (a,b) принадлежит, если ячейка со строкой a - 1 и со столбцом b - 1 = true  
    /// </summary>
    class Relations
    {
        /// <summary>
        /// Матрица.
        /// </summary>
        private bool[,] relations;

        /// <summary>
        /// Мощность матрицы, определяется по количеству вхоящих отнощений.
        /// </summary>
        private int _power = 0;

        /// <summary>
        /// Выделение памяти под квадратную матрицу.
        /// </summary>
        /// <param name="size">Размерность квадратной матрицы.</param>
        public Relations(int size)
        {
            relations = new bool[size, size];
        }

        /// <summary>
        /// Выделение памяти и инициализирование матрицы из другой матрицы.
        /// </summary>
        /// <param name="array">Матрица, для инициализации.</param>
        public Relations(bool[,] array)
        {
            relations = array;
            foreach (var element in relations)
            {
                if (element == true)
                {
                    _power++;
                }
            }
        }

        /// <summary>
        /// Всего ячеек в матрице.
        /// </summary>
        /// <value>Возвращает количество ячеек в матрице.</value>
        public int CounterCells
        {
            get { return relations.Length; }
        }

        /// <summary>
        /// Всего количество строк и столбцов.
        /// </summary>
        /// <value>Возвращает количество строк и столбцов.</value>
        public int Length
        {
            get { return (int)Math.Sqrt(relations.Length); }
        }

        /// <summary>
        /// Мощность матрицы.
        /// </summary>
        /// <value>Возвращает кол-во лежащих элементов в матрице.</value>
        public int Power
        {
            get { return _power; }
        }

        /// <summary>
        /// Удаление элемента.
        /// </summary>
        /// <param name="firstElement">Первый элемент отношения.</param>
        /// <param name="secondElement">Второй элемент отношения.</param>
        public void DeleteElmentByValues(int firstElement, int secondElement)
        {
            try
            {
                if ((secondElement > Length || firstElement > Length) || firstElement * secondElement == 0)
                    throw new ArgumentOutOfRangeException("Bad Elements");

                relations[firstElement - 1, secondElement - 1] = false;
            }
            catch (ArgumentOutOfRangeException ex)
            {
                System.Console.WriteLine(ex);
            }
        }

        /// <summary>
        /// Удаления отношения (a,b) из матрицы
        /// </summary>
        /// <param name="p">Пара (a,b)</param>
        public void DeleteElmentByValues(in Tuple<int, int> p)
        {
            try
            {
                if ((p.Item1 > Length || p.Item2 > Length) || p.Item2 * p.Item1 == 0)
                    throw new ArgumentOutOfRangeException("Bad Elements");

                relations[p.Item1 - 1, p.Item2 - 1] = false;
            }
            catch (ArgumentOutOfRangeException ex)
            {
                System.Console.WriteLine(ex);
            }
        }

        /// <summary>
        /// Перегрузка [,]
        /// </summary>
        /// <value>Возвращает true, если элемент под индексом indexRow, indexColumn, есть в матрице.</value>
        public bool this[int indexRow, int indexColumn]
        {
            get
            {
                if (indexColumn >= Length || indexRow >= Length)
                    throw new ArgumentOutOfRangeException("Bad indexs");
                return relations[indexRow, indexColumn];
            }

            set
            {
                if (indexColumn >= Length || indexRow >= Length)
                    throw new ArgumentOutOfRangeException("Bad indexs");
                relations[indexRow, indexColumn] = value;
            }
        }

        /// <summary>
        /// Вывод отношений.
        /// </summary>
        /// <param name="r">Отношение.</param>
        public static void OutputRelations(in Relations r)
        {
            Console.Write('{');
            for (int row = 0; row < r.Length; row++)
            {
                for (int column = 0; column < r.Length; column++)
                {
                    if (r.relations[row, column])
                    {
                        Console.Write($"({row + 1},{column + 1})");
                    }
                }
            }
            Console.WriteLine('}');
        }

        /// <summary>
        /// Ввод отношений.
        /// </summary>
        /// <param name="totalPairs">Кол-во элементов в отношениях.</param>
        /// <returns>Возвращает отношения.</returns>
        public static Relations InputRelations(int totalPairs)
        {
            var list = new List<Tuple<int, int>>();
            int maxElement = 0;
            for (int i = 0; i < totalPairs; i++)
            {
                var tuple = Console.ReadLine().Split(' ');
                list.Add(Tuple.Create<int, int>(Convert.ToInt32(tuple[0]), Convert.ToInt32(tuple[1])));

                maxElement = Math.Max(maxElement, Math.Max(list[i].Item1, list[i].Item2));
            }


            Relations res = new Relations(maxElement);
            foreach (var pair in list)
            {
                res[pair.Item1 - 1, pair.Item2 - 1] = true;
            }

            return res;

        }

        /// <summary>
        /// Является ли матрица отношений r1 подможеством матрицы отношений r2.
        /// </summary>
        /// <param name="r1">Отношения первое.</param>
        /// <param name="r2">Отношение второе.</param>
        /// <returns>Возвращает true, если r1 подмножество множества r2</returns>
        public static bool operator >=(in Relations r1, in Relations r2)
        {
            if (r1.Length > r2.Length)
            {
                return false;
            }

            for (int row = 0; row < r1.Length; row++)
            {
                for (int column = 0; column < r1.Length; column++)
                {
                    if (r1[row, column] != r2[row, column])
                    {
                        return false;
                    }
                }
            }

            return true;
        }

        /// <summary>
        /// Является ли матрица отношений r2 подможеством матрицы отношений r1.
        /// </summary>
        /// <param name="r1">Отношения первое.</param>
        /// <param name="r2">Отношение второе.</param>
        /// <returns>Возвращает true, если r2 подмножество множества r1, иначе false</returns>
        public static bool operator <=(in Relations r1, in Relations r2)
        {
            return r2 >= r1;
        }

        /// <summary>
        /// Равенство отношений.
        /// </summary>
        /// <param name="r1">Отношение первое.</param>
        /// <param name="r2">Отношение второе.</param>
        /// <returns>Возвращает true если r1 равняется r2, иначе false.</returns>
        public static bool operator ==(in Relations r1, in Relations r2)
        {
            if (r1.Length != r2.Length)
            {
                return false;
            }

            return r1 >= r2;
        }

        /// <summary>
        /// Неравенство отношений.
        /// </summary>
        /// <param name="r1">Отношение первое.</param>
        /// <param name="r2">Отношение второе.</param>
        /// <returns>Возвращает true если r1 неравняется r2, иначе false.</returns>
        public static bool operator !=(in Relations r1, in Relations r2)
        {
            return !(r1 == r2);
        }

        /// <summary>
        /// Включена ли строго матрица отношений r1 в матрицу отношений r2.
        /// </summary>
        /// <param name="r1">Отношения первое.</param>
        /// <param name="r2">Отношение второе.</param>
        /// <returns>Возвращает true, если r1 строго включено в r2, иначе false.</returns>
        public static bool operator >(in Relations r1, in Relations r2)
        {
            return r1 != r2 && r1 >= r2;
        }

        /// <summary>
        /// Включена ли строго матрица отношений r2 в матрицу отношений r1.
        /// </summary>
        /// <param name="r1">Отношения первое.</param>
        /// <param name="r2">Отношение второе.</param>
        /// <returns>Возвращает true, если r2 строго включено в r1, иначе false.</returns>
        public static bool operator <(in Relations r1, in Relations r2)
        {
            return r2 > r1;
        }

        /// <summary>
        /// Объеденение двух Отношений.
        /// </summary>
        /// <param name="r1">Отношения первое.</param>
        /// <param name="r2">Отношение второе.</param>
        /// <returns>Возвращает результат объеденения r1 и r2.</returns>
        public static Relations operator +(in Relations r1, in Relations r2)
        {
            Relations res = new Relations(Math.Max(r1.Length, r2.Length));

            for (int row = 0; row < res.Length; row++)
            {
                for (int column = 0; column < res.Length; column++)
                {
                    if (((row <= r1.Length - 1) && (column <= r1.Length - 1)) && (((row <= r2.Length - 1) && (column <= r2.Length - 1))))
                    {
                        res[row, column] = r1[row, column] || r2[row, column];
                    }
                    else if ((row <= r1.Length - 1) && (column <= r1.Length - 1))
                    {
                        res[row, column] = r1[row, column];
                    }
                    else if ((row <= r2.Length - 1) && (column <= r2.Length - 1))
                    {
                        res[row, column] = r2[row, column];
                    }
                }
            }

            return res;
        }


        /// <summary>
        /// Пересечение двух Отношений.
        /// </summary>
        /// <param name="r1">Отношения первое.</param>
        /// <param name="r2">Отношение второе.</param>
        /// <returns>Возвращает результат Пересечения r1 и r2.</returns>
        public static Relations operator *(in Relations r1, in Relations r2)
        {
            Relations res = new Relations(Math.Min(r1.Length, r2.Length));

            for (int row = 0; row < res.Length; row++)
            {
                for (int column = 0; column < res.Length; column++)
                {
                    if (((row <= r1.Length - 1) && (column <= r1.Length - 1)) && (((row <= r2.Length - 1) && (column <= r2.Length - 1))))
                    {
                        res[row, column] = r1[row, column] && r2[row, column];
                    }
                }
            }

            return res;
        }

        /// <summary>
        /// Разность двух Отношений.
        /// </summary>
        /// <param name="r1">Отношения первое.</param>
        /// <param name="r2">Отношение второе.</param>
        /// <returns>Возвращает результат Разности r1 и r2.</returns>
        public static Relations operator -(in Relations r1, in Relations r2)
        {
            Relations res = new Relations(r1.Length);

            for (int row = 0; row < res.Length; row++)
            {
                for (int column = 0; column < res.Length; column++)
                {
                    if (((row <= r1.Length - 1) && (column <= r1.Length - 1)) && (((row <= r2.Length - 1) && (column <= r2.Length - 1))))
                    {
                        res[row, column] = r1[row, column] ^ r2[row, column];
                    }
                    else if (((row <= r1.Length - 1) && (column <= r1.Length - 1)))
                    {
                        res[row, column] = r1[row, column];
                    }
                }
            }

            return res;

        }

        /// <summary>
        /// Симметрическая разность двух Отношений.
        /// </summary>
        /// <param name="r1">Отношения первое.</param>
        /// <param name="r2">Отношение второе.</param>
        /// <returns>Возвращает результат Симметрической разности r1 и r2.</returns>
        public static Relations operator ^(in Relations r1, in Relations r2)
        {
            Relations res = new Relations(Math.Max(r1.Length, r2.Length));

            for (int row = 0; row < res.Length; row++)
            {
                for (int column = 0; column < res.Length; column++)
                {
                    if (((row <= r1.Length - 1) && (column <= r1.Length - 1)) && (((row <= r2.Length - 1) && (column <= r2.Length - 1))))
                    {
                        res[row, column] = r1[row, column] ^ r2[row, column];
                    }
                    else if (((row <= r1.Length - 1) && (column <= r1.Length - 1)))
                    {
                        res[row, column] = r1[row, column];
                    }
                    else if ((row <= r2.Length - 1) && (column <= r2.Length - 1))
                    {
                        res[row, column] = r2[row, column];
                    }
                }
            }

            return res;

        }

        public static Relations Conversion(in Relations r)
        {
            var list = new List<Tuple<int, int>>();
            int maxElement = 0;
            for (int i = 0; i < r.Length; i++)
            {
                if (r[i, i])
                {
                    list.Add(Tuple.Create<int, int>(i, i));
                    maxElement = i;
                }
            }

            Relations res = new Relations(maxElement);
            foreach (var pair in list)
            {
                res[pair.Item1 - 1, pair.Item2 - 1] = true;
            }

            return res;
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            bool[,] array = {
                {false,true,true},
                {false,false,false},
                {true,false,false}
            };

            Relations a = Relations.InputRelations(3);
            Relations.OutputRelations(a);
        }
    }

}