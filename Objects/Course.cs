using System.Collections.Generic;
using System.Data.SqlClient;
using System;

namespace Registrar.Objects
{
    public class Course
    {
        private int _id;
        private string _name;
        private string _courseNumber;

        public Course(string name, string courseNumber, int id = 0)
        {
            _id = id;
            _name = name;
            _courseNumber = courseNumber;
        }

        public string GetName()
        {
            return _name;
        }

        public int GetId()
        {
            return _id;
        }

        public string GetCourseNumber()
        {
            return _courseNumber;
        }

        public override bool Equals(System.Object otherCourse)
        {
            if (!(otherCourse is Course))
            {
                return false;
            }
            else
            {
                Course newCourse = (Course) otherCourse;
                bool courseIdEquality = (this.GetId() == newCourse.GetId());
                bool nameEquality = (this.GetName() == newCourse.GetName());
                bool courseNumberEquality = (this.GetCourseNumber() == newCourse.GetCourseNumber());
                return (courseIdEquality && nameEquality && courseNumberEquality);
            }
        }

        public void Save()
        {
            SqlConnection conn = DB.Connection();
            conn.Open();

            SqlCommand cmd = new SqlCommand("INSERT INTO courses(name, course_number) OUTPUT INSERTED.id VALUES(@Name, @CourseNumber);", conn);

            SqlParameter nameParameter = new SqlParameter("@Name", this.GetName());
            SqlParameter courseNumberParameter = new SqlParameter("@CourseNumber", this.GetCourseNumber());

            cmd.Parameters.Add(nameParameter);
            cmd.Parameters.Add(courseNumberParameter);
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

        public static List<Course> GetAll()
        {
            List<Course> allCourses = new List<Course>{};

            SqlConnection conn = DB.Connection();
            conn.Open();

            SqlCommand cmd = new SqlCommand("SELECT * FROM courses;", conn);
            SqlDataReader rdr = cmd.ExecuteReader();

            while(rdr.Read())
            {
                int courseId = rdr.GetInt32(0);
                string courseName = rdr.GetString(1);
                string courseNumber = rdr.GetString(2);
                Course newCourse = new Course(courseName,courseNumber, courseId);
                allCourses.Add(newCourse);
            }
            if(rdr != null)
            {
                rdr.Close();
            }
            if(conn != null)
            {
                conn.Close();
            }
            return allCourses;
        }

        public static Course Find(int id)
        {
            SqlConnection conn = DB.Connection();
            conn.Open();
            SqlCommand cmd = new SqlCommand("SELECT * FROM courses WHERE id = @Course_Id;", conn);

            SqlParameter courseIdParameter = new SqlParameter("@Course_Id", id);
            cmd.Parameters.Add(courseIdParameter);

            SqlDataReader rdr = cmd.ExecuteReader();

            int foundCourseId = 0;
            string foundCourseName = null;
            string foundCourseNumber = null;

            while(rdr.Read())
            {
                foundCourseId = rdr.GetInt32(0);
                foundCourseName = rdr.GetString(1);
                foundCourseNumber = rdr.GetString(2);
            }

            Course foundCourse = new Course(foundCourseName, foundCourseNumber, foundCourseId);
            if (rdr != null)
            {
                rdr.Close();
            }
            if (conn != null)
            {
                conn.Close();
            }

            return foundCourse;
        }

        public void Update(string newName, string newCourseNumber)
        {
            SqlConnection conn = DB.Connection();
            conn.Open();

            SqlCommand cmd = new SqlCommand("UPDATE courses SET name = @NewName, course_number = @NewCourseNumber OUTPUT INSERTED.* WHERE id = @CourseId;", conn);

            SqlParameter newNameParameter = new SqlParameter();
            newNameParameter.ParameterName = "@NewName";
            newNameParameter.Value = newName;
            cmd.Parameters.Add(newNameParameter);

            SqlParameter newCourseNumberParameter = new SqlParameter();
            newCourseNumberParameter.ParameterName = "@NewCourseNumber";
            newCourseNumberParameter.Value = newCourseNumber;
            cmd.Parameters.Add(newCourseNumberParameter);

            SqlParameter courseIdParameter = new SqlParameter();
            courseIdParameter.ParameterName = "@CourseId";
            courseIdParameter.Value = this.GetId();
            cmd.Parameters.Add(courseIdParameter);
            SqlDataReader rdr = cmd.ExecuteReader();

            while(rdr.Read())
            {
                this._name = rdr.GetString(1);
                this._courseNumber = rdr.GetString(2);
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

        public void AddStudent(int studentId)
        {
            SqlConnection conn = DB.Connection();
            conn.Open();
            SqlCommand cmd = new SqlCommand("INSERT INTO students_courses (student_id, course_id) VALUES (@StudentId, @CourseId);", conn);
            SqlParameter studentIdParameter = new SqlParameter("@StudentId", studentId);
            SqlParameter courseIdParameter = new SqlParameter("@CourseId", this.GetId());
            cmd.Parameters.Add(studentIdParameter);
            cmd.Parameters.Add(courseIdParameter);

            cmd.ExecuteNonQuery();

            if (conn != null)
            {
                conn.Close();
            }
        }

        public void AddCompletedOrFailed(bool complete, int studentId)
        {
            SqlConnection conn = DB.Connection();
            conn.Open();
            SqlCommand cmd;
            if (complete)
            {
                cmd = new SqlCommand("INSERT INTO students_completedcourses (student_id, course_id) VALUES (@StudentId, @CourseId);", conn);
            }
            else
            {
                cmd = new SqlCommand("INSERT INTO students_failedcourses (student_id, course_id) VALUES (@StudentId, @CourseId);", conn);
            }

            SqlParameter studentIdParameter = new SqlParameter("@StudentId", studentId);
            SqlParameter courseIdParameter = new SqlParameter("@CourseId", this.GetId());
            cmd.Parameters.Add(studentIdParameter);
            cmd.Parameters.Add(courseIdParameter);

            cmd.ExecuteNonQuery();

            if (conn != null)
            {
                conn.Close();
            }
        }

        public List<Student> GetStudents()
        {
            SqlConnection conn = DB.Connection();
            conn.Open();

            SqlCommand cmd = new SqlCommand("SELECT students.* FROM courses JOIN students_courses ON (courses.id = students_courses.course_id) JOIN students ON (students_courses.student_id = students.id) WHERE course_id = @CourseId;", conn);
            SqlParameter courseIdParameter = new SqlParameter();
            courseIdParameter.ParameterName = "@CourseId";
            courseIdParameter.Value = this.GetId();

            cmd.Parameters.Add(courseIdParameter);

            SqlDataReader rdr = cmd.ExecuteReader();

            List<Student> students = new List<Student> {};
            while(rdr.Read())
            {
                int studentId = rdr.GetInt32(0);
                string studentName = rdr.GetString(1);
                string studentEnrollment = rdr.GetString(2);
                Student newStudent = new Student(studentName, studentEnrollment, studentId);
                students.Add(newStudent);
            }
            if (rdr != null)
            {
                rdr.Close();
            }
            if (conn != null)
            {
                conn.Close();
            }
            return students;
        }


        public void Delete()
        {
            SqlConnection conn = DB.Connection();
            conn.Open();
            SqlCommand cmd = new SqlCommand("DELETE FROM courses WHERE name = @CourseName; DELETE FROM students_courses WHERE course_id = @CourseId; DELETE FROM courses_departments WHERE course_id = @CourseId;", conn);
            SqlParameter courseParameter = new SqlParameter("@CourseName", this.GetName());
            SqlParameter idParameter = new SqlParameter("@CourseId", this.GetId());
            cmd.Parameters.Add(courseParameter);
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
            SqlCommand cmd = new SqlCommand("DELETE FROM courses;", conn);
            cmd.ExecuteNonQuery();
            conn.Close();
        }
    }
}
