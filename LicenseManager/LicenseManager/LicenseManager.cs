using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Win32;
using Rhino.Licensing;

namespace LicenseManager
{
    internal class LicenseManager
    {
        internal static string GetPublicKey()
        {
            //Public key from LicenseGenerator/AdminTool.exe
            return @"<RSAKeyValue><Modulus>qEvHbFwDTZQLcwY+T5dTVgbjWzGYJZOAUPqaCxnNqHhcYocRgew7tTe7JHAOp1cfoEykGjiirS9uAZfybNdXgWvZ6aLb7i/p3ufLC+Gf7Kg59J74W7yp+jKoqVN+hUASCZHeVzLKxj8EDk3FtWiikCM9+AggbIXjZ/u+xEX/5QE=</Modulus><Exponent>AQAB</Exponent></RSAKeyValue>";
        }

        internal static bool CheckLicense()
        {
            //RhinoLicensing
            try
            {
                var publicKey = GetPublicKey();

                string _licensePath = "\\license.xml";

                LicenseValidator _licenseValidator = new LicenseValidator(publicKey, _licensePath);
                _licenseValidator.AssertValidLicense();

                LicenseType _licenseType = _licenseValidator.LicenseType;


                return true;

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return false;
            }
        }


        internal static bool CheckLicenseStatus(out LicenseType _licenseType, out string _name, out string _email)
        {
            _name = "";
            _email = "";
            string _macIDFromSystem = GetMacAddress();
            string _macIDFromLicenseFile = "";

            //RhinoLicensing
            try
            {
                var publicKey = GetPublicKey();

                string _licensePath = "license.xml";
                //MessageBox.Show(_licensePath);

                LicenseValidator _licenseValidator = new LicenseValidator(publicKey, _licensePath);
                _licenseValidator.AssertValidLicense();
                _licenseType = _licenseValidator.LicenseType;
                _email = _licenseValidator.LicenseAttributes["email"];
                _macIDFromLicenseFile = _licenseValidator.LicenseAttributes["mac"];

                if (_macIDFromLicenseFile != _macIDFromSystem)
                    return false;

                if (System.DateTime.Today > _licenseValidator.ExpirationDate)
                    return false;

                //if (HasTimeBeenTampered())
                //    return false;

                _name = _licenseValidator.Name;
                return true;
            }
            catch (Exception ex)
            {
                //MessageBox.Show(ex.ToString());
                _licenseType = LicenseType.None;
                return false;
            }
        }



        internal static void CheckLicenseStatus2(out LicenseType _licenseType, out string _name, out string _email)
        {
            _name = "";
            _email = "";

            //RhinoLicensing
            try
            {
                var publicKey = GetPublicKey();

                string _licensePath = "license.xml";
                //MessageBox.Show(_licensePath);

                string _floatingServerURL = "192.138.2.2";
                Guid _clientID = Guid.NewGuid();

                LicenseValidator _licenseValidator = new LicenseValidator(publicKey, _licensePath, _floatingServerURL, _clientID);
                _licenseValidator.AssertValidLicense();
                _licenseType = _licenseValidator.LicenseType;
                _email = _licenseValidator.LicenseAttributes["email"];
                _name = _licenseValidator.Name;
                int _limit = Convert.ToInt32(_licenseValidator.LicenseAttributes["limit"]);

                LicensingService _licenseService = new LicensingService();
                StringLicenseValidator stringLicenseValidator = new StringLicenseValidator(publicKey, _licensePath);


            }
            catch (Exception ex)
            {
                //MessageBox.Show(ex.ToString());
                _licenseType = LicenseType.None;
                //return false;
            }
        }


        internal static string GetMacAddress()
        {//https://stackoverflow.com/questions/2333149/how-to-fast-get-hardware-id-in-c
            try
            {
                string location = @"SOFTWARE\Microsoft\Cryptography";
                string name = "MachineGuid";

                using (RegistryKey localMachineX64View =
                    RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, RegistryView.Registry64))
                {
                    using (RegistryKey rk = localMachineX64View.OpenSubKey(location))
                    {
                        if (rk == null)
                            throw new KeyNotFoundException(
                                string.Format("Key Not Found: {0}", location));

                        object machineGuid = rk.GetValue(name);
                        if (machineGuid == null)
                            throw new IndexOutOfRangeException(
                                string.Format("Index Not Found: {0}", name));

                        return machineGuid.ToString();
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return "";
            }
        }

        //internal static string GetMacAddress()
        //{//http://www.eigo.co.uk/News-Article.aspx?NewsArticleID=63
        //    try
        //    {
        //        Guid guidMachineGUID = Guid.Empty;
        //        if (Microsoft.Win32.Registry.LocalMachine.OpenSubKey("SOFTWARE\\Microsoft\\Cryptography") != null)
        //        {
        //            if (Microsoft.Win32.Registry.LocalMachine.OpenSubKey("SOFTWARE\\Microsoft\\Cryptography").GetValue("MachineGuid") != null)
        //            {
        //                // Get the stored value
        //                guidMachineGUID = new Guid(Microsoft.Win32.Registry.LocalMachine.OpenSubKey("SOFTWARE\\Microsoft\\Cryptography").GetValue("MachineGuid").ToString());
        //            }
        //        }
        //        return guidMachineGUID.ToString("N");
        //    }
        //    catch (Exception ex)
        //    {
        //        MessageBox.Show(ex.ToString());
        //        return "";
        //    }
        //}


        public static bool HasTimeBeenTampered()
        {//https://www.codeproject.com/Questions/5257324/How-can-I-detect-someone-has-changed-system-date-t
            bool time = false;

            EventLog log = new EventLog("Security");
            var entries = log.Entries.Cast<EventLogEntry>()
              //verify two days ago
              .Where(x => x.TimeWritten >= DateTime.Now.AddHours(-48))
              .Select(x => new
              {
                  x.InstanceId,
                  x.ReplacementStrings,
              }).ToList();

            foreach (var entrie in entries)
            {
                if (entrie.InstanceId == 4616)
                {
                    //"S-1-5-18" = Local System
                    //"S-1-5-19" = NT Authority
                    //"S-1-5-21" = Administrator
                    if (entrie.ReplacementStrings[0].Contains("S-1-5-21"))
                    {
                        time = true;
                    }
                }
            }

            return time;
        }
    }
}
