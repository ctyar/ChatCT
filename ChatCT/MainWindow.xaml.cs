using System;
using System.Collections.Concurrent;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using ChatCT.Core;

namespace ChatCT.Wpf
{
    /// <summary>
    ///     Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private MessageManager _messageManager;
        private readonly EmoteManager _emoteManager;
        private CancellationTokenSource _cancellationTokenSource;
        private ScrollViewer _chatBoxScrollViewer;

        public MainWindow()
        {
            _emoteManager = new EmoteManager();

            InitializeComponent();

            Connect.Click += ConnectOnClick;
            Disconnect.Click += DisconnectOnClick;
        }

        private void DisconnectOnClick(object sender, RoutedEventArgs e)
        {
            Disconnect.IsEnabled = false;
            Connect.IsEnabled = true;

            _messageManager?.Disconnect();
        }

        private async void ConnectOnClick(object sender, RoutedEventArgs e)
        {
            Connect.IsEnabled = false;
            Disconnect.IsEnabled = true;

            _cancellationTokenSource = _cancellationTokenSource ?? new CancellationTokenSource();
            var blockingCollection = new BlockingCollection<string>();
            _messageManager = new MessageManager(blockingCollection, _cancellationTokenSource.Token);

            var username = Username.Text;
            var accessToken = AccessToken.Text;
            var channel = Channel.Text;

            var producerTask = Task.Run(() => _messageManager.Connect(username, accessToken, channel));

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
            var stackPanel = new StackPanel { Orientation = Orientation.Horizontal };

            var messageResult = _emoteManager.ParseEmotes(message);
            foreach (var messageItem in messageResult.Items)
            {
                if (messageItem.Type == ItemType.Emote)
                {
                    stackPanel.Children.Add(GetEmote(messageItem.Content));
                }
                else
                {
                    stackPanel.Children.Add(GetStringMessage(messageItem.Content));
                }
            }

            ChatBox.Items.Add(stackPanel);
            if (_chatBoxScrollViewer == null)
            {
                var border = (Border)VisualTreeHelper.GetChild(ChatBox, 0);
                _chatBoxScrollViewer = (ScrollViewer)VisualTreeHelper.GetChild(border, 0);
            }

            _chatBoxScrollViewer.ScrollToBottom();
        }

        private UIElement GetStringMessage(string message) => new TextBlock { Text = message };

        private UIElement GetEmote(string emotePath)
        {
            var image = new Image
            {
                Source = new BitmapImage(new Uri(emotePath, UriKind.Relative)),
                Height = 20
            };

            return image;
        }
    }
}