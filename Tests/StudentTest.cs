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

        public void Dispose()
        {
            Student.DeleteAll();
        }

    }
}
