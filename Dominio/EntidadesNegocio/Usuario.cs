using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography; 
using System.IO;
using System.ComponentModel.DataAnnotations; 
using System.ComponentModel.DataAnnotations.Schema;

namespace Dominio
{
    [Table("Usuarios")]
    public class Usuario
    {
        #region Propiedades
        public int Id { get; set; }
        [Required, Index(IsUnique = true), EmailAddress, StringLength(50)] //El mail lo validamos con EmailAdress, si dato con un formato incorrecto, el resto de los usuarios no los carga, es unico y el strenth length es para el unique
        public string Email { get; set; }
        [Required]
        public string Contrasenia { get; set; }
        [Required, RegularExpression("^(?=.*?[A-Z])(?=.*?[a-z])(?=.*?[0-9]).{6,}$")] //Que requiera mayusculas y minusculas y que requiera digitos (\d)
        public string ContraseniaDesencriptada { get; set; }
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
            byte[] cipherBytes = Encoding.Unicode.GetBytes(stringEncriptacion);
            using (Aes encryptor = Aes.Create())
            {
                Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(llaveEncriptacion, new byte[] { 0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76 });
                encryptor.Key = pdb.GetBytes(32);
                encryptor.IV = pdb.GetBytes(16);
                using (MemoryStream ms = new MemoryStream())
                {
                    using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateEncryptor(), CryptoStreamMode.Write))
                    {
                        cs.Write(cipherBytes, 0, cipherBytes.Length);
                        cs.Close();
                    }
                    stringEncriptacion = Convert.ToBase64String(ms.ToArray());
                }
            }
            return stringEncriptacion;
        }

        public string Desencriptacion(string stringDesencriptar)
        {
            string llaveEncriptacion = "f3m8ajfe=@9r3?-e1231fdASXXe12-e===1";
            byte[] cipherBytes = Convert.FromBase64String(stringDesencriptar);
            using (Aes encryptor = Aes.Create())
            {
                Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(llaveEncriptacion, new byte[]
                     { 0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76 });
                encryptor.Key = pdb.GetBytes(32);
                encryptor.IV = pdb.GetBytes(16);
                using (MemoryStream ms = new MemoryStream())
                {
                    using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateDecryptor(),
                                               CryptoStreamMode.Write))
                    {
                        cs.Write(cipherBytes, 0, cipherBytes.Length);
                        cs.Close();
                    }
                    stringDesencriptar = Encoding.Unicode.GetString(ms.ToArray());
                }
            }
            return stringDesencriptar;
        }
        #endregion
    }
}
