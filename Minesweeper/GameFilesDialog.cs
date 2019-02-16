using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Minesweeper
{
    class GameFilesDialog
    {

        public Form GameFilesForm;
        private ListBox savedGamesList;
        private TextBox selectedGameTextbox;
        private Label saveGamesLabel;
        private Label saveGameLabel;
        public Button saveGameButton;
        public string saveString;
        public bool dialogDone = false;
        public GameMap GameMapSender;
        private SaveTypeDialog senderObject;
        private string callType;

        public enum ActionsAfterDialog
        {
            Nothing,
            Restart,
            Exit
        }
        private ActionsAfterDialog actionAfterDialog;

        public GameFilesDialog(ActionsAfterDialog ActionAfterDialog, GameMap gameMapSender, SaveTypeDialog sender, string finishString)
        {
            callType = finishString;
            GameMapSender = gameMapSender;
            senderObject = sender;
            actionAfterDialog = ActionAfterDialog;
            CreateDialog();
        }

        public void CreateDialog()
        {
            GameFilesForm = new Form();
            GameFilesForm.TopMost = true;
            GameFilesForm.Show();
            GameFilesForm.FormClosing += new FormClosingEventHandler(ExitButtonClicked);
            GameFilesForm.FormBorderStyle = FormBorderStyle.Fixed3D;
            GameFilesForm.MaximizeBox = false;
            savedGamesList = new ListBox();
            savedGamesList.Location = new System.Drawing.Point((GameFilesForm.Width - savedGamesList.Width) / 2, ((GameFilesForm.Height) / 2) - savedGamesList.Height);
            GameFilesForm.Controls.Add(savedGamesList);
            selectedGameTextbox = new TextBox();
            selectedGameTextbox.Width = savedGamesList.Width;
            selectedGameTextbox.Location = new System.Drawing.Point(savedGamesList.Location.X, savedGamesList.Location.Y + savedGamesList.Height + 10);
            GameFilesForm.Controls.Add(selectedGameTextbox);
            saveGamesLabel = new Label();
            GameFilesForm.Controls.Add(saveGamesLabel);
            saveGamesLabel.Width = savedGamesList.Width;
            saveGamesLabel.Location = new System.Drawing.Point(savedGamesList.Location.X, savedGamesList.Location.Y - saveGamesLabel.Height - 10);
            saveGamesLabel.Text = "Saved Games:";
            saveGameLabel = new Label();
            GameFilesForm.Controls.Add(saveGameLabel);
            saveGameLabel.Width = 60;
            saveGameLabel.Location = new System.Drawing.Point(savedGamesList.Location.X - saveGameLabel.Width ,selectedGameTextbox.Location.Y+5);
            saveGameLabel.Text = "Name:";
            saveGameButton = new Button();
            GameFilesForm.Controls.Add(saveGameButton);
            saveGameButton.Width = savedGamesList.Width;
            saveGameButton.Location = new System.Drawing.Point(savedGamesList.Location.X, selectedGameTextbox.Location.Y + saveGameButton.Height + 10);
            saveGameButton.Click += new EventHandler(ButtonClicked);
            savedGamesList.MouseClick += new MouseEventHandler(SaveListItemSingleClicked);
            savedGamesList.MouseDoubleClick += new MouseEventHandler(SaveListItemDoubleClicked);
            PopulateSaveList();
        }
        public void SaveGameDialogDone()
        {
            switch (actionAfterDialog)
            {
                case ActionsAfterDialog.Restart:
                    System.Diagnostics.Process.Start(Application.ExecutablePath);
                    Environment.Exit(0);
                    break;
                case ActionsAfterDialog.Exit:
                    Environment.Exit(0);
                    break;
                default:
                    if (dialogDone)
                    {
                        GameFilesForm.Close();
                    }
                    break;
            }
        }

        private void ExitButtonClicked(object sender, FormClosingEventArgs e)
        {
            if (!dialogDone)
            {
                DialogResult shouldSave = MessageBox.Show("Are you sure that you do not want to " + callType + "?", "Save", MessageBoxButtons.YesNo);
                if (shouldSave == DialogResult.Yes)
                {
                    SaveGameDialogDone();
                }
                else
                {
                    e.Cancel = true;
                }

            }

        }
        private void ButtonClicked(object sender, EventArgs e)
        {
            if (selectedGameTextbox.Text != "")
            {
                if (IsLetter(selectedGameTextbox.Text.First()))
                {
                    saveString = selectedGameTextbox.Text;
                    senderObject.ButtonClicked();
                }
                else
                {
                    MessageBox.Show("The name of your save must start with a letter");
                }
            }
            else
            {
                MessageBox.Show("The name of your save must start with a letter");
            }
        }
        private bool IsLetter(char letter)
        {
            if ((letter >= 'a' && letter <= 'z') || (letter >= 'A' && letter <= 'Z'))
            {
                return true;
            }
            else
            {
                return false;
            }

        }

        public void PopulateSaveList()
        {
            string path = Application.StartupPath;
            int substringIndex = 0;
            savedGamesList.Items.Clear();
            foreach (string file in System.IO.Directory.GetFiles(path))
            {
                if (file.Substring(file.Length - 4) == ".txt")
                {
                    try
                    {
                        substringIndex = file.LastIndexOf('\\') + 1;
                    }
                    catch (Exception)
                    {

                        throw;
                    }
                    savedGamesList.Items.Add(file.Substring(substringIndex).Substring(0, file.Length - substringIndex - 4));
                }
            }

        }
        private void SaveListItemSingleClicked(object sender, MouseEventArgs e)
        {
            if (savedGamesList.SelectedItem != null)
            {
                selectedGameTextbox.Text = savedGamesList.SelectedItem.ToString();
            }

        }
        private void SaveListItemDoubleClicked(object sender, MouseEventArgs e)
        {
            if (savedGamesList.SelectedItem != null)
            {
                selectedGameTextbox.Text = savedGamesList.SelectedItem.ToString();
                saveString = selectedGameTextbox.Text;
                senderObject.ButtonClicked();
            }
        }
        public bool alreadyExists()
        {
            foreach (object item in savedGamesList.Items)
            {
                if (item.ToString() == saveString)
                {
                    return true;
                }
            }
            return false;
        }
    }
}
