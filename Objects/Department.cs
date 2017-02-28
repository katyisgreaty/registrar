using System.Collections.Generic;
using System.Data.SqlClient;
using System;

namespace Registrar.Objects
{
    public class Department
    {
        private int _id;
        private string _major;

        public Department(string major,  int id = 0)
        {
            _id = id;
            _major = major;
        }

        public string GetMajor()
        {
            return _major;
        }

        public int GetId()
        {
            return _id;
        }

        public override bool Equals(System.Object otherDepartment)
        {
            if (!(otherDepartment is Department))
            {
                return false;
            }
            else
            {
                Department newDepartment = (Department) otherDepartment;
                bool departmentIdEquality = (this.GetId() == newDepartment.GetId());
                bool majorEquality = (this.GetMajor() == newDepartment.GetMajor());
                return (departmentIdEquality && majorEquality);
            }
        }

        public static List<Department> GetAll()
        {
            List<Department> allDepartments = new List<Department>{};

            SqlConnection conn = DB.Connection();
            conn.Open();

            SqlCommand cmd = new SqlCommand("SELECT * FROM departments;", conn);
            SqlDataReader rdr = cmd.ExecuteReader();

            while(rdr.Read())
            {
                int departmentId = rdr.GetInt32(0);
                string departmentMajor = rdr.GetString(1);
                Department newDepartment = new Department(departmentMajor, departmentId);
                allDepartments.Add(newDepartment);
            }
            if(rdr != null)
            {
                rdr.Close();
            }
            if(conn != null)
            {
                conn.Close();
            }
            return allDepartments;
        }

        public void Save()
        {
            SqlConnection conn = DB.Connection();
            conn.Open();

            SqlCommand cmd = new SqlCommand("INSERT INTO departments(major) OUTPUT INSERTED.id VALUES(@Major);", conn);

            SqlParameter majorParameter = new SqlParameter("@Major", this.GetMajor());

            cmd.Parameters.Add(majorParameter);
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

        public static Department Find(int id)
        {
            SqlConnection conn = DB.Connection();
            conn.Open();
            SqlCommand cmd = new SqlCommand("SELECT * FROM departments WHERE id = @Department_Id;", conn);

            SqlParameter departmentIdParameter = new SqlParameter("@Department_Id", id);
            cmd.Parameters.Add(departmentIdParameter);

            SqlDataReader rdr = cmd.ExecuteReader();

            int foundDepartmentId = 0;
            string foundDepartmentMajor = null;

            while(rdr.Read())
            {
                foundDepartmentId = rdr.GetInt32(0);
                foundDepartmentMajor = rdr.GetString(1);
            }

            Department foundDepartment = new Department(foundDepartmentMajor, foundDepartmentId);
            if (rdr != null)
            {
                rdr.Close();
            }
            if (conn != null)
            {
                conn.Close();
            }

            return foundDepartment;
        }


        public static void DeleteAll()
        {
            SqlConnection conn = DB.Connection();
            conn.Open();
            SqlCommand cmd = new SqlCommand("DELETE FROM departments;", conn);
            cmd.ExecuteNonQuery();
            conn.Close();
        }

    }
}
