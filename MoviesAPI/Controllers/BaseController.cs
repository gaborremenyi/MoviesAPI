using Microsoft.AspNetCore.Mvc;
using MoviesAPI.Data;

namespace MoviesAPI.Controllers
{
    public class BaseController : Controller
    {
        protected IDataService dataService;

        public BaseController(IDataService dataService)
        {
            this.dataService = dataService;
        }
    }
}
