using DatingApp.Entity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DatingApp.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions options) : base(options)
        {

        }
        public DbSet<AppUser> Users { get; set; } //proprietate de tipul DbSet->ia tipul clasei caruia vrei sa-i cream un DbSet, dupa dam call la tabelul  din baza de date AppUsers
        public DbSet<UserLike> Likes{get;set;}

        public DbSet<Message> Messages { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<UserLike>()
                .HasKey(k => new { k.SourceUserId, k.LikedUserId }); //aici formam primary key pt acest tabels

            builder.Entity<UserLike>()
                .HasOne(s => s.SourceUser)  //a source user 
                .WithMany(l => l.LikedUsers) // can like many other users
                .HasForeignKey(s => s.SourceUserId) //
                .OnDelete(DeleteBehavior.Cascade);//daca stergem un user stergem toate entitatile related


            builder.Entity<UserLike>()
                .HasOne(s => s.LikedUser) // a liked users 
                .WithMany(l => l.LikedByUsers)  //can be liked by many other uses
                .HasForeignKey(s => s.LikedUserId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<Message>()
                .HasOne(u => u.Recipient)
                .WithMany(m => m.MessagesReceived)
                .OnDelete(DeleteBehavior.Restrict); //deoarece vrem sa stergem de pe sv doar cand ambii useri au facut asta

            builder.Entity<Message>()
                .HasOne(u => u.Sender)
                .WithMany(m => m.MessagesSent)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
