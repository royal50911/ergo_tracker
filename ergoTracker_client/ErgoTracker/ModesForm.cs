using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ErgoTracker
{
    public partial class ModesForm : Form
    {
        public ModesForm()
        {
            InitializeComponent();

            if (ApplicationInformation.Instance.isTrainingModeOn()) this.checkBox1.Checked = true;
            else this.checkBox1.Checked = false;

            if (ApplicationInformation.Instance.isDiagnosticModeOn()) this.checkBox2.Checked = true;
            else this.checkBox2.Checked = false;
        }
    }
}
