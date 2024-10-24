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

namespace WeCareWebApp.Pages.RolePages
{
    public class DeleteModel : PageModel
    {
        private readonly HttpClient _client;

        public DeleteModel(HttpClient client)
        {
            _client = client;
        }

        [BindProperty]
        public Role Role { get; set; }

        public async Task<IActionResult> OnGetAsync(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Role = await _client.GetFromJsonAsync<Role>(Connection.ApiHost + $"/Roles/{id}");
            

            if (Role == null)
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

            Role = await _client.GetFromJsonAsync<Role>(Connection.ApiHost + $"/Roles/{id}");

            if (Role != null)
            {
               
                await _client.DeleteAsync(Connection.ApiHost + $"/Roles/{id}");
            }

            return RedirectToPage("./Index");
        }
    }
}
