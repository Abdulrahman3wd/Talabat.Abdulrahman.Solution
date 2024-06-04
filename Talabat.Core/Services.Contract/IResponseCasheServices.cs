using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Talabat.Core.Services.Contract
{
    public interface IResponseCasheServices
    {
        Task CasheResponseAsync(string key , object Response , TimeSpan TimeToLive);
        Task<string?> GetCashedResponseAsync(string key);


    }
}
