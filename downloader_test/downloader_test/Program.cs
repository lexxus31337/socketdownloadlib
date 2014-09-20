using System;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Collections.Generic;
using System.Collections;

namespace downloader_test
{
	class MainClass
	{
		public static void Main (string[] args)
		{
			IPAddress ipAddress = Dns.Resolve("www.te.com").AddressList[0];
			IPEndPoint ipe = new IPEndPoint (ipAddress, 80);
			string a = SocketSendReceive (ipe);

//			using (Socket socket = new Socket (ipe.AddressFamily, SocketType.Stream, ProtocolType.Tcp)) 
//			{
//				try
//				{
//					socket.Connect(ipe);
//					if (!socket.Connected)
//						Console.WriteLine("Не удалось подключится");
//				}
//				catch (SocketException ex)
//				{
//					Console.WriteLine(ex.Message);
//				}
//				StringBuilder query = new StringBuilder ();
//				query.Append (@"GET /commerce/DocumentDelivery/DDEController?Action=srchrtrv&DocNm=207333&DocType=Customer+Drawing&DocLang=English HTTP/1.1 \r\n");
//
//				Byte[] bytesSent = Encoding.ASCII.GetBytes(query.ToString());
//				Byte[] bytesReceived = new Byte[1024];
//				socket.Send(bytesSent, bytesSent.Length, 0);
//				string page = "";
//				int bytes = 0;
//				do
//				{
//					bytes = socket.Receive(bytesReceived, bytesReceived.Length, 0);
//					page = page + Encoding.ASCII.GetString(bytesReceived);
//				}
//				while (bytes > 0);
//				socket.Disconnect(false);

			
//			}


		}
		private static string SocketSendReceive(IPEndPoint ipe) 
		{
//			string request = "GET / HTTP/1.1\r\nHost: " + server + 
//				"\r\nConnection: Close\r\n\r\n";
			StringBuilder request = new StringBuilder ();
			request.Append ("HEAD /commerce/DocumentDelivery/DDEController?Action=srchrtrv");
			request.Append("&DocNm=207333&DocType=Customer+Drawing&DocLang=English HTTP/1.1\r\n");
			request.Append ("Host: www.te.com\r\n");
			request.Append ("User-Agent: Mozilla/5.0 (Windows NT 6.1; WOW64; rv:32.0) Gecko/20100101 Firefox/32.0\r\n");
			request.Append ("Accept-Language: text/html,application/xhtml+xml,application/xml;q=0.9,*/*;q=0.8\r\n");
			request.Append ("Accept-Encoding: ru-RU,ru;q=0.8,en-US;q=0.5,en;q=0.3\r\n");
			request.Append ("Referer: http://www.digikey.com/product-detail/en/207333-1/A25080-ND/307477\r\n");
			request.Append ("Connection: keep-alive\r\n\r\n");


			Byte[] bytesSent = Encoding.ASCII.GetBytes(request.ToString());
			Byte[] bytesReceived = new Byte[256];

			// Create a socket connection with the specified server and port.
			Socket s = new Socket (ipe.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
			s.Connect(ipe);
			if (s == null)
				return ("Connection failed");

			// Send request to the server.
			s.Send(bytesSent, bytesSent.Length, 0);  

			// Receive the server home page content.
			int bytes = 0;
			string page = "Default HTML page on " + ipe.Address.ToString() + ":\r\n";

			// The following will block until te page is transmitted.
			do {
				bytes = s.Receive(bytesReceived, bytesReceived.Length, 0);
				page = page + Encoding.ASCII.GetString(bytesReceived, 0, bytes);
			}
			while (bytes > 0);

			return page;
		}

	}
}
