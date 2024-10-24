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
    public class EditModel : PageModel
    {
        private readonly HttpClient _client;

        public EditModel(HttpClient client)
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

        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            var bl = new ReferenceInputDto();
            bl.Name = BusinessService.Name;

            await _client.PutAsJsonAsync(Connection.ApiHost + $"/businessServices/{BusinessService.Id}",bl);

            return RedirectToPage("./Index");
        }

        
    }
}
