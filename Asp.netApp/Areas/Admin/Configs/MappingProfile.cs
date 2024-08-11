using AutoMapper;
using Asp.netApp.Areas.Admin.Models.DataModel;
using Asp.netApp.Areas.Admin.Models.ViewModels.Account;
using AutoMapper;
using Asp.netApp.Areas.Admin.Models.ViewModels.Brand;
namespace Asp.netApp.Areas.Admin.Configs
{


    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<AccountViewModel, Account>();
            CreateMap<Account, AccountViewModel>();
            CreateMap<AccountEditViewModel, Account>();
            CreateMap<Account, AccountEditViewModel>();
            //mapper brand
            CreateMap<BrandViewModel, Brand>();
            CreateMap<Brand, BrandViewModel>();
        }
    }

}
