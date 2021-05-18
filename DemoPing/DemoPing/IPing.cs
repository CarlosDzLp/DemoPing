using System;
using System.Threading.Tasks;

namespace DemoPing
{
    public interface IPing
    {
        Task<string> Ping(string host);
    }
}
