namespace Asp.netApp.Areas.Admin.Models.ViewModels.Shared
{
    public class LanguageViewModel:ILanguageViewModel
    {
        public string Parent { get; set; }
        public string Child { get; set; }
        public string SecondChild { get; set; }
    }
}
