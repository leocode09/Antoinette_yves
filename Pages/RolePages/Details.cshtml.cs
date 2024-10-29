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

namespace WeCareWebApp.Pages.RolePages
{
    public class DetailsModel : PageModel
    {
        private readonly HttpClient _client;

        public DetailsModel(HttpClient client)
        {
            _client = client;
        }

        public RoleDto Role { get; set; }

        public async Task<IActionResult> OnGetAsync(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Role = await _client.GetFromJsonAsync<RoleDto>(Connection.ApiHost + $"/Roles/{id}");

            if (Role == null)
            {
                return NotFound();
            }
            return Page();
        }
    }
}
