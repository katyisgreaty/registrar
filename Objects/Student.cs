using System.Collections.Generic;
using System.Data.SqlClient;
using System;

namespace Registrar.Objects
{
    public class Student
    {
      private int _id;
      private string _name;
      private string _enrollment;

      public Student(string name, string enrollment, int id = 0)
      {
        _id = id;
        _name = name;
        _enrollment = enrollment;
      }

      public string GetName()
        {
            return _name;
        }

        public int GetId()
        {
            return _id;
        }

        public string GetEnrollment()
        {
            return _enrollment;
        }

        public override bool Equals(System.Object otherStudent)
       {
           if (!(otherStudent is Student))
           {
               return false;
           }
           else
           {
               Student newStudent = (Student) otherStudent;
               bool studentIdEquality = (this.GetId() == newStudent.GetId());
               bool nameEquality = (this.GetName() == newStudent.GetName());
               bool enrollmentEquality = (this.GetEnrollment() == newStudent.GetEnrollment());
               return (studentIdEquality && nameEquality && enrollmentEquality);
           }
       }

       public static List<Student> GetAll()
        {
            List<Student> allStudents = new List<Student>{};

            SqlConnection conn = DB.Connection();
            conn.Open();

            SqlCommand cmd = new SqlCommand("SELECT * FROM students;", conn);
            SqlDataReader rdr = cmd.ExecuteReader();

            while(rdr.Read())
            {
                int studentId = rdr.GetInt32(0);
                string studentName = rdr.GetString(1);
                string enrollment = rdr.GetString(2);
                Student newStudent = new Student(studentName,enrollment, studentId);
                allStudents.Add(newStudent);
            }
            if(rdr != null)
            {
                rdr.Close();
            }
            if(conn != null)
            {
                conn.Close();
            }
            return allStudents;
        }

       public static void DeleteAll()
        {
          SqlConnection conn = DB.Connection();
          conn.Open();
          SqlCommand cmd = new SqlCommand("DELETE FROM students;", conn);
          cmd.ExecuteNonQuery();
          conn.Close();
        }

    }
}
