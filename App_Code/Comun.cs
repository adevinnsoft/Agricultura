using System;
using System.Text;
using System.Data;
using System.Collections.Generic;
using System.IO;
using System.Web.UI.WebControls;


/// <summary>
/// Descripción breve de Common
/// </summary>
public static class Comun
{
    
    public enum STATUS
    {
        Process,
        User_Response,
        Canceled,
        Solved,
        Diagnostic,
        New,
        Review
    }

    public enum ROLES_USERS
    {
        Data_Owner,
        Application_Manager,
        Infrastructure_Manager,
        Development_Manager,
        Director,
        Ticket_User,
        IT_Responsable
    }

    public enum MESSAGE_TYPE
    {
        Error,
        Info,
        Warning,
        Success,
        YesNo,
        YesNoCancel
    }

    public enum PRIORITY
    {
        Low,
        Normal,
        High,
        Critical
    }

    public enum DEPARTMENT
    {
        HD,
        IT,
        DE,
        US,
        APP
    }

    public enum RFC_STATUS
    {
        User_Capture,
        IT_Analysis,
        Approval,
        Verification,
        Accepting
    }

    public static int getIDRFCStatusNumber(RFC_STATUS rfcStatus)
    {

        switch (rfcStatus)
        {
            case RFC_STATUS.User_Capture:
                return 1;
            case RFC_STATUS.IT_Analysis:
                return 2;
            case RFC_STATUS.Approval:
                return 3;
            case RFC_STATUS.Verification:
                return 4;
            case RFC_STATUS.Accepting:
                return 5;
            default:
                return 1;
        }
    }

    public static RFC_STATUS getRFCStatusByNumber(int idRfcStatus)
    {
        switch (idRfcStatus)
        {
            case 1:
                return RFC_STATUS.User_Capture;
            case 2:
                return RFC_STATUS.IT_Analysis;
            case 3:
                return RFC_STATUS.Approval;
            case 4:
                return RFC_STATUS.Verification;
            case 5:
                return RFC_STATUS.Accepting;
            default:
                return RFC_STATUS.User_Capture;
        }
    }

    public static int getIDRoleUsersNumber(ROLES_USERS roleUsers)
    {
        switch (roleUsers)
        {
            case ROLES_USERS.Data_Owner:
                return 1;
            case ROLES_USERS.Application_Manager:
                return 2;
            case ROLES_USERS.Infrastructure_Manager:
                return 3;
            case ROLES_USERS.Development_Manager:
                return 4;
            case ROLES_USERS.Director:
                return 5;
            case ROLES_USERS.Ticket_User:
                return 6;
            case ROLES_USERS.IT_Responsable:
                return 7;
        }
        return 6;
    }

    public static ROLES_USERS getRoleUsersByID(int roleUsers)
    {
        switch (roleUsers)
        {
            case 1:
                return ROLES_USERS.Data_Owner;
            case 2:
                return ROLES_USERS.Application_Manager;
            case 3:
                return ROLES_USERS.Infrastructure_Manager;
            case 4:
                return ROLES_USERS.Development_Manager;
            case 5:
                return ROLES_USERS.Director;
            case 6:
                return ROLES_USERS.Ticket_User;
            case 7:
                return ROLES_USERS.IT_Responsable;
        }
        return ROLES_USERS.Ticket_User;
    }


    public static int getIdDepartmentNumber(Comun.DEPARTMENT department)
    {
        switch (department)
        {
            case DEPARTMENT.HD:
                return 1;
            case DEPARTMENT.IT:
                return 3;
            case DEPARTMENT.DE:
                return 4;
            case DEPARTMENT.US:
                return 0;
            case DEPARTMENT.APP:
                return 2;
            default:
                return 0;
        }
    }

    public static Comun.DEPARTMENT getDepartmentEnum(int department)
    {
        switch (department)
        {
            case 1:
                return DEPARTMENT.HD;
            case 3:
                return DEPARTMENT.IT;
            case 4:
                return DEPARTMENT.DE;
            case 0:
                return DEPARTMENT.US;
            case 2:
                return DEPARTMENT.APP;
            default:
                return DEPARTMENT.US;
        }
    }

    public static int getPriorityNumber(Comun.PRIORITY priority)
    {
        switch (priority)
        {
            case PRIORITY.Low:
                return 1;
            case PRIORITY.Normal:
                return 2;
            case PRIORITY.High:
                return 3;
            case PRIORITY.Critical:
                return 4;
            default:
                return 1;
                break;
        }
    }
    public static Comun.PRIORITY getPriorityEnumByNumber(int priority)
    {
        switch (priority)
        {
            case 1:
                return PRIORITY.Low;
            case 2:
                return PRIORITY.Normal;
            case 3:
                return PRIORITY.High;
            case 4:
                return PRIORITY.Critical;
            default:
                return PRIORITY.Low;
                break;
        }
    }

    public static int getStatusNumber(Comun.STATUS status)
    {
        if (status == STATUS.Solved)
        { return 1; }
        if (status == STATUS.Process)
        { return 2; }
        if (status == STATUS.Canceled)
        { return 3; }
        if (status == STATUS.User_Response)
        { return 4; }
        if (status == STATUS.Diagnostic)
        { return 5; }
        if (status == STATUS.New)
        { return 6; }
        if (status == STATUS.Review)
        { return 7; }
        return 0;

    }

    public static STATUS getStatusByNumber(int idStatus)
    {
        switch (idStatus)
        {
            case 1:
                return STATUS.Solved;
            case 2:
                return STATUS.Process;
            case 3:
                return STATUS.Canceled;
            case 4:
                return STATUS.User_Response;
            case 5:
                return STATUS.Diagnostic;
            case 6:
                return STATUS.New;
            case 7:
                return STATUS.Review;
            default:
                return STATUS.Solved;
        }
    }

    public enum TYPE
    {
        Incident,
        Requirement   
    }

    public enum ActionHD
    {
        Created,
        AddCommentUser,
        AddCommentIT,
        Escalated,
        Critical
    }

    public static int getTypeNumber(Comun.TYPE  type)
    {
        if (type == Comun.TYPE.Requirement)
        { return 1; }
        if (type == Comun.TYPE.Incident)
        { return 2; }
        return 0;
    }

    public static void deleteDirectory(string sourceFile)
    {
        try
        {
            if (sourceFile != "" && Directory.Exists(sourceFile))
            {
                Directory.Delete(sourceFile, true);
            }
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
            
        }
    }

    public static void deleteFile(string sourceFile)
    {
        try
        {
            if (sourceFile != "" && File.Exists(sourceFile))
            {
                File.Delete(sourceFile);
            }
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);   
        }
    }

    public static void makeDirectoryIfNotExists(string NewDirectory)
    {
        try
        {
            if (!Directory.Exists(NewDirectory))
            {
                Directory.CreateDirectory(NewDirectory);
            }
        }
        catch (IOException ex)
        {
            throw ex;
        }
    }

    /// <summary>
    /// If the user has the access to be in this page
    /// </summary>
    /// <param name="department"></param>
    /// <param name="pageName">"IT" IT | "HD" Help Desk | "US" User</param>
    /// <returns></returns>
    public static bool isTheUserAllow(int department, string pageName)
    {
        switch (pageName)
        {
            case "US":
                if (department == 0)
                { return true; }
                else
                { return false; }
            case "IT":
                if (department != 0 && department != 1)
                { return true; }
                else
                { return false; }
            case "HD":
                if (department == 1)
                { return true; }
                else
                { return false; }
            default: return false;
        }
    }

    public static bool isTheUserAdmin(int adminRights)
    {
        return adminRights == 0 ? false : true;
    }

    /// <summary>
    /// Get Infotmation data for Radio Button
    /// </summary>
    /// <param name="english">true to english: false to spanish</param>
    /// <returns></returns>
    public static DataTable getRadioButtonInformation(bool english)
    {
        DataTable dt = new DataTable();
        dt.Columns.Add("ID");
        dt.Columns.Add("Description");
        DataRow dr = dt.NewRow();
        dr["ID"] = "1";
        dr["Description"] = english ? "User" : "Usuario";
        dt.Rows.Add(dr);
        dr = dt.NewRow();
        dr["ID"] = "2";
        dr["Description"] = english ? "IT Department" : "Departamento de IT";
        dt.Rows.Add(dr);
        return dt;
    }

    public static DataTable getRadioButtonSearchOptionCreateInformation(bool english)
    {
        DataTable dt = new DataTable();
        dt.Columns.Add("ID");
        dt.Columns.Add("Description");
        DataRow dr = dt.NewRow();
        dr["ID"] = "1";
        dr["Description"] = english ? "User Name" : "Nombre de Usuario";
        dt.Rows.Add(dr);
        dr = dt.NewRow();
        dr["ID"] = "2";
        dr["Description"] = english ? "Name" : "Nombre";
        dt.Rows.Add(dr);
        return dt;
    }

    public static DataTable getRadioButtonRFCUserCaputre(bool english)
    {
        DataTable dt = new DataTable();
        dt.Columns.Add("ID");
        dt.Columns.Add("Description");
        DataRow dr = dt.NewRow();
        dr["ID"] = "1";
        dr["Description"] = english ? "Corrective Action" : "Acción Correctiva";
        dt.Rows.Add(dr);
        dr = dt.NewRow();
        dr["ID"] = "2";
        dr["Description"] = english ? "Preventive Action" : "Acción Preventiva";
        dt.Rows.Add(dr);
        return dt;
    }

    public static string formatEmailFromDataTable(DataTable dt)
    {
        StringBuilder emailsFormat = new StringBuilder();

        foreach (DataRow item in dt.Rows)
        {
            emailsFormat.Append(item["Email"].ToString());
            emailsFormat.Append(";");
        }
        return emailsFormat.ToString();
    }

    public static string formatEmailFromDataTable(List<string> list)
    {
        StringBuilder emailsFormat = new StringBuilder();

        foreach (string item in list)
        {
            emailsFormat.Append(item);
            emailsFormat.Append(";");
        }
        return emailsFormat.ToString();
    }

    public static string getDateToFile(string fileName)
    {
        StringBuilder sbCadDate = new StringBuilder();
        DateTime dTime = DateTime.Now;
        if (fileName.Length > 50)
        {
            fileName = fileName.Substring(1, 50);
        }
        sbCadDate.Append(fileName);
        sbCadDate.Append(dTime.Year.ToString());
        sbCadDate.Append(dTime.Month.ToString());
        sbCadDate.Append(dTime.Day.ToString());
        sbCadDate.Append(dTime.Hour.ToString());
        sbCadDate.Append(dTime.Minute.ToString());
        sbCadDate.Append(dTime.Second.ToString());

        return sbCadDate.ToString();
    }

    public static void insertTicketLog(string ticketNumber, int idStatus, string userAssign,string userAction, string connection)
    {
        DataAccess dataaccess = new DataAccess();
         //@IdTicket int,  
         //@IdStatus int,  
         //@UserNameAssign varchar(20),  
         //@UserNameAction varchar(20)  
        Dictionary<string, object> parameters = new Dictionary<string, object>();
        parameters.Add("@IdTicket", double.Parse(ticketNumber));
        parameters.Add("@IdStatus", idStatus);
        parameters.Add("@UserNameAssign", userAssign);
        parameters.Add("@UserNameAction", userAction);
        try
        {
            dataaccess.executeStoreProcedureNonQuery("dbo.sp_addTicketLog", parameters);
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }

    public static string getEmailUserHDFromDB(string userName, string connection)
    {
        DataAccess dataaccess = new DataAccess();
        try
        {
            Dictionary<string, object> parameters = new Dictionary<string, object>();
            parameters.Add("@UserName", userName);
            DataTable dt = dataaccess.executeStoreProcedureDataTable("dbo.sp_getEmailHDAndIT", parameters);
            if (dt.Rows.Count > 0)
            {
                return dt.Rows[0]["Email"].ToString();
            }
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
        return "";
    }

    public static DataTable getUserInformationFromDB(string userName, string connection)
    {
        DataAccess dataaccess = new DataAccess();
        try
        {
            Dictionary<string, object> parameters = new Dictionary<string, object>();
            parameters.Add("@UserName", userName);
            parameters.Add("@Password", "");
            parameters.Add("@Domain", "");
            DataTable dt = dataaccess.executeStoreProcedureDataTable("dbo.sp_getUserLogInformation", parameters);
            return dt.Rows.Count > 0 ? dt : null;
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }

    //public static void processInternalMail(Common.ActionHD action, Dictionary<string, string> parameters, string leng)
    //{
    //    /*
    //     Parameters
    //     * userTo
    //     */
    //    dgMail mail;
    //    switch (action)
    //    {
    //        case Common.ActionHD.Created:
    //            break;
    //        case Common.ActionHD.AddCommentUser:
    //            //To User
    //            mail = new dgMail(
    //                            parameters["userTo"],
    //                            parameters["subject"],
    //                            Common.getHtmlBodyAssignedTicketUser(
    //                                                        parameters["ticketNumber"],
    //                                                        parameters["userHD"],
    //                                                        parameters["password"],
    //                                                        parameters["comment"],
    //                                                        parameters["userName"],
    //                                                        parameters["status"],
    //                                                        leng,
    //                                                        bool.Parse(
    //                                                            parameters["SolvedCanceled"])
    //                                                        ,
    //                                                        parameters["IniDate"]),
    //                            true);
    //            mail.SendMail();
    //            break;
    //        case Common.ActionHD.AddCommentIT:
    //            mail = new dgMail(
    //                            parameters["userTo"],
    //                            parameters["subject"],
    //                            Common.getHtmlBodyCommentToIT(
    //                                                        parameters["ticketNumber"],
    //                                                        parameters["subject"],
    //                                                        parameters["userHD"],
    //                                                        parameters["comment"],
    //                                                        leng),
    //                            true);
    //            mail.SendMail();
    //            break;
    //        case Common.ActionHD.Escalated:
    //            mail = new dgMail(
    //                            parameters["userTo"],
    //                            parameters["subject"],
    //                            Common.getHtmlBodyEscalation(
    //                                                        parameters["ticketNumber"],
    //                                                        parameters["subject"],
    //                                                        parameters["description"],
    //                                                        parameters["userName"],
    //                                                        parameters["userPhone"],
    //                                                        parameters["comment"],
    //                                                        leng),
    //                            true);
    //            mail.SendMail();
    //            break;
    //        case Common.ActionHD.Critical:
    //            //To al managers
    //            //it can be with high priority
    //            mail = new dgMail(
    //                            getAllManagersEmails(),
    //                            "CRITICAL: " + parameters["subject"],
    //                            Common.getHtmlBodyAssignedCritical(
    //                                                        parameters["ticketNumber"],
    //                                                        parameters["subject"],
    //                                                        parameters["description"],
    //                                                        parameters["userName"],
    //                                                        parameters["userPhone"],
    //                                                        leng),
    //                            true);
    //            mail.SendMail();
    //            break;
    //        default:
    //            break;
    //    }
    //}

    //private static string getAllManagersEmails()
    //{
    //    try
    //    {
    //        return Common.formatEmailFromDataTable(dataaccess.executeStoreProcedureDataTable("dbo.sp_getAllManagersEmails", new Dictionary<string, object>()));
    //    }
    //    catch (Exception)
    //    {
    //        return "";
    //    }
    //}

    //public static string getHtmlBodyNewTicketCreated(string user, string password, string ticketNumber, string subject ,string leng)
    //{
    //    StringBuilder HTMLbody = new StringBuilder();
    //        HTMLbody.Append("<html>");
    //        HTMLbody.Append("<body style = \"font-family:Calibri;\">");
    //        HTMLbody.Append(string.Format("<p>Ticket: <strong>{0}</strong></p>",ticketNumber));

    //        if (leng == "ES")
    //        {
    //            HTMLbody.Append(string.Format(lablesXML.getNameSpanish("SpanishEmailTicketCreated1"), subject)); HTMLbody.Append(".<br />");
    //            HTMLbody.Append(lablesXML.getNameSpanish("SpanishEmailTicketCreated2"));
    //            HTMLbody.Append(string.Format(lablesXML.getNameSpanish("SpanishEmailTicketCreated3"),
    //                                        user, Security.Encrypt(password), ticketNumber, Security.Encrypt("Usr").Substring(1, 5), Security.Encrypt("Pass").Substring(1, 5), Security.Encrypt("Ticket").Substring(1, 5)));
    //        }
    //        else
    //        {
    //            HTMLbody.Append(string.Format(lablesXML.getNameSpanish("EnglishEmailTicketCreated1"), subject)); HTMLbody.Append(".<br />");
    //            HTMLbody.Append(lablesXML.getNameSpanish("EnglishEmailTicketCreated2"));
    //            HTMLbody.Append(string.Format(lablesXML.getNameSpanish("EnglishEmailTicketCreated3"),
    //                                        user, Security.Encrypt(password), ticketNumber, Security.Encrypt("Usr").Substring(1, 5), Security.Encrypt("Pass").Substring(1, 5), Security.Encrypt("Ticket").Substring(1, 5)));
    //        }

    //        HTMLbody.Append("<br/>FAST</p>");
    //        HTMLbody.Append("</body>");
    //        HTMLbody.Append("</html>");
    //    return HTMLbody.ToString();
    //}

    //public static string getHtmlBodyNewTicketCreatedToHelpDesk(string ticketNumber, string subject, string descripcion, string user, string phone ,string leng)
    //{
    //    StringBuilder HTMLbody = new StringBuilder();

    //        HTMLbody.Append("<html>");
    //        HTMLbody.Append("<body style = \"font-family:Calibri;\">");

    //        if (leng == "ES")
    //        {
    //            HTMLbody.Append(string.Format(lablesXML.getNameSpanish("SpanishEmailNewTicketCreatedToHelpDesk"), ticketNumber));	 
    //        }
    //        else
    //        {
    //            HTMLbody.Append(string.Format(lablesXML.getNameSpanish("EnglishEmailNewTicketCreatedToHelpDesk"), ticketNumber));	                         
    //        }
    //        HTMLbody.Append(string.Format("<p>{1}: <strong>{0}</strong><br />", subject, lablesXML.getNameSpanish("SUBJECT", leng)));
    //        HTMLbody.Append(string.Format("<p>{1}: <strong>{0}</strong><br />", user, lablesXML.getNameSpanish("USER", leng)));
    //        HTMLbody.Append(string.Format("<p>{1}: <strong>{0}</strong><br />", descripcion, lablesXML.getNameSpanish("DESCRIPTION", leng)));
    //        HTMLbody.Append(string.Format("<p>{1}: <strong>{0}</strong><br />", phone, lablesXML.getNameSpanish("PHONE", leng)));
    //        HTMLbody.Append(string.Format("<a href=\"{1}/frmLogin.aspx\">{0}</a>", 
    //                                              System.Configuration.ConfigurationManager.AppSettings.Get("appTitle")
    //                                            , System.Configuration.ConfigurationManager.AppSettings.Get("SourceWebPage")));
    //        HTMLbody.Append("<br/>PITS</p>");
    //        HTMLbody.Append("</body>");
    //        HTMLbody.Append("</html>");
    //    return HTMLbody.ToString();
    //}

    //public static string getHtmlBodyUserCommentedTicket(string userName, string ticketNumber, string commented)
    //{
    //    StringBuilder HTMLbody = new StringBuilder();
    //    HTMLbody.Append("<html>");
    //    HTMLbody.Append("<body style = \"font-family:Calibri;\">");
    //    HTMLbody.AppendFormat(lablesXML.getNameSpanish("EnglishEmailAssignedTicketUser"), ticketNumber);	 
    //    HTMLbody.AppendFormat(lablesXML.getNameSpanish("EnglishEmailAssignedTicketUser1"), userName, commented);	 
    //    HTMLbody.Append("<br/>PITS</p>");
    //    HTMLbody.Append("</body>");
    //    HTMLbody.Append("</html>");
    //    return HTMLbody.ToString();
    //}

    //public static string getHtmlBodyAssignedTicketUser(string ticketNumber, string userHD, string password, string comment, string user, string status, string leng, bool isSolvedCanceled,string dateTime)
    //{
    //    StringBuilder HTMLbody = new StringBuilder();

    //        HTMLbody.Append("<html>");
    //        HTMLbody.Append("<body style = \"font-family:Calibri;\">");
    //        //HTMLbody.Append(string.Format("<p>¿Qué tal {0}?</p>", user));
    //        string checkEmailHere = string.Empty;
    //        if (leng == "ES")
    //        { 
    //            HTMLbody.Append(string.Format(lablesXML.getNameSpanish("SpanishEmailAssignedTicketUser"), ticketNumber));
    //            HTMLbody.Append(string.Format(lablesXML.getNameSpanish("SpanishEmailAssignedTicketUser1"), userHD, comment));
    //            checkEmailHere = lablesXML.getNameSpanish("SpanishEmailAssignedTicketUser2");
    //        }
    //        else
    //        { 
    //            HTMLbody.Append(string.Format(lablesXML.getNameSpanish("EnglishEmailAssignedTicketUser"), ticketNumber));
    //            HTMLbody.Append(string.Format(lablesXML.getNameSpanish("EnglishEmailAssignedTicketUser1"), userHD, comment));
    //            checkEmailHere = lablesXML.getNameSpanish("EnlgishEmailAssignedTicketUser2");
    //        }
            
    //        HTMLbody.Append(string.Format("<p>{1}: <strong>{0}</strong><br />", status,lablesXML.getNameSpanish("STATUS")));

    //        if (!isSolvedCanceled)
    //        {
    //            HTMLbody.Append(string.Format(checkEmailHere,
    //                                    user, password, ticketNumber,
    //                                    Security.Encrypt("Usr").Substring(1, 5),
    //                                    Security.Encrypt("Pass").Substring(1, 5),
    //                                    Security.Encrypt("Ticket").Substring(1, 5)));
    //        }
    //        if (isSolvedCanceled)
    //        {
    //            HTMLbody.Append(string.Format(
    //                            lablesXML.getNameSpanish(
    //                                    leng == "EN" ? "EnglishNotAgree" : "SpanishNotAgree"),
    //                                        Security.Encrypt("TicketNo").Substring(1, 5), 
    //                                        Security.Encrypt(ticketNumber), 
    //                                        Security.Encrypt("UserName").Substring(1,5),
    //                                        Security.Encrypt(user))); 
                
    //        } 
    //        HTMLbody.Append("<br/>PITS</p>");
    //        HTMLbody.Append("</body>");
    //        HTMLbody.Append("</html>");

    //    return HTMLbody.ToString();
    //}


    public static string getHtmlBodyAssignedCritical(string ticketNumber, string subject, string descripcion, string user, string phone, string leng)
    {
        StringBuilder HTMLbody = new StringBuilder();
        if (leng == "ES")
        {
            HTMLbody.Append("<html>");
            HTMLbody.Append("<body style = \"font-family:Calibri;\">");
            HTMLbody.Append("<p><strong>TICKET CRÍTICO</strong></p>");
            HTMLbody.Append(string.Format("<p>Ticket número: <strong>{0}</strong></p>", ticketNumber));
            HTMLbody.Append(string.Format("<p>ASUNTO: <strong>{0}</strong><br />", subject));
            HTMLbody.Append(string.Format("USUARIO: <strong>{0}</strong><br />", user));
            HTMLbody.Append(string.Format("DESCRIPCIÓN: <strong>{0}</strong><br />", descripcion));
            HTMLbody.Append(string.Format("TELÉFONO: <strong>{0}</strong><br /></p>", phone));
            HTMLbody.Append(string.Format("<a href=\"{1}/frmLogin.aspx\">{0}</a>", 
                                                      System.Configuration.ConfigurationManager.AppSettings.Get("appTitle")
                                                    , System.Configuration.ConfigurationManager.AppSettings.Get("SourceWebPage")));
            HTMLbody.Append("<br/>PITS");
            HTMLbody.Append("</body>");
            HTMLbody.Append("</html>");
        }
        else
        {
            HTMLbody.Append("<html>");
            HTMLbody.Append("<body style = \"font-family:Calibri;\">");
            HTMLbody.Append("<p><strong>TICKET CRÍTICO</strong></p>");
            HTMLbody.Append(string.Format("<p>Ticket número: <strong>{0}</strong></p>", ticketNumber));
            HTMLbody.Append(string.Format("<p>ASUNTO: <strong>{0}</strong><br />", subject));
            HTMLbody.Append(string.Format("USUARIO: <strong>{0}</strong><br />", user));
            HTMLbody.Append(string.Format("DESCRIPCIÓN: <strong>{0}</strong><br />", descripcion));
            HTMLbody.Append(string.Format("TELÉFONO: <strong>{0}</strong><br /></p>", phone));
            HTMLbody.Append(string.Format("<a href=\"{1}/frmLogin.aspx\">{0}</a>", 
                                                      System.Configuration.ConfigurationManager.AppSettings.Get("appTitle")
                                                    , System.Configuration.ConfigurationManager.AppSettings.Get("SourceWebPage")));
            HTMLbody.Append("<br/>PITS");
            HTMLbody.Append("</body>");
            HTMLbody.Append("</html>");

        }
        return HTMLbody.ToString();
    }

    public static string getHtmlBodyEscalation(string ticketNumber, string subject, string descripcion, string user, string phone, string comment, string leng)
    {
        StringBuilder HTMLbody = new StringBuilder();
        //testing
        if (true || leng == "ES")
        {
            HTMLbody.Append("<html>");
            HTMLbody.Append("<body style = \"font-family:Calibri;\">");
            HTMLbody.Append(string.Format("<p>Ticket número: <strong>{0}</strong></p>", ticketNumber));
            HTMLbody.Append("<p><strong>ESTE TICKET FUE ESCALADO PARA TI</strong></p>");
            HTMLbody.Append(string.Format("<p>Comentario: {0}<br /><br />", comment));
            HTMLbody.Append(string.Format("ASUNTO: <strong>{0}</strong><br />", subject));
            HTMLbody.Append(string.Format("USUARIO: <strong>{0}</strong><br />", user));
            HTMLbody.Append(string.Format("DESCRIPCIÓN: <strong>{0}</strong><br />", descripcion));
            HTMLbody.Append(string.Format("TELÉFONO: <strong>{0}</strong><br /></p>", phone));
            HTMLbody.Append(string.Format("<a href=\"{1}/frmLogin.aspx\">{0}</a>", 
                                                      System.Configuration.ConfigurationManager.AppSettings.Get("appTitle")
                                                    , System.Configuration.ConfigurationManager.AppSettings.Get("SourceWebPage")));
            HTMLbody.Append("<br/>PITS");
            HTMLbody.Append("</body>");
            HTMLbody.Append("</html>");
        }
        return HTMLbody.ToString();
    }

    public static string getHtmlBodyNotAgreeToHDManager(string ticketNumber, string userName, string comment, string connection)
    {
        DataAccess dataaccess = new DataAccess();
        StringBuilder HTMLbody = new StringBuilder();
        DataTable dt = null;
        string leng = "";
        
        try
        {
            Dictionary<string, object> parameters = new Dictionary<string, object>();
            parameters.Add("@idTicket", ticketNumber);
            dt = dataaccess.executeStoreProcedureDataTable("dbo.sp_getTicketInfo", parameters);
        }catch { }

        if (true || leng == "ES")
        {
            HTMLbody.Append("<html>");
            HTMLbody.Append("<body style = \"font-family:Calibri;\">");
            HTMLbody.Append(string.Format("<p>En el ticket: <strong>{0}</strong> que perteneciente al usuario: {1}</p>", ticketNumber,userName));
            HTMLbody.Append(string.Format("<p>No le pareció correcta la decisión de su ticket, por lo cual comentó:<br/>{0}</p>", comment));
            if (dt != null && dt.Rows.Count > 0)
            {
                HTMLbody.Append(string.Format("<p>Cerrado por: {0}", dt.Rows[0]["Responsable"].ToString()));
                HTMLbody.Append(string.Format("<br/>Último Sub-Responsable por: {0}", dt.Rows[0]["AssignedTo"].ToString()));
                HTMLbody.Append(string.Format("<br/>Teléfono del usuario: {0}</p>", dt.Rows[0]["UserPhone"].ToString()));
            }
            HTMLbody.Append(string.Format("<br/><a href=\"{1}/frmLogin.aspx\">{0}</a>", 
                                                              System.Configuration.ConfigurationManager.AppSettings.Get("appTitle")
                                                            , System.Configuration.ConfigurationManager.AppSettings.Get("SourceWebPage")));
            HTMLbody.Append("<br/>PITS");
            HTMLbody.Append("</body>");
            HTMLbody.Append("</html>");
        }
        return HTMLbody.ToString();
    }

    public static string getHtmlBodyNotAgreeToUser(string HDManager, string fullName)
    {
        StringBuilder HTMLbody = new StringBuilder();
        string leng = "";
        HTMLbody.Append("<html>");
        HTMLbody.Append("<body style = \"font-family:Calibri;\">");

        if (leng == "ES")
        {
            HTMLbody.Append(string.Format("<p> {1} Gracias por tu comentario, {0} estará a cargo de este ticket</p>", HDManager, fullName));
        }
        HTMLbody.Append(string.Format("<br/><a href=\"{1}/frmLogin.aspx\">{0}</a>", 
                                                  System.Configuration.ConfigurationManager.AppSettings.Get("appTitle")
                                                , System.Configuration.ConfigurationManager.AppSettings.Get("SourceWebPage")));
        HTMLbody.Append("<br/>PITS");
        HTMLbody.Append("</body>");
        HTMLbody.Append("</html>");

        return HTMLbody.ToString();
    }

    public static string getHtmlBodyCommentToIT(string ticketNumber, string subject, string userHD, string comment, string leng)
    {
        StringBuilder HTMLbody = new StringBuilder();
        //testing
        if (true || leng == "ES")
        {
            HTMLbody.Append("<html>");
            HTMLbody.Append("<body style = \"font-family:Calibri;\">");
            HTMLbody.Append(string.Format("<p>Ticket número: <strong>{0}</strong></p>", ticketNumber));
            HTMLbody.Append(string.Format("<p>{0} Comentó: {1}<br /><br />", userHD, comment));
            HTMLbody.Append(string.Format("ASUNTO: <strong>{0}</strong><br />", subject));
            HTMLbody.Append(string.Format("<a href=\"{1}/frmLogin.aspx\">{0}</a>", 
                                                              System.Configuration.ConfigurationManager.AppSettings.Get("appTitle")
                                                            , System.Configuration.ConfigurationManager.AppSettings.Get("SourceWebPage")));
            HTMLbody.Append("<br/>PITS");
            HTMLbody.Append("</body>");
            HTMLbody.Append("</html>");
        }
        return HTMLbody.ToString();
    }

    public static string cleanScriptTags(string textToClean)
    {
        //textToClean = textToClean.ToLowerInvariant();
        textToClean = textToClean.Replace("<script>", "-script-");
        textToClean = textToClean.Replace("</script>", "-/script-");
        return textToClean;

    }
    
    public static string ConvertSortDirectionToSql( SortDirection sortDirection)
    {
        string newSortDirection = String.Empty;

        switch (sortDirection)
        {
            case SortDirection.Ascending:
                newSortDirection = "ASC";
                break;

            case SortDirection.Descending:
                newSortDirection = "DESC";
                break;
            default:
                newSortDirection = "ASC";
                break;
        }
        return newSortDirection;
    }

    /// <summary>SendMailByDictionary is a method in the MailSend Class.
    /// <para>Allowed String Keys, DataInformation, ApprovedInvoid and RejectedInvoice.</para>
    /// </summary>
    public static void SendMailByDictionary(Dictionary<string, string> dicEmailInfo, Dictionary<string, Stream> dicFiles, string emailTo, String Key, string sms)
    {
        clsEmail email = new clsEmail(Key);
        try
        {

            email.setMessage(dicEmailInfo);
            email.setFiles(dicFiles);
            email.ToAdress = emailTo;
            email.Send(sms);
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }

}
