using System.Drawing.Printing;

namespace Commons.Utilities
{
	public class Printer : PrintDocument
	{
		public delegate void OnPrintPageDelgate(PrintPageEventArgs e);
		public OnPrintPageDelgate OnPrintPageHandler { get; set; }

		protected override void OnPrintPage(PrintPageEventArgs e)
		{
			base.OnPrintPage(e);
			OnPrintPageHandler(e);
		}
	}
}
