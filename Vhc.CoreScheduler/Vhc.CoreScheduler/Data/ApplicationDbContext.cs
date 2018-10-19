using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Vhc.CoreScheduler.Common.Models;

namespace Vhc.CoreScheduler.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public DbSet<ExecutionEnvironment> ExecutionEnvironments { get; set; }
        public DbSet<ExecutionVariable> Variables { get; set; }
        public DbSet<JobGroup> Groups { get; set; }


        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
    }
}
