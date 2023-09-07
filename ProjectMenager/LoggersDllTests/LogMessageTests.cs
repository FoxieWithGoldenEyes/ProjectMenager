using APIsDll.Local.Models;

namespace LoggersDllTests
{
    [TestClass]
    public class LogMessageTests
    {
        [TestMethod]
        public void Properties_OnObjectCreate_AreNotDefaultOrNull()
        {
            // Arrange
            string message = "Hello World";

            // Act
            LogMessage logMessage = new LogMessage(message);

            // Assert
            Assert.AreNotEqual(string.Empty, logMessage.OnlyMessage);
            Assert.AreNotEqual(null, logMessage.OnlyMessage);

            Assert.AreNotEqual(TimeOnly.FromTimeSpan(TimeSpan.Zero), logMessage.TimeStamp);
            Assert.AreNotEqual(null, logMessage.TimeStamp);

            Assert.AreNotEqual(null, logMessage.Type);
        }

        [TestMethod]
        public void GetMessageReturnedString__ExaualsToStringReturnedString()
        {
            // Arrange
            LogMessage logMessage = new LogMessage("Hello world");

            // Act
            Assert.AreEqual(logMessage.GetAllMessage(), logMessage.ToString());
        }
    } 
}