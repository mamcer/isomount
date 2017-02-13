using IsoMount.Properties;
using System;
using System.Configuration;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Forms;
using VirtualDrive;

namespace IsoMount
{
    public partial class Main : Form
    {
        public Main()
        {
            InitializeComponent();

            UnitLetter = ConfigurationManager.AppSettings["UnitLetter"];
            var vcdMountExePath = ConfigurationManager.AppSettings["IsoMountPath"];
            if (!File.Exists(vcdMountExePath))
            {
                MessageBox.Show(Resources.VCDMountExeNotFound, Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
                Environment.Exit(1);
            }

            var driveInfo = DriveInfo.GetDrives();

            foreach (var info in driveInfo)
            {
                if (info.DriveType == DriveType.CDRom)
                {
                    cmbUnit.Items.Add(info.Name);
                }
            }

            if (cmbUnit.Items.Count == 0)
            {
                MessageBox.Show(Resources.NoCDRomUnit, Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
                Environment.Exit(1);
            }

            cmbUnit.SelectedIndexChanged -= cmbUnit_SelectedIndexChanged;
            if (cmbUnit.Items.Contains(UnitLetter + ":\\"))
            {
                cmbUnit.SelectedIndex = cmbUnit.Items.IndexOf(UnitLetter + ":\\");
            }
            else
            {
                cmbUnit.SelectedIndex = 0;
                UnitLetter = cmbUnit.Items[0].ToString().Substring(0,1);
            }
            cmbUnit.SelectedIndexChanged += cmbUnit_SelectedIndexChanged;

            VirtualDrive = new VirtualCloneDriveWrapper(UnitLetter, vcdMountExePath);
        }

        public override sealed string Text
        {
            get { return base.Text; }
            set { base.Text = value; }
        }

        private string UnitLetter { get; set; }

        private IVirtualDrive VirtualDrive { get; set; }

        private void Main_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop, true))
            {
                e.Effect = DragDropEffects.Link;
            }
        }

        private async void Mount(string isoFilePath)
        {
            WriteConsoleMessage("Mounting file...");
            var result = await VirtualDrive.MountAsync(isoFilePath);
            if (!result.HasError)
            {
                WriteConsoleMessage(string.Format("Mounting of '{0}' successfully completed", Path.GetFileName(isoFilePath)));
                Process.Start("explorer", UnitLetter + @":\");
            }
            else
            {
                WriteConsoleMessage("Mount Error: " + result.ErrorMessage);
            }
        }

        private async Task<bool> UnMount()
        {
            WriteConsoleMessage("Unmounting file...");
            var result = await VirtualDrive.UnMountAsync();
            if (!result.HasError)
            {
                WriteConsoleMessage("Unmount successfully completed");
                return true;
            }
            WriteConsoleMessage("Unmount Error: " + result.ErrorMessage);
            return false;
        }

        private void WriteConsoleMessage(string message)
        {
            lstConsole.Items.Add(string.Format("{0} - {1}", DateTime.Now.ToLongTimeString(), message));
        }

        private async void Main_DragDrop(object sender, DragEventArgs e)
        {
            if (!e.Data.GetDataPresent(DataFormats.FileDrop, true))
            {
                return;
            }

            var fileNames = e.Data.GetData(DataFormats.FileDrop, true) as string[];
            if (fileNames == null)
            {
                return;
            }

            foreach (var file in fileNames)
            {
                string fileExtension = Path.GetExtension(file);
                if (File.Exists(file) && fileExtension != null &&
                    (fileExtension.ToLower() == ".iso" || fileExtension.ToLower() == ".bin"))
                {
                    var unmountSuccessful = await UnMount();
                    if (unmountSuccessful)
                    {
                        Mount(file);
                    }
                }
            }
        }
        
        private async void BtnUnmount_Click(object sender, EventArgs e)
        {
            if (
                MessageBox.Show(
                    Resources.UnmountConfirmation,
                    Text, MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                var unmountResult = await UnMount();
                if (!unmountResult)
                {
                    WriteConsoleMessage("Unmount Error.");
                }
            }
        }

        private async void btnMount_Click(object sender, EventArgs e)
        {
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                var unmountSuccessful = await UnMount();
                if (unmountSuccessful)
                {
                    Mount(openFileDialog.FileName);
                }
            }
        }

        private void cmbUnit_SelectedIndexChanged(object sender, EventArgs e)
        {
            UnitLetter = cmbUnit.SelectedItem.ToString().Substring(0, 1);
            VirtualDrive.UnitLetter = UnitLetter;
        }
    }
}