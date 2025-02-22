using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Models;
using Microsoft.AspNetCore.Identity;

namespace API.Data
{
    public class DbInitializer
    {
        public static async Task Initialize(StoreContext context, UserManager<User> userManager)
{
    if (!userManager.Users.Any())
    {
        // Prvo kreiraj razred u bazi podataka
        var schoolClasses = new List<SchoolClass>
        {
            new SchoolClass
            {
                Name = "I-1",
                Students = new List<User>(), // Ovdje možeš dodati studente ako ih imaš
                Teachers = new List<User>()  // Ovdje možeš dodati profesore ako ih imaš
            },
            // Dodaj ostale razrede...
        };

        // Seed u bazu podataka
        await context.SchoolClasses.AddRangeAsync(schoolClasses);
        await context.SaveChangesAsync();

        var selectedSchoolClass = schoolClasses[0]; // Odaberi željeni razred (npr. I1)

        // Kreiraj profesora
        var teacher = new User
        {
            FirstName = "Jovan",
            LastName = "Petrovic",
            UserName = "jovan_teacher",
            Email = "profesor@email.com",
            JMBG = "0123456789",
            SchoolClassId = null, // Profesori nemaju samo jedan razred
            TeachingClasses = new List<SchoolClass>() // Dodeljujemo profesoru listu razreda koje predaje
        };

        foreach (var schoolClass in schoolClasses)
        {
            teacher.TeachingClasses.Add(schoolClass);  // Dodaj razred profesoru
            schoolClass.Teachers.Add(teacher);         // Dodaj profesora razredu
        }

        // Kreiraj učenika
        var student = new User
        {
            FirstName = "Marko",
            LastName = "Markovic",
            UserName = "markostudent",
            Email = "marko@school.com",
            JMBG = "9876543210",
            SchoolClassId = selectedSchoolClass.Id, // Povezujemo učenika sa razredom
            Score=0
        };

        // Dodaj studenta u odgovarajući razred
        selectedSchoolClass.Students.Add(student);

        // Dodaj početni skor za studenta
      //  var initialScore = new Score
      
        // Kreiranje korisnika i dodela uloga
        await userManager.CreateAsync(teacher, "Pa$$w0rd");
        await userManager.AddToRolesAsync(teacher, new List<string> { "Admin" });

        await userManager.CreateAsync(student, "Pa$$w0rd");
        await userManager.AddToRolesAsync(student, new List<string> { "Member" });

        // Ažuriraj profesora u bazi da bi veza bila sačuvana
        context.Users.Update(teacher);
        await context.SaveChangesAsync();
    }
}
 
    }
    
}
