using AutoMapper;
using ShoppingList.Business.Repositories.IRepositories;
using ShoppingList.Data;
using ShoppingList.Data.Entities;
using ShoppingList.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using static Azure.Core.HttpHeader;

namespace ShoppingList.Business.Repositories
{
    public class ListItemRepository : IListItemRepository
    {
        private readonly ApplicationDbContext db;
        private readonly IMapper mapper;

        public ListItemRepository(ApplicationDbContext db, IMapper mapper)
        {
            this.db = db;
            this.mapper = mapper;
        }

        public async Task CreateAsync(ListItemDTO objDTO)
        {
            var obj = mapper.Map<ListItemDTO, ListItem>(objDTO);
            db.ListItems.Add(obj);
            await db.SaveChangesAsync();
        }

        public async Task<ListItem> GetAsync(string productName)
        {
            return await db.ListItems.FirstOrDefaultAsync(u => u.Product.ToLower() == productName.ToLower());
        }


        //public async Task<ICollection<Coupon>> GetAllAsync()
        //{
        //    return await db.Coupons.ToListAsync();
        //}

        public async Task<IEnumerable<ListItemDTO>> GetAllAsync()
        {
            var obj = await db.ListItems.ToListAsync();
            return mapper.Map<IEnumerable<ListItem>, IEnumerable<ListItemDTO>>(obj);
        }

        public async Task<ListItemDTO> GetAsync(int id)
        {
            var obj = await db.ListItems.FirstOrDefaultAsync(u => u.Id == id);
            return mapper.Map<ListItem, ListItemDTO>(obj);
        }

        public async Task<int> RemoveAsync(int id)
        {
            var obj = await db.ListItems.FirstOrDefaultAsync(u => u.Id == id);
            if (obj != null)
            {
                db.ListItems.Remove(obj);
                return await db.SaveChangesAsync();
            }
            return 0;
        }

        public async Task SaveAsync()
        {
            await db.SaveChangesAsync();
        }

        public async Task UpdateAsync(ListItemDTO objDTO)
        {
            var objFromDb = await db.ListItems.FirstOrDefaultAsync(u => u.Id == objDTO.Id);
            if (objFromDb != null)
            {
                objFromDb.Product = objDTO.Product;
                objFromDb.Quantity = objDTO.Quantity;
                objFromDb.IsBought = objDTO.IsBought;

                db.ListItems.Update(objFromDb);
                await db.SaveChangesAsync();
            }
        }
    }
}
