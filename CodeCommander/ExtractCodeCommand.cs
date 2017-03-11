//------------------------------------------------------------------------------
// <copyright file="ExtractCodeCommand.cs" company="Company">
//     Copyright (c) Company.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

using System;
using System.ComponentModel.Design;
using System.Globalization;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;
using Microsoft.VisualStudio.Text.Editor;
using Microsoft.VisualStudio.TextManager.Interop;
using Microsoft.VisualStudio.Editor;
using System.Windows.Forms;

namespace CodeCommander {
	/// <summary>
	/// Command handler
	/// </summary>
	internal sealed class ExtractCodeCommand {
		/// <summary>
		/// Command ID.
		/// </summary>
		public const int CommandId = 4129;

		/// <summary>
		/// Command menu group (command set GUID).
		/// </summary>
		public static readonly Guid CommandSet = new Guid("acb59b3a-a4cc-4ea8-9d3c-709467934ea2");

		/// <summary>
		/// VS Package that provides this command, not null.
		/// </summary>
		private readonly Package package;

		/// <summary>
		/// Initializes a new instance of the <see cref="ExtractCodeCommand"/> class.
		/// Adds our command handlers for menu (commands must exist in the command table file)
		/// </summary>
		/// <param name="package">Owner package, not null.</param>
		private ExtractCodeCommand(Package package) {
			if (package == null)
			{
				throw new ArgumentNullException("package");
			}

			this.package = package;

			OleMenuCommandService commandService = this.ServiceProvider.GetService(typeof(IMenuCommandService)) as OleMenuCommandService;
			if (commandService != null)
			{
				var menuCommandID = new CommandID(CommandSet, CommandId);
				var menuItem = new MenuCommand(this.MenuItemCallback, menuCommandID);
				commandService.AddCommand(menuItem);
			}
		}

		/// <summary>
		/// Gets the instance of the command.
		/// </summary>
		public static ExtractCodeCommand Instance
		{
			get;
			private set;
		}

		/// <summary>
		/// Gets the service provider from the owner package.
		/// </summary>
		private IServiceProvider ServiceProvider
		{
			get
			{
				return this.package;
			}
		}

		/// <summary>
		/// Initializes the singleton instance of the command.
		/// </summary>
		/// <param name="package">Owner package, not null.</param>
		public static void Initialize(Package package) {
			Instance = new ExtractCodeCommand(package);
		}



		/// <summary>
		/// This function is the callback used to execute the command when the menu item is clicked.
		/// See the constructor to see how the menu item is associated with this function using
		/// OleMenuCommandService service and MenuCommand class.
		/// </summary>
		/// <param name="sender">Event sender.</param>
		/// <param name="e">Event args.</param>
		private void MenuItemCallback(object sender, EventArgs e) {

			var textHelper = new TextHelper();
			var viewHost = textHelper.GetCurrentViewHost(this.ServiceProvider);

			var selectedText = textHelper.GetSelectedTextFromEditView(viewHost);



			string title = "Extract Code";

			// use the WPF message box instead of the VS message box;
			System.Windows.MessageBox.Show(selectedText, title, System.Windows.MessageBoxButton.OK);

			//	// Show a message box to prove we were here
			//	VsShellUtilities.ShowMessageBox(
			//			this.ServiceProvider,
			//			selectedCode,
			//			title,
			//			OLEMSGICON.OLEMSGICON_INFO,
			//			OLEMSGBUTTON.OLEMSGBUTTON_OK,
			//			OLEMSGDEFBUTTON.OLEMSGDEFBUTTON_FIRST);
			//
		}
	}
}
