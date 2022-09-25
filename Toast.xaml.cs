using System;
using System.ComponentModel;
using System.IO;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Interop;
using System.Windows.Media.Animation;
using System.Windows.Threading;

namespace ToastWPF
{
    public static class TranparentWindowsServices
    {
        const int WS_EX_TRANSPARENT = 0x00000020;
        const int GWL_EXSTYLE = (-20);

        [DllImport("user32.dll")]
        static extern int GetWindowLong(IntPtr hwnd, int index);

        [DllImport("user32.dll")]
        static extern int SetWindowLong(IntPtr hwnd, int index, int newStyle);

        public static void SetWindowExTransparent(IntPtr hwnd)
        {
            var extendedStyle = GetWindowLong(hwnd, GWL_EXSTYLE);
            SetWindowLong(hwnd, GWL_EXSTYLE, extendedStyle | WS_EX_TRANSPARENT);
        }
    }
    public partial class Toast : Window, INotifyPropertyChanged, IDisposable
    {
        private static Toast Instance { get; }
        private DispatcherTimer Timer { get; }
        private Storyboard FadeInStoryBoard { get; }
        private Storyboard FadeOutStoryBoard { get; }

        #region Notifier
        public event PropertyChangedEventHandler? PropertyChanged;
        private void NotifyPropertyChanged([CallerMemberName] String propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion

        private double HorizontalPos { get; set; }
        private double VerticalPos { get; set; }

        private float? DurationMs { get; set; }

        private string _message;
        public string Message { get { return _message; } set { _message = value; NotifyPropertyChanged(); } }

        static Toast()
        {
            if (Instance != null)
                return;

            Instance = new Toast();
        }
        private Toast()
        {
            InitializeComponent();

            Timer = new DispatcherTimer();
            Timer.Tick += Stop;

            FadeInStoryBoard = (Storyboard)FindResource("fadeInWindow");
            FadeOutStoryBoard = (Storyboard)FindResource("fadeOutWindow");
            Opacity = 0;

            _message = string.Empty;
        }
        protected override void OnSourceInitialized(EventArgs e) //Pass all events
        {
            base.OnSourceInitialized(e);
            var hwnd = new WindowInteropHelper(this).Handle;
            TranparentWindowsServices.SetWindowExTransparent(hwnd);
        }

        public static void Show(string message, float? durationMs = null)
        {
            //메시지
            Instance.Message = message;
            Instance.DurationMs = durationMs;

            //Reset
            Instance.Reset();

            //타이머 시작. 사용자 입력 시간이 없으면 계산.
            Instance.Start(Instance.DurationMs ?? CalcShowingTimeMs(message.Length));

            //윈도우 위치 업데이트
            Instance.UpdatePosition();

            //UI 쓰레드에서 실행
            Instance.Dispatcher.InvokeAsync(() =>
            {
                Instance.Show();
            });
        }
        public static void SetPosition(Window owner, double horizontalPos = 0.5, double verticalPos = 0.5)
        {
            Instance.Owner = owner;
            Instance.HorizontalPos = horizontalPos;
            Instance.VerticalPos = verticalPos;
        }

        private static float CalcShowingTimeMs(int textLength)
        {
            return 1000f + (textLength * 100f);
        }

        #region Timer and Animations
        private void Reset()
        {
            FadeInStoryBoard.Stop();
            FadeOutStoryBoard.Stop();

            Timer.Stop();
        }
        private void Start(float duration)
        {
            Timer.Interval = TimeSpan.FromMilliseconds(duration);
            Timer.Start();

            FadeInStoryBoard.Begin();
        }
        private void Stop(object? sender, EventArgs e)
        {
            FadeOutStoryBoard.Begin();

            Timer.Stop();
        }
        #endregion

        private void UpdatePosition()
        {
            if (Owner == null)
                return;

            UpdateLayout(); //현재 사이즈를 받기 위해 레이아웃 업데이트 필요

            double horizontalCenter = Owner.Left + (Owner.Width * HorizontalPos);
            double verticalCenter = Owner.Top + (Owner.Height * VerticalPos);

            double margin = 10;
            double limitLeft = Owner.Left + margin;
            double limitTop = Owner.Top + margin;
            double limitRight = Owner.Left + Owner.Width - ActualWidth - margin;
            double limitBottom = Owner.Top + Owner.Height - ActualHeight - margin;

            //Owner 범위를 벗어나지 않도록 함.
            Left = Math.Min(limitRight, Math.Max(limitLeft, horizontalCenter - (ActualWidth / 2d)));
            Top = Math.Min(limitBottom, Math.Max(limitTop, verticalCenter - (ActualHeight / 2d)));
        }

        public void Dispose()
        {
            FadeInStoryBoard.Stop();
            FadeOutStoryBoard.Stop();
            Timer.Stop();
            Close();
        }
    }
}
