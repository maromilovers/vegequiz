using PCLStorage;
using System.Reflection;
using Xamarin.Forms;
using System.Threading.Tasks;

namespace quiz
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            Task thread = Task.Factory.StartNew(() => setDB());

            Task.WaitAll(thread);

            MainPage = new NavigationPage(new MainPage());
        }

        private async Task setDB()
        {
            const string databaseFileName = "quiz.db";
            // ルートフォルダを取得する
            IFolder rootFolder = FileSystem.Current.LocalStorage;

            // ファイルシステム上のDBファイルの存在チェックを行う
            var result = await rootFolder.CheckExistsAsync(databaseFileName);
            if (result != ExistenceCheckResult.NotFound)
            {
                // 存在する場合削除
                var file = (IFile)rootFolder.GetFileAsync(databaseFileName).Result;
                await file.DeleteAsync();

            }
            // 新たに空のDBファイルを作成する
            var newFile = await rootFolder.CreateFileAsync(databaseFileName, CreationCollisionOption.ReplaceExisting);
            // Assemblyに埋め込んだDBファイルをストリームで取得し、空ファイルにコピーする
            var assembly = typeof(App).GetTypeInfo().Assembly;

            using (var stream = assembly.GetManifestResourceStream("quiz.quiz.db"))
            {
                using (var outputStream = await newFile.OpenAsync(FileAccess.ReadAndWrite))
                {
                    stream.CopyTo(outputStream);
                    outputStream.Flush();
                }
            }

            return;
        }

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
