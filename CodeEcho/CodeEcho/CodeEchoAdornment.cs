//------------------------------------------------------------------------------
// <copyright file="CodeEchoAdornment.cs" company="Company">
//     Copyright (c) Company.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

using Microsoft.VisualStudio.Text.Editor;
using System;
using System.IO;
using System.Windows.Controls;

namespace CodeEcho {

  /// <summary>
  /// Adornment class that draws a square box in the top right hand corner of the viewport
  /// </summary>
  internal class CodeEchoAdornment {
    private IWpfTextView _view;
    private IAdornmentLayer _adornmentLayer;
    private StatsPage _statsPage = new StatsPage();

    /// <summary>
    /// Creates a square image and attaches an event handler to the layout changed event that
    /// adds the the square in the upper right-hand corner of the TextView via the adornment layer
    /// </summary>
    /// <param name="view">The <see cref="IWpfTextView"/> upon which the adornment will be drawn</param>
    public CodeEchoAdornment(IWpfTextView view) {
      _view = view;
      _view.LayoutChanged += this.OnLayoutChanged;

      //Grab a reference to the adornment layer//
      _adornmentLayer = view.GetAdornmentLayer("CodeEchoAdornment");
      _view.LayoutChanged += View_LayoutChanged;
    }

    private void View_LayoutChanged(object sender, TextViewLayoutChangedEventArgs e) {
      //clear the adornment layer of previous adornments
      _adornmentLayer.RemoveAllAdornments();

      //Place the statsPage in the top right hand corner of the Viewport
      Canvas.SetLeft(_statsPage, _view.ViewportRight - (_statsPage.ActualWidth + 20));
      Canvas.SetTop(_statsPage, _view.ViewportTop + 20);

      //add the statsPage to the adornment layer and make it relative to the viewport
      _adornmentLayer.AddAdornment(AdornmentPositioningBehavior.ViewportRelative, null, null, _statsPage, null);
    }

    private void OnLayoutChanged(object sender, TextViewLayoutChangedEventArgs e) {
      CalculateLineCounts(_view);
    }

    public void CalculateLineCounts(IWpfTextView view) {
      //get all the code text
      string codeText = view.TextSnapshot.GetText();

      int totalCounter = 1;
      int whiteSpaceCounter = 0;
      int commentCounter = 0;
      int varCounter = 0;

      _statsPage.ProgrammingLanguage = DetectLanguage(view).ToString();
      if (string.IsNullOrEmpty(_statsPage.ProgrammingLanguage))
      {
        return;
      }
      //create string reader
      using (StringReader reader = new StringReader(codeText))
      {
        string line;
        while ((line = reader.ReadLine()) != null)
        {
          //count line of code
          totalCounter++;

          if (line.Trim() == "")
          {
            whiteSpaceCounter++;
          }
          // assumes that we are working with C# or VB
          if (line.TrimStart().StartsWith("//") || line.TrimStart().StartsWith("'"))
          {
            commentCounter++;
          }

          // assumes that we are working with C# or VB
          if (line.TrimStart().StartsWith("var ", System.StringComparison.OrdinalIgnoreCase) ||
              line.TrimStart().StartsWith("dim ", System.StringComparison.OrdinalIgnoreCase))
          {
            varCounter++;
          }
        }
      }

      //set statsPage UI to count values
      _statsPage.CommentLines = commentCounter;
      _statsPage.TotalLines = totalCounter;
      _statsPage.WhiteSpaceLines = whiteSpaceCounter;
      _statsPage.VarLines = varCounter;
    }

    internal Language DetectLanguage(IWpfTextView view) {
      string langtype = view.FormattedLineSource.TextAndAdornmentSequencer.
SourceBuffer.ContentType.DisplayName;
      if (langtype.Equals("CSHARP", StringComparison.InvariantCultureIgnoreCase))
      {
        return Language.CSharp;
      }
      else if (langtype.Equals("BASIC", StringComparison.InvariantCultureIgnoreCase))
      {
        return Language.VisualBasic;
      }
      else if (langtype.Equals("HTMLXProjection", StringComparison.InvariantCultureIgnoreCase))
      {
        return Language.Html;
      }
      else if (langtype.Equals("Css", StringComparison.InvariantCultureIgnoreCase))
      {
        return Language.Css;
      }
      else
      {
        return Language.Indeterminate;
      };
    }

    public enum Language {
      CSharp, VisualBasic, Html, Xml, Css, Text, Indeterminate
    }
  }
}