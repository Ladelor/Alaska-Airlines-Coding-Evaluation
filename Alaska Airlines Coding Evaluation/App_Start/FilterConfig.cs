using System.Web;
using System.Web.Mvc;

namespace Alaska_Airlines_Coding_Evaluation
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }
    }
}
