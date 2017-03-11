using EnvDTE;
using Microsoft.VisualStudio.Editor;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Text;
using Microsoft.VisualStudio.Text.Editor;
using Microsoft.VisualStudio.TextManager.Interop;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeCommander {
	internal class TextHelper {

		internal IWpfTextViewHost GetCurrentViewHost(System.IServiceProvider sp) {
			var textManager = sp.GetService(serviceType:
																									typeof(SVsTextManager)) as IVsTextManager;
			IVsTextView textView = null;
			int needsFocus = 1;
			textManager.GetActiveView(fMustHaveFocus: needsFocus,
																pBuffer: null,
																ppView: out textView);

			var userData = textView as IVsUserData;
			if (userData == null)
			{
				return null;
			}
			else
			{
				Guid guidForViewHost = DefGuidList.guidIWpfTextViewHost;
				object holder;
				userData.GetData(ref guidForViewHost, out holder);
				var viewHost = (IWpfTextViewHost)holder;
				return viewHost;
			}


		}

		internal string GetSelectedTextFromEditView(IWpfTextViewHost viewHost) {
			var results = viewHost.TextView.Selection.SelectedSpans[0].GetText();
			return results;
		}

		internal string GetAllTextFromEditView(IWpfTextViewHost viewHost) {
			var results = viewHost.TextView.TextSnapshot.GetText();
			return results;
		}




	}
}
