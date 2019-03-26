using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using MxReturnCode;

namespace Gdpr.Domain
{
    public enum UrdCodeStd { Admin = 1, Controller = 2, Standard = 3, Guest = 4, System = 5, Ghost = 6, Undefined = 7 };
    public enum UrdStatus { NotImplemented = 0, PublishProduction = 10  };
    public enum RpdChildFlag { NotChild = 0, Under16 = 1 };


    public interface IGdprDomain
    {
        string GetComponentName();
        string GetComponentVersion();

    }
}
