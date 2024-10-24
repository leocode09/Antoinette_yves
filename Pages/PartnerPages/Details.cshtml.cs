using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using WeCareWebApp.Entities;
using WeCareWebApp.Helpers;
using WeCareWebApp.Models;

namespace WeCareWebApp.Pages.PartnerPages
{
    public class DetailsModel : PageModel
    {
        private readonly HttpClient _client;

        public DetailsModel(HttpClient client)
        {
            _client = client;
        }

        public PartnerDto Partner { get; set; }

        public async Task<IActionResult> OnGetAsync(int id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Partner = await _client.GetFromJsonAsync<PartnerDto>(Connection.ApiHost + $"/partner/{id}");

            if (Partner == null)
            {
                return NotFound();
            }
            return Page();
        }
    }
}
