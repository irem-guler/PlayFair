using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PlayFair
{
    public partial class Form1: Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }
        public string key()
        {
            string key = txtKey.Text;
            key = key.ToUpper().Replace("İ","I").Replace("J", "I").Replace(" ", "");
            string alphabet = "ABCDEFGHIKLMNOPQRSTUVWXYZ";
            StringBuilder sb = new StringBuilder();
            for(int i = 0; i < key.Length; i++)
            {
                char c = key[i];
                if (!alphabet.Contains(c))
                {
                    return "Invalid Key";
                }
                if(!sb.ToString().Contains(c))
                {
                    sb.Append(c);
                }
            }
            return sb.ToString();
        }
        public char[,] matrix(string key)
        {
            string alphabet = "ABCDEFGHIKLMNOPQRSTUVWXYZ";
            StringBuilder sb = new StringBuilder();
            sb.Append(key);
            char[,] matrix = new char[5, 5];
            int index = 0;
            foreach (char c in alphabet)
            {
                if (!sb.ToString().Contains(c))
                {
                    sb.Append(c);
                }
            }
            string newalphabet = sb.ToString();
            sb.Clear();
            for (int row = 0; row <5; row++)
            {
                for(int col = 0; col < 5; col++)
                {
                    matrix[row, col] = newalphabet[index];
                    index++;
                }
            }
            for (int row = 0; row < 5; row++)
            {
                for (int col = 0; col < 5; col++)
                {
                    sb.Append(matrix[row, col] + " ");
                }
                sb.AppendLine();
            }

            return matrix;
        }
        public string matrixtostring(char[,] matrix)
        {
            StringBuilder sb = new StringBuilder();

            for (int r = 0; r < 5; r++)
            {
                for (int c = 0; c < 5; c++)
                {
                    sb.Append(matrix[r, c] + " ");
                }
                sb.AppendLine();
            }

            return sb.ToString();
        }
        private void button1_Click(object sender, EventArgs e)
        {
            // 1) Key + matrix her iki modda da lazım
            string cleanKey = key();
            textBox1.Text = cleanKey;

            // Key geçersizse devam etme (yoksa matris bozulur)
            if (cleanKey == "Invalid Key" || string.IsNullOrEmpty(cleanKey))
            {
                MessageBox.Show("Key geçersiz. Sadece A-Z harf gir (J yok, J=I).");
                return;
            }

            char[,] mat = matrix(cleanKey);
            richTextBox1.Text = matrixtostring(mat);

            if (radioEncrypt.Checked)
            {
                string prepared = PrepareMessage(richTextBox2.Text);
                richTextBox3.Text = prepared;

                string encrypted = EncryptMessage(prepared, mat);
                richTextBox4.Text = encrypted;
            }
            else if (radioDecrypt.Checked)
            {

                string cipher = richTextBox4.Text;

                cipher = cipher.ToUpper().Replace(" ", "");
                if (cipher.Length % 2 == 1)
                {
                    MessageBox.Show("Şifreli metin uzunluğu tek olamaz. (Çift olmalı)");
                    return;
                }

                string plain = DecryptMessage(cipher, mat);
                plain = RemovePaddingX(plain);
                richTextBox2.Text = plain;

                richTextBox3.Clear();
            }

        }

        public string PrepareMessage(string message)
        {
            message = message.ToUpper().Replace("İ", "I").Replace("J", "I").Replace(" ", "");
            StringBuilder clean = new StringBuilder();
            foreach(char c in message)
            {
                if(c >= 'A' && c <= 'Z')
                {
                    clean.Append(c);
                }
            }
            StringBuilder result = new StringBuilder();
            int i = 0;
            while (i < clean.Length)
            {
                char first = clean[i];
                char second = (i + 1 < clean.Length) ? clean[i + 1] : 'X';
                if(first == second)
                {
                    result.Append(first);
                    result.Append('X');
                    i++;
                }
                if(first != second)
                {
                    result.Append(first);
                    result.Append(second);
                    i += 2;
                }
            }
            return result.ToString();
        }
        public void FindPos(char[,] mat, char ch, out int row, out int col)
        {
            if (ch == 'J') ch = 'I';

            for (int r = 0; r < 5; r++)
            {
                for (int c = 0; c < 5; c++)
                {
                    if (mat[r, c] == ch)
                    {
                        row = r;
                        col = c;
                        return;
                    }
                }
            }

            row = -1;
            col = -1;
        }
        public string EncryptPair(char[,] mat, char a, char b)
        {

            if (a == 'J') a = 'I';
            if (b == 'J') b = 'I';

            int r1, c1, r2, c2;
            FindPos(mat, a, out r1, out c1);
            FindPos(mat, b, out r2, out c2);


            if (r1 == r2)
            {
                c1 = (c1 + 1) % 5;
                c2 = (c2 + 1) % 5;
            }

            else if (c1 == c2)
            {
                r1 = (r1 + 1) % 5;
                r2 = (r2 + 1) % 5;
            }

            else
            {
                int temp = c1;
                c1 = c2;
                c2 = temp;
            }

            return $"{mat[r1, c1]}{mat[r2, c2]}";
        }
        public string EncryptMessage(string prepared, char[,] mat)
        {
            StringBuilder sb = new StringBuilder();

            for (int i = 0; i < prepared.Length; i += 2)
            {
                char a = prepared[i];
                char b = prepared[i + 1];

                sb.Append(EncryptPair(mat, a, b));
            }

            return sb.ToString();
        }
        public string DecryptPair(char[,] mat, char a, char b)
        {
            if (a == 'J') a = 'I';
            if (b == 'J') b = 'I';

            int r1, c1, r2, c2;
            FindPos(mat, a, out r1, out c1);
            FindPos(mat, b, out r2, out c2);

            if (r1 == r2)
            {
                c1 = (c1 + 4) % 5;   
                c2 = (c2 + 4) % 5;
            }

            else if (c1 == c2)
            {
                r1 = (r1 + 4) % 5;   
                r2 = (r2 + 4) % 5;
            }
            else
            {
                int temp = c1;
                c1 = c2;
                c2 = temp;
            }

            return $"{mat[r1, c1]}{mat[r2, c2]}";
        }
        public string DecryptMessage(string cipher, char[,] mat)
        {
            cipher = cipher.ToUpper().Replace("İ", "I").Replace("J", "I").Replace(" ", "");

            StringBuilder sb = new StringBuilder();

            for (int i = 0; i < cipher.Length; i += 2)
            {
                char a = cipher[i];
                char b = cipher[i + 1];
                sb.Append(DecryptPair(mat, a, b));
            }

            return sb.ToString();
        }
        public string RemovePaddingX(string text)
        {
            if (string.IsNullOrEmpty(text))
                return text;

            text = text.ToUpper().Replace("İ", "I");

            StringBuilder sb = new StringBuilder();

            for (int i = 0; i < text.Length; i++)
            {
                char current = text[i];

                if (current == 'X' && i > 0 && i < text.Length - 1)
                {
                    char left = text[i - 1];
                    char right = text[i + 1];

                    if (left == right)
                    {
                        continue;
                    }
                }

                sb.Append(current);
            }

            if (sb.Length > 0 && sb[sb.Length - 1] == 'X')
            {
                sb.Remove(sb.Length - 1, 1);
            }

            return sb.ToString();
        }


    }

}
