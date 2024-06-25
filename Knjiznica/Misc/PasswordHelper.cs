using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace Knjiznica.Misc
{
    public class PasswordHelper
    {
            public static string IzracunajHash(string lozinka)
            {
                var sBytes = new UTF8Encoding().GetBytes(lozinka);
                byte[] hBytes;
                using (var alg = new System.Security.Cryptography.SHA256Managed())
                {
                    hBytes = alg.ComputeHash(sBytes);
                }
                return Convert.ToBase64String(hBytes);
            }
        }
    }

