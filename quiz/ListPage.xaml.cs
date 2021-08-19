using PCLStorage;
using SQLite;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace quiz
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ListPage : CarouselPage
    {
        public int cntMax = 8; // 1ページ当たりの最大数

        Dictionary<string, string> dicHiragana = new Dictionary<string, string>();
        Dictionary<string, string> dicKatakana = new Dictionary<string, string>();
        Dictionary<string, string> dicEigo = new Dictionary<string, string>();
      
        List<string> lstImages = new List<string>();

        private int mode = 0;

        ISoundEffect soundEffect = DependencyService.Get<ISoundEffect>();

        private string str = AppResource.List;

        public ListPage()
        {
            InitializeComponent();

            // 一覧セット
            setList();

            // 画面セット
            setDisplay();

        }

        // コントロールサイズ調整
        protected override void OnSizeAllocated(double width, double height)
        {
            base.OnSizeAllocated(width, height);

            grdHead.WidthRequest = width - grdHead.Margin.Left - grdHead.Margin.Right;
            grdHead.HeightRequest = height * 0.1 - grdHead.Margin.Top - grdHead.Margin.Bottom;

            grdMain.WidthRequest = width - grdMain.Margin.Left - grdMain.Margin.Right;
            grdMain.HeightRequest = height * 0.9 - grdMain.Margin.Top - grdMain.Margin.Bottom;

            //// メイングリッド
            //grdMain.WidthRequest = width - 20;
            //grdMain.HeightRequest = height - 20;

            grdDetail.WidthRequest = width - grdDetail.Margin.Left - grdDetail.Margin.Right;
            grdDetail.HeightRequest = height * 0.9 - grdDetail.Margin.Top - grdDetail.Margin.Bottom;
        }

        // 一覧セット
        private void setList()
        {

            IFolder rootFolder = PCLStorage.FileSystem.Current.LocalStorage;
            var file = rootFolder.CreateFileAsync("quiz.db", CreationCollisionOption.OpenIfExists).Result;
            using (var connection = new SQLiteConnection(file.Path))
            {
                foreach (var question in connection.Table<Question>())
                {
                    if(!lstImages.Contains(question.Image.ToString()))
                    {
                        lstImages.Add(question.Image.ToString());
                    }

                    switch(question.Category)
                    {
                        case 0:
                            dicHiragana.Add(question.Image.ToString(), question.Answer);
                            break;
                        case 1:
                            dicKatakana.Add(question.Image.ToString(), question.Answer);
                            break;
                        case 2:
                            dicEigo.Add(question.Image.ToString(), question.Answer);
                            break;
                        default:
                            break;

                    }
                }
            }
        }

        // 画面セット
        private void setDisplay()
        {
            var All = new ObservableCollection<ListDataModel>();
            
            Boolean flg = true;
            Boolean AddFlg = true;

            while (flg)
            {    
                var dm = new ListDataModel();

                if (lstImages.Count >= All.Count * cntMax + 1)
                {
                    dm.title = (All.Count + 1).ToString() + str;
                    dm.grdWidth = Application.Current.MainPage.Width - 20;
                    dm.grdHeight = Application.Current.MainPage.Height - 20;

                    dm.img1 = "img" + (All.Count * cntMax + 1).ToString();
                    AddFlg = true;
                }
                else
                {
                    dm.img1 = null;
                    flg = false;
                    AddFlg = false;
                }

                if (lstImages.Count >= All.Count * cntMax + 2)
                {
                    dm.img2 = "img" + (All.Count * cntMax + 2).ToString();
                }
                else
                {
                    dm.img2 = null;
                    flg = false;
                }

                if (lstImages.Count >= All.Count * cntMax + 3)
                {
                    dm.img3 = "img" + (All.Count * cntMax + 3).ToString();
                }
                else
                {
                    dm.img3 = null;
                    flg = false;
                }

                if (lstImages.Count >= All.Count * cntMax + 4)
                {
                    dm.img4 = "img" + (All.Count * cntMax + 4).ToString();
                }
                else
                {
                    dm.img4 = null;
                    flg = false;
                }

                if (lstImages.Count >= All.Count * cntMax + 5)
                {
                    dm.img5 = "img" + (All.Count * cntMax + 5).ToString();
                }
                else
                {
                    dm.img5 = null;
                    flg = false;
                }

                if (lstImages.Count >= All.Count * cntMax + 6)
                {
                    dm.img6 = "img" + (All.Count * cntMax + 6).ToString();
                }
                else
                {
                    dm.img6 = null;
                    flg = false;
                }

                if (lstImages.Count >= All.Count * cntMax + 7)
                {
                    dm.img7 = "img" + (All.Count * cntMax + 7).ToString();
                }
                else
                {
                    dm.img7 = null;
                    flg = false;
                }

                if (lstImages.Count >= All.Count * cntMax + 8)
                {
                    dm.img8 = "img" + (All.Count * cntMax + 8).ToString();
                }
                else
                {
                    dm.img8 = null;
                    flg = false;
                }

                if (AddFlg) All.Add(dm);

            }

            vControl.ItemsSource = All;
        }

        private void img_Clicked(object sender, EventArgs e)
        {
            ImageButton ib = (ImageButton)sender;

            if (ib.Source == null) return;

            imgDetailMain.Source = ib.Source;

            setDetailLabel(getNumber(ib.Source.ToString()));

            grdMain.IsEnabled = false;
            grdDetail.IsVisible = true;
        }

        private string getNumber(string str)
        {
            return str.Replace("img", "").Replace("File: ", "");

        }

        private void setDetailLabel(string str)
        {
            switch(mode)
            {
                case 0:
                    lblDetailMain.Text = dicHiragana[str];
                    break;
                case 1:
                    lblDetailMain.Text = dicKatakana[str];
                    break;
                case 2:
                    lblDetailMain.Text = dicEigo[str];
                    break;
                default:
                    lblDetailMain.Text = dicHiragana[str];
                    break;
            }

        }

        private void imgBack_Clicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new MainPage());
        }


        protected override bool OnBackButtonPressed()
        {
            return true;
        }

        private void imgReturn_Clicked(object sender, EventArgs e)
        {
            grdDetail.IsVisible = false;
            grdMain.IsEnabled = true;
        }

        private void ibHiragana_Clicked(object sender, EventArgs e)
        {
            mode = 0;
            ibHiragana.BackgroundColor = Color.SkyBlue;
            ibKatakana.BackgroundColor = Color.White;
            ibEnglish.BackgroundColor = Color.White;
            setDetailLabel(getNumber(imgDetailMain.Source.ToString()));
        }

        private void ibKatakana_Clicked(object sender, EventArgs e)
        {
            mode = 1;
            ibHiragana.BackgroundColor = Color.White;
            ibKatakana.BackgroundColor = Color.SkyBlue;
            ibEnglish.BackgroundColor = Color.White;
            setDetailLabel(getNumber(imgDetailMain.Source.ToString()));

        }

        private void ibEnglish_Clicked(object sender, EventArgs e)
        {
            mode = 2;
            ibHiragana.BackgroundColor = Color.White;
            ibKatakana.BackgroundColor = Color.White;
            ibEnglish.BackgroundColor = Color.SkyBlue;
            setDetailLabel(getNumber(imgDetailMain.Source.ToString()));

        }
    }

    public class ListDataModel
    {
        public string title { get; set; }

        public string img1 { get; set; }
        public string img2 { get; set; }
        public string img3 { get; set; }
        public string img4 { get; set; }
        public string img5 { get; set; }
        public string img6 { get; set; }
        public string img7 { get; set; }
        public string img8 { get; set; }

        public double grdWidth { get; set; }

        public double grdHeight { get; set; }


    }
}