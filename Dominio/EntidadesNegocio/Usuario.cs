using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography; 
using System.IO;

namespace Dominio
{
    public class Usuario
    {
        #region Propiedades
        public string Email { get; set; }
        public string Contrasenia { get; set; }
        #endregion

        #region Validaciones
        public bool ValidarContrasenia(string contrasenia)
        {
            if(contrasenia != null) 
            {
                return contrasenia.Length >= 6 && contrasenia.Any(char.IsDigit) && contrasenia.Any(char.IsLower) && contrasenia.Any(char.IsUpper);
            }
            else
            {
                return false;
            }
        }

        public bool ValidarMail(string mail)
        {
            try
            {
                var unEmail = new System.Net.Mail.MailAddress(mail);
                return unEmail.Address == mail && mail.Length > 4;
            }
            catch
            {
                return false;
            }
        }
        #endregion

        #region Metodos
        public string Encriptacion(string stringEncriptacion)
        {
            string llaveEncriptacion = "f3m8ajfe=@9r3?-e1231fdASXXe12-e===1";
            byte[] clearBytes = Encoding.Unicode.GetBytes(stringEncriptacion);
            using (Aes encryptor = Aes.Create())
            {
                Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(llaveEncriptacion, new byte[] { 0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76 });
                encryptor.Key = pdb.GetBytes(32);
                encryptor.IV = pdb.GetBytes(16);
                using (MemoryStream ms = new MemoryStream())
                {
                    using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateEncryptor(), CryptoStreamMode.Write))
                    {
                        cs.Write(clearBytes, 0, clearBytes.Length);
                        cs.Close();
                    }
                    stringEncriptacion = Convert.ToBase64String(ms.ToArray());
                }
            }
            return stringEncriptacion;
        }
        #endregion
    }
}
