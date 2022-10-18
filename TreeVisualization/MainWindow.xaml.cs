namespace Graphviz4Net.WPF.TreeVisualization
{
    using System.Windows;
    using System.Windows.Input;

    public partial class MainWindow : Window
    {
        private MainWindowViewModel viewModel;

        public MainWindow()
        {
            this.viewModel = new MainWindowViewModel(this.Dispatcher);
            this.DataContext = viewModel;
            InitializeComponent();
        }

        private void Window_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (e.Key == Key.Space)
            {
                viewModel.DoSingleStep();
            }
        }
    }
}
