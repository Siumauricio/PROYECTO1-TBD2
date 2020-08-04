﻿using IBM.Data.DB2;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using IBM.Data.DB2.iSeries;
using System.Configuration;
using System.Diagnostics;
using System.IO;

namespace Proyecto1_TBD2.Conexion {
    public partial class IngresarConexion:Form {
        TreeView arbol;
        List<ContextMenuStrip> subMenus;
        TreeNode node1;
        TreeNode node2;
        TreeNode node3;
        TreeNode node4;
        TreeNode node5;
        TreeNode node6;
        public IngresarConexion(TreeView _arbol, List<ContextMenuStrip> _subMenus) {
            arbol = _arbol;
            subMenus = _subMenus;
            InitializeComponent();
        }

        private void IngresarConexion_Load(object sender, EventArgs e) {

        }

        private void button1_Click(object sender, EventArgs e) {
            DB2ConnectionStringBuilder cn = new DB2ConnectionStringBuilder();
            cn.UserID = usuario.Text;
            cn.Password = contrasena.Text;
            cn.Database = name_db.Text;
            cn.Server = server.Text;
            DB2Connection connect = new DB2Connection(cn.ToString());
            try {
                connect.Open();
                MessageBox.Show("Conexion Exitosa!\n" + "Version servidor: " + connect.ServerVersion + " Base de datos: " + connect.Database.ToString());
            } catch (DB2Exception error) {
                MessageBox.Show("A ocurrido un error!\n" + error.Message);
            }
            connect.Close();
        }

        private void button3_Click(object sender, EventArgs e) {
            CrearBD cd = new CrearBD();
            cd.Show();
            this.Hide();
        }

        private void button2_Click(object sender, EventArgs e) {
            DB2ConnectionStringBuilder cn = new DB2ConnectionStringBuilder();
            cn.UserID = usuario.Text;
            cn.Password = contrasena.Text;
            cn.Database = name_db.Text;
            cn.Server = server.Text;
            DB2Connection connect = new DB2Connection(cn.ToString());
            try {
                connect.Open();
                MessageBox.Show("Conexion Exitosa!\n" + "Version servidor: " + connect.ServerVersion + " Base de datos: " + connect.Database.ToString());
                DibujarConexion(cn.Database);
                extraerTablas(connect);
                extraerIndicies(connect);
                extrarProcedimientos(connect);
            } catch (DB2Exception error) {
                MessageBox.Show("A ocurrido un error!\n" + error.Message);
            }

            connect.Close();
            this.Hide();
        }

        public void extraerTablas(DB2Connection connect) {
            DB2Command cmd = new DB2Command("SELECT NAME FROM SYSIBM.SYSTABLES WHERE type = 'T' AND creator = '" + usuario.Text.ToUpper()+"';", connect);//OBTENER TABLAS DE LA BASE DE DATOS
            DB2DataReader bff = cmd.ExecuteReader();
            while (bff.Read()) {
                var Names = bff ["NAME"].ToString();
                TreeNode nodo = node1.Nodes.Add(Names.ToString());
                nodo.ImageIndex = 2;
                nodo.SelectedImageIndex = 2;
                nodo.ContextMenuStrip = subMenus [2];
            }
            bff.Close();
        }

        public void extraerIndicies(DB2Connection connect) {
            DB2Command cmd = new DB2Command("select * from syscat.indexes where tabschema = '" + usuario.Text.ToUpper() + "';", connect);//OBTENER TABLAS DE LA BASE DE DATOS
            DB2DataReader bff = cmd.ExecuteReader();
            while (bff.Read()) {
                var Names = bff ["indname"].ToString();
                TreeNode nodo = node2.Nodes.Add(Names.ToString());
                nodo.ImageIndex = 3;
                nodo.SelectedImageIndex = 3;
                nodo.ContextMenuStrip = subMenus [4];
            }
            bff.Close();
        }

        public void extrarProcedimientos(DB2Connection connect) {
            DB2Command cmd = new DB2Command("SELECT procname FROM syscat.procedures WHERE procschema = '" + usuario.Text.ToUpper() + "';", connect);//OBTENER TABLAS DE LA BASE DE DATOS
            DB2DataReader bff = cmd.ExecuteReader();
            DB2DataAdapter adapter = new DB2DataAdapter(cmd);

            while (bff.Read()) {
            var Names = bff ["PROCNAME"].ToString();
            TreeNode nodo = node4.Nodes.Add(Names.ToString());
              nodo.ImageIndex = 4;
              nodo.SelectedImageIndex =4;
              nodo.ContextMenuStrip = subMenus [6];
          }
            bff.Close();
        }

        public void DibujarConexion(string db) {
            TreeNode node0 = arbol.Nodes.Add(db);
            node0.ContextMenuStrip = subMenus [1];

            node1 = node0.Nodes.Add("Tablas");
            node1.ImageIndex = 1;
            node1.SelectedImageIndex = 1;
            node1.ContextMenuStrip = subMenus[0];

            node2 = node0.Nodes.Add("Indices");
            node2.ImageIndex = 1;
            node2.SelectedImageIndex = 1;
            node2.ContextMenuStrip = subMenus [3];

            node4 = node0.Nodes.Add("Procedimiento Almacenados");
            node4.ImageIndex = 1;
            node4.SelectedImageIndex = 1;
            node4.ContextMenuStrip = subMenus [5];

            node3 = node0.Nodes.Add("Vistas");
            node3.ImageIndex = 1;
            node3.SelectedImageIndex = 1;


            node5 = node0.Nodes.Add("Funciones");
            node5.ImageIndex = 1;
            node5.SelectedImageIndex = 1;

            node6 = node0.Nodes.Add("Triggers");
            node6.ImageIndex = 1;
            node6.SelectedImageIndex = 1;
        }
    }
}


//string comando = "\"C:\\Program Files\\IBM\\SQLLIB\\BIN\\DB2CMD.exe\" DB2SETCP.BAT DB2.EXE";
//string crear = "create database pepino";
////System.Diagnostics.Process.Start(comando, "CREATE");
//Process p = new Process();
//ProcessStartInfo info = new ProcessStartInfo();
//info.FileName = "cmd.exe";
//info.RedirectStandardInput = true;
//info.UseShellExecute = false;

//p.StartInfo = info;
//p.Start();

//using (StreamWriter sw = p.StandardInput) {
//    if (sw.BaseStream.CanWrite) {
//        sw.WriteLine(comando + " " + crear);
//        //  sw.WriteLine(crear);
//        //sw.WriteLine("mypassword");
//        ///sw.WriteLine("use mydb;");
//    }
//}