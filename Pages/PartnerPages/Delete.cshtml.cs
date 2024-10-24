using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using WeCareWebApp.Entities;
using WeCareWebApp.Helpers;
using WeCareWebApp.Models;

namespace WeCareWebApp.Pages.PartnerPages
{
    public class DeleteModel : PageModel
    {
        private readonly HttpClient _client;

        public DeleteModel(HttpClient client)
        {
            _client = client;
        }

        [BindProperty]
        public PartnerDto Partner { get; set; }

        public async Task<IActionResult> OnGetAsync(int id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Partner = await _client.GetFromJsonAsync<PartnerDto>(Connection.ApiHost + $"/partners/{id}");
            

            if (Partner == null)
            {
                return NotFound();
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Partner = await _client.GetFromJsonAsync<PartnerDto>(Connection.ApiHost + $"/partners/{id}");

            if (Partner != null)
            {
               
                await _client.DeleteAsync(Connection.ApiHost + $"/partner/{id}");
            }

            return RedirectToPage("./Index");
        }
    }
}
