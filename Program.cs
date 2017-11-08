using System;

namespace RegisterDBServerToSLD
{
    class Program
    {
        static int Main(string[] args)
        {
            try
            {
                string SLDURL = null;
                string SLD_Username = null;
                string SLD_Password = null;
                string InstanceName = null;
                SQLVersion? TheSQLVersion = null;
                string DBUsername = null;
                string DBPassword = null;

                int p_nbParamUsed = 0;

                try
                {
                    for (int i = 0; i < args.Length; i++)
                    {
                        switch (args[i])
                        {
                            case "/sldserver":
                                if (args.Length < i + 2)
                                {
                                    PrintHelp();
                                    return -1;
                                }

                                SLDURL = args[i + 1];
                                SLDURL = "https://" + SLDURL + ":40000/sld/sld.svc";

                                p_nbParamUsed += 2;
                                break;

                            /*
                        case "/sldhdbostname":
                            if (args.Length < i + 2)
                            {
                                PrintHelp();
                                return -1;
                            }

                            SLD_DBHostname = args[i + 1];

                            p_nbParamUsed += 2;
                            break;*/

                            case "/sldusername":
                                if (args.Length < i + 2)
                                {
                                    PrintHelp();
                                    return -1;
                                }

                                SLD_Username = args[i + 1];

                                p_nbParamUsed += 2;
                                break;

                            case "/sldpassword":
                                if (args.Length < i + 2)
                                {
                                    PrintHelp();
                                    return -1;
                                }

                                SLD_Password = args[i + 1];

                                p_nbParamUsed += 2;
                                break;

                            case "/instancename":
                                if (args.Length < i + 2)
                                {
                                    PrintHelp();
                                    return -1;
                                }

                                InstanceName = args[i + 1];

                                p_nbParamUsed += 2;
                                break;

                            case "/sqlversion":
                                if (args.Length < i + 2)
                                {
                                    PrintHelp();
                                    return -1;
                                }

                                switch (args[i + 1])
                                {
                                    case "HANA": TheSQLVersion = SQLVersion.HANA; break;
                                    case "SQL2008": TheSQLVersion = SQLVersion.MSSQL2008; break;
                                    case "SQL2008R2": TheSQLVersion = SQLVersion.MSSQL2008R2; break;
                                    case "SQL2012": TheSQLVersion = SQLVersion.MSSQL2012; break;
                                    case "SQL2014": TheSQLVersion = SQLVersion.MSSQL2014; break;
                                    case "SQL2016": TheSQLVersion = SQLVersion.MSSQL2016; break;

                                    default:
                                        PrintHelp();
                                        return -1;
                                }

                                p_nbParamUsed += 2;
                                break;

                            case "/dbusername":
                                if (args.Length < i + 2)
                                {
                                    PrintHelp();
                                    return -1;
                                }

                                DBUsername = args[i + 1];

                                p_nbParamUsed += 2;
                                break;

                            case "/dbpassword":
                                if (args.Length < i + 2)
                                {
                                    PrintHelp();
                                    return -1;
                                }

                                DBPassword = args[i + 1];

                                p_nbParamUsed += 2;
                                break;
                        }
                    }

                }
                catch
                {
                    PrintHelp();
                    return -1;
                }

                if (p_nbParamUsed != args.Length)
                {
                    PrintHelp();
                    return -1;
                }

                if (SLDURL == null ||
                    SLD_Username == null ||
                    SLD_Password == null ||
                    InstanceName == null ||
                    !TheSQLVersion.HasValue ||
                    DBUsername == null ||
                    DBPassword == null)
                {
                    PrintHelp();
                    return -1;
                }


                AddToSLD p_adder = new AddToSLD();
                return p_adder.Perform(SLDURL, SLD_Username, SLD_Password, InstanceName, TheSQLVersion.Value, DBUsername, DBPassword);
            }
            catch (Exception ex)
            {
                Console.WriteLine("General Error\n" + ex.ToString());
                return -4;
            }
        }

        static private void PrintHelp()
        {
            Console.WriteLine(@"Usage: RegisterDBServerToSLD /sldserver [SLDSERVER] /sldusername [SLDUSERNAME /sldpassword [SLDPASSWORD] /instancename [INSTANCETOADD] /sqlversion [SQLVERSION] /dbusername [INSTANCETOADDUSERNAME] /dbpassword [INSTANCETOADDPASSWORD]

Arguments:

/sldserver - example: localhost if SLD is https://localhost:40000/ControlCenter

/sldusername - example: B1SiteUser

/slddassword - Password to the slddbusername

/instancename - example:  b1server,65170 for a named instance

/sqlversion - The vesion of SQL to use. Values: 
    HANA
    SQL2008
    SQL2008R2
    SQL2012
    SQL2014
    SQL2016

/dbusername - example: sa 

/dbpassword - Password to the dbusername");
        }
    }
}
