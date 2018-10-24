using Gdpr.Domain;
using System;

namespace Gdpr.UI.Cmd
{
    class Program
    {
        static void Main(string[] args)
        {
            IAdminRepository repository = new AdminRepository(); 
            Console.WriteLine(@"URD Count = {0}", repository.GetURDCount());
        }
    }
}
