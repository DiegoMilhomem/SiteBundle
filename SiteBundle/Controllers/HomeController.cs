using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using SiteBundle.Models;

namespace SiteBundle.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private IMemoryCache _cache;
        public HomeController(ILogger<HomeController> logger, IMemoryCache cache)
        {
            _logger = logger;
            _cache = cache;
        }

        public async Task<IActionResult> Index()
        {
            //gerar a data atual (Criado o Metodo abaixo)

            //colocar essa data em uma viewbag
            ViewBag.Data = await GetData();
            return View();
        }

        private async Task<DateTime> GetData()
        {
            var chaveCache = "Hora";
            DateTime data;
            
            if(!_cache.TryGetValue(chaveCache, out data))
            {
                await Task.Delay(5000);
                var opcoesDoCache = new MemoryCacheEntryOptions()
                {
                    AbsoluteExpiration = DateTime.Now.AddSeconds(30)
                };
                data = DateTime.Now;
                _cache.Set(chaveCache, data, opcoesDoCache);
            }
            return DateTime.Now;
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
