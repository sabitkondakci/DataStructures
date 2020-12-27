using Xunit;
using xUnitTestLibrary.app;

namespace xUnitTestProject.test
{
    public class SortingAlgorithmsTest
    {
        public XUnitTestsForSortingAlgorithms<int> sortingTest { get; private set; }

        public SortingAlgorithmsTest()
        {
            this.sortingTest = new XUnitTestsForSortingAlgorithms<int>();
        }

        [Fact]
        public void InsertionSort_IComparableArrays_OutputSortedArray()
        {
            //Arrange
            int[] array = new int[] { 1, 2, 4, 6, 8, 6, 9 };
            //Act
            sortingTest.InsertionSort(array);
            //Assert
            Assert.Equal<int>(array, new int[] { 1, 2, 4, 6, 6, 8, 9 });

        }

        [Theory]
        [InlineData(new int[] { 1, 2, 4, 6, 6, 8, 9 }, new int[] { 1, 2, 4, 6, 8, 6, 9 })]
        public void MergeSort_IComparableArrays_OutputSortedArray(int[] expectedArray, int[] array)
        {
            //Act
            sortingTest.InsertionSort(array);
            //Assert
            Assert.Equal<int>(array, expectedArray);

        }

        [Theory]
        [InlineData(new int[] { 1, 2, 4, 6, 6, 8, 9 }, new int[] { 1, 2, 4, 6, 8, 6, 9 })]
        public void BubbleSort_IComparableArrays_OutputSortedArray(int[] expectedArray, int[] array)
        {
            //Act
            sortingTest.BubbleSort(array);
            //Assert
            Assert.Equal<int>(array, expectedArray);

        }

        [Theory]
        [InlineData(new int[] { 1, 2, 4, 6, 6, 8, 9 }, new int[] { 1, 2, 4, 6, 8, 6, 9 })]
        public void QuickSort_IComparableArrays_OutputSortedArray(int[] expectedArray, int[] array)
        {
            //Act
            sortingTest.QuickSort(array,0,array.Length-1);
            //Assert
            Assert.Equal<int>(array, expectedArray);

        }

        [Theory]
        [InlineData(new int[] { 1, 2, 4, 6, 6, 8, 9 }, new int[] { 1, 2, 4, 6, 8, 6, 9 })]
        public void ShellSort_IComparableArrays_OutputSortedArray(int[] expectedArray, int[] array)
        {
            //Act
            sortingTest.ShellSort(array);
            //Assert
            Assert.Equal<int>(array, expectedArray);

        }

        [Theory]
        [InlineData(new int[] { 1, 2, 4, 6, 8, 6, 9 },9)]
        public void BinarySearch_IComparableArrays_OutputSortedArray(int[] array,int expectedValue)
        {
            //Act
            int actualValue = sortingTest.BinarySearch(expectedValue, array);
            //Assert
            Assert.Equal<int>(expectedValue, actualValue);

        }

    }
}
