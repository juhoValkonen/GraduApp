using BenchmarkDotNet.Attributes;

namespace GraduApp.Benchmark.Benchmarks
{
    public class SortingBenchMark
    {
        private readonly int[] numbers;

        public SortingBenchMark()
        {
            numbers = new int[] { 1, 45, 45, 2, 3, 5, 67, 345, 2, 8, 78, 356, 67, 3, 23, 5 };
        }


        public static void InsertionSort(int[] input)
        {
            for (int i = 0; i < input.Count(); i++)
            {
                int item = input[i];
                int currentIndex = i;

                while (currentIndex > 0 && input[currentIndex - 1] > item)
                {
                    input[currentIndex] = input[currentIndex - 1];
                    currentIndex--;
                }

                input[currentIndex] = item;
            }
        }

        public static void SelectionSort(int[] input)
        {
            for (int i = 0; i < input.Length; i++)
            {
                int min = i;
                for (int j = i + 1; j < input.Length; j++)
                {
                    if (input[min] > input[j])
                    {
                        min = j;
                    }
                }

                if (min != i)
                {
                    (input[i], input[min]) = (input[min], input[i]);
                }
            }
        }

        [Benchmark]
        public void SortWithSelection()
        {
            SelectionSort(numbers);
        }

        [Benchmark]
        public void SortWithInsertion()
        {
            InsertionSort(numbers);
        }
    }
}
