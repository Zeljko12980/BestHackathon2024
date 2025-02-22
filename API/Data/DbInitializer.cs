namespace API.Data
{
    public class DbInitializer
    {
        public static async Task Initialize(StoreContext context, UserManager<User> userManager)
        {
            if (!userManager.Users.Any())
            {
                // Prvo kreiraj razrede u bazi podataka
                var schoolClasses = new List<SchoolClass>
                {
                    new SchoolClass
                    {
                        Name = "I-1",
                        Students = new List<User>(),
                        Teachers = new List<User>()
                    },
                    new SchoolClass
                    {
                        Name = "I-2",
                        Students = new List<User>(),
                        Teachers = new List<User>()
                    },
                    new SchoolClass
                    {
                        Name = "I-3",
                        Students = new List<User>(),
                        Teachers = new List<User>()
                    },
                    new SchoolClass
                    {
                        Name = "II-1",
                        Students = new List<User>(),
                        Teachers = new List<User>()
                    },
                    new SchoolClass
                    {
                        Name = "II-2",
                        Students = new List<User>(),
                        Teachers = new List<User>()
                    },
                    new SchoolClass
                    {
                        Name = "II-3",
                        Students = new List<User>(),
                        Teachers = new List<User>()
                    }
                };

                // Seed u bazu podataka
                await context.SchoolClasses.AddRangeAsync(schoolClasses);
                await context.SaveChangesAsync();

                // Kreiraj profesora
                var teacher = new User
                {
                    FirstName = "Jovan",
                    LastName = "Petrovic",
                    UserName = "jovan_teacher",
                    Email = "profesor@email.com",
                    JMBG = "0123456789",
                    SchoolClassId = null, 
                    TeachingClasses = new List<SchoolClass>()
                };

                foreach (var schoolClass in schoolClasses)
                {
                    teacher.TeachingClasses.Add(schoolClass);  
                    schoolClass.Teachers.Add(teacher);         
                }

                // Kreiraj učenike
                var students = new List<User>
                {
                    new User
                    {
                        FirstName = "Marko",
                        LastName = "Markovic",
                        UserName = "markostudent",
                        Email = "marko@school.com",
                        JMBG = "9876543210",
                        Score = 0
                    },
                    new User
                    {
                        FirstName = "Ana",
                        LastName = "Jovanovic",
                        UserName = "anastudent1",
                        Email = "ana1@school.com",
                        JMBG = "1111111111",
                        Score = 0
                    },
                    new User
                    {
                        FirstName = "Ivan",
                        LastName = "Nikolic",
                        UserName = "ivanstudent2",
                        Email = "ivan2@school.com",
                        JMBG = "2222222222",
                        Score = 0
                    },
                    new User
                    {
                        FirstName = "Petar",
                        LastName = "Petrovic",
                        UserName = "petarstudent3",
                        Email = "petar3@school.com",
                        JMBG = "3333333333",
                        Score = 0
                    },
                    new User
                    {
                        FirstName = "Jelena",
                        LastName = "Kovac",
                        UserName = "jelenastudent4",
                        Email = "jelena4@school.com",
                        JMBG = "4444444444",
                        Score = 0
                    },
                    new User
                    {
                        FirstName = "Maja",
                        LastName = "Lazic",
                        UserName = "majastudent5",
                        Email = "maja5@school.com",
                        JMBG = "5555555555",
                        Score = 0
                    },
                    new User
                    {
                        FirstName = "Bojan",
                        LastName = "Milic",
                        UserName = "bojanstudent6",
                        Email = "bojan6@school.com",
                        JMBG = "6666666666",
                        Score = 0
                    },
                    new User
                    {
                        FirstName = "Tamara",
                        LastName = "Markovic",
                        UserName = "tamarastudent7",
                        Email = "tamara7@school.com",
                        JMBG = "7777777777",
                        Score = 0
                    },
                    new User
                    {
                        FirstName = "Nikola",
                        LastName = "Djordjevic",
                        UserName = "nikolastudent8",
                        Email = "nikola8@school.com",
                        JMBG = "8888888888",
                        Score = 0
                    },
                    new User
                    {
                        FirstName = "Stefan",
                        LastName = "Ilic",
                        UserName = "stefanstudent9",
                        Email = "stefan9@school.com",
                        JMBG = "9999999999",
                        Score = 0
                    },
                    new User
                    {
                        FirstName = "Jovana",
                        LastName = "Milosevic",
                        UserName = "jovanastudent10",
                        Email = "jovana10@school.com",
                        JMBG = "1010101010",
                        Score = 0
                    },
                    new User
                    {
                        FirstName = "Vuk",
                        LastName = "Simic",
                        UserName = "vukstudent11",
                        Email = "vuk11@school.com",
                        JMBG = "1111111112",
                        Score = 0
                    },
                    new User
                    {
                        FirstName = "Daria",
                        LastName = "Knezevic",
                        UserName = "dariastudent12",
                        Email = "daria12@school.com",
                        JMBG = "2222222223",
                        Score = 0
                    },
                    new User
                    {
                        FirstName = "Luka",
                        LastName = "Dacic",
                        UserName = "lukastudent13",
                        Email = "luka13@school.com",
                        JMBG = "3333333334",
                        Score = 0
                    },
                    new User
                    {
                        FirstName = "Tanja",
                        LastName = "Vukovic",
                        UserName = "tanjastudent14",
                        Email = "tanja14@school.com",
                        JMBG = "4444444445",
                        Score = 0
                    },
                    new User
                    {
                        FirstName = "Marko",
                        LastName = "Jovic",
                        UserName = "markostudent15",
                        Email = "marko15@school.com",
                        JMBG = "5555555556",
                        Score = 0
                    }
                };

                // Dodaj studente u odgovarajuće razrede
                foreach (var schoolClass in schoolClasses)
                {
                    var classStudents = students.Take(5).ToList(); // Get first 5 students for this class
                    foreach (var student in classStudents)
                    {
                        student.SchoolClassId = schoolClass.Id;
                        schoolClass.Students.Add(student);
                    }
                    students = students.Skip(5).ToList(); // Remove added students from list
                }

                // Kreiranje korisnika i dodela uloga
                await userManager.CreateAsync(teacher, "Pa$$w0rd");
                await userManager.AddToRolesAsync(teacher, new List<string> { "Admin" });

                foreach (var student in students)
                {
                    await userManager.CreateAsync(student, "Pa$$w0rd");
                    await userManager.AddToRolesAsync(student, new List<string> { "Member" });
                }

                // Ažuriraj profesora u bazi da bi veza bila sačuvana
                context.Users.Update(teacher);
                await context.SaveChangesAsync();
            }
        }
    }
}
