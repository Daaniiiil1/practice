using System;
using System.IO;
using System.Globalization;

namespace QuickSortAlgorithm
{
    class Program
    {
        private const string HistoryFilePath = "sort_history.txt";

        static void Main()
        {
            while (true)
            {
                Console.WriteLine("1. Отсортировать массив");
                Console.WriteLine("2. Показать историю сортировок");
                Console.WriteLine("3. Очистить историю");
                Console.WriteLine("4. Выход");
                Console.Write("Выберите действие: ");

                string? choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        SortArray();
                        break;
                    case "2":
                        ShowSortHistory();
                        break;
                    case "3":
                        ClearHistory();
                        break;
                    case "4":
                        return;
                    case null:
                        Console.WriteLine("Ввод не распознан. Попробуйте снова.");
                        break;
                    default:
                        Console.WriteLine("Неверный выбор. Попробуйте снова.");
                        break;
                }

                Console.WriteLine();
            }
        }

        private static void SortArray()
        {
            Console.WriteLine("Введите элементы массива через пробел (можно вводить целые числа или дроби):");
            string? input = Console.ReadLine();

            if (string.IsNullOrWhiteSpace(input))
            {
                Console.WriteLine("Ошибка: ввод не может быть пустым.");
                return;
            }

            try
            {
                string[] elements = input.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                double[] doubleArray = Array.ConvertAll(elements, s =>
                {
                    if (double.TryParse(s, NumberStyles.Any, CultureInfo.InvariantCulture, out double result))
                        return result;
                    throw new FormatException($"Некорректный формат числа: {s}");
                });

                double[] originalArray = (double[])doubleArray.Clone();

                double[] sortedArray = QuickSortAlgorithm.QuickSort(doubleArray, 0, doubleArray.Length - 1);

                Console.WriteLine($"Отсортированный массив: {string.Join(", ", sortedArray)}");

                string historyEntry = $"{DateTime.Now:yyyy-MM-dd HH:mm:ss}: [{string.Join(", ", originalArray)}] -> [{string.Join(", ", sortedArray)}]";
                File.AppendAllLines(HistoryFilePath, new[] { historyEntry });
            }
            catch (Exception ex) when (ex is FormatException || ex is OverflowException)
            {
                Console.WriteLine($"Ошибка: {ex.Message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Неожиданная ошибка: {ex.Message}");
            }
        }

        private static void ShowSortHistory()
        {
            try
            {
                if (!File.Exists(HistoryFilePath) || new FileInfo(HistoryFilePath).Length == 0)
                {
                    Console.WriteLine("История сортировок пуста.");
                    return;
                }

                Console.WriteLine("История сортировок:");
                string[] history = File.ReadAllLines(HistoryFilePath);
                for (int i = 0; i < history.Length; i++)
                {
                    Console.WriteLine($"{i + 1}. {history[i]}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка при чтении истории: {ex.Message}");
            }
        }

        private static void ClearHistory()
        {
            try
            {
                if (File.Exists(HistoryFilePath))
                {
                    File.Delete(HistoryFilePath);
                    Console.WriteLine("История сортировок очищена.");
                }
                else
                {
                    Console.WriteLine("История сортировок уже пуста.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка при очистке истории: {ex.Message}");
            }
        }
    }
}