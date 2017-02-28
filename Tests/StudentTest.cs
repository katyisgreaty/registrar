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

        public void Dispose()
        {
            Student.DeleteAll();
        }

    }
}
