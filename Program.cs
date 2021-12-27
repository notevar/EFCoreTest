using EFCoreTest;
using EFCoreTest.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Text.Unicode;

static void CreateDbIfNotExists(IHost host)
{
    using (var scope = host.Services.CreateScope())
    {
        var services = scope.ServiceProvider;
        try
        {
            var context = services.GetRequiredService<UserContext>();
            //context.Database.EnsureCreated();
            DbInitializer.Initialize(context);
        }
        catch (Exception ex)
        {
            var logger = services.GetRequiredService<ILogger<Program>>();
            logger.LogError(ex, "An error occurred creating the DB.");
        }
    }
}


var builder = WebApplication.CreateBuilder(args);
//builder.WebHost.UseContentRoot("wwwroot");
//builder.Logging.AddConsole();
// Add services to the container.
builder.Services.AddControllers();
JsonSerializerOptions options = new() {
    ReferenceHandler = ReferenceHandler.IgnoreCycles,
    WriteIndented = true,
    Encoder = JavaScriptEncoder.Create(UnicodeRanges.All),
};
var filePath = Directory.GetCurrentDirectory();
builder.Services.AddDbContext<UserContext>(options =>options.UseSqlite($"Data Source={filePath}\\user.db"));
//builder.Services.AddSingleton<Microsoft.Extensions.Hosting.IHostEnvironment>();

var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseAuthorization();
app.MapControllers();

app.Map("/", () => "Hello World");

static string LocalFunction() => "This is local function";
app.MapGet("/local-fun", LocalFunction);
app.MapGet("/static-method", HelloHandler.SayHello);
HelloHandler helloHandler = new HelloHandler();
app.MapGet("/instance-method", helloHandler.Hello);
app.MapGet("/users/{userId:int}", (int userId) => $"user id is {userId}");
app.MapGet("/user/{name:length(2)}", (string name) => $"user name is {name}");
app.MapGet("/sayhello/{name}", (string name) => $"Hello {name}");
app.MapGet("/sayhello/{name}", (string name, int? age) => $"my name is {name},age {age}");
app.MapGet("/json", () => Results.Json(new { aaa = 1123, vvv = "555" }));
app.MapPost("/goods", ([FromBody] Goods goods) => $"添加商品{goods.GName}成功");
app.MapGet("/getip", (HttpContext context, HttpRequest request, HttpResponse response) => response.WriteAsync($"IP:{context.Connection.RemoteIpAddress},Request Method:{request.Method}"));


app.MapGet("/info", (IWebHostEnvironment env) => new {
    Time = DateTimeOffset.UtcNow,
    env.EnvironmentName
});




#region 一对一关系 增删改查
app.Map("/getuser/{id}", (UserContext dbContext, int id) =>
{
    //var user = dbContext.User.Include(c => c.UserRoles).Where(c=>c.Id == id)
    //.Select(c => new { })

    var user = dbContext.User.Include(c => c.Resume)
    .Include(c => c.UserRoles)
    .FirstOrDefault(c => c.Id == id);
    return Results.Json(user, options);
});
app.Map("/getResume/{id}", (UserContext dbContext, int id) =>
{
    var resume = dbContext.Resume.Include(c => c.User).FirstOrDefault(c => c.Id == id);
    return Results.Json(resume, options);
});

app.Map("/loaduser/{id}", (UserContext dbContext, int id) =>
{
    var user = dbContext.User.Find(id);
    dbContext.Entry(user).Collection(t => t.UserRoles)
    .Query().Where(c => c.User.Name == "111")//过滤条件
    .Load();//加载集合
    dbContext.Entry(user).Reference(t => t.Resume).Load();//加载单个
    return Results.Json(user, options);
});
#endregion


#region 一对多关系 增删改查
app.Map("/getuser/mult/{id}", (UserContext dbContext, int id) =>
{
    var user = dbContext.User.Include(c => c.Resume).Include(c => c.Orders).FirstOrDefault(c => c.Id == id);
    return Results.Json(user, options);
});
app.Map("/getResume/mult/{id}", (UserContext dbContext, int id) =>
{
    var resume = dbContext.Resume.Include(c => c.User).FirstOrDefault(c => c.Id == id);
    return Results.Json(resume, options);
});
#endregion

#region 多对多关系 增删改查
app.Map("/insertuserRole", (UserContext dbContext) =>
{
    var userRoles = new UserRole()
    {
        User = new User
        {
            Name = "Bob",
            CreateTime = DateTime.Parse("2019-09-01"),
            Orders = new List<Order>(){
                        new Order() { Amount = 100, CreateTime = DateTime.Now, OrderNo = "No0001" },
                        new Order() { Amount = 200, CreateTime = DateTime.Now, OrderNo = "No0002" }
                    },
            Resume = new Resume()
            {
                CreateTime = DateTime.Now,
                Title = "Bob的简历",
                Content = "Bob的简历",
            },
        },
        Role = new Role()
        {
            RoleName = "系统管理员",
        }
    };
    dbContext.UserRole.Add(userRoles);
    dbContext.SaveChanges();
    return Results.Ok(userRoles.UserId);
});
#endregion



app.Map("/delete/{id}", (UserContext dbContext, int id) =>{
    var order = dbContext.Order.Where(c => c.Id == id).First();
    dbContext.Remove(order);
    dbContext.SaveChanges();
});

app.Map("/deleteattach/{id}", (UserContext dbContext, int id) => {
    var entity = new Order { Id = id };
    var entry = dbContext.Entry(entity);
    entry.State = EntityState.Deleted;
    dbContext.SaveChanges();
});

app.MapMethods("/getmethods", new[] { "GET", "POST", "PUT", "DELETE" }, (HttpRequest req) => $"Current Http Method Is {req.Method}");

//app.MapMethods("/get",new string[] {"get","post" }, (UserContext dbContext, int orderId) =>
//    dbContext.Orders.Include(c => c.OrderItems)
//    .Where(i => i.Id == orderId)
//    .Select(c => new { c.Id, c.UserName, ProductName = c.OrderItems.Select(f => f.ProductName) })
//    );

//app.MapGet("/update", (UserContext dbContext) => {
//    dbContext.Database.BeginTransaction();
//    var order = dbContext.Orders.FirstOrDefault(c => c.Id == 3);
//    var orderItems = new OrderItem() { Count = 1, Price = 20, ProductName = $"IPhone{new Random().Next(1, 20)}" };
//    dbContext.OrderItems.Update(orderItems);
//    dbContext.SaveChanges();
//});

//app.MapGet("/insert", (UserContext dbContext) => {
//    var order = new Order()
//    {
//        Amount = 100,
//        //CreateTime=DateTime.Now,
//        UserName = "张三",
//        OrderNo = Guid.NewGuid().ToString(),
//        OrderItems = new List<OrderItem>()
//        {
//            new OrderItem(){ Count=1, Price=20, ProductName=$"IPhone{new Random().Next(1,20)}" },
//            new OrderItem(){ Count=2, Price=30, ProductName=$"IPhone{new Random().Next(1,20)}" },
//        }
//    };
//    dbContext.Orders.Add(order);
//    dbContext.SaveChanges();
//    return Results.Ok(order.Id);
//});

CreateDbIfNotExists(app);
app.Run();





class HelloHandler
{
    public string Hello()
    {
        return "Hello World";
    }

    public static string SayHello(string name)
    {
        return $"Hello {name}";
    }
}

class Goods
{
    public int GId { get; set; }
    public string GName { get; set; }
    public decimal Price { get; set; }
}