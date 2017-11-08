using System;
using System.Data.Services.Client;
using System.Linq;
using System.Net;


namespace RegisterDBServerToSLD
{
    public enum SQLVersion
    {
        HANA,
        MSSQL2008,
        MSSQL2008R2,
        MSSQL2012,
        MSSQL2014,
        MSSQL2016
    }


    public class AddToSLD
    {
        private DataServiceContext sldDataServiceContext;
        private CookieContainer cookies;
        private string securityToken = string.Empty;

        public int Perform(string SLDURL, string SLD_Username, string SLD_Password, string InstanceName, SQLVersion TheSQLVersion, string DBUsername, string DBPassword)
        {
            int p_errorCode = 0;
            try
            {
                cookies = new CookieContainer();

                Console.WriteLine("Login to SLD");
                bool p_logon = LoginIntoSLD(SLDURL, SLD_Username, SLD_Password);

                if (!p_logon)
                {
                    Console.WriteLine("SLD Logon failed. LogonByNamedUser failed for user: " + SLD_Username + "\n\n");
                    p_errorCode = -5;
                    return p_errorCode;
                }

                string p_commonDBName = null;
                if (TheSQLVersion == SQLVersion.HANA)
                    p_commonDBName = "SBOCOMMON";
                else
                    p_commonDBName = "SBO-COMMON";

                Console.WriteLine("Add Instance");
                try
                {
                    AddDatabaseInstance(TheSQLVersion, InstanceName, p_commonDBName, DBUsername, DBPassword);
                }
                catch (Exception ex)
                {
                    if (ex.ToString().Contains("You cannot add entity; entity already exists"))
                    {
                        Console.WriteLine("Entity already exists\n\n" + ex.ToString() + "\n\n");
                        p_errorCode = -4;
                    }
                    else
                    {
                        throw;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("General Error\n\n" + ex.ToString() + "\n\n");
                p_errorCode = -3;
            }

            return p_errorCode;
        }

        /*
         * You can find more info in this URL https://localhost:40000/sld/sld.svc/$metadata
         * change the hostname for the sld of choice
         */
        private void AddDatabaseInstance(SQLVersion TheSQLVersion, string InstanceName, string CommonDBName, string DBUsername, string DBPassword)
        {
            try
            {
                string p_sqlServerType = null;
                switch (TheSQLVersion)
                {
                    case SQLVersion.HANA:
                        p_sqlServerType = "HANA";
                        break;
                    case SQLVersion.MSSQL2008:
                        p_sqlServerType = "MSSQL2008";
                        break;
                    case SQLVersion.MSSQL2008R2:
                        p_sqlServerType = "MSSQL2008";
                        break;
                    case SQLVersion.MSSQL2012:
                        p_sqlServerType = "MSSQL2012";
                        break;
                    case SQLVersion.MSSQL2014:
                        p_sqlServerType = "MSSQL2014";
                        break;
                    case SQLVersion.MSSQL2016:
                        p_sqlServerType = "MSSQL2016";
                        break;
                }

                DataServiceQuery<DatabaseInstance> result_query;
                if (TheSQLVersion == SQLVersion.HANA)
                {

                    result_query = sldDataServiceContext.CreateQuery<DatabaseInstance>("AddDatabaseInstance").
                                                                                      AddQueryOption("DBInstance", string.Format("'{0}'", InstanceName)).
                                                                                      AddQueryOption("ServerType", string.Format("'{0}'", p_sqlServerType)).
                                                                                      AddQueryOption("IsTrustedConnection", string.Format("'{0}'", "False")).
                                                                                      AddQueryOption("Username", string.Format("'{0}'", DBUsername)).
                                                                                      AddQueryOption("Password", string.Format("'{0}'", DBPassword)).
                                                                                      AddQueryOption("CommonDB", string.Format("'{0}'", CommonDBName));
                }
                else
                {
                    result_query = sldDataServiceContext.CreateQuery<DatabaseInstance>("AddDatabaseInstance").
                                                                                      AddQueryOption("DBInstance", string.Format("'{0}'", InstanceName)).
                                                                                      AddQueryOption("ServerType", string.Format("'{0}'", p_sqlServerType)).
                                                                                      AddQueryOption("IsTrustedConnection", string.Format("'{0}'", "False")).
                                                                                      AddQueryOption("Username", string.Format("'{0}'", DBUsername)).
                                                                                      AddQueryOption("Password", string.Format("'{0}'", DBPassword)).
                                                                                      AddQueryOption("CommonDB", string.Format("'{0}'", CommonDBName));
                }


                DatabaseInstance result = result_query.FirstOrDefault();

                if (result == null)
                    throw new Exception("AddDatabaseInstance - incorrect return value");

            }
            catch
            {
                throw;
            }
        }

        private bool LoginIntoSLD(string URL, string SLDUsername, string SLDPassword)
        {
            lock (this)
            {
                try
                {

                    cookies = new CookieContainer();
                    sldDataServiceContext = new DataServiceContext(new Uri(URL));

                    sldDataServiceContext.SendingRequest += new EventHandler<SendingRequestEventArgs>(sldDataServiceContext_SendingRequest);
                    sldDataServiceContext.IgnoreMissingProperties = true;

                    string strLogonCmd = string.Empty;

                    string tempUser = System.Web.HttpUtility.UrlEncode(SLDUsername, System.Text.Encoding.UTF8);
                    string tempPassword = System.Web.HttpUtility.UrlEncode(SLDPassword, System.Text.Encoding.UTF8);
                    strLogonCmd = string.Format("LogonByNamedUser?Account='{0}'&Password='{1}'", tempUser, tempPassword);
                    Uri uri = new Uri(strLogonCmd, UriKind.Relative);

                    DataServiceRequest<bool> loginRequest = new DataServiceRequest<bool>(uri);
                    var loginResponse = sldDataServiceContext.ExecuteBatch(loginRequest);

                    foreach (var response in loginResponse)
                    {
                        var queryOperationResponse = response as QueryOperationResponse<bool>;
                        if (queryOperationResponse == null || !queryOperationResponse.FirstOrDefault())
                            return false;
                    }

                }
                catch
                {
                    throw;
                }

                try
                {
                    if (!GenerateSecurityToken())
                        return false;
                }
                catch
                {
                    throw;
                }

                return true;
            }
        }

        void sldDataServiceContext_SendingRequest(object sender, SendingRequestEventArgs e)
        {
            ((HttpWebRequest)e.Request).CookieContainer = cookies;

            if (securityToken != null && securityToken != string.Empty)
                ((HttpWebRequest)e.Request).Headers.Add("securityToken", securityToken);

            ServicePointManager.ServerCertificateValidationCallback = (a, b, c, d) => true;
        }

        bool GenerateSecurityToken()
        {
            if (securityToken != string.Empty)
                return true;

            securityToken = string.Empty;
            Uri uri = new Uri("GenerateSecurityToken", UriKind.Relative);

            DataServiceRequest<string> loginRequest = new DataServiceRequest<string>(uri);
            var loginResponse = sldDataServiceContext.ExecuteBatch(loginRequest);
            QueryOperationResponse<string> queryOperationResponse = loginResponse.FirstOrDefault() as QueryOperationResponse<string>;
            if (queryOperationResponse == null)
                return false;

            string token = queryOperationResponse.FirstOrDefault();
            if (token == null)
                return false;

            securityToken = token;

            return true;
        }

    }
}
 