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

namespace WeCareWebApp.Pages.UserPages
{
    public class EditModel : PageModel
    {
        private readonly HttpClient _client;

        public EditModel(HttpClient client)
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

        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            var bl = new UserInputDto();
            bl.Names = User.Names;
            bl.RoleId = User.RoleId;
            bl.Username = User.Username;

            await _client.PutAsJsonAsync(Connection.ApiHost + $"/Users/{User.Id}",bl);

            return RedirectToPage("./Index");
        }

        
    }
}
