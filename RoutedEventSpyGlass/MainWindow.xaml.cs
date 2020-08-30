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

namespace RoutedEventSpyGlass
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        FontFamily fontfam = new FontFamily("arial");
        private DateTime before;
        public MainWindow()
        {
            InitializeComponent();

            UIElement[] elements = {grid, btn, text};

            foreach (var element in elements)
            {
                element.PreviewKeyDown += DoEverythingEventHandler;
                element.PreviewKeyUp += DoEverythingEventHandler;
                element.PreviewTextInput += DoEverythingEventHandler;
                element.KeyDown += DoEverythingEventHandler;
                element.KeyUp += DoEverythingEventHandler;
                element.TextInput += DoEverythingEventHandler;

                element.MouseDown += DoEverythingEventHandler;
                element.MouseUp += DoEverythingEventHandler;
                element.PreviewMouseUp += DoEverythingEventHandler;
                element.PreviewMouseDown += DoEverythingEventHandler;
                
                element.AddHandler(Button.ClickEvent, new RoutedEventHandler(DoEverythingEventHandler));
            }
        }

        void DoEverythingEventHandler(object sender, RoutedEventArgs args)
        {        
            System.Windows.Controls.StackPanel sp = new StackPanel();

            DateTime now = DateTime.Now;
            if (now - before > TimeSpan.FromMilliseconds(100))
            {
                System.Windows.Controls.StackPanel sp_blank = new StackPanel();
                sp_blank.Height = 20;
                sp_blank.Background = Brushes.Gray;
                myStackPanel.Children.Add(sp_blank);
            }

            before = now;

            var width = 60;
            sp.Orientation = Orientation.Horizontal;



            TextBlock tb1 = new TextBlock();
            FormatTextBox(args, tb1, width * 2, args.RoutedEvent.Name);
            sp.Children.Add(tb1);

            TextBlock tb2 = new TextBlock();
            FormatTextBox(args, tb2, width, ShrinkTheName(sender));
            sp.Children.Add(tb2);

            TextBlock tb3 = new TextBlock();
            FormatTextBox(args, tb3, width, ShrinkTheName(args.Source));
            sp.Children.Add(tb3);

            TextBlock tb4 = new TextBlock();
            FormatTextBox(args, tb4, width, ShrinkTheName(args.OriginalSource));
            sp.Children.Add(tb4);

            TextBlock tb5 = new TextBlock();
            FormatTextBox(args, tb5, width, args.RoutedEvent.RoutingStrategy.ToString());
            sp.Children.Add(tb5);

            myStackPanel.Children.Add(sp);
            ScrollViewer.ScrollToBottom();
        }

        private void FormatTextBox(RoutedEventArgs args, TextBlock tb, int width, string content)
        {
            tb.FontFamily = FontFamily;
            tb.Width = width;
            tb.Foreground = args.RoutedEvent.RoutingStrategy == RoutingStrategy.Bubble ? Brushes.Green : Brushes.Red;
            tb.Margin = new Thickness(40, 5, 40, 5);
            tb.Text = $"{content}";
        }

        string ShrinkTheName(object obj)
        {
            var str = obj.GetType().ToString().Split('.');
            return str[str.Length - 1];
        }

        private void btnClear_Click(object sender, RoutedEventArgs e)
        {
            myStackPanel.Children.Clear();
        }
    }
}
