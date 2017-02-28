using Xunit;
using System.Collections.Generic;
using System;
using System.Data;
using System.Data.SqlClient;
using Registrar.Objects;

namespace Registrar
{
    public class StudentTest : IDisposable
    {
        public StudentTest()
        {
            DBConfiguration.ConnectionString = "Data Source=(localdb)\\mssqllocaldb;Initial Catalog=registrar_test;Integrated Security=SSPI;";
        }

        [Fact]
        public void Test_InputEqualsOutput()
        {
            //Arrange, Act
            int result = Student.GetAll().Count;

            //Assert
            Assert.Equal(0, result);
        }

        [Fact]
        public void Test_Equal_ReturnsTrueIfNamesAreTheSame()
        {
            Student firstStudent = new Student("Roger", "2016-03-12", 1);
            Student secondStudent = new Student("Roger", "2016-03-12", 1);
            Assert.Equal(firstStudent, secondStudent);
        }


        [Fact]
        public void Test_Save_ReturnsSavedStudent()
        {
          Student testStudent = new Student("McDonald", "Today");
          testStudent.Save();

          List<Student> totalStudents = Student.GetAll();
          List<Student> testStudents = new List<Student>{testStudent};

          Assert.Equal(testStudents, totalStudents);
        }

        [Fact]
        public void Test_Save_AssignsIdToObject()
        {
          Student testStudent = new Student("Mcdonald", "Yesterday");

          testStudent.Save();
          Student savedStudent = Student.GetAll()[0];

          int result = savedStudent.GetId();
          int testId = testStudent.GetId();
          Assert.Equal(testId, result);
        }

        [Fact]
        public void Test_FindFindsStudentInDatabase()
        {
          //Arrange
          Student testStudent = new Student("Wendy", "two weeks ago");
          testStudent.Save();

          //Act
          Student foundStudent = Student.Find(testStudent.GetId());

          //Assert
          Assert.Equal(testStudent, foundStudent);
        }

        [Fact]
        public void Update_UpdateInDatabase_true()
        {
            //Arrange
            string name = "Matthew Smith";
            string enrollment = "Yesterday";

            Student testStudent = new Student(name, enrollment);
            testStudent.Save();
            string newName = "rake leaves";
            string newEnrollment = "true";

            //Act
            testStudent.Update(newName, newEnrollment);
            Student result = Student.GetAll()[0];
                Console.WriteLine(result.GetName());
                Console.WriteLine(testStudent.GetName());

                Console.WriteLine(result.GetEnrollment());
                Console.WriteLine(testStudent.GetEnrollment());

            //Assert
            Assert.Equal(testStudent, result);
            // Assert.Equal(newName, result.GetName());
        }

        [Fact]
        public void GetCompletedCourses()
        {
            Student testStudent = new Student("Bill", "December 3");
            Course testCourse = new Course("Biology of Drugs in the Brain", "BIO110");
            testStudent.Save();
            testCourse.Save();

            testCourse.AddCompletedOrFailed(true, testStudent.GetId());
            List<Course> testCompletedCoursesList = new List<Course>{testCourse};
            List<Course> result = testStudent.GetCompletedCourses();

            Assert.Equal(testCompletedCoursesList, result);
        }

        [Fact]
        public void GetFailedCourses()
        {
            Student testStudent = new Student("Bill", "December 3");
            Course testCourse = new Course("Biology of Drugs in the Brain", "BIO110");
            testStudent.Save();
            testCourse.Save();

            testCourse.AddCompletedOrFailed(false, testStudent.GetId());
            List<Course> testFailedCoursesList = new List<Course>{testCourse};
            List<Course> result = testStudent.GetFailedCourses();

            Assert.Equal(testFailedCoursesList, result);
        }

        [Fact]
        public void Delete_DeleteSingleStudent_true()
        {
            Student testStudent = new Student("Mark", "Yesterday");
            Student testStudent2 = new Student("Sarah", "Tomorrow");
            testStudent.Save();
            testStudent2.Save();

            testStudent.Delete();
            Student foundStudent = Student.GetAll()[0];
            Assert.Equal(testStudent2, foundStudent);
         }

        public void Dispose()
        {
            Student.DeleteAll();
        }

    }
}
