using Xunit;
using System.Collections.Generic;
using System;
using System.Data;
using System.Data.SqlClient;
using Registrar.Objects;

namespace Registrar
{
    public class CourseTest : IDisposable
    {
        public CourseTest()
        {
            DBConfiguration.ConnectionString = "Data Source=(localdb)\\mssqllocaldb;Initial Catalog=registrar_test;Integrated Security=SSPI;";
        }

        [Fact]
        public void Test_InputEqualsOutput()
        {
            //Arrange, Act
            int result = Course.GetAll().Count;

            //Assert
            Assert.Equal(0, result);
        }

        [Fact]
        public void Test_Equal_ReturnsTrueIfNamesAreTheSame()
        {
            Course firstCourse = new Course("Roger", "2016-03-12", 1);
            Course secondCourse = new Course("Roger", "2016-03-12", 1);
            Assert.Equal(firstCourse, secondCourse);
        }


        [Fact]
        public void Test_Save_ReturnsSavedCourse()
        {
          Course testCourse = new Course("McDonald", "Today");
          testCourse.Save();

          List<Course> totalCourses = Course.GetAll();
          List<Course> testCourses = new List<Course>{testCourse};

          Assert.Equal(testCourses, totalCourses);
        }

        [Fact]
        public void Test_Save_AssignsIdToObject()
        {
          Course testCourse = new Course("Mcdonald", "Yesterday");

          testCourse.Save();
          Course savedCourse = Course.GetAll()[0];

          int result = savedCourse.GetId();
          int testId = testCourse.GetId();
          Assert.Equal(testId, result);
        }

        [Fact]
        public void Test_FindFindsCourseInDatabase()
        {
          //Arrange
          Course testCourse = new Course("Wendy", "two weeks ago");
          testCourse.Save();

          //Act
          Course foundCourse = Course.Find(testCourse.GetId());

          //Assert
          Assert.Equal(testCourse, foundCourse);
        }

        [Fact]
        public void Update_UpdateInDatabase_true()
        {
            //Arrange
            string name = "Matthew Smith";
            string enrollment = "Yesterday";

            Course testCourse = new Course(name, enrollment);
            testCourse.Save();
            string newName = "rake leaves";
            string newCourseNumber = "true";

            //Act
            testCourse.Update(newName, newCourseNumber);
            Course result = Course.GetAll()[0];
                Console.WriteLine(result.GetName());
                Console.WriteLine(testCourse.GetName());

                Console.WriteLine(result.GetCourseNumber());
                Console.WriteLine(testCourse.GetCourseNumber());

            //Assert
            Assert.Equal(testCourse, result);
            // Assert.Equal(newName, result.GetName());
        }

        public void Dispose()
        {
            Course.DeleteAll();
        }

    }
}
