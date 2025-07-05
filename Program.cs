using System;
using System.IO;
using System.Globalization;

namespace QuickSortAlgorithm
{
    class Program
    {
        private const string HistoryFilePath = "sort_history.txt";
        private const string InputArrayFilePath = "input_array.txt";
        private static readonly Random random = new Random();
        private static PivotSelectionStrategy pivotStrategy = PivotSelectionStrategy.Random;

        static void Main()
        {
            InitializeInputFile();

            while (true)
            {
                Console.WriteLine("1. Отсортировать массив");
                Console.WriteLine("2. Сортировка из файла");
                Console.WriteLine("3. Рандомный массив");
                Console.WriteLine("4. Показать историю сортировок");
                Console.WriteLine("5. Очистить историю");
                Console.WriteLine("6. Изменить стратегию выбора pivot");
                Console.WriteLine("7. Выход");
                Console.Write("Выберите действие: ");

                string? choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        SortArray();
                        break;
                    case "2":
                        SortFromFile();
                        break;
                    case "3":
                        GenerateAndSortRandomArray();
                        break;
                    case "4":
                        ShowSortHistory();
                        break;
                    case "5":
                        ClearHistory();
                        break;
                    case "6":
                        ChangePivotStrategy();
                        break;
                    case "7":
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

        private static void ChangePivotStrategy()
        {
            Console.WriteLine("Выберите стратегию выбора pivot:");
            Console.WriteLine("1. Случайный элемент (по умолчанию)");
            Console.WriteLine("2. Первый элемент");
            Console.WriteLine("3. Последний элемент");
            Console.WriteLine("4. Средний элемент");
            Console.Write("Ваш выбор: ");

            string? choice = Console.ReadLine();
            switch (choice)
            {
                case "1":
                    pivotStrategy = PivotSelectionStrategy.Random;
                    break;
                case "2":
                    pivotStrategy = PivotSelectionStrategy.First;
                    break;
                case "3":
                    pivotStrategy = PivotSelectionStrategy.Last;
                    break;
                case "4":
                    pivotStrategy = PivotSelectionStrategy.Middle;
                    break;
                default:
                    Console.WriteLine("Неверный выбор. Используется случайный элемент.");
                    pivotStrategy = PivotSelectionStrategy.Random;
                    break;
            }
            Console.WriteLine($"Выбрана стратегия: {pivotStrategy}");
        }

        private static string FormatNumber(double number)
        {
            return number % 1 == 0 ? number.ToString("0") : number.ToString("0.0##");
        }

        private static void InitializeInputFile()
        {
            try
            {
                if (!File.Exists(InputArrayFilePath))
                {
                    File.WriteAllText(InputArrayFilePath, "3.5 2 1.2 4 5");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка: {ex.Message}");
            }
        }

        private static void GenerateAndSortRandomArray()
        {
            try
            {
                Console.Write("Введите количество элементов: ");
                string? sizeInput = Console.ReadLine();
                if (!int.TryParse(sizeInput, out int size) || size <= 0)
                {
                    Console.WriteLine("Некорректный ввод. Введите целое число > 0.");
                    return;
                }

                Console.Write("Введите минимальное значение: ");
                string? minInput = Console.ReadLine();
                if (!double.TryParse(minInput, NumberStyles.Any, CultureInfo.InvariantCulture, out double min))
                {
                    Console.WriteLine("Некорректный ввод минимального значения.");
                    return;
                }

                Console.Write("Введите максимальное значение: ");
                string? maxInput = Console.ReadLine();
                if (!double.TryParse(maxInput, NumberStyles.Any, CultureInfo.InvariantCulture, out double max))
                {
                    Console.WriteLine("Некорректный ввод максимального значения.");
                    return;
                }

                if (min >= max)
                {
                    Console.WriteLine("Максимальное значение должно быть больше минимального.");
                    return;
                }

                double[] array = new double[size];
                for (int i = 0; i < size; i++)
                {
                    array[i] = Math.Round(min + (max - min) * random.NextDouble(), 1);
                }

                string arrayStr = string.Join(" ", Array.ConvertAll(array, x => FormatNumber(x)));
                Console.WriteLine($"Сгенерированный массив: {string.Join(", ", Array.ConvertAll(array, x => FormatNumber(x)))}");

                ProcessAndSortArray(arrayStr, "Рандомный массив");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка: {ex.Message}");
            }
        }

        private static void SortArray()
        {
            Console.WriteLine("Введите числа через пробел:");
            string? input = Console.ReadLine();

            if (string.IsNullOrWhiteSpace(input))
            {
                Console.WriteLine("Ввод не может быть пустым.");
                return;
            }

            ProcessAndSortArray(input.Replace(',', '.'), "Ручной ввод");
        }

        private static void SortFromFile()
        {
            try
            {
                if (!File.Exists(InputArrayFilePath))
                {
                    Console.WriteLine($"Файл {InputArrayFilePath} не найден.");
                    return;
                }

                string input = File.ReadAllText(InputArrayFilePath);
                Console.WriteLine($"Содержимое файла: {input}");
                ProcessAndSortArray(input.Replace(',', '.'), "Сортировка из файла");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка: {ex.Message}");
            }
        }

        private static void ProcessAndSortArray(string input, string sortType)
        {
            try
            {
                string normalizedInput = input.Replace("−", "-").Replace(',', '.');
                string[] elements = normalizedInput.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

                double[] numbers = Array.ConvertAll(elements, s => {
                    string trimmed = s.Trim();
                    if (trimmed == "-0" || trimmed == "-0.0") return 0;
                    return double.Parse(trimmed, NumberStyles.Any, CultureInfo.InvariantCulture);
                });

                double[] original = (double[])numbers.Clone();

                DateTime start = DateTime.Now;
                double[] sorted = QuickSortAlgorithm.QuickSort(numbers, 0, numbers.Length - 1, pivotStrategy);
                TimeSpan time = DateTime.Now - start;

                Console.WriteLine($"Отсортированный массив: {string.Join(", ", Array.ConvertAll(sorted, x => FormatNumber(x)))}");
                Console.WriteLine($"Время сортировки: {time.TotalMilliseconds} мс");
                Console.WriteLine($"Использованная стратегия pivot: {pivotStrategy}");

                string outputFileName = string.Empty;
                if (sortType != "Сортировка из файла")
                {
                    Console.Write("Введите имя файла для сохранения результата (без расширения): ");
                    string? fileName = Console.ReadLine();
                    if (!string.IsNullOrWhiteSpace(fileName))
                    {
                        outputFileName = fileName + ".txt";
                        try
                        {
                            File.WriteAllText(outputFileName, string.Join(" ", Array.ConvertAll(sorted, x => FormatNumber(x))));
                            Console.WriteLine($"Результат сохранен в файл: {outputFileName}");
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine($"Ошибка при сохранении файла: {ex.Message}");
                        }
                    }
                }

                File.AppendAllLines(HistoryFilePath, new[]
                {
                    $"{DateTime.Now:yyyy-MM-dd HH:mm:ss} | Тип: {sortType} | " +
                    $"Исходный массив: [{string.Join(", ", Array.ConvertAll(original, x => FormatNumber(x)))}] | " +
                    $"Отсортированный массив: [{string.Join(", ", Array.ConvertAll(sorted, x => FormatNumber(x)))}] | " +
                    $"Время: {time.TotalMilliseconds} мс | " +
                    $"Стратегия pivot: {pivotStrategy} | " +
                    $"Файл результата: {(string.IsNullOrEmpty(outputFileName) ? "не сохранен" : outputFileName)}"
                });
            }
            catch (FormatException)
            {
                Console.WriteLine("Ошибка: некорректный формат чисел. Используйте точку или запятую как разделитель дробной части.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка: {ex.Message}");
            }
        }

        private static void ShowSortHistory()
        {
            try
            {
                if (!File.Exists(HistoryFilePath) || new FileInfo(HistoryFilePath).Length == 0)
                {
                    Console.WriteLine("История пуста.");
                    return;
                }

                Console.WriteLine("История сортировок:");
                Console.WriteLine(File.ReadAllText(HistoryFilePath));
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка: {ex.Message}");
            }
        }

        private static void ClearHistory()
        {
            try
            {
                if (File.Exists(HistoryFilePath))
                {
                    File.Delete(HistoryFilePath);
                    Console.WriteLine("История очищена.");
                }
                else
                {
                    Console.WriteLine("История уже пуста.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка: {ex.Message}");
            }
        }
    }

    public enum PivotSelectionStrategy
    {
        Random,
        First,
        Last,
        Middle
    }
}