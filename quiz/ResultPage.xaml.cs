using System;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace quiz
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ResultPage : ContentPage
    {
        ISoundEffect soundEffect = DependencyService.Get<ISoundEffect>();

        int _mode = 0;
        int _page = 0;

        public ResultPage(int num, int mode, int page)
        {
            _mode = mode;
            _page = page;

            InitializeComponent();

            dispResult(num);
        }

        // コントロールサイズ調整
        protected override void OnSizeAllocated(double width, double height)
        {
            base.OnSizeAllocated(width, height);

            grdHead.WidthRequest = width - grdHead.Margin.Left - grdHead.Margin.Right;
            grdHead.HeightRequest = height * 0.1 - grdHead.Margin.Top - grdHead.Margin.Bottom;

            grdMain.WidthRequest = width - grdMain.Margin.Left - grdMain.Margin.Right;
            grdMain.HeightRequest = height * 0.9 - grdMain.Margin.Top - grdMain.Margin.Bottom;

        }

        // 結果画面表示
        private async void dispResult(int num)
        {
            await Task.Delay(800);

            int cnt = 0;

            // 表示
            while (cnt < num)
            {
                Image img = NameScopeExtensions.FindByName<Image>(this, string.Concat("imgScore", (cnt + 1).ToString()));
                img.Source = "hosi.png";
                using (soundEffect as IDisposable)
                {
                    soundEffect.ResultSound();
                }
                await img.RotateTo(360, 250);
                cnt = cnt + 1;
            }

            await Task.Delay(800);

            switch (num)
            {
                case 0:
                case 1:
                    imgResult.Source = "res1.png";
                    break;
                case 2:
                    imgResult.Source = "res2.png";
                    break;
                case 3:
                    imgResult.Source = "res3.png";
                    break;
                case 4:
                    imgResult.Source = "res4.png";
                    break;
                case 5:
                    imgResult.Source = "res5.png";
                    break;
                default:
                    imgResult.Source = "res5.png";
                    break;
            }

            imgResult.IsVisible = true;
            btnRetry.IsVisible = true;
        }

        private void btnRetry_Clicked(object sender, EventArgs e)
        {
            using (soundEffect as IDisposable)
            {
                soundEffect.HitSound();
            }

            switch (_page)
            {
                case 0:
                    Navigation.PushAsync(new QuestionPage(_mode));
                    break;
                case 1:
                    Navigation.PushAsync(new ImageQuestionPage(_mode));
                    break;
                case 2:
                    Navigation.PushAsync(new ClozeQuestionPage(_mode));
                    break;
                default:
                    Navigation.PushAsync(new QuestionPage(_mode));
                    break;

            }
        }

        private void btnSelect_Clicked(object sender, EventArgs e)
        {
            using (soundEffect as IDisposable)
            {
                soundEffect.HitSound();
            }

            Navigation.PushAsync(new SubPage(_mode));
        }

        private void btnBack_Clicked(object sender, EventArgs e)
        {
            using (soundEffect as IDisposable)
            {
                soundEffect.HitSound();
            }

            Navigation.PushAsync(new MainPage());
        }

        protected override bool OnBackButtonPressed()
        {
            return true;
        }
    }
}