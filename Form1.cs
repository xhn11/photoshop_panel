using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net;
using System.Net.Sockets;
using System.Xml;
using ps = Photoshop;


namespace photoshop_panel
{
    public partial class Form1 : Form
    {

        //global var
        string[] names;
        string[] export_ui;
        //
        
        //Start Photoshop Functions

        Photoshop.Application app = new Photoshop.Application();

        private void Isolate_Selection()
        {
            var idShw = app.CharIDToTypeID("Shw ");
            var desc68 = new ps.ActionDescriptor();
            var idnull = app.CharIDToTypeID("null");
            var list21 = new ps.ActionList();
            var ref49 = new Photoshop.ActionReference();
            var idLyr = app.CharIDToTypeID("Lyr ");
            var idOrdn = app.CharIDToTypeID("Ordn");
            var idTrgt = app.CharIDToTypeID("Trgt");
            ref49.PutEnumerated(idLyr, idOrdn, idTrgt);
            list21.PutReference(ref49);
            desc68.PutList(idnull, list21);
            var idTglO = app.CharIDToTypeID("TglO");
            desc68.PutBoolean(idTglO, true);
            app.ExecuteAction(idShw, desc68, ps.PsDialogModes.psDisplayNoDialogs);
        }

        private void Copy_merge()
        {
            var idCpyM = app.CharIDToTypeID("CpyM");
            app.ExecuteAction(idCpyM, null, ps.PsDialogModes.psDisplayNoDialogs);
        }

        private void Copy()
        {
            var idCpyM = app.CharIDToTypeID("Copy");
            app.ExecuteAction(idCpyM, null, ps.PsDialogModes.psDisplayNoDialogs);
        }

        private int Get_Layers_Count()
        {
            Photoshop.ActionReference act = new Photoshop.ActionReference();
            act.PutEnumerated(app.CharIDToTypeID("Dcmn"), app.CharIDToTypeID("Ordn"), app.CharIDToTypeID("Trgt"));
            var desc = app.ExecuteActionGet(act);
            return desc.GetInteger(app.CharIDToTypeID("NmbL"));
        }

        private string Get_Active_History()
        {
            return app.ActiveDocument.ActiveHistoryState.Name;
        }

        private string Get_Layer_Name(int index)
        {
            Photoshop.ActionReference act = new Photoshop.ActionReference();
            act.PutIndex(app.CharIDToTypeID("Lyr "), index+1);
            return app.ExecuteActionGet(act).GetString(app.CharIDToTypeID("Nm  "));
        }

        private void Refresh_Layers_list()
        {
            int layers_count = Get_Layers_Count();

            names = new string[Get_Layers_Count()];

            progressBar1.Maximum = Get_Layers_Count()-1;
            progressBar1.Step = 1;
            progressBar1.Value = 0;

            for (int i = 0; i < layers_count - 1; i++)
            {
                names[i] = Get_Layer_Name(i);
                progressBar1.PerformStep();
            };
        }

        private void Set_for_Export(string name, string target)
        {

        }
        //End Photoshop Functions

        public int Index_By_Name(string name)
        {
            for (int i = 0; i < names.Length +1; i++)
            {
                if (names[i] == name)
                {
                    return i;
                }
            }
            return 0;
        }

        public Form1()
        {
            InitializeComponent();
            
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            
        }
   
        private void button1_Click(object sender, EventArgs e)
        {
            Refresh_Layers_list();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            TopMost = true;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            XmlReader reader = XmlReader.Create("UltimateTextureSaver.xml");

            reader.Read();

            string text = "";

            while (reader.Read())
            {
                int i = 1;
                //MessageBox.Show(reader);
                if(reader.HasAttributes)
                {
                    if (reader.Name == "Texture")
                    {
                        reader.MoveToAttribute(0);
                        Button button_export = new Button();

                        button_export.Name = "button_export" + reader.Name;
                        button_export.Text = reader.Value;
                        flowLayoutPanel1.Controls.Add(button_export);

                    }


                    //reader.MoveToAttribute(0);
                    // Do some work here on the data.
                    text = text + "<" + reader.Name + "//" + reader.NodeType +"--"+ reader.Value + ">";
                }
            }

            MessageBox.Show(text);
           

            /*
            foreach(var item in reader)
            {
                MessageBox.Show(item);
            }
             */
        }

        private void button4_Click(object sender, EventArgs e)
        {
            MessageBox.Show(Index_By_Name("ccc").ToString());

        }

    }
}
