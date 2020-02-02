using Core.Enums.DataBaseEnums;
using Microsoft.AspNetCore.Mvc;

namespace Front.React.Filters
{
    public class AuthorizedAttribute : TypeFilterAttribute
    {
        public AuthorizedAttribute(params UserRole[] roles) : base(typeof(AuthorizedActionFilter))
        {
            Arguments = new object[] { roles };
        }
    }
}
