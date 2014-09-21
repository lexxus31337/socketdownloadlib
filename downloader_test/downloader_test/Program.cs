using System;
using System.Text;
using System.Text.RegularExpressions;
using System.Net;
using System.Net.Sockets;
using System.Collections.Generic;
using System.Collections;
using System.IO;
using System.Diagnostics;

namespace downloader_test
{
	class MainClass
	{
		public static void Main (string[] args)
		{
            string referer = "http://www.digikey.com/product-detail/en/207333-1/A25080-ND/307477";
            //string link = " /commerce/DocumentDelivery/DDEController?Action=srchrtrv&DocNm=207333&DocType=Customer+Drawing&DocLang=English";
            //string host = "www.te.com";

            string link = "/specifications/ACC02.pdf";
            string host = "www.mallory-sonalert.com";
            
            IPAddress ipAddress = Dns.Resolve(host).AddressList[0];
            IPEndPoint ipe = new IPEndPoint(ipAddress, 80);

            string a = SocketSendReceive ("GET", ipe, referer, link, host);
            
            
		}
		private static string SocketSendReceive(string type,IPEndPoint ipe, string referer, string link, string host) 
		{
            

			StringBuilder request = new StringBuilder ();
            request.Append(type); //HEAD _ GET
            request.Append (" " + link);
			request.Append(" HTTP/1.1\r\n");
			request.Append ("Host: "+ host +"\r\n");
			request.Append ("User-Agent: Mozilla/5.0 (Windows NT 6.1; WOW64; rv:32.0) Gecko/20100101 Firefox/32.0\r\n");
			request.Append ("Accept-Language: text/html,application/xhtml+xml,application/xml;q=0.9,*/*;q=0.8\r\n");
			request.Append ("Accept-Encoding: ru-RU,ru;q=0.8,en-US;q=0.5,en;q=0.3\r\n");
            //http://www.digikey.com/product-detail/en/207333-1/A25080-ND/307477\r\n
            request.Append ("Referer: ");
            request.Append(referer + "\r\n");
			request.Append ("Connection: keep-alive\r\n\r\n");

			Byte[] bytesSent = Encoding.ASCII.GetBytes(request.ToString());
			Byte[] bytesReceived = new Byte[256];

			// Create a socket connection with the specified server and port.
            string page = "Default HTML page on ";

            using (Socket s = new Socket(ipe.AddressFamily, SocketType.Stream, ProtocolType.Tcp))
            {
                s.Connect(ipe);
                if (s == null)
                    return ("Connection failed");

                // Send request to the server.
                s.Send(bytesSent, bytesSent.Length, 0);

                // Receive the server home page content.
                int bytes = 0;
                page = page + ipe.Address.ToString() + ":\r\n";

                // The following will block until te page is transmitted.
                FileStream outf = null;
                int fc = 0;
                do
                {
                    
                    bytes = s.Receive(bytesReceived, bytesReceived.Length, 0);
                    page = page + Encoding.ASCII.GetString(bytesReceived, 0, bytes);
                    if (page.Contains("filename=") & fc == 0)
                    {
                        Regex fname = new Regex(@"filename=(.*\.(.*\w))");
                        Match filename = fname.Match(page);
                        outf = File.Create("../" + filename.Groups[1].Value);
                        fc = 1;
                    }
                    if(fc == 1)
                    {
                        outf.Write(bytesReceived, 0, bytes);
                    }
                }
                while (bytes > 0);
                outf.Close();
            }

			return page;
		}

	}
}
