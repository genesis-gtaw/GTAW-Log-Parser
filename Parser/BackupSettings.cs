﻿using Microsoft.WindowsAPICodePack.Dialogs;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Parser
{
    public partial class BackupSettings : Form
    {
        public BackupSettings()
        {
            InitializeComponent();

            LoadSettings();
        }

        private void SaveSettings()
        {
            Properties.Settings.Default.BackupPath = BackupPath.Text;

            Properties.Settings.Default.BackupChatLogAutomatically = BackUpChatLogAutomatically.Checked;
            Properties.Settings.Default.EnableIntervalBackup = EnableIntervalBackup.Checked;
            Properties.Settings.Default.IntervalTime = (int)Interval.Value;

            Properties.Settings.Default.Save();
        }

        public void LoadSettings()
        {
            BackupPath.Text = Properties.Settings.Default.BackupPath;

            BackUpChatLogAutomatically.Checked = Properties.Settings.Default.BackupChatLogAutomatically;
            EnableIntervalBackup.Checked = Properties.Settings.Default.EnableIntervalBackup;
            Interval.Value = Properties.Settings.Default.IntervalTime;
        }

        private void BackupPath_MouseClick(object sender, MouseEventArgs e)
        {
            if (BackupPath.Text.Length == 0)
            {
                Browse_Click(this, EventArgs.Empty);
            }
        }

        private void Browse_Click(object sender, EventArgs e)
        {
            CommonOpenFileDialog dialog = new CommonOpenFileDialog
            {
                InitialDirectory = Path.GetPathRoot(Environment.SystemDirectory),
                IsFolderPicker = true
            };

            if (dialog.ShowDialog() == CommonFileDialogResult.Ok)
            {
                BackupPath.Text = dialog.FileName + "\\";
                Browse.Focus();
            }
        }

        private void BackUpChatLogAutomatically_CheckedChanged(object sender, EventArgs e)
        {
            EnableIntervalBackup.Enabled = BackUpChatLogAutomatically.Checked;
            if (!BackUpChatLogAutomatically.Checked)
                EnableIntervalBackup.Checked = false;
        }

        private void EnableIntervalBackup_CheckedChanged(object sender, EventArgs e)
        {
            Interval.Enabled = EnableIntervalBackup.Checked;
        }

        private void Interval_ValueChanged(object sender, EventArgs e)
        {
            EnableIntervalBackup.Text = $"Automatically back up the chat log while the game is running (every {Interval.Value} minutes)";
        }

        private void Reset_Click(object sender, EventArgs e)
        {
            BackupPath.Text = "";

            BackUpChatLogAutomatically.Checked = false;
            EnableIntervalBackup.Checked = false;
            Interval.Text = "10";
        }

        private void CloseWindow_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void BackupSettings_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (BackUpChatLogAutomatically.Checked && (BackupPath.Text == "" || !Directory.Exists(BackupPath.Text)))
            {
                e.Cancel = true;

                MessageBox.Show("Please choose a valid backup location or turn automatic backup off.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            SaveSettings();
        }
    }
}
