using ApiTestTask.Services.SearchDataProviderServices;
using ApiTestTask.Services.SearchServices;

namespace TestTask;

public partial class Program
{

    public static void Main(string[] args)
    {

        var builder = WebApplication.CreateBuilder(args);

        builder.Services.AddControllers();
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();
        builder.Services.AddHttpClient();
        builder.Services.AddScoped<ISearchService, MainSearchService>();
        builder.Services.AddScoped<ISearchDataProviderService, DataProviderOneService>();
        builder.Services.AddScoped<ISearchDataProviderService, DataProviderTwoService>();

        var app = builder.Build();
        app.UseRouting();
        app.UseAuthorization();
        app.MapControllers();
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
            app.UseDeveloperExceptionPage();
        }
        app.Run();
    }
}
