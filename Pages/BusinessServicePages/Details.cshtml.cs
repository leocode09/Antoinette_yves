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

namespace WeCareWebApp.Pages.BusinessServicePages
{
    public class DetailsModel : PageModel
    {
        private readonly HttpClient _client;

        public DetailsModel(HttpClient client)
        {
            _client = client;
        }

        public BusinessService BusinessService { get; set; }

        public async Task<IActionResult> OnGetAsync(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            BusinessService = await _client.GetFromJsonAsync<BusinessService>(Connection.ApiHost + $"/businessServices/{id}");

            if (BusinessService == null)
            {
                return NotFound();
            }
            return Page();
        }
    }
}
