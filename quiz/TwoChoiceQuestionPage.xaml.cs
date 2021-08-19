using PCLStorage;
using SQLite;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace quiz
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class TwoChoiceQuestionPage : ContentPage
    {
        int cntQuiz = 0;    // 現在の問題
        int rightQuiz = 0;  // 正解数
        int maxQuiz = 5;    // 最大問題数
        Button btnAnswer;
        string[] ary;       // 出題問題
        string[] strQuiz;
        int _mode = 0;
        Dictionary<string, string> dicAnswer = new Dictionary<string, string>();
        List<string> listAnswer = new List<string>();

        ISoundEffect soundEffect = DependencyService.Get<ISoundEffect>();

        public TwoChoiceQuestionPage(int mode)
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

            string str = "";
            
            foreach (char c in str)
            {
                listAnswer.Add(c.ToString());
            }
        }

        // 問題出題
        private void setQuestion()
        {
            imgResult.Source = null;
            // 問題作成
            string[] strAns = createQuestion();
            
            Random rnd = new Random();
            int k = rnd.Next(0, 2);

            string strNewAns = "";
            switch (k)
            {
                case 0:
                    strNewAns = dicAnswer[strAns[0]];
                    btnAnswer = btnAns1;
                    break;
                case 1:
                    strNewAns = dicAnswer[strAns[1]];
                    btnAnswer = btnAns2;
                    break;
            }

            imgQMain.Source = "img" + strAns[0] + ".png";

            lblQTitle.Text = "Q" + (cntQuiz + 1).ToString();
            lblQDetail.Text = strNewAns + " ？";

        }

        // 問題作成
        private string[] createQuestion()
        {
            List<string> strRet = new List<string>();

            Random rnd = new Random();
            
            int k = rnd.Next(0, ary.Length);

            strRet.Add(ary[k]);

            List<string> listAry = new List<string>();
            listAry.AddRange(ary);
            listAry.Remove(ary[k]);

            ary = listAry.ToArray();

            k = rnd.Next(0, ary.Length);

            strRet.Add(ary[k]);

            return strRet.ToArray();

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

            Button btn = (Button)sender;
            if (btnAnswer == btn)
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
                await Navigation.PushAsync(new ResultPage(rightQuiz, _mode, 3)).ConfigureAwait(false);
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