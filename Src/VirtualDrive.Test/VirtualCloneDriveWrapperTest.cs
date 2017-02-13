using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace VirtualDrive.Test
{
    [TestClass]
    public class VirtualCloneDriveWrapperTest
    {
        [TestMethod]
        public void ConstructorUnitLetterMountPathShouldSetProperties()
        {
            // Arrange
            string unitLetter = "Z";
            string vcdMountPath = @"C:\Program Files (x86)\Elaborate Bytes\VirtualCloneDrive\VCDMount.exe";
            VirtualCloneDriveWrapper wrapper;

            // Act
            wrapper = new VirtualCloneDriveWrapper(unitLetter, vcdMountPath);

            // Assert       
            Assert.AreEqual(unitLetter, wrapper.UnitLetter);
            Assert.AreEqual(vcdMountPath, wrapper.VcdMountPath);
            Assert.AreEqual(5, wrapper.TriesBeforeError);
            Assert.AreEqual(1000, wrapper.WaitTime);
        }

        [TestMethod]
        public void ConstructorAllParametersShouldSetProperties()
        {
            // Arrange
            string unitLetter = "Z";
            string vcdMountPath = @"C:\Program Files (x86)\Elaborate Bytes\VirtualCloneDrive\VCDMount.exe";
            int triesBeforeError = 10;
            int waitTime = 5000;
            IDriveInfo defaultDriveInfo = new DefaultDriveInfo();

            VirtualCloneDriveWrapper wrapper;

            // Act
            wrapper = new VirtualCloneDriveWrapper(unitLetter, vcdMountPath, triesBeforeError, waitTime, defaultDriveInfo);

            // Assert       
            Assert.AreEqual(unitLetter, wrapper.UnitLetter);
            Assert.AreEqual(vcdMountPath, wrapper.VcdMountPath);
            Assert.AreEqual(triesBeforeError, wrapper.TriesBeforeError);
            Assert.AreEqual(waitTime, wrapper.WaitTime);
        }

        [TestMethod]
        public void UnitLetterSetShouldCallDriveInfoSetDriveLetter()
        {
            // Arrange
            var driveInfo = new Moq.Mock<IDriveInfo>().Object;
            driveInfo.SetDriveLetter(It.IsAny<string>());
            var unitLetter = @"F:\";
            var vcdMountPath = @"C:\tmp";
            VirtualCloneDriveWrapper wrapper = new VirtualCloneDriveWrapper(unitLetter, vcdMountPath, 3, 1000, driveInfo);

            // Act


            // Assert       
        }

        // Arrange

        // Act

        // Assert       

    }
}
