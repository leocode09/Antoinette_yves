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
    public class IndexModel : PageModel
    {
        private readonly HttpClient _client;

        public IndexModel(HttpClient client)
        {
            _client = client;
        }

        public IList<UserDto> User { get;set; }

        public async Task OnGetAsync()
        {
            User = await _client.GetFromJsonAsync<List<UserDto>>(Connection.ApiHost + "/users");
        }
    }
}
