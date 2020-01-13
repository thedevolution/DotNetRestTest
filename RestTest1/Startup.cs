using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using RestTest1.Model;

namespace RestTest1
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
			services.AddAuthentication().AddJwtBearer(cfg =>
			{
				cfg.RequireHttpsMetadata = false;
				cfg.SaveToken = true;

				cfg.TokenValidationParameters = new TokenValidationParameters()
				{
					IssuerSigningKey = TokenAuthOption.Key,
					ValidAudience = TokenAuthOption.Audience,
					ValidIssuer = TokenAuthOption.Issuer,
					// When receiving a token, check that we've signed it.
					ValidateIssuerSigningKey = true,
					// When receiving a token, check that it is still valid.
					ValidateLifetime = true,
					// This defines the maximum allowable clock skew - i.e. provides a tolerance on the token expiry time 
					// when validating the lifetime. As we're creating the tokens locally and validating them on the same 
					// machines which should have synchronised time, this can be set to zero. and default value will be 5minutes
					ClockSkew = TimeSpan.FromMinutes(0)
				};

			});

			services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
		}

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseHsts();
            }

			app.UseAuthentication();
			app.UseHttpsRedirection();
            app.UseMvc();
        }
    }
}
