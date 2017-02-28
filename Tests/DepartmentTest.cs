using Xunit;
using System.Collections.Generic;
using System;
using System.Data;
using System.Data.SqlClient;
using Registrar.Objects;

namespace Registrar
{

    public class DepartmentTest : IDisposable
    {
        public DepartmentTest()
        {
            DBConfiguration.ConnectionString = "Data Source=(localdb)\\mssqllocaldb;Initial Catalog=registrar_test;Integrated Security=SSPI;";
        }

        [Fact]
        public void Test_InputEqualsOutput()
        {
            //Arrange, Act
            int result = Department.GetAll().Count;

            //Assert
            Assert.Equal(0, result);
        }

        [Fact]
        public void Test_Equal_ReturnsTrueIfNamesAreTheSame()
        {
            Department firstDepartment = new Department("English");
            Department secondDepartment = new Department("English");
            Assert.Equal(firstDepartment, secondDepartment);
        }

        [Fact]
        public void Test_Save_ReturnsSavedDepartment()
        {
          Department testDepartment = new Department("CHID");
          testDepartment.Save();

          List<Department> totalDepartments = Department.GetAll();
          List<Department> testDepartments = new List<Department>{testDepartment};

          Assert.Equal(testDepartments, totalDepartments);
        }

        [Fact]
        public void Test_Save_AssignsIdToObject()
        {
          Department testDepartment = new Department("CHID");

          testDepartment.Save();
          Department savedDepartment = Department.GetAll()[0];

          int result = savedDepartment.GetId();
          int testId = testDepartment.GetId();
          Assert.Equal(testId, result);
        }

        [Fact]
        public void Find_FindsDepartmentInDatabase()
        {
          //Arrange
          Department testDepartment = new Department("Biology");
          testDepartment.Save();

          //Act
          Department foundDepartment = Department.Find(testDepartment.GetId());

          //Assert
          Assert.Equal(testDepartment, foundDepartment);
        }


        public void Dispose()
        {
            Department.DeleteAll();
            // Student.DeleteAll();
            // Course.DeleteAll();
        }
    }
}
