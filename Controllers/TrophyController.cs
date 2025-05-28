using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace ApexVolley.Controllers
{
    public class TrophyController : Controller
    {
        public IActionResult Index()
        {
            var trofei = new List<Trofeo>
            {
                new Trofeo { Nome = "Superlega Italiana 2024", ImmagineUrl = "/images/scudetto.png"},
                new Trofeo { Nome = "Coppa Italia 2024", ImmagineUrl = "/images/coppa_italia.png" },
                new Trofeo { Nome = "CEV Champions League 2023", ImmagineUrl = "/images/champions_league.png" },
                new Trofeo { Nome = "Supercoppa Italiana 2023", ImmagineUrl = "/images/supercoppa.png" },
                new Trofeo { Nome = "Campionato del Mondo per Club 2022", ImmagineUrl = "/images/mondiale_club.png" },
                new Trofeo { Nome = "Coppa Italia 2022", ImmagineUrl = "/images/coppa_italia.png" },
                new Trofeo { Nome = "Superlega Italiana 2021", ImmagineUrl = "/images/scudetto.png" },
                new Trofeo { Nome = "Supercoppa Italiana 2019", ImmagineUrl = "/images/supercoppa.png" }
            };

            return View(trofei);
        }

        public class Trofeo
        {
            public string Nome { get; set; }
            public string ImmagineUrl { get; set; }
        }
    }
}
