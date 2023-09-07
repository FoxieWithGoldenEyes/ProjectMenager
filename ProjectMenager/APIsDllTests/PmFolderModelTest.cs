using APIsDll.Public.Models;

namespace APIsDllTests
{
    [TestClass]
    public class PmFolderModelTest
    {
        [TestMethod]
        public void Ctor_Default_CreatesAllOK()
        {
            PmFolderModel model = new PmFolderModel();
        }

        [TestMethod]
        public void Ctor_StandardCreate_CalculatesPropsCorrectly()
        {
            PmFolderModel model = new PmFolderModel()
            {
                Title = "Title",
                Description = "Siemaeniu",
                Tags = new List<string>() { "Hello", "World" }
            };

            Assert.AreEqual("Title", model.Title);
            Assert.AreEqual("Siemaeniu", model.Description);
        }
    }
}