using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;

namespace CoviDoc.Tests
{
    public class TestStartup  :Startup
    {
        public TestStartup(IConfiguration configuration, IWebHostEnvironment env) : base(configuration, env)
        {
        }
    }
}
