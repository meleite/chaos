using Microsoft.Identity.Web;
using System.Net.Http.Headers;

namespace Chaos.Services.Interfaces
{

    public interface IApi
    {
        Task<string> GetToken();
    }
}
