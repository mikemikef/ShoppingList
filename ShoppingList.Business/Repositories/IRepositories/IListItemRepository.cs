using ShoppingList.Data.Entities;
using ShoppingList.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShoppingList.Business.Repositories.IRepositories
{
    public interface IListItemRepository
    {
        Task<IEnumerable<ListItemDTO>> GetAllAsync();
        Task<ListItemDTO> GetAsync(int id);
        Task<ListItem> GetAsync(string couponName);
        Task CreateAsync(ListItemDTO item);
        Task UpdateAsync(ListItemDTO item);
        Task<int> RemoveAsync(int id);
        Task SaveAsync();

    }
}
