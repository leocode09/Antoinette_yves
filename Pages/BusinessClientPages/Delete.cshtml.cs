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

namespace WeCareWebApp.Pages.BusinessClientPages
{
    public class DeleteModel : PageModel
    {
        private readonly HttpClient _client;

        public DeleteModel(HttpClient client)
        {
            _client = client;
        }

        [BindProperty]
        public BusinessClient BusinessClient { get; set; }

        public async Task<IActionResult> OnGetAsync(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            BusinessClient = await _client.GetFromJsonAsync<BusinessClient>(Connection.ApiHost + $"/businessClients/{id}");
            

            if (BusinessClient == null)
            {
                return NotFound();
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            BusinessClient = await _client.GetFromJsonAsync<BusinessClient>(Connection.ApiHost + $"/businessClients/{id}");

            if (BusinessClient != null)
            {
               
                await _client.DeleteAsync(Connection.ApiHost + $"/businessClients/{id}");
            }

            return RedirectToPage("./Index");
        }
    }
}
