using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Reflection;
using System.Diagnostics;

namespace Doom_Loader
{
    public partial class About : Form
    {
        public About()
        {
            InitializeComponent();
        }

        private void About_Load(object sender, EventArgs e)
        {
            version.Text = "Version " +
                $"{GetType().Assembly.GetName().Version.Major}." +
                $"{GetType().Assembly.GetName().Version.Minor}." +
                $"{GetType().Assembly.GetName().Version.Build}";
        }

        // Little Easter Egg
        private void EasterEgg(object sender, EventArgs e)
        {
            new extraCredits().ShowDialog();
        }
    }
}
