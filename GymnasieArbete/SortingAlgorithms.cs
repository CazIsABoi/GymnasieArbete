using System;
using System.Collections.Generic;
using System.Linq;

namespace GymnasieArbete
{
    public class SortingAlgorithms
    {
        public interface ISortingAlgorithm<T>
        {
            T[] Items { get; }
            int CurrentI { get; }
            int CurrentJ { get; }
            bool Step();
            void Reset(T[] items);
            string StatusMessage { get; }
        }

        public class BubbleSort<T> : ISortingAlgorithm<T> where T : IComparable<T>
        {
            public T[] Items { get; private set; }
            public int CurrentI { get; private set; }
            public int CurrentJ { get; private set; }
            public string StatusMessage { get; private set; }

            private bool swapped;

            public void Reset(T[] items)
            {
                Items = (T[])items.Clone();
                CurrentI = 0;
                CurrentJ = 0;
                swapped = false;
                StatusMessage = "Ready to start";
            }

            public bool Step()
            {
                if (CurrentI < Items.Length)
                {
                    if (CurrentJ == 0) swapped = false;

                    if (CurrentJ < Items.Length - CurrentI - 1)
                    {
                        StatusMessage = $"Comparing {Items[CurrentJ]} and {Items[CurrentJ + 1]}";
                        if (Items[CurrentJ].CompareTo(Items[CurrentJ + 1]) > 0)
                        {
                            T temp = Items[CurrentJ];
                            Items[CurrentJ] = Items[CurrentJ + 1];
                            Items[CurrentJ + 1] = temp;
                            swapped = true;
                            StatusMessage = $"Swapped {Items[CurrentJ + 1]} and {Items[CurrentJ]}";
                        }
                        CurrentJ++;
                    }
                    else
                    {
                        if (!swapped)
                        {
                            StatusMessage = "Finished (no swaps in pass)";
                            CurrentI = Items.Length; // End early
                            return false;
                        }
                        CurrentJ = 0;
                        CurrentI++;
                        StatusMessage = $"Completed pass {CurrentI}";
                    }
                    return true;
                }
                StatusMessage = "Finished";
                return false;
            }
        }

        public class InsertionSort<T> : ISortingAlgorithm<T> where T : IComparable<T>
        {
            public T[] Items { get; private set; }
            public int CurrentI { get; private set; }
            public int CurrentJ { get; private set; }
            private T key;
            private bool keyPicked;
            public string StatusMessage { get; private set; }

            public void Reset(T[] items)
            {
                Items = (T[])items.Clone();
                CurrentI = 1;
                CurrentJ = 0;
                key = default!;
                keyPicked = false;
                StatusMessage = "Ready to start";
            }

            public bool Step()
            {
                if (CurrentI < Items.Length)
                {
                    if (!keyPicked)
                    {
                        key = Items[CurrentI];
                        CurrentJ = CurrentI - 1;
                        keyPicked = true;
                        StatusMessage = $"Picked key {key}";
                    }
                    if (CurrentJ >= 0 && Items[CurrentJ].CompareTo(key) > 0)
                    {
                        StatusMessage = $"Shifting {Items[CurrentJ]} right";
                        Items[CurrentJ + 1] = Items[CurrentJ];
                        CurrentJ--;
                        return true;
                    }
                    Items[CurrentJ + 1] = key;
                    StatusMessage = $"Inserted key {key} at position {CurrentJ + 1}";
                    CurrentI++;
                    keyPicked = false;
                    return true;
                }
                StatusMessage = "Finished";
                return false;
            }
        }

        public class QuickSort<T> : ISortingAlgorithm<T> where T : IComparable<T>
        {
            public T[] Items { get; private set; }
            public int CurrentI { get; private set; }
            public int CurrentJ { get; private set; }
            public string StatusMessage { get; private set; }

            private Stack<(int left, int right)> stack;
            private int left, right, i, j;
            private T pivot;
            private bool partitioning;

            private T MedianOfThree(T a, T b, T c)
            {
                if ((a.CompareTo(b) > 0) == (a.CompareTo(c) < 0)) return a;
                if ((b.CompareTo(a) > 0) == (b.CompareTo(c) < 0)) return b;
                return c;
            }

            public void Reset(T[] items)
            {
                Items = (T[])items.Clone();
                stack = new Stack<(int, int)>();
                stack.Push((0, Items.Length - 1));
                partitioning = false;
                left = right = i = j = -1;
                pivot = default!;
                CurrentI = CurrentJ = -1;
                StatusMessage = "Ready to start";
            }

            public bool Step()
            {
                if (!partitioning)
                {
                    if (stack.Count == 0)
                    {
                        StatusMessage = "Finished";
                        return false;
                    }

                    (left, right) = stack.Pop();
                    if (left >= right)
                    {
                        StatusMessage = $"Subarray [{left}, {right}] already sorted";
                        return true;
                    }

                    int mid = left + (right - left) / 2;
                    T a = Items[left], b = Items[mid], c = Items[right];
                    T median = MedianOfThree(a, b, c);
                    if (median.Equals(b))
                        (Items[mid], Items[right]) = (Items[right], Items[mid]);
                    else if (median.Equals(a))
                        (Items[left], Items[right]) = (Items[right], Items[left]);
                    pivot = Items[right];

                    i = left - 1;
                    j = left;
                    partitioning = true;
                    StatusMessage = $"Partitioning [{left}, {right}] with pivot {pivot}";
                }

                if (j < right)
                {
                    CurrentI = j;
                    CurrentJ = right;
                    StatusMessage = $"Comparing {Items[j]} with pivot {pivot}";
                    if (Items[j].CompareTo(pivot) < 0)
                    {
                        i++;
                        (Items[i], Items[j]) = (Items[j], Items[i]);
                        StatusMessage = $"Swapped {Items[i]} and {Items[j]}";
                    }
                    j++;
                    return true;
                }
                else
                {
                    (Items[i + 1], Items[right]) = (Items[right], Items[i + 1]);
                    StatusMessage = $"Placed pivot {pivot} at position {i + 1}";
                    stack.Push((left, i));
                    stack.Push((i + 2, right));
                    partitioning = false;
                    return true;
                }
            }
        }

        public class MergeSort<T> : ISortingAlgorithm<T> where T : IComparable<T>
        {
            public T[] Items { get; private set; }
            public int CurrentI { get; private set; }
            public int CurrentJ { get; private set; }
            public string StatusMessage { get; private set; }

            private T[] aux;
            private int width;
            private Queue<(int left, int mid, int right)> mergeJobs;
            private int left, mid, right, i, j, k;
            private bool merging;
            private bool done;
            private bool copyBack;
            private int copyIdx;

            public void Reset(T[] items)
            {
                Items = (T[])items.Clone();
                aux = new T[Items.Length];
                width = 1;
                mergeJobs = new Queue<(int, int, int)>();
                merging = false;
                done = false;
                copyBack = false;
                copyIdx = 0;
                CurrentI = CurrentJ = -1;
                StatusMessage = "Ready to start";
                i = j = k = 0;
            }

            public bool Step()
            {
                if (done)
                {
                    StatusMessage = "Finished";
                    return false;
                }

                if (copyBack)
                {
                    if (copyIdx < Items.Length)
                    {
                        Items[copyIdx] = aux[copyIdx];
                        CurrentJ = copyIdx;
                        StatusMessage = $"Copying back index {copyIdx}";
                        copyIdx++;
                        return true;
                    }
                    else
                    {
                        width *= 2;
                        merging = false;
                        copyBack = false;
                        copyIdx = 0;
                        StatusMessage = $"Increased merge width to {width}";
                        return true;
                    }
                }

                if (!merging)
                {
                    if (width >= Items.Length)
                    {
                        for (int idx = 0; idx < Items.Length; idx++)
                            Items[idx] = aux[idx];
                        done = true;
                        StatusMessage = "Finished";
                        return false;
                    }
                    mergeJobs.Clear();
                    for (int l = 0; l < Items.Length - 1; l += 2 * width)
                    {
                        int m = Math.Min(l + width - 1, Items.Length - 1);
                        int r = Math.Min(l + 2 * width - 1, Items.Length - 1);
                        mergeJobs.Enqueue((l, m, r));
                    }
                    merging = true;
                    StatusMessage = $"Prepared merge jobs for width {width}";
                    i = j = k = 0;
                }

                if (merging && mergeJobs.Count > 0)
                {
                    if (k == 0)
                    {
                        (left, mid, right) = mergeJobs.Peek();
                        i = left;
                        j = mid + 1;
                        k = left;
                        StatusMessage = $"Merging [{left}, {mid}] and [{mid + 1}, {right}]";
                    }

                    if (i <= mid && (j > right || Items[i].CompareTo(Items[j]) <= 0))
                    {
                        aux[k] = Items[i];
                        CurrentJ = i;
                        StatusMessage = $"Taking {Items[i]} from left";
                        i++;
                    }
                    else if (j <= right)
                    {
                        aux[k] = Items[j];
                        CurrentJ = j;
                        StatusMessage = $"Taking {Items[j]} from right";
                        j++;
                    }
                    k++;

                    if (k > right)
                    {
                        mergeJobs.Dequeue();
                        k = 0;
                        StatusMessage = $"Finished merging [{left}, {right}]";
                    }

                    return true;
                }
                else if (merging && mergeJobs.Count == 0)
                {
                    copyBack = true;
                    copyIdx = 0;
                    StatusMessage = $"Copying merged array back";
                    return true;
                }

                StatusMessage = "Finished";
                return false;
            }
        }

        public class SelectionSort<T> : ISortingAlgorithm<T> where T : IComparable<T>
        {
            public T[] Items { get; private set; }
            public int CurrentI { get; private set; }
            public int CurrentJ { get; private set; }
            public string StatusMessage { get; private set; }

            private int minIndex;
            private bool searching;

            public void Reset(T[] items)
            {
                Items = (T[])items.Clone();
                CurrentI = 0;
                CurrentJ = 1;
                minIndex = 0;
                searching = true;
                StatusMessage = "Ready to start";
            }

            public bool Step()
            {
                if (CurrentI >= Items.Length - 1)
                {
                    StatusMessage = "Finished";
                    return false;
                }

                if (searching)
                {
                    if (CurrentJ < Items.Length)
                    {
                        StatusMessage = $"Comparing {Items[CurrentJ]} with current min {Items[minIndex]}";
                        if (Items[CurrentJ].CompareTo(Items[minIndex]) < 0)
                        {
                            minIndex = CurrentJ;
                            StatusMessage = $"New min found: {Items[minIndex]} at {minIndex}";
                        }
                        CurrentJ++;
                        return true;
                    }
                    else
                    {
                        T temp = Items[CurrentI];
                        Items[CurrentI] = Items[minIndex];
                        Items[minIndex] = temp;
                        StatusMessage = $"Swapped {Items[CurrentI]} and {Items[minIndex]}";
                        CurrentI++;
                        minIndex = CurrentI;
                        CurrentJ = CurrentI + 1;
                        searching = true;
                        return true;
                    }
                }
                StatusMessage = "Finished";
                return false;
            }
        }

        public class HeapSort<T> : ISortingAlgorithm<T> where T : IComparable<T>
        {
            public T[] Items { get; private set; }
            public int CurrentI { get; private set; }
            public int CurrentJ { get; private set; }
            public string StatusMessage { get; private set; }

            private int heapSize;
            private int phase; // 0 = build heap, 1 = sort
            private int siftIndex;
            private bool sifting;

            public void Reset(T[] items)
            {
                Items = (T[])items.Clone();
                heapSize = Items.Length;
                phase = 0;
                siftIndex = heapSize / 2 - 1;
                sifting = false;
                CurrentI = -1;
                CurrentJ = -1;
                StatusMessage = "Ready to start";
            }

            public bool Step()
            {
                if (phase == 0)
                {
                    if (siftIndex >= 0)
                    {
                        StatusMessage = $"Sifting down index {siftIndex}";
                        SiftDown(siftIndex, heapSize);
                        siftIndex--;
                        return true;
                    }
                    else
                    {
                        phase = 1;
                        CurrentI = heapSize - 1;
                        StatusMessage = "Heap built, starting sort";
                    }
                }
                if (phase == 1)
                {
                    if (heapSize > 1)
                    {
                        T temp = Items[0];
                        Items[0] = Items[heapSize - 1];
                        Items[heapSize - 1] = temp;
                        StatusMessage = $"Swapped root {Items[heapSize - 1]} with {Items[0]}";
                        heapSize--;
                        SiftDown(0, heapSize);
                        CurrentI = heapSize - 1;
                        return true;
                    }
                    else
                    {
                        StatusMessage = "Finished";
                        return false;
                    }
                }
                StatusMessage = "Finished";
                return false;
            }

            private void SiftDown(int i, int n)
            {
                int largest = i;
                int left = 2 * i + 1;
                int right = 2 * i + 2;

                if (left < n && Items[left].CompareTo(Items[largest]) > 0)
                    largest = left;
                if (right < n && Items[right].CompareTo(Items[largest]) > 0)
                    largest = right;

                CurrentJ = largest;

                if (largest != i)
                {
                    T temp = Items[i];
                    Items[i] = Items[largest];
                    Items[largest] = temp;
                    StatusMessage = $"Sifted down: swapped {Items[largest]} and {Items[i]}";
                    SiftDown(largest, n);
                }
                else
                {
                    StatusMessage = $"No swap needed at index {i}";
                }
            }
        }

        public class BogoSort<T> : ISortingAlgorithm<T> where T : IComparable<T>
        {
            public T[] Items { get; private set; }
            public int CurrentI { get; private set; }
            public int CurrentJ { get; private set; }
            public string StatusMessage { get; private set; }

            private Random rand = new Random();
            private int checkIndex;

            public void Reset(T[] items)
            {
                Items = (T[])items.Clone();
                checkIndex = 0;
                CurrentI = 0;
                CurrentJ = 1;
                StatusMessage = "Ready to start";
            }

            private bool IsSorted()
            {
                for (int i = 1; i < Items.Length; i++)
                    if (Items[i - 1].CompareTo(Items[i]) > 0)
                        return false;
                return true;
            }

            public bool Step()
            {
                if (Items.Length <= 1)
                {
                    StatusMessage = "Finished";
                    return false;
                }

                if (IsSorted())
                {
                    StatusMessage = "Finished";
                    return false;
                }

                for (int i = Items.Length - 1; i > 0; i--)
                {
                    int j = rand.Next(i + 1);
                    T temp = Items[i];
                    Items[i] = Items[j];
                    Items[j] = temp;
                }
                StatusMessage = "Shuffled array";
                CurrentI = 0;
                CurrentJ = 1;
                return true;
            }
        }

        // CountingSort and BucketSort are not included here because they are not suitable for generic types
        // and are only meaningful for integer types.
    }
}
