using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using GuestIn.Data;
using System;
using System.Linq;
namespace GuestIn.Models
{
    public class initial
    {
        public static void Initialize(IServiceProvider serviceProvider) { 
            using (var context = new GuestInContext(
                serviceProvider.GetRequiredService<DbContextOptions<GuestInContext>>()))
            {
                return;
            }
        }
    }
}
