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
