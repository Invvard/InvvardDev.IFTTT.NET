using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace InvvardDev.Ifttt.Hosting;

public static class SwaggerGenOptionsExtensions
{
    private const string SecuritySchemeName = "IftttApiKey";
    
    public static void AddIftttSecurityKeyScheme(this SwaggerGenOptions options)
    {
        options.AddSecurityDefinition(SecuritySchemeName, new OpenApiSecurityScheme
                                                          {
                                                              Type = SecuritySchemeType.ApiKey,
                                                              Name = IftttConstants.ServiceKeyHeader,
                                                              In = ParameterLocation.Header,
                                                              Description = "Authorization API key based header",
                                                              Scheme = "ApiKeyScheme"
                                                          });
        var key = new OpenApiSecurityScheme()
                  {
                      Reference = new OpenApiReference
                                  {
                                      Type = ReferenceType.SecurityScheme,
                                      Id = SecuritySchemeName
                                  },
                      In = ParameterLocation.Header
                  };
        options.AddSecurityRequirement(new OpenApiSecurityRequirement { { key, new List<string>() } });
    }
}