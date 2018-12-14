using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Threading;

namespace Sudoku
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            CheckForIllegalCrossThreadCalls = false;
        }

        Thread th = new Thread(t => { });
        int time_start = 0;
        int[,] grille = new int[9, 9];
        int[,] grille_solve = new int[9, 9];

        private void bt_solve_Click(object sender, EventArgs e)
        {
            if (th.IsAlive)
            {
                timer.Stop();
                th.Abort();
                bt_solve.Text = "Résoudre";
                return;
            }
            save(grille);
            save(grille_solve);
            time_start = Environment.TickCount;
            timer.Start();
            lb_init.Text = "0";
            bt_solve.Text = "Annuler";
            lb_attemps.Text = "1";
            th = new Thread(t =>
            {
                bool smart = false;
                while (!check_solved())
                {
                    int empty_tb = nb_empty_tb();
                    for (int i = 1; i <= 9; i++)
                        for (int j = 1; j <= 9; j++)
                        {
                            lb_temoin.Text = "Résolution de la grille...";
                            if (GetTbByName(i, j) == null)
                                continue;
                            if (GetTbByName(i, j).Text != "")
                                continue;
                            List<int> nb_next = new List<int>();

                            int k = i;
                            int l = j;
                            while (k > 3) k -= 3;
                            while (l > 3) l -= 3;
                            for (int m = k; m <= 9; m += 3)
                                for (int n = l; n <= 9; n += 3)
                                    if (GetTbByName(m, n) != null)
                                        if (GetTbByName(m, n).Text != "")
                                            nb_next.Add(Convert.ToInt32(GetTbByName(m, n).Text));
                            int o = i;
                            int p = j;
                            if (o <= 3) o = 1;
                            else if (o <= 6) o = 4;
                            else o = 7;
                            if (p <= 3) p = 1;
                            else if (p <= 6) p = 4;
                            else p = 7;
                            //while (o != 1 || o != 4 || o != 7) o -= 1;
                            //while (p != 1 || p != 4 || p != 7) p -= 1;
                            for (int q = o; q < o + 3; q++)
                                for (int r = p; r < p + 3; r++)
                                    if (GetTbByName(q, r) != null)
                                        if (GetTbByName(q, r).Text != "")
                                            nb_next.Add(Convert.ToInt32(GetTbByName(q, r).Text));
                            int s = i;
                            for (int u = 1; u <= 9; u++)
                                if (GetTbByName(s, u) != null)
                                    if (GetTbByName(s, u).Text != "")
                                        nb_next.Add(Convert.ToInt32(GetTbByName(s, u).Text));

                            List<int> nb_all = new List<int>(new int[] { 1, 2, 3, 4, 5, 6, 7, 8, 9 });
                            foreach (int nb in new int[] { 1, 2, 3, 4, 5, 6, 7, 8, 9 })
                                if (nb_next.Contains(nb))
                                    nb_all.Remove(nb);
                            if (nb_all.Count == 1)
                                GetTbByName(i, j).Text = nb_all.First().ToString();
                            else if (nb_all.Count == 2 && smart)
                            {
                                Random rnd = new Random();
                                GetTbByName(i, j).Text = nb_all[rnd.Next(2)].ToString();
                                smart = false;
                            }
                        }
                    if (smart)
                    {
                        lb_init.Text = "0";
                        smart = false;
                        lb_attemps.Text = (Convert.ToInt32(lb_attemps.Text) + 1).ToString();
                        load(grille_solve);
                        //timer.Stop();
                        /*DialogResult dr = MessageBox.Show(this, "La résolution de cette grille à échouée, réessayer ?", "Erreur", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                        if (dr == DialogResult.Yes)
                        {
                            smart = false;
                            lb_init.Text = "0";
                            lb_time.Text = "0,000";
                            load();
                            time_start = Environment.TickCount;
                            timer.Start();
                            continue;
                        }
                        else*/
                        /*{
                            MessageBox.Show("La résolution de cette grille à échouée !", "Echec", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            lb_temoin.Text = "Echec";
                            bt_solve.Text = "Résoudre";
                            return;
                        }*/
                    }
                    if (nb_empty_tb() == empty_tb)
                    {
                        if (lb_init.Text == "0")
                            save(grille_solve);
                        lb_init.Text = (Convert.ToInt32(lb_init.Text) + 1).ToString();
                        smart = true;
                    }
                }
                timer.Stop();
                lb_temoin.Text = "Grille réussie !";
                bt_solve.Text = "Résoudre";
                MessageBox.Show(this, "Sudoku terminé !", "Grille remplie", MessageBoxButtons.OK, MessageBoxIcon.Information);
            });
            th.Start();
        }

        private bool check_solved()
        {
            if (tb_11.Text != "" && tb_12.Text != "" && tb_13.Text != "" && tb_14.Text != "" && tb_15.Text != "" && tb_16.Text != "" && tb_17.Text != "" && tb_18.Text != "" && tb_19.Text != "" &&
                tb_21.Text != "" && tb_22.Text != "" && tb_23.Text != "" && tb_24.Text != "" && tb_25.Text != "" && tb_26.Text != "" && tb_27.Text != "" && tb_28.Text != "" && tb_29.Text != "" &&
                tb_31.Text != "" && tb_32.Text != "" && tb_33.Text != "" && tb_34.Text != "" && tb_35.Text != "" && tb_36.Text != "" && tb_37.Text != "" && tb_38.Text != "" && tb_39.Text != "" &&
                tb_41.Text != "" && tb_42.Text != "" && tb_43.Text != "" && tb_44.Text != "" && tb_45.Text != "" && tb_46.Text != "" && tb_47.Text != "" && tb_48.Text != "" && tb_49.Text != "" &&
                tb_51.Text != "" && tb_52.Text != "" && tb_53.Text != "" && tb_54.Text != "" && tb_55.Text != "" && tb_56.Text != "" && tb_57.Text != "" && tb_58.Text != "" && tb_59.Text != "" &&
                tb_61.Text != "" && tb_62.Text != "" && tb_63.Text != "" && tb_64.Text != "" && tb_65.Text != "" && tb_66.Text != "" && tb_67.Text != "" && tb_68.Text != "" && tb_69.Text != "" &&
                tb_71.Text != "" && tb_72.Text != "" && tb_73.Text != "" && tb_74.Text != "" && tb_75.Text != "" && tb_76.Text != "" && tb_77.Text != "" && tb_78.Text != "" && tb_79.Text != "" &&
                tb_81.Text != "" && tb_82.Text != "" && tb_83.Text != "" && tb_84.Text != "" && tb_85.Text != "" && tb_86.Text != "" && tb_87.Text != "" && tb_88.Text != "" && tb_89.Text != "" &&
                tb_91.Text != "" && tb_92.Text != "" && tb_93.Text != "" && tb_94.Text != "" && tb_95.Text != "" && tb_96.Text != "" && tb_97.Text != "" && tb_98.Text != "" && tb_99.Text != "")
                return true;
            else
                return false;
        }

        private TextBox GetTbByName(int bc, int sc)
        {
            string name = "tb_" + bc + sc;
            foreach (Control control in gb_grille.Controls)
                if (control.Name == name)
                    return (TextBox)control;
            return null;
        }

        private void bt_reset_Click(object sender, EventArgs e)
        {
            if (th.IsAlive)
                th.Abort();
            timer.Stop();
            bt_solve.Text = "Résoudre";
            lb_time.Text = "0,000";
            lb_temoin.Text = "En attente";
            lb_init.Text = "0";
            lb_attemps.Text = "0";
            for (int i = 1; i <= 9; i++)
                for (int j = 1; j <= 9; j++)
                    if (GetTbByName(i, j) != null)
                        GetTbByName(i, j).Text = "";
        }

        private int nb_empty_tb()
        {
            int nb = 0;
            for (int i = 1; i <= 9; i++)
                for (int j = 1; j <= 9; j++)
                    if (GetTbByName(i, j) != null)
                        if (GetTbByName(i, j).Text == "")
                            nb++;
            return nb;
        }

        private void timer_Tick(object sender, EventArgs e)
        {
            decimal t = (Environment.TickCount - time_start) / 1000m;
            lb_time.Text = t.ToString("0.000");
        }

        private void save(int[,] gr)
        {
            for (int i = 1; i <= 9; i++)
                for (int j = 1; j <= 9; j++)
                    if (GetTbByName(i, j) != null)
                        if (GetTbByName(i, j).Text == "")
                            gr[i - 1, j - 1] = 0;
                        else
                            gr[i - 1, j - 1] = Convert.ToInt32(GetTbByName(i, j).Text);
        }

        private void load(int[,] gr)
        {
            for (int i = 1; i <= 9; i++)
                for (int j = 1; j <= 9; j++)
                    if (GetTbByName(i, j) != null)
                        if (gr[i - 1, j - 1] == 0)
                            GetTbByName(i, j).Text = "";
                        else
                            GetTbByName(i, j).Text = gr[i - 1, j - 1].ToString();
        }

        private void bt_reset_Click_1(object sender, EventArgs e)
        {
            if (th.IsAlive)
                th.Abort();
            timer.Stop();
            bt_solve.Text = "Résoudre";
            lb_init.Text = "0";
            lb_time.Text = "0,000";
            lb_temoin.Text = "En attente";
            lb_attemps.Text = "0";
            load(grille);
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (th.IsAlive)
                th.Abort();
        }

        private void tb_TextChanged(object sender, EventArgs e)
        {
            int tb = 0;
            for (int i = 1; i <= 9; i++)
                for (int j = 1; j <= 9; j++)
                    if (GetTbByName(i, j) != null)
                        if (GetTbByName(i, j).Focused)
                            if (!int.TryParse(GetTbByName(i, j).Text, out tb))
                                GetTbByName(i, j).Text = "";
        }
    }
}
