using BlazingPizza.Data;
using BlazingPizza.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();
builder.Services.AddHttpClient();
builder.Services.AddSqlite<PizzaStoreContext>("Data Source=pizza.db");
builder.Services.AddScoped<OrderState>();
var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
}

app.UseStaticFiles();
app.UseRouting();

app.MapRazorPages();
app.MapBlazorHub();
app.MapFallbackToPage("/_Host");
// JsonReader fix
//웹 애플리케이션의 URL을 컨트롤러의 액션 메서드와 연결하는 역할. 이 설정은 주로 ASP.NET Core의 MVC (Model-View-Controller) 패턴을 사용하는 웹 애플리케이션에서 사용
//라우팅은 클라이언트의 요청 URL을 해석하여 해당 요청을 처리할 컨트롤러와 액션 메서드를 결정하는 프로세스
// "default": 이 부분은 라우팅의 이름을 지정합니다. 이름을 지정하는 것은 선택사항
// "{controller=Home}/{action=Index}/{id?}": 이 부분은 라우팅 템플릿(template)을 정의
// - {controller=Home}: URL에서 class 이름 부분을 의미합니다. class에서도 Controller라고 붙은 부분을 빼고 사용함. 즉, /ControllerName 형태의 URL에서 "ControllerName" 부분을 추출. 
// - {action=Index}: URL에서 액션 메서드 부분(함수 부분)을 의미합니다. 즉, /ControllerName/ActionName 형태의 URL에서 "ActionName" 부분을 추출합니다. 기본값으로 "Index"가 지정되어 있으므로, 만약 URL에 액션 메서드가 지정되지 않은 경우 기본적으로 Index 액션 메서드가 실행
// - {id?}: URL에서 선택적인 매개변수를 의미합니다. 이 부분은 /ControllerName/ActionName/SomeValue 형태의 URL에서 "SomeValue"를 추출합니다. 물음표(?)는 이 매개변수가 선택적이라는 것을 나타냅니다. 따라서 이 부분은 생략될 수 있습니다.
// 위의 라우팅 템플릿은 대부분의 기본적인 웹 애플리케이션에서 사용되는 일반적인 설정으로, 요청된 URL에 따라 적절한 컨트롤러와 액션 메서드를 호출하여 해당하는 웹 페이지 또는 기능을 제공하는 데 사용
// https://learn.microsoft.com/ko-kr/aspnet/core/mvc/controllers/routing?view=aspnetcore-7.0
app.MapDefaultControllerRoute();
//app.MapControllerRoute("default", "{controller=Home}/{action=Index}/{id?}");

//Initialize the db and seed data inject
// IServiceScopeFactory 인터페이스를 사용하여 서비스의 스코프를 생성하는 팩토리 객체 가지고 오기.

var scopeFactory = app.Services.GetRequiredService<IServiceScopeFactory>();
// scope 변수를 만들며 사용한 블록이 끝나면 자동으로 정리
using (var scope = scopeFactory.CreateScope())
{
    //scope를 사용하여 PizzaStoreContext 클래스의 인스턴스를 서비스 컨테이너에 가지고 온다.
    var db = scope.ServiceProvider.GetRequiredService<PizzaStoreContext>();
    if (db.Database.EnsureCreated())
    {
        SeedData.Initialize(db);
    }
}

app.Run();