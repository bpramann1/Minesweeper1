﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Minesweeper
{
    /// <summary>
    /// Manages a file menu for the player 
    /// </summary>
    class GameFilesDialog
    {

        public Form GameFilesForm;
        private ListBox savedGamesList;
        private TextBox selectedGameTextbox;
        private Label saveGamesLabel;
        private Label saveGameLabel;
        private Button saveGameButton;
        private string saveString;
        private bool dialogDone = false;
        private GameMap GameMapSender;
        private SaveTypeDialog senderObject;
        private string callType;

        /// <summary>
        /// Contains a value that dictates what actions need to be taken after the dialog is closed 
        /// </summary>
        public enum ActionsAfterDialog
        {
            Nothing,
            Restart,
            Exit
        }
        private ActionsAfterDialog actionAfterDialog;

        /// <summary>
        /// Constructs a new files dialog 
        /// </summary>
        /// <param name="ActionAfterDialog">
        /// An enum that dictates the action to be taken after the dialog is over 
        /// </param>
        /// <param name="gameMapSender">
        /// The game map that called the dialog 
        /// </param>
        /// <param name="sender">
        /// The type of action to be taken (save, load, or delete)
        /// </param>
        /// <param name="finishString">
        /// The string to be displayed at the end of the game 
        /// </param>
        /// <param name="buttonText">
        /// The text to be displayed by the button 
        /// </param>
        public GameFilesDialog(ActionsAfterDialog ActionAfterDialog, GameMap gameMapSender, SaveTypeDialog sender, string finishString, string buttonText)
        {
            callType = finishString;
            GameMapSender = gameMapSender;
            senderObject = sender;
            actionAfterDialog = ActionAfterDialog;
            CreateDialog();
            saveGameButton.Text = buttonText;
        }

        /// <summary>
        /// Gets the string to be displayed when saved 
        /// </summary>
        /// <returns>
        /// The string to be displayed when saved 
        /// </returns>
        public string getSaveString()
        {
            return saveString;
        }

        /// <summary>
        /// Sets the dialogue to either be done or not done 
        /// </summary>
        /// <param name="value">
        /// The value to set dialogDone to 
        /// </param>
        public void setDialogDone(bool value)
        {
            dialogDone = value;
        }

        /// <summary>
        /// Creates a new dialog 
        /// </summary>
        private void CreateDialog()
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

        /// <summary>
        /// Performs the neccessary actions to be done after the dialog is closed. 
        /// </summary>
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

        /// <summary>
        /// Makes sure the user actually wants to exit if the game is still going and then closes the game. 
        /// </summary>
        /// <param name="sender">
        /// The object that called this function
        /// </param>
        /// <param name="e">
        /// Contains information about the closing event 
        /// </param>
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

        /// <summary>
        /// Varifies that the user has entered a correct name for the save file and then saves the name of it  
        /// </summary>
        /// <param name="sender">
        /// The object that called the function
        /// </param>
        /// <param name="e">
        /// Contains information about the event 
        /// </param>
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

        /// <summary>
        /// Determines if letter is a letter 
        /// </summary>
        /// <param name="letter">
        /// The char to be tested for being a letter 
        /// </param>
        /// <returns>
        /// True if letter is a letter, false otherwise 
        /// </returns>
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

        /// <summary>
        /// Populates the save list 
        /// </summary>
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

        /// <summary>
        /// Populates the selectedGameTextbox with the name of the game to open 
        /// </summary>
        /// <param name="sender">
        /// The object that called this function
        /// </param>
        /// <param name="e">
        /// Contains information about the event that caused this function to be called 
        /// </param>
        private void SaveListItemSingleClicked(object sender, MouseEventArgs e)
        {
            if (savedGamesList.SelectedItem != null)
            {
                selectedGameTextbox.Text = savedGamesList.SelectedItem.ToString();
            }

        }

        /// <summary>
        /// Prompts the user to make sure they want to overwrite a previous save and then overwrites the save they double clicked 
        /// </summary>
        /// <param name="sender">
        /// Contains information about the object that called this function
        /// </param>
        /// <param name="e">
        /// Contains information about the event that caused this function to be called 
        /// </param>
        private void SaveListItemDoubleClicked(object sender, MouseEventArgs e)
        {
            if (savedGamesList.SelectedItem != null)
            {
                selectedGameTextbox.Text = savedGamesList.SelectedItem.ToString();
                saveString = selectedGameTextbox.Text;
                senderObject.ButtonClicked();
            }
        }

        /// <summary>
        /// Determines whether the inputted name for the save already exists 
        /// </summary>
        /// <returns>
        /// True if the name already exists, false otherwise
        /// </returns>
        public bool saveStringAlreadyExists()
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
