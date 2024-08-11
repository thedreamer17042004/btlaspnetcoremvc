using Microsoft.Extensions.Localization;
using System.Reflection;
using Asp.netApp.Areas.Admin.Resources;
using System.Globalization;


namespace Asp.netApp.Areas.Services
{
    public class LanguageService
    {
      private readonly IStringLocalizer _localizer;
        public LanguageService(IStringLocalizerFactory factory)
        {
            var type = typeof(SharedResource);

            var assemblyName = new AssemblyName(type.GetTypeInfo().Assembly.FullName);

            _localizer = factory.Create("AdminShared", assemblyName.Name);
        }
        public LocalizedString GetKey(string key)
        {
            return _localizer[key];
        }


        public string GetCurrentLanguage()
        {
            var currentCulture = CultureInfo.CurrentCulture;
            var currentCultureCode = currentCulture?.TwoLetterISOLanguageName;
            return currentCultureCode;
        }
      
       
    }
}
