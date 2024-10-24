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

namespace WeCareWebApp.Pages.UserPages
{
    public class DeleteModel : PageModel
    {
        private readonly HttpClient _client;

        public DeleteModel(HttpClient client)
        {
            _client = client;
        }

        [BindProperty]
        public UserDto User { get; set; }

        public async Task<IActionResult> OnGetAsync(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            User = await _client.GetFromJsonAsync<UserDto>(Connection.ApiHost + $"/Users/{id}");
            

            if (User == null)
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

            User = await _client.GetFromJsonAsync<UserDto>(Connection.ApiHost + $"/Users/{id}");

            if (User != null)
            {
               
                await _client.DeleteAsync(Connection.ApiHost + $"/Users/{id}");
            }

            return RedirectToPage("./Index");
        }
    }
}
