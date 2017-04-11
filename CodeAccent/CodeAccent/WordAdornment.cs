//------------------------------------------------------------------------------
// <copyright file="WordAdornment.cs" company="Company">
//     Copyright (c) Company.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

using Microsoft.VisualStudio.Text;
using Microsoft.VisualStudio.Text.Editor;
using Microsoft.VisualStudio.Text.Formatting;
using System;
using System.Windows.Controls;
using System.Windows.Media;

namespace CodeAccent
{
    /// <summary>
    /// WordAdornment places colored boxes behind certain characters in the editor window
    /// </summary>
    internal sealed class WordAdornment
    {
        /// <summary>
        /// The layer of the adornment.
        /// </summary>
        private readonly IAdornmentLayer layer;

        /// <summary>
        /// Text view where the adornment is created.
        /// </summary>
        private readonly IWpfTextView view;

        /// <summary>
        /// Adornment brush.
        /// </summary>
        private readonly Brush brush;

        /// <summary>
        /// Adornment pen.
        /// </summary>
        private readonly Pen pen;

        /// <summary>
        /// Initializes a new instance of the <see cref="WordAdornment"/> class.
        /// </summary>
        /// <param name="view">Text view to create the adornment for</param>
        public WordAdornment(IWpfTextView view)
        {
            if (view == null)
            {
                throw new ArgumentNullException("view");
            }

            this.layer = view.GetAdornmentLayer("WordAdornment");

            this.view = view;

            // LayoutChanged event is raised whenever the rendered text 
            // displayed in the changes.
            this.view.LayoutChanged += this.OnLayoutChanged;
        }

        /// <summary>
        /// Handles whenever the text displayed in the view changes by adding the adornment to any reformatted lines
        /// </summary>
        /// <remarks><para>This event is raised whenever the rendered text displayed in the <see cref="ITextView"/> changes.</para>
        /// <para>It is raised whenever the view does a layout (which happens when DisplayTextLineContainingBufferPosition is called or in response to text or classification changes).</para>
        /// <para>It is also raised whenever the view scrolls horizontally or when its size changes.</para>
        /// </remarks>
        /// <param name="sender">The event sender.</param>
        /// <param name="e">The event arguments.</param>
        internal void OnLayoutChanged(object sender, TextViewLayoutChangedEventArgs e)
        {
            foreach (ITextViewLine line in e.NewOrReformattedLines)
            {
                this.CreateVisuals(line);
            }
        }

        /// <summary>
        /// Adds the highlight box behind the certain characters within the given line
        /// </summary>
        /// <param name="line">Line to add the adornments</param>
        private void CreateVisuals(ITextViewLine line)
        {
            IWpfTextViewLineCollection textViewLines = this.view.TextViewLines;

            // Loop through each character, and place a box around a character
            for (int charIndex = line.Start; charIndex < line.End; charIndex++)
            {
                if (this.view.TextSnapshot[charIndex] == '<' ||
                    this.view.TextSnapshot[charIndex] == '>')
                {  // Create the pen and brush to color the box behind the char
                    var brush = new SolidColorBrush(Colors.DarkOrange);
                    var penBrush = new SolidColorBrush(Colors.Yellow);
                    var pen = new Pen(penBrush, 0.5);

                    DrawBox(textViewLines, charIndex, brush, pen);
                }
                if (this.view.TextSnapshot[charIndex] == '"')
                {
                    var brush = new SolidColorBrush(Colors.DarkGreen);
                    var penBrush = new SolidColorBrush(Colors.Green);
                    var pen = new Pen(penBrush, 0.5);

                    DrawBox(textViewLines, charIndex, brush, pen);
                }
            }
        }

        private void DrawBox(IWpfTextViewLineCollection textViewLines, int charIndex,
                                Brush brush, Pen pen)
        {
            var span = new SnapshotSpan(this.view.TextSnapshot, Span.FromBounds(charIndex, charIndex + 1));
            Geometry geometry = textViewLines.GetMarkerGeometry(span);
            if (geometry != null)
            {
                var geoDrawing = new GeometryDrawing(brush: brush,
                                                  pen: pen,
                                                  geometry: geometry);
                geoDrawing.Freeze();

                var drawingImage = new DrawingImage(drawing: geoDrawing);
                drawingImage.Freeze();

                var adornmentImage = new Image
                {
                    Source = drawingImage,
                };

                // Align the image with the top of the bounds of the text geometry
                Canvas.SetLeft(element: adornmentImage,
                               length: geometry.Bounds.Left);   
                Canvas.SetTop(element: adornmentImage,
                              length: geometry.Bounds.Top);

                this.layer.AddAdornment(behavior: AdornmentPositioningBehavior.TextRelative,
                                        visualSpan: span,
                                        tag: null,
                                        adornment: adornmentImage,
                                        removedCallback: null);

            }
        }
    }
}