using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShoppingList.Data.Entities
{
    public class ApplicationUser : IdentityUser
    {
        public string Name { get; set; }
    }
}
