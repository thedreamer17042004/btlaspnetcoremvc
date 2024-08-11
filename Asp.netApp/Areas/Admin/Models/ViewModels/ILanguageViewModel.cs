namespace Asp.netApp.Areas.Admin.Models.ViewModels
{
    public interface ILanguageViewModel
    {
        public string Parent { get; set; }
        public string Child { get; set; }
        public string SecondChild { get; set; }
    }
}
