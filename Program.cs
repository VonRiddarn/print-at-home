using System.Text;
using ESCPOS_NET;
using ESCPOS_NET.Emitters;
using ESCPOS_NET.Utilities;

namespace print_at_home;

class Program
{
	static void Main(string[] _)
	{
		// Creates a byte encoder that forces characters to be sent using 1 byte
		// This is necessary when using åäö because of legacy printer stuff from what I understand.
		// TL;DR: Old printers can't understand UTF-8.
		Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
		var se = Encoding.GetEncoding(1252);

		const string PRINTER_ADDRESS = "192.168.10.119:9100";


		using var printer = new NetworkPrinter(settings: new() { ConnectionString = PRINTER_ADDRESS });
		var e = new EPSON();

		var receipt = ByteSplicer.Combine(

			// Switch to legacy byte-char encoding... (WPC1252)
			// This could be changed in the webb-interface: http://192.168.10.119
			// By setting "International Character Set" to Sweden, and Default Code Page to either WPC1252 or PC865.
			e.Initialize(),
			[27, 116, 16],

			// The actual printing.
			// Note that when using the swedish encoder, we need to pass newline characters manually.
			// We could write either an extention of the Printer class, or a wrapper to make this prettier.
			e.CenterAlign(),
			e.PrintLine("=========================================="),
			e.PrintLine("                 KVITTO                   "),
			e.PrintLine("=========================================="),
			e.LeftAlign(),
			e.PrintLine(""),
			se.GetBytes("Städning, kök\t\t30.000,90 SEK"),
			e.PrintLine(""),
			e.CenterAlign(),
			e.SetStyles(PrintStyle.FontB),
			se.GetBytes("Moms kan tillkomma.\n"),
			e.SetStyles(PrintStyle.None),
			e.FeedLines(4),
			e.FullCut()
		);

		printer.Write(receipt);
		Console.WriteLine("Print command sent!");

		// Hacky way to just keep the app alive whilst sending data.
		Thread.Sleep(2000);
	}
}
