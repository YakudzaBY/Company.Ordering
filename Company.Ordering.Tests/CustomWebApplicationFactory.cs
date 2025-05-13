using Company.Ordering.Infrastructure;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Company.Ordering.Tests;

public class CustomWebApplicationFactory<TProgram>
    : WebApplicationFactory<TProgram> where TProgram : class
{
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureServices(services =>
        {
            var dbContextDescriptor = services
                .SingleOrDefault(d => d.ServiceType == typeof(DbContextOptions<OrderingDbContext>));

            services.Remove(dbContextDescriptor);

            var suffix = DateTime.UtcNow.Ticks.ToString();
            services.AddDbContext<OrderingDbContext>(options =>
            {
                options.UseSqlServer($"Server=(localdb)\\MSSQLLocalDB;Database=Ordering_{suffix};Integrated Security=true;");
            });

            var provider = services.BuildServiceProvider();

            using (var scope = provider.CreateScope())
            {
                var db = scope.ServiceProvider.GetRequiredService<OrderingDbContext>();
                db.Database.EnsureCreated();
            }
        });
    }
}
