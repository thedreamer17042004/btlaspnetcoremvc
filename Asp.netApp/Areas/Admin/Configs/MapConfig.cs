using Asp.netApp.Areas.Admin.Models.DataModel;
using Asp.netApp.Areas.Admin.Models.ViewModels.Account;
using AutoMapper;

namespace Asp.netApp.Areas.Admin.Configs
{
    public class MapConfig
    {
        public static Mapper InitializeAutomapper()
        {
            //Provide all the Mapping Configuration
            var config = new MapperConfiguration(cfg =>
            {
                //Configuring Employee and EmployeeDTO
                cfg.CreateMap<Account, AccountViewModel>();
                 cfg.CreateMap<AccountViewModel, Account>();
                //Any Other Mapping Configuration ....
            });
            //Create an Instance of Mapper and return that Instance
            var mapper = new Mapper(config);
            return mapper;
        }
    }
}
