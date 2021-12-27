using EFCoreTest.Models;

namespace EFCoreTest
{
    public static class DbInitializer
    {
        public static void Initialize(UserContext context)
        {
            context.Database.EnsureCreated();

            // Look for any students.
            if (context.User.Any())
            {
                return;   // DB has been seeded
            }

            var users = new User[]
            {
                new User{
                    Name="张三",
                    CreateTime = DateTime.Parse("2019-09-01"),
                    Orders = new List<Order>(){
                        new Order() { Amount = 100, CreateTime = DateTime.Now, OrderNo = "No0001" },
                        new Order() { Amount = 200, CreateTime = DateTime.Now, OrderNo = "No0002" } 
                    },
                    Resume = new Resume(){
                        CreateTime =DateTime.Now,
                        Title="张三的简历",
                         Content="张三的简历",
                    },
                },
                new User{
                    Name="李四",
                    CreateTime=DateTime.Parse("2017-09-01"),
                    Orders = new List<Order>(){
                        new Order() { Amount = 300, CreateTime = DateTime.Now, OrderNo = "No0003" },
                        new Order() { Amount = 400, CreateTime = DateTime.Now, OrderNo = "No0004" } 
                    },
                    Resume = new Resume(){
                        CreateTime =DateTime.Now,
                        Title="李四的简历",
                         Content="李四的简历",
                    }
                },
                new User{
                    Name="王五",
                    CreateTime=DateTime.Parse("2018-09-01"),
                    Orders = new List<Order>(){
                        new Order() { Amount = 500, CreateTime = DateTime.Now, OrderNo = "No0005" },
                        new Order() { Amount = 600, CreateTime = DateTime.Now, OrderNo = "No0006" } 
                    },
                    Resume = new Resume(){
                        CreateTime =DateTime.Now,
                        Title="王五的简历",
                         Content="王五的简历",
                    }
                },
                new User{
                    Name="赵六",
                    CreateTime=DateTime.Parse("2017-09-01"),
                    Orders = new List<Order>(){
                        new Order() { Amount = 700, CreateTime = DateTime.Now, OrderNo = "No0007" },
                        new Order() { Amount = 800, CreateTime = DateTime.Now, OrderNo = "No0008" } 
                    },
                    Resume = new Resume(){
                        CreateTime =DateTime.Now,
                        Title="赵六的简历",
                         Content="赵六的简历",
                    }
                },
            };

            context.User.AddRange(users);
            context.SaveChanges();

            //var courses = new Course[]
            //{
            //    new Course{CourseID=1050,Title="Chemistry",Credits=3},
            //    new Course{CourseID=4022,Title="Microeconomics",Credits=3},
            //    new Course{CourseID=4041,Title="Macroeconomics",Credits=3},
            //    new Course{CourseID=1045,Title="Calculus",Credits=4},
            //    new Course{CourseID=3141,Title="Trigonometry",Credits=4},
            //    new Course{CourseID=2021,Title="Composition",Credits=3},
            //    new Course{CourseID=2042,Title="Literature",Credits=4}
            //};

            //context.Courses.AddRange(courses);
            //context.SaveChanges();

            //var enrollments = new Enrollment[]
            //{
            //    new Enrollment{StudentID=1,CourseID=1050,Grade=Grade.A},
            //    new Enrollment{StudentID=1,CourseID=4022,Grade=Grade.C},
            //    new Enrollment{StudentID=1,CourseID=4041,Grade=Grade.B},
            //    new Enrollment{StudentID=2,CourseID=1045,Grade=Grade.B},
            //    new Enrollment{StudentID=2,CourseID=3141,Grade=Grade.F},
            //    new Enrollment{StudentID=2,CourseID=2021,Grade=Grade.F},
            //    new Enrollment{StudentID=3,CourseID=1050},
            //    new Enrollment{StudentID=4,CourseID=1050},
            //    new Enrollment{StudentID=4,CourseID=4022,Grade=Grade.F},
            //    new Enrollment{StudentID=5,CourseID=4041,Grade=Grade.C},
            //    new Enrollment{StudentID=6,CourseID=1045},
            //    new Enrollment{StudentID=7,CourseID=3141,Grade=Grade.A},
            //};

            //context.Enrollments.AddRange(enrollments);
            //context.SaveChanges();
        }
    }
}
