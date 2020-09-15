using System;
using System.Text;

namespace Storage.API.Models
{
    public class User
    {
		public int id { get; set; }
        public string username { get; set; }
        public string passwordHash { get; set; }
        public string passwordSalt { get; set; }

        public void SetPassword(string password)
        {
            passwordHash = password;
            passwordSalt = "salt";
            //using (var hmac = new System.Security.Cryptography.HMACSHA512())
            //{
            //    passwordSalt = Encoding.UTF8.GetString(hmac.Key);
            //    passwordHash = Encoding.UTF8.GetString(hmac.ComputeHash(Encoding.UTF8.GetBytes(password)));
            //}
        }

        public bool VerifyPassword(string password)
        {
            if (password == passwordHash) return true;
            else return false;
            //using (var hmac = new System.Security.Cryptography.HMACSHA512(Encoding.UTF8.GetBytes(passwordSalt)))
            //{
            //    var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
            //    Console.WriteLine("{0} {1}",Encoding.UTF8.GetString(computedHash), passwordHash);
            //    for (int i = 0; i < computedHash.Length; i++)
            //    {
            //        if (computedHash[i] != passwordHash[i])
            //        {
            //            return false;
            //        }
            //    }
            //    return true;
            //}
        }
    }
}
