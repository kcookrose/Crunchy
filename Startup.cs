using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Crunchy.Models;

using Crunchy.Services;
using Crunchy.Services.Interfaces;

namespace Crunchy
{
    public class Startup {

        public Startup(IConfiguration configuration)  {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services) {
            services.AddTransient<ITodoItemService, TodoItemService>();
            services.AddDbContext<TodoContext>();
            services.AddMvc();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env) {
            app.UseStaticFiles();
            app.UseMvc();
        }
    }
}
