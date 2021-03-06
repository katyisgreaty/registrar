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

        public void Save()
        {
            SqlConnection conn = DB.Connection();
            conn.Open();

            SqlCommand cmd = new SqlCommand("INSERT INTO students(name, enrollment) OUTPUT INSERTED.id VALUES(@Name, @Enrollment);", conn);

            SqlParameter nameParameter = new SqlParameter("@Name", this.GetName());
            SqlParameter enrollmentParameter = new SqlParameter("@Enrollment", this.GetEnrollment());

            cmd.Parameters.Add(nameParameter);
            cmd.Parameters.Add(enrollmentParameter);
            SqlDataReader rdr = cmd.ExecuteReader();

            while(rdr.Read())
            {
                this._id = rdr.GetInt32(0);
            }
            if (rdr != null)
            {
                rdr.Close();
            }
            if(conn != null)
            {
                conn.Close();
            }
        } // end save

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

        public List<Course> GetCompletedCourses()
        {
            List<Course> allCompletedCourses = new List<Course>{};

            SqlConnection conn = DB.Connection();
            conn.Open();

            SqlCommand cmd = new SqlCommand("SELECT courses.* FROM students JOIN students_completedcourses ON (students.id = students_completedcourses.student_id) JOIN courses ON (students_completedcourses.course_id = courses.id) WHERE students.id = @StudentId;", conn);
            SqlParameter studentIdParameter = new SqlParameter("@StudentId", this.GetId());
            cmd.Parameters.Add(studentIdParameter);
            SqlDataReader rdr = cmd.ExecuteReader();

            while(rdr.Read())
            {
                string name = rdr.GetString(1);
                int courseId = rdr.GetInt32(0);
                string courseNumber = rdr.GetString(2);
                Course completedCourse = new Course(name, courseNumber, courseId);
                allCompletedCourses.Add(completedCourse);
            }
            if(rdr != null)
            {
                rdr.Close();
            }
            if(conn != null)
            {
                conn.Close();
            }
            return allCompletedCourses;
        }

        public List<Course> GetFailedCourses()
        {
            List<Course> allFailedCourses = new List<Course>{};

            SqlConnection conn = DB.Connection();
            conn.Open();

            SqlCommand cmd = new SqlCommand("SELECT courses.* FROM students JOIN students_failedcourses ON (students.id = students_failedcourses.student_id) JOIN courses ON (students_failedcourses.course_id = courses.id) WHERE students.id = @StudentId;", conn);
            SqlParameter studentIdParameter = new SqlParameter("@StudentId", this.GetId());
            cmd.Parameters.Add(studentIdParameter);
            SqlDataReader rdr = cmd.ExecuteReader();

            while(rdr.Read())
            {
                string name = rdr.GetString(1);
                int courseId = rdr.GetInt32(0);
                string courseNumber = rdr.GetString(2);
                Course failedCourse = new Course(name, courseNumber, courseId);
                allFailedCourses.Add(failedCourse);
            }
            if(rdr != null)
            {
                rdr.Close();
            }
            if(conn != null)
            {
                conn.Close();
            }
            return allFailedCourses;
        }

        public static Student Find(int id)
        {
            SqlConnection conn = DB.Connection();
            conn.Open();
            SqlCommand cmd = new SqlCommand("SELECT * FROM students WHERE id = @Student_Id;", conn);

            SqlParameter studentIdParameter = new SqlParameter("@Student_Id", id);
            cmd.Parameters.Add(studentIdParameter);

            SqlDataReader rdr = cmd.ExecuteReader();

            int foundStudentId = 0;
            string foundStudentName = null;
            string foundEnrollment = null;

            while(rdr.Read())
            {
                foundStudentId = rdr.GetInt32(0);
                foundStudentName = rdr.GetString(1);
                foundEnrollment = rdr.GetString(2);
            }

            Student foundStudent = new Student(foundStudentName, foundEnrollment, foundStudentId);
            if (rdr != null)
            {
                rdr.Close();
            }
            if (conn != null)
            {
                conn.Close();
            }

            return foundStudent;
        }

        public void Update(string newName, string newEnrollment)
        {
            SqlConnection conn = DB.Connection();
            conn.Open();

            SqlCommand cmd = new SqlCommand("UPDATE students SET name = @NewName, enrollment = @NewEnrollment OUTPUT INSERTED.* WHERE id = @StudentId;", conn);

            SqlParameter newNameParameter = new SqlParameter();
            newNameParameter.ParameterName = "@NewName";
            newNameParameter.Value = newName;
            cmd.Parameters.Add(newNameParameter);

            SqlParameter newEnrollmentParameter = new SqlParameter();
            newEnrollmentParameter.ParameterName = "@NewEnrollment";
            newEnrollmentParameter.Value = newEnrollment;
            cmd.Parameters.Add(newEnrollmentParameter);

            SqlParameter studentIdParameter = new SqlParameter();
            studentIdParameter.ParameterName = "@StudentId";
            studentIdParameter.Value = this.GetId();
            cmd.Parameters.Add(studentIdParameter);
            SqlDataReader rdr = cmd.ExecuteReader();

            while(rdr.Read())
         {
           this._name = rdr.GetString(1);
           this._enrollment = rdr.GetString(2);
         }

         if (rdr != null)
         {
           rdr.Close();
         }

         if (conn != null)
         {
           conn.Close();
         }
       }

       public void Delete()
       {
           SqlConnection conn = DB.Connection();
           conn.Open();
           SqlCommand cmd = new SqlCommand("DELETE FROM students WHERE name = @StudentName; DELETE FROM students_courses WHERE student_id = @StudentId; DELETE FROM students_departments WHERE student_id = @StudentId", conn);
           SqlParameter studentParameter = new SqlParameter("@StudentName", this.GetName());
           SqlParameter idParameter = new SqlParameter("@StudentId", this.GetId());
           cmd.Parameters.Add(studentParameter);
           cmd.Parameters.Add(idParameter);
           cmd.ExecuteNonQuery();

           if (conn != null)
           {
             conn.Close();
           }
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
