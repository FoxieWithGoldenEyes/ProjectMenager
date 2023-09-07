using APIsDll.Public.Models;
using IODll;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace IODllTests
{
    [TestClass]
    public class AppFileTests
    {
        static string _dataFolderPath = Path.Combine(Environment.CurrentDirectory, "Data");

        // Runs before every test
        [TestInitialize]
        public void InitializeTests()
        {
            Directory.CreateDirectory(_dataFolderPath);
        }

        // Runs after every test
        [TestCleanup]
        public void CleanupTests()
        {
            Directory.Delete(_dataFolderPath, true);
        }


        // Ctors
        [TestMethod]
        public void Ctor__DoesNotThrowsException()
        {
            AppFile<string> file = new AppFile<string>(Path.Combine(_dataFolderPath, "log.txt"), AppFile<string>.FileType.LinedText);
        }

        [TestMethod]
        public void Ctor_JsonFileType_CreatesDefaultContent()
        {
            AppFile<string> file = new AppFile<string>(Path.Combine(_dataFolderPath, "data.json"), AppFile<string>.FileType.JsonData);
            string fileText = File.ReadAllText(file.Path);
            string expectedText = "[]";

            Assert.AreEqual(expectedText, fileText);
        }


        // Simple Read/Write
        [TestMethod]
        public void WriteAllEntries_TextLogFile_WritesToFile()
        {
            AppFile<string> logFile = new AppFile<string>(Path.Combine(_dataFolderPath, "log.txt"), AppFile<string>.FileType.LinedText);
            List<string> lines = new List<string>()
            {
                "Hello world"
            };

            logFile.WriteAllEntries(lines);
            var textLines = File.ReadAllLines(logFile.Path).ToList();

            Assert.AreEqual(lines.Count, textLines.Count);
            for (int i = 0; i < textLines.Count; i++)
            {
                Assert.AreEqual(lines[i], textLines[i]);
            }
        }

        [TestMethod]
        public void WriteAllEntries_JsonFile_WritesToFile()
        {
            List<string> dataListToSerialize = new List<string>
            {
                "Hello World"
            };
            string jsonExpectedSerializeData = System.Text.Json.JsonSerializer.Serialize(dataListToSerialize, new JsonSerializerOptions { WriteIndented = true });
            AppFile<string> dataFile = new AppFile<string>(Path.Combine(_dataFolderPath, "data.json"), AppFile<string>.FileType.JsonData);

            dataFile.WriteAllEntries(dataListToSerialize);
            string fileText = File.ReadAllText(dataFile.Path);

            Assert.AreEqual(jsonExpectedSerializeData, fileText);
        }



        // IEnumerable Tests
        [TestMethod]
        public void Any_OnSimpleText_WorksFine()
        {
            AppFile<string> dataFile = new AppFile<string>(Path.Combine(_dataFolderPath, "data.json"), AppFile<string>.FileType.JsonData);
            dataFile.WriteAllEntries(new List<string> { "Hello World" });

            bool result = dataFile.Any(x => x == "Hello World");

            Assert.IsTrue(result);
        }

        [TestMethod]
        public void Add_SimpleObject_WorksFine()
        {
            AppFile<string> dataFile = new AppFile<string>(Path.Combine(_dataFolderPath, "data.json"), AppFile<string>.FileType.JsonData);
            List<string> data = new List<string>()
            {
                "Hello",
                "World",
                "Man"
            };

            dataFile.AddRange(data);
            var fileData = dataFile.ReadAllEntries();

            Assert.AreEqual(data.Count, fileData.Count);
            for (int i = 0; i < data.Count; i++)
            {
                Assert.AreEqual(data[i], fileData[i]);
            }
        }


        // Serialization Tests
        [TestMethod]
        public void WritesAllEntries_PmFolder_SerializeObject()
        {
            PmFolderModel model = new PmFolderModel()
            {
                Title = "Test",
                Description = "Test",
                Path = "Path",
                Tags = new List<string>()
            };
            AppFile<PmFolderModel> folderModelDataFile = new AppFile<PmFolderModel>(Path.Combine(_dataFolderPath, "data.json"), AppFile<PmFolderModel>.FileType.JsonData);


            folderModelDataFile.Add(model);
            var reuslt = folderModelDataFile.ReadAllEntries()[0];

            Assert.AreEqual(model, reuslt);
        }
    }
}
