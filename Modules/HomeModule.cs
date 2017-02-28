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
    }
  }
}
