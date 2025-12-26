using System;
using System.Collections.Generic;

namespace EkzKuranov
{
    class Program
    {
        static void Main()
        {
            Console.WriteLine("Введите количество точек на карте:");
            int n = int.Parse(Console.ReadLine());

            double[,] matrix = new double[n, n];

            for (int i = 0; i < n; i++)
                for (int j = 0; j < n; j++)
                    matrix[i, j] = (i == j) ? 0 : Double.MaxValue;

            Console.WriteLine("Введите связи между точками в формате: точка1 точка2 расстояние");
            Console.WriteLine("Для завершения ввода связей введите '0 0 0' (Три нуля)");

            while (true)
            {
                string[] input = Console.ReadLine().Split();
                int from = int.Parse(input[0]);
                int to = int.Parse(input[1]);
                double distance = double.Parse(input[2]);

                if (from == 0 && to == 0 && distance == 0)
                    break;

                matrix[from - 1, to - 1] = distance;
                matrix[to - 1, from - 1] = distance;
            }

            Console.WriteLine("Введите расход топлива на 100 км:");
            double fuelPer100km = double.Parse(Console.ReadLine());

        }
    }
}