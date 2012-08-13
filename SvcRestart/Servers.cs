using System;
using System.Collections.Generic;
using System.DirectoryServices;
using System.Management;

using log4net;

namespace SvcRestart
{
    public class Servers
    {
        private string ldap;

        private readonly ILog log;

        public Servers(string ldap, ILog log)
        {
            this.ldap = ldap;
            this.log = log;
        }

        public List<string> Get()
        {
            var rtn = new List<string>();

            int versionCompareResult;

            if(!ldap.StartsWith("LDAP://"))
            {
                ldap = string.Format("LDAP://{0}",ldap);
            }

            var dirEntry = new DirectoryEntry(ldap);
            var dirSearch = new DirectorySearcher(dirEntry);
            var serverQuery = new ManagementObjectSearcher("SELECT * FROM Win32_OperatingSystem");
            var serverQueryCollection = serverQuery.Get();
            dirSearch.Filter = "(objectClass=computer)";

            foreach (ManagementObject mo in serverQueryCollection)
            {
                versionCompareResult = string.CompareOrdinal(mo["version"].ToString(), "5.1 (2600)");
                mo["version"].ToString();

                if (versionCompareResult == 1)
                {
                    try
                    {
                        foreach (SearchResult sr in dirSearch.FindAll())
                        {
                            using (DirectoryEntry computer = sr.GetDirectoryEntry())
                            {
                                string version = "notSet";
                                if(computer.Properties["operatingsystemversion"].Value != null)
                                {
                                    version = computer.Properties["operatingsystemversion"].Value.ToString();    
                                }
                                
                                string name = computer.Name.Substring(3);
                                log.Debug(string.Format("{0} {1}", computer.Name.Substring(3), version));                                
                                switch (version)
                                {
                                    case "5.2 (3790)":
                                        {
                                            rtn.Add(name);
                                            break;
                                        }
                                    case "6.0 (6002)":
                                        {
                                            rtn.Add(name);
                                            break;
                                        }

                                    case "6.1 (7601)":
                                        {
                                            rtn.Add(name);
                                            break;
                                        }

                                    default:
                                        {
                                            break;
                                        }
                                }
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        log.Error(ex);
                        log.Error(ex.StackTrace);
                    }
                }
            }
            return rtn;
        }
    }
}