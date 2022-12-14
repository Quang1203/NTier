using MISA.Web07.GD.ndquang.BL;
using MISA.Web07.GD.ndquang.DL;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddScoped(typeof(IBaseDL<>), typeof(BaseDL<>));
builder.Services.AddScoped(typeof(IBaseBL<>), typeof(BaseBL<>));
builder.Services.AddScoped<IEmployeeBL, EmployeeBL>();
builder.Services.AddScoped<IEmployeeDL, EmployeeDL>();
builder.Services.AddScoped<IGroupBL, GroupBL>();
builder.Services.AddScoped<IGroupDL, GroupDL>();
builder.Services.AddScoped<ISubjectBL, SubjectBL>();
builder.Services.AddScoped<ISubjectDL, SubjectDL>();
builder.Services.AddScoped<IStorageRoomBL, StorageRoomBL>();
builder.Services.AddScoped<IStorageRoomDL, StorageRoomDL>();
builder.Services.AddScoped<IEmployeeDetailBL, EmployeeDetailBL>();
builder.Services.AddScoped<IEmployeeDetailDL, EmployeeDetailDL>();

// Lấy dữ liệu connection string từ file appsettings
DatabaseContext.ConnectionString = builder.Configuration.GetConnectionString("MySqlConnection");

// cho các prop của các bảng bắt đầu viết hoa giống DB
builder.Services.AddControllers().AddJsonOptions(options =>
options.JsonSerializerOptions.PropertyNamingPolicy = null);

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


var app = builder.Build();

// cấp quyền truy cập cho api
app.UseCors(builder => builder
    .AllowAnyOrigin()
    .AllowAnyMethod()
    .AllowAnyHeader());
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();

app.MapControllers();

app.Run();
