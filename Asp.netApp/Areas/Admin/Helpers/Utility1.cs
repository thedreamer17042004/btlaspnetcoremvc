using System.Text;
using System.Security.Cryptography;


namespace Authorize_authentication.Areas.Admin.Models
{
    public class Utility1
    {
        public static string MD5Hash(string s)
        {

            using (MD5 md5 = MD5.Create())
            {
                StringBuilder hash = new StringBuilder();
             
                byte[] bytes = md5.ComputeHash(new UTF8Encoding().GetBytes(s));

                for (int i = 0; i < bytes.Length; i++)
                {
                    hash.Append(bytes[i].ToString("x2"));
                }
                return hash.ToString();
            }
        }
    }
}
