using System;
using System.Collections.Generic;
using System.Net.Mail;

//namespace ReportesAuditoriasExternas.Common {

    public class Mail {
        string From = ""; //de quien procede, puede ser un alias
        string To;  //a quien vamos a enviar el mail
        string Message;  //mensaje
        string Subject; //asunto
        List<string> Archivo = new List<string>(); //lista de archivos a enviar
        string DE = "desarrollo.baltazar@gmail.com"; //nuestro usuario de smtp
        string PASS = "Desarrollo123"; //nuestro password de smtp

        MailMessage Email;

        public string error = "";
        
        public Mail(string FROM, string Para, string Mensaje, string Asunto, List<string> ArchivoPedido_ = null) {
            From = FROM;
            To = Para;
            Message = Mensaje;
            Subject = Asunto;
            Archivo = ArchivoPedido_;

        }

        public bool enviaMail() {
            //una validación básica
            if (To.Trim().Equals("") || Message.Trim().Equals("") || Subject.Trim().Equals("")) {
                error = "El mail, el asunto y el mensaje son obligatorios";
                return false;
            }

            try {
                //creamos un objeto tipo MailMessage
                //este objeto recibe el sujeto o persona que envia el mail,
                //la direccion de procedencia, el asunto y el mensaje
                Email = new MailMessage(From, To, Subject, Message);

                //si viene archivo a adjuntar
                //realizamos un recorrido por todos los adjuntos enviados en la lista
                //la lista se llena con direcciones fisicas, por ejemplo: c:/pato.txt
                if (Archivo != null) {
                    //agregado de archivo
                    foreach (string archivo in Archivo) {
                        //comprobamos si existe el archivo y lo agregamos a los adjuntos
                        if (System.IO.File.Exists(@archivo))
                            Email.Attachments.Add(new Attachment(@archivo));
                    }
                }

                Email.IsBodyHtml = true; //definimos si el contenido sera html
                Email.From = new MailAddress(From); //definimos la direccion de procedencia

                //aqui creamos un objeto tipo SmtpClient el cual recibe el servidor que utilizaremos como smtp
                //en este caso me colgare de gmail
                SmtpClient smtpMail = new SmtpClient("smtp.gmail.com");

                smtpMail.EnableSsl = false;//le definimos si es conexión ssl
                smtpMail.UseDefaultCredentials = false; //le decimos que no utilice la credencial por defecto
                smtpMail.Host = "stmp.gmail.com"; //agregamos el servidor smtp
                smtpMail.Port = 465; //le asignamos el puerto, en este caso gmail utiliza el 465
                smtpMail.Credentials = new System.Net.NetworkCredential(DE, PASS); //agregamos nuestro usuario y pass de gmail

                //enviamos el mail
                smtpMail.Send(Email);

                //eliminamos el objeto
                smtpMail.Dispose();

                //regresamos true
                return true;
            } catch (Exception ex) {
                //si ocurre un error regresamos false y el error
                error = "Ocurrio un error: " + ex.Message;
                return false;
            }

            //return false;
        }
    }
//}