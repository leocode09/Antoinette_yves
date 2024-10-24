using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using WeCareWebApp.Helpers;
using WeCareWebApp.Models;

namespace WeCareWebApp.Pages.PartnerPages
{
    public class IndexModel : PageModel
    {
        private readonly HttpClient _client;

        public IndexModel(HttpClient client)
        {
            _client = client;
        }

        public IList<PartnerDto> Partner { get; set; }

        public async Task OnGetAsync()
        {
            Partner = await _client.GetFromJsonAsync<List<PartnerDto>>(Connection.ApiHost + "/partners");
        }
    }
}
