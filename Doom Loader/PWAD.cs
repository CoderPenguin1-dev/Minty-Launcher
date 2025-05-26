using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;

namespace Doom_Loader
{
    public partial class PWAD : Form
    {
        public PWAD()
        {
            InitializeComponent();
        }

        private void ManagerSetup(object sender, EventArgs e)
        {
            // Load PWADs into list
            if (ApplicationVariables.externalFiles.Length != 0)
                foreach (string PWAD in ApplicationVariables.externalFiles)
                {
                    if (PWAD.Contains("<M>"))
                        pwadList.Items.Add("<M>" + Path.GetFileName(PWAD));
                    else
                        pwadList.Items.Add(Path.GetFileName(PWAD));
                }

            // Setup tool tips
            toolTips.SetToolTip(addItemButton, "Add File(s).\nRight-click to use -merge instead.");
            toolTips.SetToolTip(removeItemButton, "Remove File(s).");
            toolTips.SetToolTip(reorderUpButton, "Reorder Selected Item Up.");
            toolTips.SetToolTip(reorderDownButton, "Reorder Selected Item Down.");
            toolTips.SetToolTip(pwadList, "Right-click to view file paths.");
        } // Ran at Startup, sets up tooltips and PWAD list

        /// <summary>
        /// Used to reload the PWAD Manager's PWAD List with the mods's filenames only.
        /// It will also disable the reorder and remove buttons.
        /// </summary>
        private void Reload()
        {
            pwadList.Items.Clear();
            foreach (string PWAD in ApplicationVariables.externalFiles)
            {
                // Make sure the merge indicator stays when removing the filepath.
                if (PWAD.Contains("<M>"))
                    pwadList.Items.Add("<M>" + Path.GetFileName(PWAD));
                else
                    pwadList.Items.Add(Path.GetFileName(PWAD));
            }

            // Disable reorder buttons and remove button.
            reorderUpButton.Enabled = false;
            reorderDownButton.Enabled = false;
            removeItemButton.Enabled = false;
        }

        #region PWAD Addition & Removal
        private void AddPWAD(object sender, MouseEventArgs e)
        {
            if (e.Button != MouseButtons.Middle)
            {
                if (addPWADDialog.ShowDialog() != DialogResult.Cancel)
                {
                    string modified = "";
                    if (e.Button == MouseButtons.Right)
                        modified = "<M>";
                    foreach (string PWAD in addPWADDialog.FileNames)
                    {
                        List<string> PWADs = new([.. ApplicationVariables.externalFiles]) { modified + PWAD }; // Turn the original array into a usable list and add in the PWAD.
                        ApplicationVariables.externalFiles = [.. PWADs]; // Merge the list into the array.
                    }
                    Reload();
                }
            }
        }

        private void RemovePWAD(object sender, EventArgs e)
        {
            List<string> PWADs = new(ApplicationVariables.externalFiles.ToList()); // Turn the original array into a usable list.
            for (int i = pwadList.Items.Count - 1; i >= 0; i--)
            {
                if (pwadList.GetSelected(i)) // If the PWAD in the PWAD Listbox is apart of the selected items, remove it.
                {
                    PWADs.RemoveAt(i);
                }
            }
            ApplicationVariables.externalFiles = [.. PWADs]; // Turn the edited list back into an array
            Reload();
        }
        #endregion

        #region Reorder Buttons
        private void ReorderItemUp(object sender, EventArgs e)
        {
            // Reset reorder buttons.
            reorderUpButton.Enabled = true;
            reorderDownButton.Enabled = true;

            string data = ApplicationVariables.externalFiles[pwadList.SelectedIndex];
            int index = pwadList.SelectedIndex;
            if (index != 0) // Check if the item is not already at the top.
            {
                List<string> PWADs = new(ApplicationVariables.externalFiles.ToList());
                // Move the item
                PWADs.RemoveAt(index);
                PWADs.Insert(index - 1, data);
                ApplicationVariables.externalFiles = [.. PWADs];
                Reload();
                pwadList.SelectedIndex = index - 1; // Set the cursor to the new position.

                // Disable reorder button if it's at the top.
                if (pwadList.SelectedIndex == 0)
                    reorderUpButton.Enabled = false;
            }
        }

        private void ReorderItemDown(object sender, EventArgs e)
        {
            // Reset reorder buttons.
            reorderUpButton.Enabled = true;
            reorderDownButton.Enabled = true;

            string data = ApplicationVariables.externalFiles[pwadList.SelectedIndex];
            int index = pwadList.SelectedIndex;
            if (index != pwadList.Items.Count - 1) // Check if the item is not already at the bottom.
            {
                List<string> PWADs = new(ApplicationVariables.externalFiles.ToList());
                // Move the item
                PWADs.RemoveAt(index);
                PWADs.Insert(index + 1, data);
                ApplicationVariables.externalFiles = [.. PWADs];
                Reload();
                pwadList.SelectedIndex = index + 1; // Set the cursor to the new position.

                // Disable reorder button if it's at the bottom.
                if (pwadList.SelectedIndex == pwadList.Items.Count - 1)
                    reorderDownButton.Enabled = false;
            }
        }
        #endregion

        #region File Drag And Drop
        private void PWADDragDrop(object sender, DragEventArgs e)
        {
            string[] items = (string[])e.Data.GetData(DataFormats.FileDrop);
            List<string> PWADs = new(ApplicationVariables.externalFiles.ToList());
            foreach (string item in items)
            {
                if (Directory.Exists(item)) continue; // Ignore item if it's a folder
                else
                {
                    PWADs.Add(item);
                }
            }
            ApplicationVariables.externalFiles = PWADs.ToArray();
            Reload();
        }

        private void PWADDragOver(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop)) e.Effect = DragDropEffects.Copy;
            else e.Effect = DragDropEffects.None;
        }
        #endregion

        private void CheckAmountSelected(object sender, EventArgs e)
        {
            // Disable reorder buttons if there's more than one or no file selected,
            // or if there's only one file.
            if (pwadList.SelectedIndices.Count > 1 || pwadList.SelectedIndices.Count == 0 || pwadList.Items.Count == 1)
            {
                reorderUpButton.Enabled = false;
                reorderDownButton.Enabled = false;
            }
            else
            {
                reorderUpButton.Enabled = true;
                reorderDownButton.Enabled = true;
            }

            // Enable remove button if any amount of files are selected.
            if (pwadList.SelectedIndices.Count > 0)
                removeItemButton.Enabled = true;
            else removeItemButton.Enabled = false;

            // If the selected index is at the top or bottom, disable the respective reorder button.
            if (pwadList.SelectedIndex == pwadList.Items.Count - 1)
                reorderDownButton.Enabled = false;
            if (pwadList.SelectedIndex == 0)
                reorderUpButton.Enabled = false;
        }

        private void ShowExternalFilePaths(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                string output = "";
                List<string> files = [];

                if (pwadList.SelectedItems.Count > 0)
                {
                    foreach (int item in pwadList.SelectedIndices)
                    {
                        files.Add(ApplicationVariables.externalFiles[item]);
                    }
                }
                else files = [.. ApplicationVariables.externalFiles];

                if (ApplicationVariables.externalFiles.Length > 0)
                {
                    foreach (string file in files)
                    {
                        // Remove the merge indicator if there is one.
                        if (file.StartsWith("<M>"))
                            output += file.Remove(0, 3);
                        else output += file;
                        if (files.IndexOf(file) != files.Count - 1)
                            output += "\n\n";
                    }
                }
                else output = "No external files.";

                MessageBox.Show(output, "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
    }
}
