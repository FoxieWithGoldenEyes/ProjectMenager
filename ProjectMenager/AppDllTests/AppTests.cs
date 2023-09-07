using APIsDll.Public.APIs;
using APIsDll.Public.Models;
using AppDll;

namespace AppDllTests
{
    [TestClass]
    public class AppTests
    {
        [TestCleanup]
        public void Cleanup_TestFolder_NameTest()
        {
            if(Directory.Exists("Test"))
                Directory.Delete("Test", true);
        }

        [TestMethod]
        public void InitFolder__CreatesFile()
        {
            App app = new App();
            PmFolderModel model = new PmFolderModel()
            {
                Title = "Test",
                Description = "Test",
                Path = "Test",
                Tags = new List<string>()
            };

            bool funcResult = app.InitFolder(model);
            bool directoryExist = Directory.Exists(model.Path);

            Assert.IsTrue(directoryExist);
            Assert.IsTrue(funcResult);

            if (Directory.Exists(model.Path) )
                Directory.Delete(model.Path, true);
        }

        [TestMethod]
        public void Init_AndDelete_AllOk()
        {
            IApp app = new App();
            var folder = new PmFolderModel
            {
                Title = "Test",
                Path = "Test"
            };

            var result1 = app.InitFolder(folder);
            var result2 = app.DeleteFolder(folder);

            Assert.IsTrue(result1);
            Assert.IsTrue(result2);
        }
    }
}