using DatingApp.Entity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace DatingApp.Data
{
    public static class Seed //clasa se fol pt a adauga intrari in baza de date dummydata
    {
        public static async Task  SeedUsers(DataContext context)
        {
            if (await context.Users.AnyAsync()) return; //verificam daca exista useri in tabelul USERS

            var userData = await System.IO.File.ReadAllTextAsync("Data/UserSeedData.json");
            var users = JsonSerializer.Deserialize<List<AppUser>>(userData); //deserializam jsonul intr-un obiect;
            foreach(var user in users)
            {
                using var hmac = new HMACSHA512();

                user.UserName = user.UserName.ToLower();
                user.PasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes("Pa$$w0rd"));
                user.PasswordSalt = hmac.Key;
                context.Users.Add(user); //aici facem tracking
                
            }
            await context.SaveChangesAsync(); //aici salvam in baza de date
        }
    }
}
