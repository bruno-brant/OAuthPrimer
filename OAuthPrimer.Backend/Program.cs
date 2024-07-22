using Microsoft.AspNetCore.Authentication.JwtBearer;

//
// ⚒️ T O D O: Register the OAuthPrimer.Backend application with Azure AD -
//             1. Again, go to https://entra.microsoft.com and add a new application. 
//             2. This time select the supported account types as Accounts in
//             any organizational directory. Don't choose a redirect URI.
//             3. Take note of the Application (client) ID and Tentant ID
//

const string tenantId = TODO;
const string AppUri = TODO;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

builder.Services
	.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
	.AddJwtBearer(options =>
	{    
        options.MetadataAddress = $"https://login.microsoftonline.com/{tenantId}/v2.0/.well-known/openid-configuration";

        //
        // ⚒️ T O D O: Configure the audience for this appication
        //
        options.Audience = TODO;

        // This is an additional issuer, because (at least here) the token is being issued as if by a v1 endpoint.
        options.TokenValidationParameters = new()
        {
            ValidateIssuer = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = $"https://sts.windows.net/{tenantId}/",
            
            ValidateAudience = true,
            
            //
            // ⚒️ T O D O: Configure the audience for this appication
            //
            ValidAudience = AppUri,

            ValidateLifetime = true,
        };
	});

// Configure the HTTP request pipeline.
var app = builder.Build();

//app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

// ⚒️ T O D O: Add a controller to the application that requires authorization
app
	.MapControllers()
	.RequireAuthorization();

app.Run();
