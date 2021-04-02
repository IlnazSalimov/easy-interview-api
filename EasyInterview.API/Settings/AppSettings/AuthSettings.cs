using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EasyInterview.API.Settings.AppSettings
{
    public class AuthSettings
    {
        public JwtSettings Jwt { get; set; }
        public GoogleSettings Google { get; set; }
    }
}
