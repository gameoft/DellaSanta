using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;

namespace Dellasanta.Web.Common.Automapper
{
    public static class AutoMapperConfiguration
    {
        public static void Initialize()
        {
            MapperConfiguration = new MapperConfiguration(cfg =>
            {
                //// ----- EmailTemplate -----
                //cfg.CreateMap<EmailTemplate, EmailTemplateModel>();
                //cfg.CreateMap<EmailTemplateModel, EmailTemplate>();

                //// ----- Log -----
                //cfg.CreateMap<Log, LogModel>();
                //cfg.CreateMap<LogModel, Log>();

                //// ----- Setting -----
                //cfg.CreateMap<Setting, SettingModel>();
                //cfg.CreateMap<SettingModel, Setting>();

                //// ----- TraceLog -----
                //cfg.CreateMap<TraceLog, TraceLogModel>();
                //cfg.CreateMap<TraceLogModel, TraceLog>();

                //// ----- User -----
                //cfg.CreateMap<User, UserModel>()
                //    .ForMember(dest => dest.AuthorizedRoleNames,
                //        mo => mo.MapFrom(src => src.Roles != null ? string.Join(", ", src.Roles.Select(r => r.Name).OrderBy(r => r).ToList()) : ""));
                //cfg.CreateMap<User, UserCreateUpdateModel>()
                //    .ForMember(dest => dest.AvailableRoleNames, opt => opt.Ignore())
                //    .ForMember(dest => dest.AvailableDomains, opt => opt.Ignore())
                //    .ForMember(dest => dest.SelectedRoleIds,
                //        mo => mo.MapFrom(src => src.Roles != null ? src.Roles.Select(r => r.Id).ToList() : new List<int>()));
            });
            Mapper = MapperConfiguration.CreateMapper();
        }

        public static IMapper Mapper { get; private set; }

        public static MapperConfiguration MapperConfiguration { get; private set; }
    }
}
