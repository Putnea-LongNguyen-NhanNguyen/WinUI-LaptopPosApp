using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LaptopPosApp.Dao
{
    public partial class DbContextInMemoryMock : DbContextWithMock
    {
        public DbContextInMemoryMock(DbContextOptions options): base(options) {
        }
    }
}
