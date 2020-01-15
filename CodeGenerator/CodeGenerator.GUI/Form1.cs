﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Exceptions;
using CodeGenerator.Controller;
using System.IO;

namespace CodeGenerator.GUI
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        #region Clicks
        /// <summary>
        /// Wenn der SelectFileButton geklickt wird, wird die openFileDialog-Komponente aufgerufen, 
        /// um die Modellierdatei auszuwählen. Wenn ein Dateipfad ausgewählt wurde, wird 
        /// dieser in das Path_Model-Label geschrieben.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SelectFileButton_Click(object sender, EventArgs e)
        {
            if(openFileDialogFile.ShowDialog()==DialogResult.OK)
            {
                PathModelLabel.Text = openFileDialogFile.FileName;
            }
        }

        /// <summary>
        /// Wenn der SelectOutputButton geklickt wird, wird die folderBrowserDialog-Komponente 
        /// aufgerufen, um den Ausgabeort auszuwählen. Wenn ein Ausgabeort ausgewählt wurde, wird 
        /// dieser in das Path_Output-Label geschrieben.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SelectOutputButton_Click(object sender, EventArgs e)
        {
            if(folderBrowserDialogOutput.ShowDialog() == DialogResult.OK)
            {
                PathOutputLabel.Text = folderBrowserDialogOutput.SelectedPath;
            }
        }

        /// <summary>
        /// Wenn der GenerateButton geklickt wird, werden die beiden ausgewählten Pfade in
        /// string Variablen gespeichert, geprüft ob diese ausgewählt wurden und CreateController() aufgerufen.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void GenerateButton_Click(object sender, EventArgs e)
        {
            string filePath_Model = PathModelLabel.Text;
            string filePath_Output = PathOutputLabel.Text;
            string noModel = "Keine Datei ausgewählt!";
            string noOutput = "Keinen Ausgabeort ausgewählt!";

            // Wenn Datei nicht ausgewählt und Ausgabeort ausgewählt wurde, wird 
            // FilePictureBox rot und der ErrorProvider ausgelöst.
            if (filePath_Model == noModel && filePath_Output != noOutput)
            {
                errorProvider1.SetError(FilePictureBox,"Bitte wählen Sie eine \".graphml\"- Datei!");
                FilePictureBox.BackColor = Color.Red;
            }

            // Wenn Datei ausgewählt und Ausgabeort nicht ausgewählt wurde, wird
            // OutputPictureBox rot und der ErrorProvider ausgelöst
            else if (filePath_Output == noOutput && filePath_Model != noModel)
            {
                errorProvider1.SetError(OutputPictureBox, "Bitte wählen Sie einen Ausgabeort!");
                OutputPictureBox.BackColor = Color.Red;
            }

            // Wenn beide nicht ausgewählt wurden, werden bei beiden PictureBoxes rot und ErrorProvider ausgelöst.
            else if (filePath_Output == noOutput && filePath_Model == noModel)
            {
                errorProvider1.SetError(FilePictureBox, "Bitte wählen Sie eine \".graphml\"- Datei!");
                errorProvider1.SetError(OutputPictureBox, "Bitte wählen Sie einen Ausgabeort!");
                OutputPictureBox.BackColor = Color.Red;
                FilePictureBox.BackColor = Color.Red;
            }

            // Letzte Möglichkeit: Beide ausgewählt. Im Status Label wird der Text ersetzt und
            // CreateController wird ausgeführt.
            else
            {
                toolStripStatusLabel1.Text = "Dateien werden erstellt...";
                CreateController(filePath_Model, filePath_Output);
            }
                
        }

        /// <summary>
        /// Wenn der Hilfe Anzeigen Button im Menu-Strip angeklickt wird, wird automatisch die Readme-Datei geöffnet.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void HilfeAnzeigenLassenToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string currentDir = Environment.CurrentDirectory;
            int index = currentDir.IndexOf(@"\CodeGenerator");
            string FilePath = currentDir.Substring(0, index) + @"\README.md";
            System.IO.File.OpenRead(FilePath);
        }
        #endregion

        /// <summary>
        /// Erstellt neues Controller-Objekt und ruft dessen StartProcess-Methode auf.
        /// Wenn die Methode eine Exception zurückgibt, wird CreateNewErrorForm() aufgerufen.
        /// </summary>
        /// <param name="filePath_Model">Dateipfad im Typ string</param>
        /// <param name="filePath_Output">Ausgabepfad im Typ string</param>
        public void CreateController(string filePath_Model, string filePath_Output)
        {
            bool finish;
            Controller.Controller controller = new Controller.Controller();
            Exception ex = controller.StartProcess(filePath_Model, filePath_Output, out finish);
            if (ex != null)
            {
                new Form2(ex).ShowDialog();
                this.Show();
            }
            if (finish)
                WritePrewiev(filePath_Output);
        }
        
        #region MouseHover
        /// <summary>
        /// Wenn die Maus über den SelectFileButton geht, wird im StatusLabel der Text angezeigt.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SelectFileButton_MouseHover(object sender, EventArgs e)
        {
            toolStripStatusLabel1.Text = "Klicken Sie hier, um ein Diagramm, aus ihrem lokalen Speicher hochzuladen.";
        }

        /// <summary>
        /// Wenn die Maus über den SelectOutputButton geht, wird im StatusLabel der Text angezeigt.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SelectOutputButton_MouseHover(object sender, EventArgs e)
        {
            toolStripStatusLabel1.Text = "Klicken Sie hier, um einen Speicherort, der zu generierenden Dateien auszuwählen.";
        }

        /// <summary>
        /// Wenn die Maus über das PathModelLabel geht und eine Datei ausgewählt wurde, 
        /// wird im StatusLabel der Pafad angezeigt.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void PathModelLabel_MouseHover(object sender, EventArgs e)
        {
            if (PathModelLabel.Text != "Keine Datei ausgewählt!")
                toolStripStatusLabel1.Text = PathModelLabel.Text;
        }

        /// <summary>
        /// Wenn die Maus über das PathOutputLabel geht und ein Ausgabeort ausgewählt wurde, 
        /// wird im StatusLabel der Pfad angezeigt.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void PathOutputLabel_MouseHover(object sender, EventArgs e)
        {
            if (PathOutputLabel.Text != "Keinen Ausgabeort ausgewählt!")
                toolStripStatusLabel1.Text = PathOutputLabel.Text;
        }

        /// <summary>
        /// Wenn die Maus über den GenerateButton geht, wird im StatusLabel der Text angezeigt.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void GenerateButton_MouseHover(object sender, EventArgs e)
        {
            toolStripStatusLabel1.Text = "Klicken Sie hier, um aus dem Diagramm den Code generieren zu lassen.";
        }

        /// <summary>
        /// Wenn die Maus über das HilfeStripMenu geht, wird im StatusLabel der Text angezeigt.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void HilfeToolStripMenuItem_MouseHover(object sender, EventArgs e)
        {
            toolStripStatusLabel1.Text = "Sie benötigen Hilfe? Wir helfen gern!";
        }

        #endregion

        #region MouseLeave
        /// <summary>
        /// Wenn die Maus den SelectFileButton verlässt, wird der Text im StatusLabel gelöscht.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SelectFileButton_MouseLeave(object sender, EventArgs e)
        {
            toolStripStatusLabel1.Text = "";
        }

        /// <summary>
        /// Wenn die Maus den SelectOutputButton verlässt, wird der Text im StatusLabel gelöscht.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SelectOutputButton_MouseLeave(object sender, EventArgs e)
        {
            toolStripStatusLabel1.Text = "";
        }

        /// <summary>
        /// Wenn die Maus das PathModelLabel verlässt, wird der Text im StatusLabel gelöscht.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void PathModelLabel_MouseLeave(object sender, EventArgs e)
        {
            toolStripStatusLabel1.Text = "";
        }

        /// <summary>
        /// Wenn die Maus das PathOutputLabel verlässt, wird der Text im StatusLabel gelöscht.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void PathOutputLabel_MouseLeave(object sender, EventArgs e)
        {
            toolStripStatusLabel1.Text = "";
        }

        /// <summary>
        /// Wenn die Maus den GeneratButton verlässt, wird der Text im StatusLabel gelöscht.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void GenerateButton_MouseLeave(object sender, EventArgs e)
        {
            toolStripStatusLabel1.Text = "";
        }

        /// <summary>
        /// Wenn die Maus das HilfeStripMenu verlässt, wird der Text im StatusLabel gelöscht.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void HilfeToolStripMenuItem_MouseLeave(object sender, EventArgs e)
        {
            toolStripStatusLabel1.Text = "";
        }
        #endregion

        #region TextChanged
        /// <summary>
        /// Wenn sich der Text im PathModelLabel ändert wird der ErrorProvider und die PictureBox zurückgesetzt.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void PathModelLabel_TextChanged(object sender, EventArgs e)
        {
            errorProvider1.SetError(FilePictureBox, null);
            FilePictureBox.BackColor = DefaultBackColor;

        }

        /// <summary>
        /// /// Wenn sich der Text im PathOutputLabel ändert wird der ErrorProvider und die PictureBox zurückgesetzt.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void PathOutputLabel_TextChanged(object sender, EventArgs e)
        {
            errorProvider1.SetError(OutputPictureBox, null);
            OutputPictureBox.BackColor = DefaultBackColor;
        }
        #endregion

        public bool WritePrewiev(string FilePath_Output)
        {
            try
            {

            }
            catch(Exception)
            {
                new Form2(new GeneralException()).ShowDialog();
                this.Show();
            }
            return true;
        }
    }
}
