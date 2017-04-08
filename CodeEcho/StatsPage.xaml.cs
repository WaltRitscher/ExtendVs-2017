using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace CodeEcho
{
    /// <summary>
    /// Interaction logic for StatsPage.xaml
    /// </summary>
    public partial class StatsPage : UserControl
    {
        public StatsPage()
        {
            InitializeComponent();
        }
        private int _totalLines;

        public int TotalLines {
            get { return _totalLines; }
            set { _totalLines = value; Refresh(); }
        }

        private int _whitespaceLines;

        public int WhiteSpaceLines {
            get { return _whitespaceLines; }
            set { _whitespaceLines = value; Refresh(); }
        }

        private int _commentLines;

        public int CommentLines {
            get { return _commentLines; }
            set { _commentLines = value; Refresh(); }
        }
        private int _varLines;

        public int VarLines {
            get { return _varLines; }
            set { _varLines = value; Refresh(); }
        }

        private int _wordLines;

        public int WordLines {
            get { return _wordLines; }
            set { _wordLines = value; Refresh(); }
        }

        private string _language;

        public string ProgrammingLanguage {
            get { return _language; }
            set
            {
                _language = value; Refresh();


            }
        }



        public void Refresh()
        {
            this.WhitespaceTextBlock.Text = this.WhiteSpaceLines.ToString();
            this.TotalLinesTextBlock.Text = this.TotalLines.ToString();
            this.CommentTextBlock.Text = this.CommentLines.ToString();
            this.VarTextBlock.Text = this.VarLines.ToString();
            this.WordTextBlock.Text = this.WordLines.ToString();
            this.LanguageTextBlock.Text = this.ProgrammingLanguage;
            if (_language.Equals("Indeterminate", StringComparison.InvariantCultureIgnoreCase))
            {
                HideSection(true);
            }
            else
            {
                HideSection(false);
            }


        }

        private void HideSection(bool collapseSection)
        {
            if (collapseSection)
            {
                this.WhitespaceTextBlock.Visibility = Visibility.Collapsed;
                this.TotalLinesTextBlock.Visibility = Visibility.Collapsed;
                this.VarTextBlock.Visibility = Visibility.Collapsed;
                this.CommentTextBlock.Visibility = Visibility.Collapsed;
                this.BlankLabel.Visibility = Visibility.Collapsed;
                this.VarLabel.Visibility = Visibility.Collapsed;
                this.CommentsLabel.Visibility = Visibility.Collapsed;
                this.TotalLabel.Visibility = Visibility.Collapsed;
            }
            else
            {
                this.WhitespaceTextBlock.Visibility = Visibility.Visible;
                this.TotalLinesTextBlock.Visibility = Visibility.Visible;
                this.VarTextBlock.Visibility = Visibility.Visible;
                this.CommentTextBlock.Visibility = Visibility.Visible;
                this.BlankLabel.Visibility = Visibility.Visible;
                this.VarLabel.Visibility = Visibility.Visible;
                this.CommentsLabel.Visibility = Visibility.Visible;
                this.TotalLabel.Visibility = Visibility.Visible;
            }
        }
    }
}
