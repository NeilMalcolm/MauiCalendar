using CalendarLib;
using NUnit.Framework;
using System;
using System.Linq;

namespace MauiCalendarTests
{
    public class DateBuilderTests
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        [TestCase(2022, 04, 01)]
        [TestCase(2022, 04, 30)]
        [TestCase(2021, 12, 31)]
        [TestCase(2022, 05, 01)]
        [TestCase(2020, 02, 1)] // leap year
        public void WhenGivenMonth_AllElementsAreNotNull(int year, int month, int day)
        {
            // Arrange
            var testMonth = new DateOnly(year, month, day);

            // Act
            var result = DateBuilder.FromMonth(testMonth);
            
            // Assert
            foreach (var item in result)
            {
                Assert.IsNotNull(item);
            }
        }

        [Test]
        [TestCase(2022, 04, 01, 4, 8)]
        [TestCase(2021, 12, 31, 2, 9)]
        [TestCase(2022, 05, 01, 6, 5)]
        [TestCase(2020, 02, 1, 5, 8)] // leap year
        public void WhenGivenMonth_DifferentMonthDaysAreCorrect
        (
            int year, 
            int month, 
            int day, 
            int expectedPreviousMonthCount, 
            int expectedNextMonthCount
        )
        {
            // Arrange
            var testMonth = new DateOnly(year, month, day);
            
            // Act
            var result = DateBuilder.FromMonth(testMonth);

            // Assert
            var expectedPreviousMonthItems = result.Take(7)
                .ToList();
            var expectedNextMonthItems = result.TakeLast(14)
                .ToList();

            Assert.AreEqual(expectedPreviousMonthCount, expectedPreviousMonthItems.Where(x => x.IsDifferentMonth).ToList().Count);

            Assert.AreEqual(expectedNextMonthCount, expectedNextMonthItems.Where(x => x.IsDifferentMonth).ToList().Count);
        }

        [Test]
        [TestCase(2022, 04, 01, 4, 8)]
        [TestCase(2021, 12, 31, 2, 9)]
        [TestCase(2022, 05, 01, 6, 5)]
        [TestCase(2020, 02, 1, 5, 8)] // leap year
        public void WhenGivenMonth_SameMonthDaysAreCorrect
        (
            int year, 
            int month, 
            int day
        )
        {
            // Arrange
            var testMonth = new DateOnly(year, month, day);
            
            // Act
            var result = DateBuilder.FromMonth(testMonth);

            // Assert
            Assert.AreEqual
            (
                DateTime.DaysInMonth(year, month), 
                result.Where(x => !x.IsDifferentMonth).ToList().Count
            );
        }
    }
}