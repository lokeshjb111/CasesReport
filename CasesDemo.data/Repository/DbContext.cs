using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace CasesDemo.data.Repository
{
    public class CasesDbContext : DbContext
    {
        public CasesDbContext(DbContextOptions<CasesDbContext> options) : base(options)
        {
           
        }
        public DbSet<Cases> Cases { get; set; }
    }

    public class Cases
    {
        public int Id { get; set; }
        public string State { get; set; }
        public string Date { get; set; }
        public int NewCases { get; set; }
        public int Recovery { get; set; }
        public int Death { get; set; }
    }

    public class CasesSummary
    {
        public int Id { get; set; }
        public string State { get; set; }
        public int NewCases { get; set; }
        public int Recovery { get; set; }
        public int Death { get; set; }
    }

    public class CasesViewModel
    {
        public string FromDate { get; set; }
        public string ToDate { get; set; }
    }
}
