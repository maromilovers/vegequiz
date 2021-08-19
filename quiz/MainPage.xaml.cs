using PCLStorage;
using SQLite;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace quiz
{
    public partial class MainPage : ContentPage
    {
        List<string> lstImage = new List<string>();
        ISoundEffect soundEffect = DependencyService.Get<ISoundEffect>();

        public MainPage()
        {
            InitializeComponent();

            setDisplay();
        }

        // コントロールサイズ調整
        protected override void OnSizeAllocated(double width, double height)
        {
            base.OnSizeAllocated(width, height);

            grdMain.WidthRequest = width;
            grdMain.HeightRequest = height;
            
        }

        private void setDisplay()
        {
            IFolder rootFolder = FileSystem.Current.LocalStorage;
            var file = rootFolder.CreateFileAsync("quiz.db", CreationCollisionOption.OpenIfExists).Result;
            using (var connection = new SQLiteConnection(file.Path))
            {
                foreach (var question in connection.Table<Question>())
                {
                    lstImage.Add(question.Image.ToString());
                }
            }

            btnImage1.Source = "img" + lstImage[getRand(lstImage.Count)] + ".png";
            btnImage2.Source = "img" + lstImage[getRand(lstImage.Count)] + ".png";
            btnImage3.Source = "img" + lstImage[getRand(lstImage.Count)] + ".png";
            btnImage4.Source = "img" + lstImage[getRand(lstImage.Count)] + ".png";
            btnImage5.Source = "img" + lstImage[getRand(lstImage.Count)] + ".png";
            btnImage6.Source = "img" + lstImage[getRand(lstImage.Count)] + ".png";

        }

        private int getRand(int max)
        {
            Random rnd = new Random();
            return rnd.Next(0, max);

        }

        private void btnHiragana_Clicked(object sender, EventArgs e)
        {
            using (soundEffect as IDisposable)
            {
                soundEffect.HitSound();
            }

            Navigation.PushAsync(new SubPage(0));
        }

        private void btnKatakana_Clicked(object sender, EventArgs e)
        {
            using (soundEffect as IDisposable)
            {
                soundEffect.HitSound();
            }

            Navigation.PushAsync(new SubPage(1));
        }

        private void btnEnglish_Clicked(object sender, EventArgs e)
        {
            using (soundEffect as IDisposable)
            {
                soundEffect.HitSound();
            }

            Navigation.PushAsync(new SubPage(2));
        }

        private void btnList_Clicked(object sender, EventArgs e)
        {
            using (soundEffect as IDisposable)
            {
                soundEffect.HitSound();
            }

            Navigation.PushAsync(new ListPage());
        }

        protected override bool OnBackButtonPressed()
        {
            return true;
        }

        private void btnImage_Clicked(object sender, EventArgs e)
        {
            ImageButton ib = (ImageButton)sender;
            ib.Source = "img" + lstImage[getRand(lstImage.Count)] + ".png";
        }
    }
}
