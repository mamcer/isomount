using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Threading.Tasks;

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
            wrapper = new VirtualCloneDriveWrapper(unitLetter, vcdMountPath, triesBeforeError, waitTime, defaultDriveInfo, null, null);

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
            VirtualCloneDriveWrapper wrapper = new VirtualCloneDriveWrapper(unitLetter, vcdMountPath, 3, 1000, driveInfo, null, null);

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
            VirtualCloneDriveWrapper wrapper = new VirtualCloneDriveWrapper(unitLetter, vcdMountPath, 3, 1000, null, null, null);
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
            VirtualCloneDriveWrapper wrapper = new VirtualCloneDriveWrapper(unitLetter, vcdMountPath, 3, 1000, driveInfo, null, null);
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
            VirtualCloneDriveWrapper wrapper = new VirtualCloneDriveWrapper(unitLetter, vcdMountPath, 3, 1000, null, null, null);
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
            VirtualCloneDriveWrapper wrapper = new VirtualCloneDriveWrapper(unitLetter, vcdMountPath, 3, 1000, driveInfo, null, null);
            long totalSize;

            // Act
            totalSize = wrapper.TotalSize;

            // Assert
            Assert.AreEqual(size, totalSize);
        }

        [TestMethod]
        public async Task MountAsyncWithNonExistingIsoShouldReturnError()
        {
            // Arrange
            var unitLetter = @"F:\";
            var vcdMountPath = @"C:\tmp";
            var mockDriveInfo = new Mock<IDriveInfo>();
            var driveInfo = mockDriveInfo.Object;
            var mockFileProvider = new Mock<IFileProvider>();
            mockFileProvider.Setup(m => m.Exists(It.IsAny<string>())).Returns(false);

            VirtualCloneDriveWrapper wrapper = new VirtualCloneDriveWrapper(unitLetter, vcdMountPath, 3, 1000, driveInfo, mockFileProvider.Object, null);
            DeviceEventArgs deviceEventArgs;

            // Act
            deviceEventArgs = await wrapper.MountAsync(string.Empty);

            // Assert
            Assert.IsTrue(deviceEventArgs.HasError);
            Assert.IsTrue(deviceEventArgs.ErrorMessage.Contains("doesn't exists or don't have access."));
        }

        [TestMethod]
        public async Task MountAsyncShouldCallProcessProviderStart()
        {
            // Arrange
            var unitLetter = @"F:\";
            var vcdMountPath = @"C:\tmp";
            var mockDriveInfo = new Mock<IDriveInfo>();
            var mockFileProvider = new Mock<IFileProvider>();
            var mockProcessProvider = new Mock<IProcessProvider>();

            mockDriveInfo.Setup(m => m.IsReady).Returns(true);
            mockFileProvider.Setup(m => m.Exists(It.IsAny<string>())).Returns(true);
            mockProcessProvider.Setup(m => m.Start(It.IsAny<string>(), It.IsAny<string>())).Returns(new System.Diagnostics.Process());

            VirtualCloneDriveWrapper wrapper = new VirtualCloneDriveWrapper(unitLetter, vcdMountPath, 3, 1000, mockDriveInfo.Object, mockFileProvider.Object, mockProcessProvider.Object);
            DeviceEventArgs deviceEventArgs;

            // Act
            deviceEventArgs = await wrapper.MountAsync(@"C:\my-fake-iso.iso");

            // Assert
            Assert.IsFalse(deviceEventArgs.HasError);
        }

        [TestMethod]
        public async Task MountAsyncWithDriveNotReadyShouldReturnError()
        {
            // Arrange
            var unitLetter = @"F:\";
            var vcdMountPath = @"C:\tmp";
            var mockDriveInfo = new Mock<IDriveInfo>();
            var mockFileProvider = new Mock<IFileProvider>();
            var mockProcessProvider = new Mock<IProcessProvider>();

            mockDriveInfo.Setup(m => m.IsReady).Returns(false);
            mockFileProvider.Setup(m => m.Exists(It.IsAny<string>())).Returns(true);
            mockProcessProvider.Setup(m => m.Start(It.IsAny<string>(), It.IsAny<string>())).Returns(new System.Diagnostics.Process());

            VirtualCloneDriveWrapper wrapper = new VirtualCloneDriveWrapper(unitLetter, vcdMountPath, 3, 1000, mockDriveInfo.Object, mockFileProvider.Object, mockProcessProvider.Object);
            DeviceEventArgs deviceEventArgs;

            // Act
            deviceEventArgs = await wrapper.MountAsync(@"C:\my-fake-iso.iso");

            // Assert
            Assert.IsTrue(deviceEventArgs.HasError);
            Assert.IsTrue(deviceEventArgs.ErrorMessage.Contains("There was an error trying to Mount file on device"));
        }
    }
}