using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WeCareWebApp.Helpers
{
    public class Connection
    {
        public static string Local { get; } = "Data Source=.; initial Catalog=WkareDbXXX ;Integrated Security=True";
        public static string Development { get; } = "Server=nickodev.database.windows.net;Database=WkareDb;User ID=NickoUser;Password=d7WxCur8TB2SHcFeaYsjC3cg4awF7488;MultipleActiveResultSets=true";
        public static string Production { get; } = "Server=SQL5080.site4now.net;Database=db_a76285_gahunda;User ID=db_a76285_gahunda_admin;Password=G@hund@1h0h0#Db;MultipleActiveResultSets=true";
        public static string JwtIssuer { get; } = "http://smartHr:4286/";
        public static string JwtKey { get; } = "XQPbHd7zdrA&yVJhP-Xr3L";
        public static string EncryptionKey { get; } = "im;8b{skNI=LRo!yfFW~.`7D]m9U^a>r";

        public static string ApiHost { get; } = "http://gahunda-001-site2.itempurl.com";
    }
}
