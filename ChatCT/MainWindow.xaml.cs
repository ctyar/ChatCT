using System;
using System.Collections.Concurrent;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Media.Imaging;

namespace ChatCT.Wpf
{
    /// <summary>
    ///     Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private CancellationTokenSource _cancellationTokenSource;

        public MainWindow()
        {
            InitializeComponent();

            Start.Click += StartOnClick;
            Stop.Click += StopOnClick;
        }

        private void StopOnClick(object sender, RoutedEventArgs e)
        {
            _cancellationTokenSource?.Cancel();
        }

        private async void StartOnClick(object sender, RoutedEventArgs e)
        {
            _cancellationTokenSource = _cancellationTokenSource ?? new CancellationTokenSource();
            var blockingCollection = new BlockingCollection<string>();
            var producer = new Producer(blockingCollection, _cancellationTokenSource.Token);

            var producerTask = Task.Run(() => producer.GetMessagesAsync());

            while (!blockingCollection.IsCompleted)
            {
                try
                {
                    if (blockingCollection.TryTake(out var message, 0, _cancellationTokenSource.Token))
                    {
                        AddMessage(message);
                    }
                }
                catch (OperationCanceledException)
                {
                    break;
                }

                await Task.Delay(10);
            }

            _cancellationTokenSource.Dispose();
            _cancellationTokenSource = null;
        }

        private void AddMessage(string message)
        {
            var stackPanel = new StackPanel();
            stackPanel.Orientation = Orientation.Horizontal;
            stackPanel.Children.Add(GetStringMessage(message));
            stackPanel.Children.Add(GetEmoji());
            stackPanel.Children.Add(GetStringMessage("Some more text."));

            var blockUiContainer = new BlockUIContainer();
            blockUiContainer.Child = stackPanel;

            ChatBox.Blocks.Add(blockUiContainer);
        }

        private UIElement GetStringMessage(string message)
        {
            var label = new Label { Content = message };

            return label;
        }

        private UIElement GetEmoji()
        {
            var image = new Image
            {
                Source = new BitmapImage(new Uri(@"C:\Users\Shahriar\Pictures\1.jpg", UriKind.Absolute)),
                Height = 20
            };

            return image;
        }
    }
}