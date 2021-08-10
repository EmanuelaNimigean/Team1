using System;
using Team1Project.Models;
using Xunit;

namespace Team1Project.Test
{
    public class InternTests
    {
        [Fact]
        public void TestGetAgeEqual()
        {
            // Assume
            Intern intern = new Intern();
            intern.BirthDate = new DateTime(2000, 01, 01);
            int expectedAge = 21;

            // Act
            int computedAge = intern.getAge();

            // Assert
            Assert.Equal(expectedAge, computedAge);
        }

        [Fact]
        public void TestGetAgeNotEqual()
        {
            // Assume
            Intern intern = new Intern();
            intern.BirthDate = new DateTime(2000, 01, 01);
            int expectedAge = 1;

            // Act
            int computedAge = intern.getAge();

            // Assert
            Assert.NotEqual(expectedAge, computedAge);
        }
    }
}