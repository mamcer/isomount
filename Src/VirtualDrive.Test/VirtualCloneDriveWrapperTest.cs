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
            wrapper = new VirtualCloneDriveWrapper(unitLetter, vcdMountPath, triesBeforeError, waitTime, defaultDriveInfo, null);

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
            var mockDriveInfo = new Mock<IDriveInfo>();
                var driveInfo = mockDriveInfo.Object;
            var unitLetter = @"F:\";
            var vcdMountPath = @"C:\tmp";
            VirtualCloneDriveWrapper wrapper = new VirtualCloneDriveWrapper(unitLetter, vcdMountPath, 3, 1000, driveInfo, null);

            // Act
            wrapper.UnitLetter = "C";

            // Assert       
            Assert.AreEqual("C", wrapper.UnitLetter);
            mockDriveInfo.Verify(m => m.SetDriveLetter(It.IsAny<string>()), Times.AtLeastOnce);
        }

        [TestMethod]
        public void VolumeLabelShouldReturnStringEmptyIfDriveInfoIsNull()
        {
            // Arrange
            var unitLetter = @"F:\";
            var vcdMountPath = @"C:\tmp";
            VirtualCloneDriveWrapper wrapper = new VirtualCloneDriveWrapper(unitLetter, vcdMountPath, 3, 1000, null, null);
            string volumeLabel;

            // Act
            volumeLabel = wrapper.VolumeLabel;

            // Assert
            Assert.AreEqual(string.Empty, volumeLabel);
        }

        [TestMethod]
        public void VolumeLabelShouldReturnDriveInfoVolumeLabel()
        {
            // Arrange
            var mockDriveInfo = new Moq.Mock<IDriveInfo>();
            var label = "Hello";
            mockDriveInfo.Setup(m => m.VolumeLabel).Returns(label);
            var driveInfo = mockDriveInfo.Object;
            var unitLetter = @"F:\";
            var vcdMountPath = @"C:\tmp";
            VirtualCloneDriveWrapper wrapper = new VirtualCloneDriveWrapper(unitLetter, vcdMountPath, 3, 1000, driveInfo, null);
            string volumeLabel;

            // Act
            volumeLabel = wrapper.VolumeLabel;

            // Assert
            Assert.AreEqual(label, volumeLabel);
        }

        [TestMethod]
        public void TotalSizeShouldReturnZeroIfDriveInfoIsNull()
        {
            // Arrange
            var unitLetter = @"F:\";
            var vcdMountPath = @"C:\tmp";
            VirtualCloneDriveWrapper wrapper = new VirtualCloneDriveWrapper(unitLetter, vcdMountPath, 3, 1000, null, null);
            long totalSize;

            // Act
            totalSize = wrapper.TotalSize;

            // Assert
            Assert.AreEqual(0, totalSize);
        }

        [TestMethod]
        public void TotalSizeShouldReturnDriveInfoTotalSize()
        {
            // Arrange
            var mockDriveInfo = new Mock<IDriveInfo>();
            var size = 100;
            mockDriveInfo.Setup(m => m.TotalSize).Returns(size);
            var driveInfo = mockDriveInfo.Object;
            var unitLetter = @"F:\";
            var vcdMountPath = @"C:\tmp";
            VirtualCloneDriveWrapper wrapper = new VirtualCloneDriveWrapper(unitLetter, vcdMountPath, 3, 1000, driveInfo, null);
            long totalSize;

            // Act
            totalSize = wrapper.TotalSize;

            // Assert
            Assert.AreEqual(size, totalSize);
        }
    }
}
