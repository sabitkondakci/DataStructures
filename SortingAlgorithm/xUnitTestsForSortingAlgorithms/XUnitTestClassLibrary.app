using System;
using System.Linq;

namespace xUnitTestLibrary.app
{
    public class XUnitTestsForSortingAlgorithms<K> where K : IComparable<K>, IEquatable<K>
    {
        public void InsertionSort(K[] array) //Sıralanmış listelerde indeks bozulmalarını düzeltmede etkili bir yöntemdir.
        {
            if (array == null)
                throw new ArgumentNullException("array");

            for (int i = 1; i < array.Length; i++)
            {
                K v = array[i];
                for (int j = i - 1; j >= 0; j--)
                {
                    if (array[j].CompareTo(v) <= 0) //Worst case time complexity O(n^2)
                    {                               //Best case time complexity O(n), for already sorted list
                        break;
                    }

                    array[j + 1] = array[j];
                    array[j] = v;
                }
            }
        }

        public K[] InsertionSortParams(params K[] array)
        {
            if (array == null)
                throw new ArgumentNullException("array");

            for (int i = 1; i < array.Length; i++)
            {
                K v = array[i];
                for (int j = i - 1; j >= 0; j--)
                {
                    if (array[j].CompareTo(v) <= 0)
                    {
                        break;
                    }

                    array[j + 1] = array[j];
                    array[j] = v;
                }
            }

            return array;
        }

        public K BinarySearch(K value, params K[] array)
        {
            if (array == null)
                throw new ArgumentNullException("array");

            Array.Sort(array);
            int size = array.Length;
            int middleElement = 0, left = 0, right = array.Length - 1;

            while (left <= right)
            {
                middleElement = (left + right) / 2;

                if (array[middleElement].CompareTo(value) < 0)
                {
                    left = middleElement + 1;
                }
                else if (array[middleElement].CompareTo(value) > 0)
                {
                    right = middleElement - 1;
                }
                else
                {
                    return array[middleElement];
                }
            }

            return default;

        }

        public void BubbleSort(params K[] array)
        {
            if (array == null)
                throw new ArgumentNullException("array");
            int repeat = array.Length - 1;
            for (int i = 0; i < repeat; i++)
            {
                int checkFlag = 0;
                for (int j = 0; j < repeat - i; j++)
                {
                    if (array[j].CompareTo(array[j + 1]) > 0)
                    {
                        SwapSort(j, j + 1, array);
                        checkFlag = 1;
                    }
                }

                if (checkFlag == 0) break;
            }


        }//O(n^2) worst case time complexity

        public void ShellSort(K[] array)
        {
            if (array == null)
                throw new ArgumentNullException("array");

            int n = array.Length;
            for (int gap = n / 2; gap >= 1; gap /= 2)
            {
                for (int k = gap; k < n; k++)
                {
                    for (int t = k - gap; t >= 0; t -= gap)
                    {
                        if (array[t + gap].CompareTo(array[t]) > 0)
                        {
                            break;
                        }
                        else
                        {
                            SwapSort(t, t + gap, array);
                        }
                    }
                }
            }
        }

        public void QuickSort(K[] arr, int leftIndex, int rightIndex)
        {
            // K[] arr: Due to the fact that array is a bunch of pointers,
            // you may pass it as a parameter to change pointers' positions
            if (arr == null)
                throw new ArgumentNullException("array");

            K pivot;
            int left, right;
            if (leftIndex >= rightIndex) return;

            left = leftIndex;
            right = rightIndex;

            pivot = arr[(left + right) / 2];

            while (left < right)
            {
                while (arr[left].CompareTo(pivot) < 0) left++;
                while (arr[right].CompareTo(pivot) > 0) right--;

                if (left <= right)
                {
                    SwapSort(left++, right--, arr);
                }
            }

            QuickSort(arr, leftIndex, right);
            QuickSort(arr, left, rightIndex);

        }

        public K[] MWaySortedMerging(K[] firstArray, K[] secondArray)
        {
            if (firstArray == null || secondArray == null)
                throw new ArgumentNullException("firstArray or secondArray is null");

            Array.Sort(firstArray);
            Array.Sort(secondArray);

            K[] fetchArray = new K[firstArray.Length + secondArray.Length];
            int i = 0, j = 0, k = 0;

            while (i < firstArray.Length && j < secondArray.Length)
            {
                if (firstArray[i].CompareTo(secondArray[j]) > 0)
                {
                    fetchArray[k++] = secondArray[j++];
                }
                else
                {
                    fetchArray[k++] = firstArray[i++];
                }
            }

            if (i == firstArray.Length)
            {
                while (j < secondArray.Length)
                {
                    fetchArray[k++] = secondArray[j++];
                }
            }
            else
            {
                while (i < firstArray.Length)
                {
                    fetchArray[k++] = firstArray[i++];
                }
            }

            return fetchArray;

        }
        //Merging the elements of jaggedArray
        public K[] MultipleMerging(K[][] jaggedArray)
        {
            if (jaggedArray == null || jaggedArray.Length == 1)
                throw new ArgumentNullException("jaggedArray parameter is null or jaggedArray has single/zero element");

            int lastRepeat = jaggedArray.Length;
            //For the sake of maximum performance we should sort the jaggedArray on the basis of objects' Length
            K[][] sortedJaggedList = jaggedArray.OrderBy(x => x?.Length).ToArray();
            K[] tempReturn = sortedJaggedList[0];

            for (int i = 1; i < lastRepeat; i++)
            {
                tempReturn = MWaySortedMerging(tempReturn, sortedJaggedList[i]);
            }

            return tempReturn;
        }

        public void MergeSort(K[] arr, int lowerBound, int upperBound)//O(m+n) time complexity at worst case
        {
            //Best case time complexity O(nlogn)
            if (arr == null)
                throw new ArgumentNullException("array");

            if (lowerBound < upperBound)
            {
                int midIndex = (lowerBound + upperBound) / 2;
                MergeSort(arr, lowerBound, midIndex);
                MergeSort(arr, midIndex + 1, upperBound);
                Merge(arr, lowerBound, midIndex, upperBound);
            }
        }

        private void Merge(K[] arr, int lowerBound, int midIndex, int upperBound)
        {
            int i = lowerBound, j = midIndex + 1, k = lowerBound;
            K[] tempArr = new K[upperBound + 1];

            while (i <= midIndex && j <= upperBound)
            {
                if (arr[i].CompareTo(arr[j]) <= 0)
                {
                    tempArr[k++] = arr[i++];
                }
                else
                {
                    tempArr[k++] = arr[j++];
                }
            }

            if (i > midIndex)
            {
                while (j <= upperBound)
                {
                    tempArr[k++] = arr[j++];
                }
            }
            else
            {
                while (i <= midIndex)
                {
                    tempArr[k++] = arr[i++];
                }
            }

            for (int t = lowerBound; t <= upperBound; t++)
            {
                arr[t] = tempArr[t];
            }
        }

        private void SwapSort(int x, int y, K[] array)
        {
            if (array == null)
                throw new ArgumentNullException("array");

            K temp = array[x];
            array[x] = array[y];
            array[y] = temp;
        }

    }
}
