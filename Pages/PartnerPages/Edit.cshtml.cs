﻿using System;
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
    public class EditModel : PageModel
    {
        private readonly HttpClient _client;

        public EditModel(HttpClient client)
        {
            _client = client;
        }

        [BindProperty]
        public PartnerDto Partner { get; set; }

        public async Task<IActionResult> OnGetAsync(string id)
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

        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            var bl = new PartnerInputDto();
            bl.Phone = Partner.Phone;
            bl.Name = Partner.Name;
            bl.Email = Partner.Email;
            bl.Location = Partner.Location;
            bl.MomoCode = Partner.MomoCode;
            bl.Name = Partner.Name;
          

            await _client.PutAsJsonAsync(Connection.ApiHost + $"/businessClients/{Partner.Id}",bl);

            return RedirectToPage("./Index");
        }

        
    }
}
