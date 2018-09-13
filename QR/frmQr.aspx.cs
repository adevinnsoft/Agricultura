using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using MessagingToolkit.QRCode.Codec;
using MessagingToolkit.QRCode.Codec.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;

public partial class QR_frmQr : BasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        

    }

    public void generaQr()
    {
        QRCodeEncoder encoder = new QRCodeEncoder();
        int inicio=0;
        int fin=0;
        System.Drawing.Image logo;
        try
        {
            inicio = int.Parse(txtInicio.Text);
            fin = int.Parse(txtFin.Text);

            //inicio = inicio + 100000;
            //fin = fin + 100000;

            for (var start = inicio; start < fin; start++)
            {
                encoder.QRCodeScale = 15;
                encoder.QRCodeErrorCorrect = QRCodeEncoder.ERROR_CORRECTION.H;
                Bitmap img = encoder.Encode(txt_Prefix.Text + start.ToString(),System.Text.Encoding.UTF8);
                

                //logo = DrawText(txt_Prefix.Text + start.ToString());

                //int left = img.Width / 2 - logo.Width / 2;
                //int top = img.Height /2  - logo.Height /2;

                //Graphics g = Graphics.FromImage(img);

                //g.DrawImage(logo, new Point(left, top));



                img.Save("d:\\Projects\\QR\\"+txt_Prefix.Text+start.ToString()+".jpg", ImageFormat.Jpeg);
            }
        }
        catch (Exception ex)
        {
            //Log.Error(ex);
        }
    }
    protected void GeneraQr_Click(object sender, EventArgs e)
    {
        generaQr();
    }

    

    private System.Drawing.Image DrawText(String text)
    {
        Font font = new Font("Arial", 20.0F);
        //first, create a dummy bitmap just to get a graphics object
        System.Drawing.Image img = new Bitmap(1, 1);
        Graphics drawing = Graphics.FromImage(img);

        //measure the string to see how big the image needs to be
        SizeF textSize = drawing.MeasureString(text,font );

        //free up the dummy image and old graphics object
        img.Dispose();
        drawing.Dispose();

        //create a new image of the right size
        img = new Bitmap((int)textSize.Width, (int)textSize.Height);

        drawing = Graphics.FromImage(img);

        //paint the background
        drawing.Clear(Color.White);

        //create a brush for the text
        Brush textBrush = new SolidBrush(Color.Black);

        drawing.DrawString(text, font, textBrush, 0, 0);

        drawing.Save();

        textBrush.Dispose();
        drawing.Dispose();

        return img;

    }
}