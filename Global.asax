<%@ Application Language="C#" %>
<%@ Import Namespace="System.Web.Configuration" %>

<script runat="server">
    
    /*      QUARTZ       */
    Quartz.IScheduler sched = null;
    public void Application_Start(object sender, EventArgs e)
    {
        // Code that runs on application startup
        Quartz.ISchedulerFactory schedFact = new Quartz.Impl.StdSchedulerFactory();
        sched = schedFact.GetScheduler();
        sched.Start();
    }

    public void Application_End(object sender, EventArgs e)
    {
        // Code that runs on application shutdown
        try
        {
            sched.Shutdown();
        }
        catch (Exception ex)
        {

        }
    }
    /*          FIN QUARTZ       */

    void Application_Error(object sender, EventArgs e) 
    { 
        // Code that runs when an unhandled error occurs
        HttpContext context = ((HttpApplication)sender).Context;
        if (GlobalHelper.IsMaxRequestExceededException(this.Server.GetLastError()))
        {
            this.Server.ClearError();
            context.Response.Redirect("~/error/GenericError.aspx");
        }
    }

    private class GlobalHelper
    {
        const int TimedOutExceptionCode = -2147467259;
        public static bool IsMaxRequestExceededException(Exception e)
        {
            // unhandled errors = caught at global.ascx level
            // http exception = caught at page level

            Exception main;
            var unhandled = e as HttpUnhandledException;

            if (unhandled != null && unhandled.ErrorCode == TimedOutExceptionCode)
            {
                main = unhandled.InnerException;
            }
            else
            {
                main = e;
            }


            var http = main as HttpException;

            if (http != null && http.ErrorCode == TimedOutExceptionCode)
            {
                // hack: no real method of identifying if the error is max request exceeded as 
                // it is treated as a timeout exception
                if (http.StackTrace.Contains("GetEntireRawContent"))
                {
                    // MAX REQUEST HAS BEEN EXCEEDED
                    return true;
                }
            }

            return false;
        }

    }
    
    void Session_Start(object sender, EventArgs e) 
    {
        // Code that runs when a new session is started

    }

    void Session_End(object sender, EventArgs e) 
    {
        // Code that runs when a session ends. 
        // Note: The Session_End event is raised only when the sessionstate mode
        // is set to InProc in the Web.config file. If session mode is set to StateServer 
        // or SQLServer, the event is not raised.

    }

    protected void Application_BeginRequest(Object sender, EventArgs e)
    {
        HttpRuntimeSection runTime = (HttpRuntimeSection)WebConfigurationManager.GetSection("system.web/httpRuntime");
        //Approx 100 Kb(for page content) size has been deducted because the maxRequestLength proprty is the page size, not only the file upload size
        int maxRequestLength = (runTime.MaxRequestLength - 100) * 1024;

        //This code is used to check the request length of the page and if the request length is greater than 
        //MaxRequestLength then retrun to the same page with extra query string value action=exception

        HttpContext context = ((HttpApplication)sender).Context;
        if (context.Request.ContentLength > maxRequestLength)
        {
            IServiceProvider provider = (IServiceProvider)context;
            HttpWorkerRequest workerRequest = (HttpWorkerRequest)provider.GetService(typeof(HttpWorkerRequest));
            // Check if body contains data
            if (workerRequest.HasEntityBody())
            {
                // get the total body length
                int requestLength = workerRequest.GetTotalEntityBodyLength();
                // Get the initial bytes loaded
                int initialBytes = 0;
                if (workerRequest.GetPreloadedEntityBody() != null)
                    initialBytes = workerRequest.GetPreloadedEntityBody().Length;
                if (!workerRequest.IsEntireEntityBodyIsPreloaded())
                {
                    byte[] buffer = new byte[512000];
                    // Set the received bytes to initial bytes before start reading
                    int receivedBytes = initialBytes;
                    while (requestLength - receivedBytes >= initialBytes)
                    {
                        // Read another set of bytes
                        initialBytes = workerRequest.ReadEntityBody(buffer, buffer.Length);
                        // Update the received bytes
                        receivedBytes += initialBytes;
                    }
                    initialBytes = workerRequest.ReadEntityBody(buffer, requestLength - receivedBytes);
                }
            }
            // Redirect the user to the same page with querystring action=exception. 
            context.Response.Redirect("~/error/UploadTooLarge.aspx");
        }
    }     
</script>
