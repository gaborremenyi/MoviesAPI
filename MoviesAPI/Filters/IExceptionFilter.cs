using Microsoft.AspNetCore.Mvc.Filters;

namespace MoviesAPI.Filters
{
    public interface IExceptionFilter : IFilterMetadata
    {
        void OnException(ExceptionContext context);
    }
}
