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
