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

    }
  }
}
