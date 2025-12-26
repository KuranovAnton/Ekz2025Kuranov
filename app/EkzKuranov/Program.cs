using System;
using System.Collections.Generic;

namespace EkzKuranov
{
    class Program
    {
        static void Main()
        {
            int n = 0;
            while (true)
            {
                Console.WriteLine("Введите количество точек на карте (положительное число):");
                string inputN = Console.ReadLine();
                if (int.TryParse(inputN, out n) && n > 0)
                    break;
                Console.WriteLine("Некорректный ввод. Попробуйте ещё раз.");
            }

            double[,] matrix = new double[n, n];

            for (int i = 0; i < n; i++)
                for (int j = 0; j < n; j++)
                    matrix[i, j] = (i == j) ? 0 : Double.MaxValue;

            Console.WriteLine("Введите связи между точками в формате: точка1 точка2 расстояние");
            Console.WriteLine("Для завершения ввода связей введите '0 0 0'");

            while (true)
            {
                Console.Write("Введите связь: ");
                string line = Console.ReadLine();
                string[] input = line.Split();

                if (input.Length != 3)
                {
                    Console.WriteLine("Некорректный формат. Попробуйте ещё раз.");
                    continue;
                }

                bool fromParsed = int.TryParse(input[0], out int from);
                bool toParsed = int.TryParse(input[1], out int to);
                bool distParsed = double.TryParse(input[2], out double distance);

                if (!fromParsed || !toParsed || !distParsed)
                {
                    Console.WriteLine("Введены некорректные числа. Попробуйте ещё раз.");
                    continue;
                }

                if (from == 0 && to == 0 && Math.Abs(distance) < 1e-9)
                    break;

                if (from < 1 || to < 1 || from > n || to > n)
                {
                    Console.WriteLine($"Номера точек должны быть в диапазоне от 1 до {n}. Попробуйте ещё раз.");
                    continue;
                }

                if (distance < 0)
                {
                    Console.WriteLine("Расстояние не может быть отрицательным. Попробуйте ещё раз.");
                    continue;
                }

                matrix[from - 1, to - 1] = distance;
                matrix[to - 1, from - 1] = distance;
            }

            double fuelPer100km = 0;
            while (true)
            {
                Console.WriteLine("Введите расход топлива на 100 км:");
                string fuelInput = Console.ReadLine();
                if (double.TryParse(fuelInput, out fuelPer100km) && fuelPer100km >= 0)
                    break;
                Console.WriteLine("Некорректный ввод. Попробуйте ещё раз.");
            }

            while (true)
            {
                Console.Write("Введите номера точек для поиска пути (или 0 0 для выхода): ");
                string line = Console.ReadLine();
                string[] input = line.Split();

                if (input.Length != 2)
                {
                    Console.WriteLine("Некорректный формат. Попробуйте ещё раз.");
                    continue;
                }

                bool startParsed = int.TryParse(input[0], out int startPoint);
                bool endParsed = int.TryParse(input[1], out int endPoint);

                if (!startParsed || !endParsed)
                {
                    Console.WriteLine("Введены некорректные числа. Попробуйте ещё раз.");
                    continue;
                }

                if (startPoint == 0 || endPoint == 0)
                    break;

                if (startPoint < 1 || startPoint > n || endPoint < 1 || endPoint > n)
                {
                    Console.WriteLine($"Номера точек должны быть в диапазоне от 1 до {n}. Попробуйте ещё раз.");
                    continue;
                }

                var dist = DijkstraAlg.Dijkstra(matrix, startPoint - 1);
                List<int> path = ReconstructPath(matrix, dist, startPoint - 1, endPoint - 1);

                if (path.Count == 0)
                {
                    Console.WriteLine("Путь не найден.");
                    continue;
                }

                Console.WriteLine("Кратчайший путь:");
                foreach (int node in path)
                {
                    Console.Write($"{node + 1} ");
                }
                Console.WriteLine();

                double totalDistance = 0;
                for (int i = 0; i < path.Count - 1; i++)
                {
                    totalDistance += matrix[path[i], path[i + 1]];
                }

                double totalFuel = (totalDistance / 100.0) * fuelPer100km;
                Console.WriteLine($"Расстояние: {totalDistance} км");
                Console.WriteLine($"Расход топлива: {totalFuel} литров");
            }

            static List<int> ReconstructPath(double[,] a, double[] dist, int start, int end)
            {
                List<int> path = new List<int>();
                int current = end;
                path.Add(current);
                while (current != start)
                {
                    bool foundPrev = false;
                    for (int neighbor = 0; neighbor < a.GetLength(0); neighbor++)
                    {
                        if (a[neighbor, current] != Double.MaxValue && neighbor != current)
                        {
                            if (dist[neighbor] != Double.MaxValue && Math.Abs(dist[neighbor] + a[neighbor, current] - dist[current]) < 1e-9)
                            {
                                current = neighbor;
                                path.Add(current);
                                foundPrev = true;
                                break;
                            }
                        }
                    }
                    if (!foundPrev)
                    {
                        return new List<int>();
                    }
                }
                path.Reverse();
                return path;
            }
        }
    }
}