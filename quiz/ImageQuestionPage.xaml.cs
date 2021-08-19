using PCLStorage;
using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace quiz
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ImageQuestionPage : ContentPage
    {
        int cntQuiz = 0;    // 現在の問題
        int rightQuiz = 0;  // 正解数
        int maxQuiz = 5;    // 最大問題数
        ImageButton ibAnswer;
        string[] ary;       // 出題問題
        string[] strQuiz;
        int _mode = 0;
        Dictionary<string, string> dicAnswer = new Dictionary<string, string>();

        ISoundEffect soundEffect = DependencyService.Get<ISoundEffect>();

        public ImageQuestionPage(int mode)
        {
            InitializeComponent();

            _mode = mode;

            // 解答Dictionary作成
            setAnswer();

            ary = strQuiz;

            // 問題表示
            setQuestion();
        }

        // コントロールサイズ調整
        protected override void OnSizeAllocated(double width, double height)
        {
            base.OnSizeAllocated(width, height);

            //grdMain.WidthRequest = width - 20;
            //grdMain.HeightRequest = height - 20;
            //imgResult.WidthRequest = width - 20;
            //imgResult.HeightRequest = height - 20;

            grdHead.WidthRequest = width - grdHead.Margin.Left - grdHead.Margin.Right;
            grdHead.HeightRequest = height * 0.1 - grdHead.Margin.Top - grdHead.Margin.Bottom;

            grdMain.WidthRequest = width - grdMain.Margin.Left - grdMain.Margin.Right;
            grdMain.HeightRequest = height * 0.9 - grdMain.Margin.Top - grdMain.Margin.Bottom;
            imgResult.WidthRequest = width - grdMain.Margin.Left - grdMain.Margin.Right;
            imgResult.HeightRequest = height * 0.9 - grdMain.Margin.Top - grdMain.Margin.Bottom;
        }

        // 解答Dictionary作成
        private void setAnswer()
        {

            IFolder rootFolder = FileSystem.Current.LocalStorage;
            var file = rootFolder.CreateFileAsync("quiz.db", CreationCollisionOption.OpenIfExists).Result;
            using (var connection = new SQLiteConnection(file.Path))
            {
                foreach (var question in connection.Table<Question>())
                {
                    if (question.Category == _mode)
                    { 
                        dicAnswer.Add(question.Image.ToString(), question.Answer);
                    }
                }
            }

            // 問題リスト作成
            List<string> lst = new List<string>();
            foreach(var a in dicAnswer.Keys)
            {
                lst.Add(a);
            }

            strQuiz = lst.ToArray();

        }

        // 問題出題
        private void setQuestion()
        {
            imgResult.Source = null;
            // 問題作成
            string[] strQes = createQuestion();

            lblQMain.Text = dicAnswer[strQes[0]];

            string[] strQes2 = strQes.OrderBy(i => Guid.NewGuid()).ToArray();

            btnAns1.Source = "img" + strQes2[0] + ".png";
            btnAns2.Source = "img" + strQes2[1] + ".png";
            btnAns3.Source = "img" + strQes2[2] + ".png";
            btnAns4.Source = "img" + strQes2[3] + ".png";

            if (strQes[0] == strQes2[0])
            {
                ibAnswer = btnAns1;
            }
            else if (strQes[0] == strQes2[1])
            {
                ibAnswer = btnAns2;
            }
            else if (strQes[0] == strQes2[2])
            {
                ibAnswer = btnAns3;
            }
            else if (strQes[0] == strQes2[3])
            {
                ibAnswer = btnAns4;
            }

            lblQTitle.Text = "Q" + (cntQuiz + 1).ToString();
            lblQDetail.Text = AppResource.ImageQuiz;

        }

        // 問題作成
        private string[] createQuestion()
        {
            List<string> listRet = new List<string>();
            Random rnd = new Random();
            
            int k = rnd.Next(0, ary.Length);

            listRet.Add(ary[k]);

            List<string> listAry = new List<string>();
            listAry.AddRange(ary);
            listAry.Remove(ary[k]);

            ary = listAry.ToArray();

            while (listRet.Count < 4)
            {
                string str = strQuiz[rnd.Next(0, strQuiz.Length)];

                if (!listRet.Contains(str))
                {
                    listRet.Add(str);
                }
            }

            return listRet.ToArray();

        }

        private void imgBack_Clicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new MainPage());
        }

        protected override bool OnBackButtonPressed()
        {
            return true;
        }

        private async void btnAns_Clicked(object sender, EventArgs e)
        {
            grdMain.IsEnabled = false;

            ImageButton btn = (ImageButton)sender;
            if (ibAnswer == btn)
            {
                imgResult.Source = "maru.png";
                using (soundEffect as IDisposable)
                {
                    soundEffect.CorrectSound();
                }
                rightQuiz++;
            }
            else
            {
                imgResult.Source = "batu.png";
                using (soundEffect as IDisposable)
                {
                    soundEffect.IncorrectSound();
                }

            }

            imgResult.IsVisible = true;

            cntQuiz++;

            await Task.Delay(1000);

            imgResult.IsVisible = false;

            if (cntQuiz >= maxQuiz)
            {
                await Navigation.PushAsync(new ResultPage(rightQuiz, _mode, 1)).ConfigureAwait(false);
            }
            else
            {
                setQuestion();
            }

            grdMain.IsEnabled = true;
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
    }
}