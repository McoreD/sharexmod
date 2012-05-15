﻿#region License Information (GPL v3)

/*
    ShareX - A program that allows you to take screenshots and share any file type
    Copyright (C) 2012 ShareX Developers

    This program is free software; you can redistribute it and/or
    modify it under the terms of the GNU General Public License
    as published by the Free Software Foundation; either version 2
    of the License, or (at your option) any later version.

    This program is distributed in the hope that it will be useful,
    but WITHOUT ANY WARRANTY; without even the implied warranty of
    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
    GNU General Public License for more details.

    You should have received a copy of the GNU General Public License
    along with this program; if not, write to the Free Software
    Foundation, Inc., 51 Franklin Street, Fifth Floor, Boston, MA  02110-1301, USA.

    Optionally you can also view the license at <http://www.gnu.org/licenses/>.
*/

#endregion License Information (GPL v3)

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Text;
using System.IO;
using System.Linq;
using System.Media;
using System.Windows.Forms;
using HelpersLib;
using HistoryLib;
using ShareX.HelperClasses;
using ShareX.Properties;
using UploadersLib;
using UploadersLib.HelperClasses;

namespace ShareX
{
    public static class UploadManager
    {
        private static log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public static ImageDestination ImageUploader { get; set; }
        public static TextDestination TextUploader { get; set; }
        public static FileDestination FileUploader { get; set; }
        public static UrlShortenerType URLShortener { get; set; }
        public static MyListView ListViewControl { get; set; }
        public static List<Task> Tasks { get; private set; }

        private static object uploadManagerLock = new object();

        static UploadManager()
        {
            Tasks = new List<Task>();
        }

        #region Files

        public static void UploadFiles(string[] files, AfterCaptureActivity jobs = null)
        {
            if (files != null && files.Length > 0)
            {
                foreach (string file in files)
                {
                    UploadFile(file, jobs);
                }
            }
        }

        public static void UploadFile(AfterCaptureActivity jobs = null)
        {
            using (OpenFileDialog ofd = new OpenFileDialog())
            {
                ofd.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
                ofd.Multiselect = true;
                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    UploadFiles(ofd.FileNames, jobs);
                }
            }
        }

        public static void UploadFile(string path, AfterCaptureActivity act = null)
        {
            if (!string.IsNullOrEmpty(path))
            {
                if (File.Exists(path))
                {
                    EDataType type;
                    EDataType destination = EDataType.Default;

                    if (Helpers.IsImageFile(path))
                    {
                        type = EDataType.Image;

                        if (ImageUploader == ImageDestination.FileUploader)
                        {
                            destination = EDataType.File;
                        }
                    }
                    else if (Helpers.IsTextFile(path))
                    {
                        type = EDataType.Text;

                        if (TextUploader == TextDestination.FileUploader)
                        {
                            destination = EDataType.File;
                        }
                    }
                    else
                    {
                        type = EDataType.File;
                    }

                    if (act == null)
                        act = AfterCaptureActivity.GetNew();

                    foreach (FileDestination fileUploader in act.Uploaders.FileUploaders)
                    {
                        Task task = Task.CreateFileUploaderTask(type, path, destination);
                        task.Info.Uploaders = act.Uploaders;
                        StartUpload(task);
                        break;
                    }
                }
                else if (Directory.Exists(path))
                {
                    string[] files = Directory.GetFiles(path, "*.*", SearchOption.AllDirectories);
                    UploadFiles(files);
                }
            }
        }

        #endregion Files

        #region Images

        public static void DoImageWork(ImageData imageData, AfterCaptureActivity act)
        {
            if (imageData != null)
            {
                foreach (ImageDestination imageUploader in act.Uploaders.ImageUploaders)
                {
                    EDataType destination = imageUploader == ImageDestination.FileUploader ? EDataType.File : EDataType.Image;
                    Task task = Task.CreateImageUploaderTask(imageData, destination);
                    task.Info.ImageJob = act.ImageJobs;
                    task.Info.Uploaders = act.Uploaders;
                    StartUpload(task);
                    break; // ShareX 7.1 will support creation of multiple tasks
                }
            }
        }

        public static void UploadImage(Image img)
        {
            DoImageWork(new ImageData(img), AfterCaptureActivity.GetNew());
        }

        #endregion Images

        #region Text

        /// <summary>
        /// Optionally takes AfterCaptureActivity to configure task specific text uploaders
        /// </summary>
        /// <param name="text"></param>
        /// <param name="act"></param>
        public static void UploadText(string text, AfterCaptureActivity act = null)
        {
            if (act == null)
                act = AfterCaptureActivity.GetNew();
            else if (AfterCaptureActivity.IsNullOrEmpty(act))
                act.GetDefaults();

            if (!string.IsNullOrEmpty(text))
            {
                foreach (TextDestination textUploader in act.Uploaders.TextUploaders)
                {
                    EDataType destination = textUploader == TextDestination.FileUploader ? EDataType.File : EDataType.Text;
                    Task task = Task.CreateTextUploaderTask(text, destination);
                    task.Info.Uploaders = act.Uploaders;
                    StartUpload(task);
                    break; // TODO: ShareXmod 7.1 to have multiple destination support
                }
            }
        }

        public static void ShortenURL(string url, AfterCaptureActivity act = null)
        {
            if (act == null) act = AfterCaptureActivity.GetNew();
            else if (AfterCaptureActivity.IsNullOrEmpty(act)) act.GetDefaults();

            if (!string.IsNullOrEmpty(url))
            {
                Task task = Task.CreateURLShortenerTask(url);
                task.Info.Uploaders = act.Uploaders;
                StartUpload(task);
            }
        }

        #endregion Text

        #region Clipboard Upload

        public static void ClipboardUpload(AfterCaptureActivity jobs = null)
        {
            if (Clipboard.ContainsImage())
            {
                Image img = Clipboard.GetImage();
                UploadImage(img);
            }
            else if (Clipboard.ContainsFileDropList())
            {
                string[] files = Clipboard.GetFileDropList().Cast<string>().ToArray();
                UploadFiles(files);
            }
            else if (Clipboard.ContainsText())
            {
                string text = Clipboard.GetText();

                if (Program.Settings.ClipboardUploadAutoDetectURL && Helpers.IsValidURL(text))
                {
                    ShortenURL(text.Trim(), jobs);
                }
                else
                {
                    UploadText(text, jobs);
                }
            }
        }

        public static void ClipboardUploadWithContentViewer()
        {
            if (Program.Settings.ShowClipboardContentViewer)
            {
                using (ClipboardContentViewer ccv = new ClipboardContentViewer())
                {
                    if (ccv.ShowDialog() == DialogResult.OK && !ccv.IsClipboardEmpty)
                    {
                        UploadManager.ClipboardUpload();
                    }

                    Program.Settings.ShowClipboardContentViewer = !ccv.DontShowThisWindow;
                }
            }
            else
            {
                UploadManager.ClipboardUpload();
            }
        }

        #endregion Clipboard Upload

        #region Drag n Drop

        public static void DragDropUpload(IDataObject data)
        {
            if (data.GetDataPresent(DataFormats.FileDrop, false))
            {
                string[] files = data.GetData(DataFormats.FileDrop, false) as string[];
                UploadFiles(files);
            }
            else if (data.GetDataPresent(DataFormats.Bitmap, false))
            {
                Image img = data.GetData(DataFormats.Bitmap, false) as Image;
                UploadImage(img);
            }
            else if (data.GetDataPresent(DataFormats.Text, false))
            {
                string text = data.GetData(DataFormats.Text, false) as string;
                UploadText(text);
            }
        }

        #endregion Drag n Drop

        public static void UploadImageStream(Stream stream, string filePath)
        {
            if (stream != null && stream.Length > 0 && !string.IsNullOrEmpty(filePath))
            {
                EDataType destination = ImageUploader == ImageDestination.FileUploader ? EDataType.File : EDataType.Image;
                Task task = Task.CreateDataUploaderTask(EDataType.Image, stream, filePath, destination);

                StartUpload(task);
            }
        }

        private static void StartUpload(Task task)
        {
            Tasks.Add(task);
            task.Info.ID = Tasks.Count - 1;
            task.UploadPreparing += new Task.TaskEventHandler(task_UploadPreparing);
            task.UploadStarted += new Task.TaskEventHandler(task_UploadStarted);
            task.UploadProgressChanged += new Task.TaskEventHandler(task_UploadProgressChanged);
            task.UploadCompleted += new Task.TaskEventHandler(task_UploadCompleted);
            CreateListViewItem(task.Info);
            StartTasks();
            TrayIconManager.UpdateTrayIcon();
        }

        private static void StartTasks()
        {
            int workingTasksCount = Tasks.Count(x => x.IsWorking);
            Task[] inQueueTasks = Tasks.Where(x => x.Status == TaskStatus.InQueue).ToArray();

            if (inQueueTasks.Length > 0)
            {
                int len;

                if (Program.Settings.UploadLimit == 0)
                {
                    len = inQueueTasks.Length;
                }
                else
                {
                    len = (Program.Settings.UploadLimit - workingTasksCount).Between(0, inQueueTasks.Length);
                }

                for (int i = 0; i < len; i++)
                {
                    inQueueTasks[i].Start();
                }
            }
        }

        public static void StopUpload(int index)
        {
            if (Tasks.Count < index)
            {
                Tasks[index].Stop();
            }
        }

        public static void UpdateProxySettings()
        {
            ProxySettings proxy = new ProxySettings();
            if (!string.IsNullOrEmpty(Program.Settings.ProxySettings.Host))
            {
                proxy.ProxyConfig = EProxyConfigType.ManualProxy;
            }
            proxy.ProxyActive = Program.Settings.ProxySettings;
            Uploader.ProxySettings = proxy;
        }

        private static void ChangeListViewItemStatus(UploadInfo info)
        {
            if (ListViewControl != null && info.ImageJob.HasFlag(TaskImageJob.UploadImageToHost))
            {
                ListViewItem lvi = ListViewControl.Items[info.ID];
                lvi.SubItems[1].Text = info.Status;
            }
        }

        private static void CreateListViewItem(UploadInfo info)
        {
            if (ListViewControl != null)
            {
                log.InfoFormat("Upload in queue. ID: {0}, Job: {1}, Type: {2}, Host: {3}", info.ID, info.Job, info.UploadDestination, info.Destination);

                ListViewItem lvi = new ListViewItem();
                lvi.Text = info.FileName;
                lvi.SubItems.Add("In queue");
                lvi.SubItems.Add(string.Empty);
                lvi.SubItems.Add(string.Empty);
                lvi.SubItems.Add(string.Empty);
                lvi.SubItems.Add(string.Empty);
                lvi.SubItems.Add(info.DataType.ToString());

                var taskImageJobs = Enum.GetValues(typeof(TaskImageJob)).Cast<TaskImageJob>();
                foreach (TaskImageJob job in taskImageJobs)
                {
                    switch (job)
                    {
                        case TaskImageJob.None:
                            continue;
                    }

                    if (info.ImageJob.HasFlag(TaskImageJob.UploadImageToHost))
                    {
                        lvi.SubItems.Add(info.Destination);
                        break;
                    }
                    else if (info.ImageJob.HasFlag(job))
                    {
                        lvi.SubItems.Add(job.GetDescription());
                        break;
                    }
                    else
                    {
                        lvi.SubItems.Add(string.Empty);
                        break;
                    }
                }

                lvi.SubItems.Add(string.Empty);

                lvi.BackColor = info.ID % 2 == 0 ? Color.White : Color.WhiteSmoke;
                lvi.ImageIndex = 3;
                ListViewControl.Items.Add(lvi);
                lvi.EnsureVisible();
                ListViewControl.FillLastColumn();
            }
        }

        #region Task Event Handler Methods

        private static void task_UploadPreparing(UploadInfo info)
        {
            log.Info(string.Format("Upload preparing. ID: {0}", info.ID));
            ChangeListViewItemStatus(info);
        }

        private static void task_UploadStarted(UploadInfo info)
        {
            string status = string.Format("Upload started. ID: {0}, Filename: {1}", info.ID, info.FileName);
            if (!string.IsNullOrEmpty(info.FilePath)) status += ", Filepath: " + info.FilePath;
            log.Info(status);

            ListViewItem lvi = ListViewControl.Items[info.ID];
            lvi.Text = info.FileName;
            lvi.SubItems[1].Text = info.Status;
            lvi.ImageIndex = 0;
        }

        private static void task_UploadProgressChanged(UploadInfo info)
        {
            if (ListViewControl != null)
            {
                ListViewItem lvi = ListViewControl.Items[info.ID];
                lvi.SubItems[1].Text = string.Format("{0:0.0}%", info.Progress.Percentage);
                lvi.SubItems[2].Text = string.Format("{0} / {1}", Helpers.ProperFileSize(info.Progress.Position, "", true), Helpers.ProperFileSize(info.Progress.Length, "", true));
                if (info.Progress.Speed > 0)
                    lvi.SubItems[3].Text = Helpers.ProperFileSize((long)info.Progress.Speed, "/s", true);
                lvi.SubItems[4].Text = ProperTimeSpan(info.Progress.Elapsed);
                lvi.SubItems[5].Text = ProperTimeSpan(info.Progress.Remaining);
            }
        }

        private static string ProperTimeSpan(TimeSpan ts)
        {
            string time = string.Format("{0:00}:{1:00}", ts.Minutes, ts.Seconds);
            int hours = (int)ts.TotalHours;
            if (hours > 0) time = hours + ":" + time;
            return time;
        }

        /// <summary>
        /// Mod 01: Not just uploads, everything gets added to List e.g. Saving to file
        /// </summary>
        /// <param name="info">UploadInfo</param>
        private static void task_UploadCompleted(UploadInfo info)
        {
            try
            {
                if (ListViewControl != null && info != null && info.Result != null)
                {
                    ListViewItem lvi = ListViewControl.Items[info.ID];
                    lvi.Tag = info.Result;

                    if (info.Result.IsError)
                    {
                        string errors = string.Join("\r\n\r\n", info.Result.Errors.ToArray());

                        log.ErrorFormat("Upload failed. ID: {0}, Filename: {1}, Errors:\r\n{2}", info.ID, info.FileName, errors);

                        lvi.SubItems[1].Text = "Error";
                        lvi.SubItems[8].Text = string.Empty;
                        lvi.ImageIndex = 1;
                    }
                    else
                    {
                        log.InfoFormat("Upload completed. ID: {0}, Filename: {1}, URL: {2}, Duration: {3} ms", info.ID, info.FileName,
                            info.Result.URL, (int)info.UploadDuration.TotalMilliseconds);

                        lvi.SubItems[1].Text = info.Status;
                        lvi.ImageIndex = 2;

                        if (!string.IsNullOrEmpty(info.Result.URL))
                        {
                            string url = string.IsNullOrEmpty(info.Result.ShortenedURL) ? info.Result.URL : info.Result.ShortenedURL;

                            lvi.SubItems[8].Text = url;

                            if (Program.Settings.ClipboardAutoCopy)
                            {
                                Helpers.CopyTextSafely(url);
                            }

                            if (Program.Settings.SaveHistory)
                            {
                                HistoryManager.AddHistoryItemAsync(Program.HistoryFilePath, info.GetHistoryItem());
                            }

                            if (FormsHelper.Main.niTray.Visible)
                            {
                                FormsHelper.Main.niTray.Tag = url;
                                FormsHelper.Main.niTray.ShowBalloonTip(5000, "ShareX - Upload completed", url, ToolTipIcon.Info);
                            }

                            if (Program.Settings.ShowClipboardOptionsWizard)
                            {
                                WindowAfterUpload dlg = new WindowAfterUpload(info) { Icon = Resources.ShareX };
                                NativeMethods.ShowWindow(dlg.Handle, (int)WindowShowStyle.ShowNoActivate);
                            }
                        }
                    }

                    if (Program.Settings.PlaySoundAfterUpload)
                    {
                        SystemSounds.Exclamation.Play();
                    }

                    lvi.EnsureVisible();
                }
            }
            finally
            {
                StartTasks();
            }
        }

        #endregion Task Event Handler Methods
    }
}