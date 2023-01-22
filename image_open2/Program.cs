using System;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;

namespace image_open
{
    public static class WindowsPhotoViewer
    {
        private const string FilePath32 = @"c:\program files (x86)\Windows Photo Viewer\PhotoViewer.dll";
        private const string FilePath64 = @"c:\program files\Windows Photo Viewer\PhotoViewer.dll";

        [DllImport(FilePath32, CharSet = CharSet.Unicode, EntryPoint = "ImageView_FullscreenW")]
        private static extern void ImageView_Fullscreen32(
            IntPtr unknown1, IntPtr unknown2, string path, int unknown3);

        [DllImport(FilePath64, CharSet = CharSet.Unicode, EntryPoint = "ImageView_FullscreenW")]
        private static extern void ImageView_Fullscreen64(
            IntPtr unknown1, IntPtr unknown2, string path, int unknown3);

        public static bool ShowImage(FileInfo imageFile)
        {
            if ((IntPtr.Size == 8) && File.Exists(FilePath64) && imageFile.Exists)
            {
                ImageView_Fullscreen64(IntPtr.Zero, IntPtr.Zero, imageFile.FullName, 0);
                return true;
            }
            else if ((IntPtr.Size == 4) && File.Exists(FilePath32) && imageFile.Exists)
            {
                ImageView_Fullscreen32(IntPtr.Zero, IntPtr.Zero, imageFile.FullName, 0);
                return true;
            }
            return false;
        }
    }
    internal class Program
    {
        static void Main(string[] args)
        {
            string roamingfolder = System.IO.Directory.GetParent(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData)).FullName
                                                                                                                 + "\\Roaming\\hah";
            string roamingfile = System.IO.Directory.GetParent(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData)).FullName
                                                                                                                 + "\\Roaming\\hah\\image1.jpg";



            void showphoto()
            {
                Assembly asmbly = Assembly.GetExecutingAssembly();
                const string NAME = "image_open2.Resources.Billi1609482658k220y8154.jpg";
                if (File.Exists(roamingfile))
                    File.Delete(roamingfile);
                if (!Directory.Exists(roamingfolder))
                    Directory.CreateDirectory(roamingfolder);
                using (Stream stream = asmbly.GetManifestResourceStream(NAME))
                {
                    using (FileStream fileStream = new FileStream(roamingfile, FileMode.Create))
                    {
                        for (int i = 0; i < stream.Length; i++)
                        {
                            fileStream.WriteByte((byte)stream.ReadByte());
                        }
                        fileStream.Close();
                    }
                }
                WindowsPhotoViewer.ShowImage(new FileInfo(roamingfile));
            }

            showphoto();
        }
    }
}
