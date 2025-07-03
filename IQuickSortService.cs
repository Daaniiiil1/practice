using System;

namespace QuickSortAlgorithm
{
    public static class QuickSortAlgorithm
    {
        private static readonly Random random = new Random();

        public static T[] QuickSort<T>(T[] array, int minIndex, int maxIndex, PivotSelectionStrategy strategy = PivotSelectionStrategy.Random) where T : IComparable<T>
        {
            if (array == null)
                throw new ArgumentNullException(nameof(array));

            if (minIndex >= maxIndex)
                return array;

            int pivotIndex = GetPivotIndex(array, minIndex, maxIndex, strategy);

            QuickSort(array, minIndex, pivotIndex - 1, strategy);
            QuickSort(array, pivotIndex + 1, maxIndex, strategy);

            return array;
        }

        private static int GetPivotIndex<T>(T[] array, int minIndex, int maxIndex, PivotSelectionStrategy strategy) where T : IComparable<T>
        {
            int pivotIndex = ChoosePivotIndex(array, minIndex, maxIndex, strategy);

            Swap(ref array[pivotIndex], ref array[maxIndex]);

            int pivot = minIndex - 1;

            for (int i = minIndex; i <= maxIndex; i++)
            {
                if (array[i].CompareTo(array[maxIndex]) < 0)
                {
                    pivot++;
                    Swap(ref array[pivot], ref array[i]);
                }
            }

            pivot++;
            Swap(ref array[pivot], ref array[maxIndex]);

            return pivot;
        }

        private static int ChoosePivotIndex<T>(T[] array, int minIndex, int maxIndex, PivotSelectionStrategy strategy) where T : IComparable<T>
        {
            return strategy switch
            {
                PivotSelectionStrategy.First => minIndex,
                PivotSelectionStrategy.Last => maxIndex,
                PivotSelectionStrategy.Middle => (minIndex + maxIndex) / 2,
                _ => random.Next(minIndex, maxIndex + 1) // Random по умолчанию
            };
        }

        private static void Swap<T>(ref T leftItem, ref T rightItem)
        {
            (leftItem, rightItem) = (rightItem, leftItem);
        }
    }
}