using System;
using Xamarin.Forms;

namespace quiz
{
    public partial class SubPage : ContentPage
    {
        int _mode = 0;

        ISoundEffect soundEffect = DependencyService.Get<ISoundEffect>();

        public SubPage(int mode)
        {
            _mode = mode;
            InitializeComponent();
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

        private void btnString_Clicked(object sender, EventArgs e)
        {
            using (soundEffect as IDisposable)
            {
                soundEffect.HitSound();
            }

            Navigation.PushAsync(new QuestionPage(_mode));
        }

        private void btnImage_Clicked(object sender, EventArgs e)
        {
            using (soundEffect as IDisposable)
            {
                soundEffect.HitSound();
            }

            Navigation.PushAsync(new ImageQuestionPage(_mode));
        }

        private void btnCloze_Clicked(object sender, EventArgs e)
        {
            using (soundEffect as IDisposable)
            {
                soundEffect.HitSound();
            }

            Navigation.PushAsync(new ClozeQuestionPage(_mode));
        }

        private void btnTwoChoice_Clicked(object sender, EventArgs e)
        {
            using (soundEffect as IDisposable)
            {
                soundEffect.HitSound();
            }

            Navigation.PushAsync(new TwoChoiceQuestionPage(_mode));
        }

        protected override bool OnBackButtonPressed()
        {
            return true;
        }

        private void ImageButton_Clicked(object sender, EventArgs e)
        {
            using (soundEffect as IDisposable)
            {
                soundEffect.HitSound();
            }

            Navigation.PushAsync(new MainPage());
        }
    }
}
