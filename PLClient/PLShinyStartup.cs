using Microsoft.Extensions.DependencyInjection;
using Shiny;
using System;
using System.Collections.Generic;
using System.Text;

namespace PLClient {
    public class PLShinyStartup : Shiny.ShinyStartup {
        public override void ConfigureServices(IServiceCollection builder) {
            builder.UseBleClient();
        }
    }
}
