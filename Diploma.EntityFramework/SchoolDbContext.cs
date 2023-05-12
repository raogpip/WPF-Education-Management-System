﻿using Diploma.Domain.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Diploma.EntityFramework
{
    public class SchoolDbContext : DbContext 
    {
        

        public DbSet<Student> Students { get; set; }
        public DbSet<Teacher> Teachers { get; set; }
        public DbSet<Subject> Subjects { get; set; }
        public DbSet<Lecture> Lectures { get; set; }    
        public DbSet<Grade> Grades { get; set; }
        public DbSet<Student_Lecture> Student_Lectures { get; set; }
        public DbSet<Notebook> Notebooks { get; set; }
        public DbSet<Note> Notes { get; set; }
        
        public SchoolDbContext(DbContextOptions options) : base(options) {  }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Student_Lecture>().HasNoKey();

            base.OnModelCreating(modelBuilder);
        }
    }
}
