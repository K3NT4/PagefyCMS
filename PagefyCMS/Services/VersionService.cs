using System.Reflection;

namespace PagefyCMS.Services
{
    public interface IVersionService
    {
        string GetCurrentVersion();
    }

    public class VersionService : IVersionService
    {
        public string GetCurrentVersion()
        {
            var version = Assembly.GetExecutingAssembly()
                .GetCustomAttribute<AssemblyInformationalVersionAttribute>()?
                .InformationalVersion;

            return version ?? "1.0.0-dev";
        }
    }
}
