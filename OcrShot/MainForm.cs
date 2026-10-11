using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace OcrShot;

public partial class MainForm : Form
{
    private readonly OcrService orc = new();

    public MainForm()
    {
        InitializeComponent();
    }

    private void OnClickCapture(object sender, EventArgs e)
    {
        
    }
}
