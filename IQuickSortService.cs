using System;

namespace QuickSortAlgorithm
{
    public static class QuickSortAlgorithm
    {
        private static readonly Random rand = new Random();

        public static T[] QuickSort<T>(T[] array, int minIndex, int maxIndex) where T : IComparable<T>
        {
            if (array == null)
                throw new ArgumentNullException(nameof(array));

            if (minIndex >= maxIndex)
                return array;

            int pivotIndex = GetPivotIndex(array, minIndex, maxIndex);

            QuickSort(array, minIndex, pivotIndex - 1);
            QuickSort(array, pivotIndex + 1, maxIndex);

            return array;
        }

        private static int GetPivotIndex<T>(T[] array, int minIndex, int maxIndex) where T : IComparable<T>
        {
            int randomIndex = rand.Next(minIndex, maxIndex + 1);
            Swap(ref array[randomIndex], ref array[maxIndex]);

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

        private static void Swap<T>(ref T leftItem, ref T rightItem)
        {
            (leftItem, rightItem) = (rightItem, leftItem);
        }
    }
}
