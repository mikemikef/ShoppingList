using AutoMapper;
using ShoppingList.Data.Entities;
using ShoppingList.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace ShoppingList.Business.Mapper
{
    public class MappingConfig : Profile
    {
        public MappingConfig()
        {
            CreateMap<ListItem, ListItemDTO>().ReverseMap();
            CreateMap<ListItem, ListItemCreateDTO>().ReverseMap();
            CreateMap<ListItem, ListItemUpdateDTO>().ReverseMap();
            CreateMap<ListItemDTO, ListItemCreateDTO>().ReverseMap();
            CreateMap<ListItemDTO, ListItemUpdateDTO>().ReverseMap();
            CreateMap<LocalUser, UserDTO>().ReverseMap();
            CreateMap<ApplicationUser, UserDTO>().ReverseMap();
        }
    }
}
