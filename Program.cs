/*
- Creates a WebApplicationBuilder
- Loads:
        - Configuration(appsettings.json, env vars, etc.)
        - Logging
        - Dependency Injection container

- args = command-line arguments

Think of builder as:
“The object where I configure my app before it starts”
*/

var builder = WebApplication.CreateBuilder(args);

//Add services - service container

builder.Services.AddControllers();
/*Enables MVC-style controllers
[ApiController]
[Route("api/[controller]")]
public class UsersController : ControllerBase
*/
builder.Services.AddRazorPages();

builder.Services.AddEndpointsApiExplorer();
/*
Helps Swagger discover  API endpoints.
It scans:
    - Controllers
    - Routes
    - HTTP methods
*/

builder.Services.AddSwaggerGen();
/*
Adds Swagger generator.
Swagger:
   - Auto-generates API documentation
   - Gives UI to test APIs in browser
 */

var app = builder.Build();
/*
This:
    - Locks all configurations
    - Creates the actual running web application
After this can’t add services anymore.
*/

//Pipeline
/*
This is the HTTP request pipeline.
Every request flows like:
Request → Middleware 1 → Middleware 2 → Controller → Response
 */


//Development-only middleware
/*
Only runs in Development mode.
It enables:
    - Swagger JSON
    - Swagger UI page
In production → Swagger is disabled for security.
 */
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
/*
Connects controllers to routing
Without this APIs return 404
*/
app.UseStaticFiles(); //for css or js 
app.UseRouting();

app.MapRazorPages();//enable pages
app.MapControllers();
//Starts the web server (Kestrel).
app.Run();