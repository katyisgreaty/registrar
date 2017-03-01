using System.Collections.Generic;
using Nancy;
using Nancy.ViewEngines.Razor;
using Registrar.Objects;
using System.Linq;

namespace Registrar
{
  public class HomeModule : NancyModule
  {
    public HomeModule()
    {

        Get["/"] =_=>
        {
            return View["index.cshtml"];
        };

        Get["/students"]= _ => {
            List<Student> allStudents = Student.GetAll();
            return View["students.cshtml", allStudents];
        };

        Post["/students"] = _ =>
        {
            Student newStudent = new Student(Request.Form["student-name"], Request.Form["student-enrollment"]);
            newStudent.Save();
            List<Student> allStudents = Student.GetAll();
            return View["students.cshtml", allStudents];
        };

        Get["/courses"]= _ => {
            List<Course> allCourses = Course.GetAll();
            return View["courses.cshtml", allCourses];
        };

        Post["/courses"] = _ =>
        {
            Course newCourse = new Course(Request.Form["course-name"], Request.Form["course-number"]);
            newCourse.Save();
            List<Course> allCourses = Course.GetAll();
            return View["courses.cshtml", allCourses];
        };

        Get["/departments"]= _ => {
            List<Department> allDepartments = Department.GetAll();
            return View["departments.cshtml", allDepartments];
        };

        Post["/departments"] = _ =>
        {
            Department newDepartment = new Department(Request.Form["department-major"]);
            newDepartment.Save();
            List<Department> allDepartments = Department.GetAll();
            return View["departments.cshtml", allDepartments];
        };

        Get["/departments/{id}"] = parameter =>{
            Department foundDepartment = Department.Find(parameter.id);
            List<string> courses = new List<string>{};
            List<string> students = new List<string>{};
            foreach(var course in foundDepartment.GetCourses())
            {
                courses.Add(course.GetName());
            };
            foreach(var student in foundDepartment.GetStudents())
            {
                students.Add(student.GetName());
            };
            Dictionary<string, List<string>> departmentDeets = new Dictionary<string, List<string>>{{"courses", courses}, {"students", students}};
            return View["department.cshtml", departmentDeets];
        };

        Get["courses/{id}"] = parameters => {
               Dictionary<string, object> model = new Dictionary<string, object>();
               Course SelectedCourse = Course.Find(parameters.id);
               List<Student> CourseStudents = SelectedCourse.GetStudents();
               List<Student> AllStudents = Student.GetAll();
               model.Add("course", SelectedCourse);
               model.Add("courseStudents", CourseStudents);
               model.Add("allStudents", AllStudents);
               return View["course.cshtml", model];
           };



        // Post["course/add_student"] = _ => {
        //        Student student = Student.Find(Request.Form["student-id"]);
        //        Course course = Course.Find(Request.Form["course-id"]);
        //        course.AddStudent(student);
        //        return View["course.cshtml"];
        //    };

        // Post["student/add_course"] = _ => {
        //    Student student = Student.Find(Request.Form["student-id"]);
        //    Course course = Course.Find(Request.Form["course-id"]);
        //    student.AddCourse(course);
        //    return View["student.cshtml"];
        // };
    }
  }
}
