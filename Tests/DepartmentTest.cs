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

        [Fact]
        public void Update_UpdateInDatabase_true()
        {
            //Arrange
            string name = "Math";

            Department testDepartment = new Department(name);
            testDepartment.Save();
            string newName = "Mathematics";

            //Act
            testDepartment.Update(newName);
            Department result = Department.GetAll()[0];
                Console.WriteLine(result.GetMajor());
                Console.WriteLine(testDepartment.GetMajor());

            //Assert
            Assert.Equal(testDepartment, result);
            // Assert.Equal(newName, result.GetName());
        }

        [Fact]
        public void AddStudent_AddStudentToDepartment_true()
        {
            Department testDepartment = new Department("Business");
            testDepartment.Save();
            Student testStudent = new Student("Bill Jones", "March 30th");
            testStudent.Save();
            testDepartment.AddStudent(testStudent.GetId());

            List<Student> testList = testDepartment.GetStudents();
            List<Student> testList2 = new List<Student>(){testStudent};

            Assert.Equal(testList2, testList);
        }



        public void Dispose()
        {
            Department.DeleteAll();
            // Student.DeleteAll();
            // Course.DeleteAll();
        }
    }
}
