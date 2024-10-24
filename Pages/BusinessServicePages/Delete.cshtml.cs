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

namespace WeCareWebApp.Pages.BusinessServicePages
{
    public class DeleteModel : PageModel
    {
        private readonly HttpClient _client;

        public DeleteModel(HttpClient client)
        {
            _client = client;
        }

        [BindProperty]
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

        public async Task<IActionResult> OnPostAsync(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            BusinessService = await _client.GetFromJsonAsync<BusinessService>(Connection.ApiHost + $"/businessServices/{id}");

            if (BusinessService != null)
            {
               
                await _client.DeleteAsync(Connection.ApiHost + $"/businessServices/{id}");
            }

            return RedirectToPage("./Index");
        }
    }
}
