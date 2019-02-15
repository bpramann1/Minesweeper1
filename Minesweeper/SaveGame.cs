using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Minesweeper
{
    class SaveGame
    {

        private Form saveGameForm;
        private GameMap GameMapSender;
        private ListBox savedGamesList;
        private TextBox selectedGameTextbox;
        private Label saveGamesLabel;
        private Label saveGameLabel;
        private Button saveGameButton;
        private string saveString;
        private bool doneSaving = false;

        public enum ActionsAfterSave
        {
            Nothing,
            Restart,
            Exit
        }
        private ActionsAfterSave actionAfterSave;

        public SaveGame(GameMap sender, ActionsAfterSave ActionAfterSave)
        {
            actionAfterSave = ActionAfterSave;
            GameMapSender = sender;
            CreateSaveGameDialog();
        }

        public void CreateSaveGameDialog()
        {
            saveGameForm = new Form();
            saveGameForm.TopMost = true;
            saveGameForm.Show();
            saveGameForm.FormClosing += new FormClosingEventHandler(ExitButtonClicked);
            saveGameForm.FormBorderStyle = FormBorderStyle.Fixed3D;
            saveGameForm.MaximizeBox = false;
            savedGamesList = new ListBox();
            savedGamesList.Location = new System.Drawing.Point((saveGameForm.Width - savedGamesList.Width)/2, ((saveGameForm.Height) / 2) - savedGamesList.Height);
            saveGameForm.Controls.Add(savedGamesList);
            selectedGameTextbox = new TextBox();
            selectedGameTextbox.Width = savedGamesList.Width;
            selectedGameTextbox.Location = new System.Drawing.Point(savedGamesList.Location.X, savedGamesList.Location.Y + savedGamesList.Height + 10);
            saveGameForm.Controls.Add(selectedGameTextbox);
            saveGamesLabel = new Label();
            saveGameForm.Controls.Add(saveGamesLabel);
            saveGamesLabel.Width = savedGamesList.Width;
            saveGamesLabel.Location = new System.Drawing.Point(savedGamesList.Location.X, savedGamesList.Location.Y - saveGamesLabel.Height - 10);
            saveGamesLabel.Text = "Saved Games:";
            saveGameLabel = new Label();
            saveGameForm.Controls.Add(saveGameLabel);
            saveGameLabel.Width = 60;
            saveGameLabel.Location = new System.Drawing.Point(savedGamesList.Location.X- saveGameLabel.Width -10, selectedGameTextbox.Location.Y);
            saveGameLabel.Text = "Save As:";
            saveGameButton = new Button();
            saveGameForm.Controls.Add(saveGameButton);
            saveGameButton.Width = savedGamesList.Width;
            saveGameButton.Location = new System.Drawing.Point(savedGamesList.Location.X, selectedGameTextbox.Location.Y + saveGameButton.Height + 10);
            saveGameButton.Text = "Save";
            saveGameButton.Click += new EventHandler(SaveClicked);
            savedGamesList.MouseClick += new MouseEventHandler(SaveListItemSingleClicked);
            savedGamesList.MouseDoubleClick += new MouseEventHandler(SaveListItemDoubleClicked);
            PopulateSaveList();
        }
        public void SaveGameDialogDone()
        {
            switch (actionAfterSave)
            {
                case ActionsAfterSave.Restart:
                    System.Diagnostics.Process.Start(Application.ExecutablePath);
                    Environment.Exit(0);
                    break;
                case ActionsAfterSave.Exit:
                    Environment.Exit(0);
                    break;
                default:
                    break;
            }
        }
        
        private void ExitButtonClicked(object sender, FormClosingEventArgs e)
        {
            if (!doneSaving)
            {
                DialogResult shouldSave = MessageBox.Show("Are you sure that you do not want to save?", "Save", MessageBoxButtons.YesNo);
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
        private void SaveClicked(object sender, EventArgs e)
        {
            if (selectedGameTextbox.Text != "")
            {
                if (IsLetter(selectedGameTextbox.Text.First()))
                {
                    saveString = selectedGameTextbox.Text;
                    SaveFile();
                    savedGamesList.Items.Add(selectedGameTextbox.Text);
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
            if ((letter >= 'a' && letter<='z') || (letter>='A' && letter<='Z'))
            {
                return true;
            }
            else
            {
                return false;
            }
            
        }
        private void SaveFile()
        {
            bool alreadyExists = false;
            bool skip = false;
            try
            {
                foreach (object item in savedGamesList.Items)
                {
                    if (item.ToString() == saveString)
                    {
                        alreadyExists = true;
                    }
                }
                if (alreadyExists)
                {
                    DialogResult shouldSave = MessageBox.Show("Are you sure that you want to overwrite the old save by that name?", "Save", MessageBoxButtons.YesNo);
                    if (shouldSave == DialogResult.No)
                    {
                        skip = true;
                    }
                }
                if (!skip)
                {
                    doneSaving = true;
                    System.IO.File.WriteAllText(Application.StartupPath + "\\" + saveString + ".txt", "");
                    writeNewLineText(GameMapSender.numberOfColumns.ToString());
                    writeNewLineText(GameMapSender.numberOfRows.ToString());
                    foreach (bool mine in GameMapSender.containsMine)
                    {
                        if (mine)
                        {
                            writeSpaceText("1");
                        }
                        else
                        {
                            writeSpaceText("0");
                        }
                    }
                    writeNewLineText("");
                    foreach (int mine in GameMapSender.stateOfMineSpace)
                    {
                        writeSpaceText(mine.ToString());
                    }
                    writeNewLineText("");

                    saveGameForm.Close();
                }
            }
            catch (Exception)
            {
                MessageBox.Show("Error encountered while trying to save the file!");
            }

        }
        private void writeNewLineText(string text)
        {
            System.IO.File.AppendAllText(Application.StartupPath + "\\" + saveString + ".txt", text + Environment.NewLine);

        }
        private void writeSpaceText(string text)
        {
            System.IO.File.AppendAllText(Application.StartupPath + "\\" + saveString + ".txt", text + " ");

        }
        private void PopulateSaveList()
        {
            string path = Application.StartupPath;
            int substringIndex = 0;

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
                    savedGamesList.Items.Add(file.Substring(substringIndex).Substring(0,file.Length - substringIndex - 4));
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
                SaveFile();
            }
        }
    }
}
