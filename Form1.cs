namespace SystemModeling5
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        // ��� ����� ����� �� �����
        private static double TimeIncomeA; // ����� ����������� ����� ������ �
        private static double TimeIncomeB; // ����� ����������� ����� ������ �
        private static double TimeIncomeC; // ����� ����������� ����� ������ �
        private static double TimeSolvingA; // ����� ������� ����� ������ �
        private static double TimeSolvingB; // ����� ������� ����� ������ �
        private static double TimeSolvingC; // ����� ������� ����� ������ �
        private static double BufferA; // ������ ����� ������ �
        private static double BufferB; // ������ ����� ������ �
        private static double BufferC; // ������ ����� ������ �
        private static double GeneralBuffer; // ����� ������
        private static double t; // ����� �������������
        private static double MaxQueue; // ������������ ������� ������� �������

        // ��� ����� ��� �������������
        private static int MinutesInDay = 24 * 60; // ������� � ������
        private static double Delay; // �������� �������������
        private static int QueueA; // ������� � � ������� 
        private static int QueueB; // ������� � � �������
        private static int QueueC; // ������� � � �������
        private static double Time; // ����������� ������ �����
        private static string[] Protocol; // ��������
        private static int SolvedA; // ������ ������� �
        private static int SolvedB; // ������ ������� �
        private static int SolvedC; // ������ ������� �
        private static int DeclinedA; // �������� ������� �
        private static int DeclinedB; // �������� ������� �
        private static int DeclinedC; // �������� ������� �

        // ���������� ������
        private static double Productivity; // ������������������ ���
        private static int Cost; // ��������� ������ � ���

        // ��������������� ����������
        private static double IsSolvingTimeA = 0; // �������� ������� ������ ������ � 
        private static double IsSolvingTimeB = 0; // �������� ������� ������ ������ �
        private static double IsSolvingTimeC = 0; // �������� ������� ������ ������ �
        private static double BufferAtTheMoment = 0; // ���������� ����� � ������� � ������� ������ �������
        private static int CostA;
        private static int CostB;
        private static int CostC;

        // ����� �������������
        private static bool isRunning = false; // ���������� ������������� ��� ���
        private static bool isSolvingA = false; // �������� �� ������ �
        private static bool isSolvingB = false; // �������� �� ������ �
        private static bool isSolvingC = false; // �������� �� ������ �

        private void Form1_Load(object sender, EventArgs e)
        {
            textBox1.Text = "20";
            textBox2.Text = "20";
            textBox3.Text = "30";
            textBox4.Text = "16";
            textBox5.Text = "21";
            textBox6.Text = "28";
            textBox7.Text = "200";
            textBox8.Text = "260";
            textBox9.Text = "400";
            textBox10.Text = "640";
            textBox11.Text = "1"; // 0,5
            textBox12.Text = "15";

            // ��������� ������
            textBox16.Text = "50";
            textBox17.Text = "60";
            textBox18.Text = "80";

            // �������������� PictureBox -- ���� �������������� ����� ������ �� ������
            pictureBox1.Image = Properties.Resources.NoWorkingCat;
            pictureBox2.Image = Properties.Resources.NoWorkingCat;
            pictureBox3.Image = Properties.Resources.NoWorkingCat;
        }

        private int FindMaxQueue(int Queue1, int Queue2, int Queue3)
        {
            int Max = Queue1;
            int Class = 1;

            if (Queue2 > Max)
            {
                Max = Queue2;
                Class = 2;
            }

            if (Queue3 > Max)
            {
                Max = Queue3;
                Class = 3;
            }

            if (Queue1 == Queue2 && Queue1 == Queue3)
            {
                if (Queue1 == 0)
                {
                    Class = -1;
                }
                else
                {
                    Class = 0;
                }
            }

            return Class;

        }
        async private void Modeling()
        {
       

            // ���������� ����������� ��������
            TimeIncomeA = Convert.ToDouble(textBox1.Text);
            TimeIncomeB = Convert.ToDouble(textBox2.Text);
            TimeIncomeC = Convert.ToDouble(textBox3.Text);
            TimeSolvingA = Convert.ToDouble(textBox4.Text);
            TimeSolvingB = Convert.ToDouble(textBox5.Text);
            TimeSolvingC = Convert.ToDouble(textBox6.Text);
            BufferA = Convert.ToDouble(textBox7.Text);
            BufferB = Convert.ToDouble(textBox8.Text);
            BufferC = Convert.ToDouble(textBox9.Text);
            GeneralBuffer = Convert.ToDouble(textBox10.Text);
            t = Convert.ToDouble(textBox11.Text);
            MaxQueue = Convert.ToDouble(textBox12.Text);

            // ��������� ���������
            CostA = Convert.ToInt32(textBox16.Text);
            CostB = Convert.ToInt32(textBox17.Text);
            CostC = Convert.ToInt32(textBox18.Text);

            BufferAtTheMoment = GeneralBuffer; // ������������� ������� � ������� ������ �������

            // ������� ���������� ��� �������� ���������� ������� �������������
            double localTime1 = TimeIncomeA; // ��������� ����� ������� �
            double localTime2 = TimeIncomeB; // ��������� ����� ������� �
            double localTime3 = TimeIncomeC; // ��������� ����� ������� �

            // ������ � ������
            Delay = Convert.ToInt32(trackBar1.Value); // ������� �������� ��������
            progressBar1.Value = 0; // ������ �������� ������ �������� �� ������� (�.�. �� �����)
            progressBar1.Maximum = MinutesInDay * 10 + 10; // ��� ������������� �������� ������ ��������

            int iterationCounter = 0; // ���������� �������� � �������������

            // ���� ���������� �������: �� ���� ����� �� ����� ����� � ������, ��� ������� ����������� �������������.� ������ �������� �����
            // ������������� �� ��������, ���������� �� ����� (����� ��������� 0.5 = ��� ����� 30 ������)

            for (; Time <= MinutesInDay && isRunning; Time += t)
            {
                iterationCounter++;

                Protocol = new string[13] { "", "", "", "", "", "", "", "", "", "", "", "", "" }; // ������������� ���������� ���������
                
                // ������ ������� ������ �������
                // ���� ������ ������� ������ �
                if (Time - localTime1 >= 0)
                {
                    // ���� ���������� ������������ �������
                    if (QueueA == MaxQueue)
                    {
                        DeclinedA += 1;
                    }
                    else
                    {
                        QueueA += 1;
                    }
                    localTime1 += TimeIncomeA;

                }

                // ���� ������ ������� ������ �
                if (Time - localTime2 >= 0)
                {
                    if (QueueB == MaxQueue)
                    {
                        DeclinedB += 1;
                    }
                    else
                    {
                        QueueB += 1;
                    }
                    localTime2 += TimeIncomeB;

                }

                // ���� ������ ������� ������ �
                if (Time - localTime3 >= 0)
                {
                    if (QueueC == MaxQueue)
                    {
                        DeclinedC += 1;
                    }
                    else
                    {
                        QueueC += 1;
                    }
                    localTime3 += TimeIncomeC;

                }

                // ��������, �� �������� �� �������
                if (isSolvingA)
                {
                    IsSolvingTimeA -= t;
                    // ���� ������ ��������� ��������
                    if (IsSolvingTimeA <= 0)
                    {
                        isSolvingA = false;
                        SolvedA += 1;
                        BufferAtTheMoment = GeneralBuffer;
                        pictureBox1.Image = Properties.Resources.NoWorkingCat;
                    }
                }

                if (isSolvingB)
                {
                    IsSolvingTimeB -= t;
                    // ���� ������ ��������� ��������
                    if (IsSolvingTimeB <= 0)
                    {
                        isSolvingB = false;
                        SolvedB += 1;
                        BufferAtTheMoment = GeneralBuffer;
                        pictureBox2.Image = Properties.Resources.NoWorkingCat;
                    }
                }

                if (isSolvingC)
                {
                    IsSolvingTimeC -= t;
                    // ���� ������ ��������� ��������
                    if (IsSolvingTimeC <= 0)
                    {
                        isSolvingC = false;
                        SolvedC += 1;
                        BufferAtTheMoment = GeneralBuffer;
                        pictureBox3.Image = Properties.Resources.NoWorkingCat;
                    }
                }

                // �������� ������� �� �������

                // � ������ ������� ���������: ������ �� ���
                if (!isSolvingA && !isSolvingB && !isSolvingC)
                {
                    #region OldCode
                    /*QueueC -= 1;
                    isSolvingC = true;
                    IsSolvingTimeC = TimeSolvingC;
                    BufferAtTheMoment -= BufferC;
                }
                else if (QueueC <= 0 && !isSolvingA && !isSolvingB && !isSolvingC)
                {
                    // ���� ������ �� ��������, �� ��� ���� ������� C ������, ������� �������� ������� �
                    if (QueueA > 0)
                    {
                        QueueA -= 1;
                        isSolvingA = true;
                        IsSolvingTimeA = TimeSolvingA;
                        BufferAtTheMoment -= BufferA;
                    }

                    // ���� ������� � ��������� ������, ��������� ������� �
                    if (QueueB > 0)
                    {
                        if (BufferB <= BufferAtTheMoment)
                        {
                            QueueB -= 1;
                            isSolvingB = true;
                            IsSolvingTimeB = TimeSolvingB;
                            BufferAtTheMoment -= BufferB;
                        }   
                    }*/
                    #endregion

                    int IndexMaxQueue = FindMaxQueue(QueueA, QueueB, QueueC); // ����� ������������ �������

                    // ��������� �������: � ����� ������ ����� �������, �� � ������ �� �������. �������� � ������ C.
                    // ���� ������� ����� �� ����, �� ��������� ������� �������� � 
                    if (IndexMaxQueue == 3 || IndexMaxQueue == 0)
                    {
                        QueueC -= 1;
                        isSolvingC = true;
                        IsSolvingTimeC = TimeSolvingC;
                        BufferAtTheMoment -= BufferC;
                        pictureBox3.Image = Properties.Resources.WorkingCat;
                    }

                    // ��������� ������ � �� ����� ������� �������
                    if (IndexMaxQueue == 1)
                    {
                        QueueA -= 1;
                        isSolvingA = true;
                        IsSolvingTimeA = TimeSolvingA;
                        BufferAtTheMoment -= BufferA;
                        pictureBox1.Image = Properties.Resources.WorkingCat;
                    }

                    // ��������� ������ � �� ����� ������� �������
                    if (IndexMaxQueue == 2)
                    {
                        QueueB -= 1;
                        isSolvingB = true;
                        IsSolvingTimeB = TimeSolvingB;
                        BufferAtTheMoment -= BufferB;
                        pictureBox2.Image = Properties.Resources.WorkingCat;
                    }
                    
                }

                // ����� ���������, ����� ������ ����������� �� �������: �������� � ����� ������ �.
                if (isSolvingA)
                {
                    // � ��� ���� ������ � �� ��������, ���� � ������� � ���� ������
                    if (!isSolvingB && QueueB > 0)
                    {
                        // �� ������� ��������� �� ������������ ������� ������ �
                        if (BufferB <= BufferAtTheMoment)
                        {
                            QueueB -= 1;
                            isSolvingB = true;
                            IsSolvingTimeB = TimeSolvingB;
                            BufferAtTheMoment -= BufferB;
                            pictureBox2.Image = Properties.Resources.WorkingCat;
                        }
                    }
                }

                // � ���������, �������� �� ������ ������ �.
                if (isSolvingB)
                {
                    // � ��� �� �������� ������ �, ���� � ������� � ���� ������
                    if (!isSolvingA && QueueA > 0)
                    {
                        // �� ������� ��������� �� ������������ ������� ������ �
                        if (BufferA <= BufferAtTheMoment)
                        {
                            QueueA -= 1;
                            isSolvingA = true;
                            IsSolvingTimeA = TimeSolvingA;
                            BufferAtTheMoment -= BufferA;
                            pictureBox1.Image = Properties.Resources.WorkingCat;
                        }
                    }
                }

                // � ���������, �������� �� ������ ������ �.
                if (isSolvingC)
                {
                    // � ��� ���� �� �������� ������ �, ���� � ������� � ���� ������
                    if (!isSolvingA && QueueA > 0)
                    {
                        // �� ������� ��������� �� ������������ ������� ������ �
                        if (BufferA <= BufferAtTheMoment)
                        {
                            QueueA -= 1;
                            isSolvingA = true;
                            IsSolvingTimeA = TimeSolvingA;
                            BufferAtTheMoment -= BufferA;
                            pictureBox1.Image = Properties.Resources.WorkingCat;
                        }
                    }

                    // � ��� ���� �� �������� ������ B, ���� � ������� � ���� ������
                    if (!isSolvingB && QueueB > 0)
                    {
                        // �� ������� ��������� �� ������������ ������� ������ �
                        if (BufferB <= BufferAtTheMoment)
                        {
                            QueueB -= 1;
                            isSolvingB = true;
                            IsSolvingTimeB = TimeSolvingB;
                            BufferAtTheMoment -= BufferB;
                            pictureBox2.Image = Properties.Resources.WorkingCat;
                        }
                    }
                }


                await Task.Delay((100 * Convert.ToInt32(t * 10) / 25) * Convert.ToInt32(Delay)); // ����������� ��������

                progressBar1.Value += Convert.ToInt32(t * 10); // ��������� ������� �������� ������

                // �������� ��������
                Protocol[0] = Convert.ToString(Math.Round(Time, 2));
                Protocol[1] = Convert.ToString(QueueA);
                Protocol[2] = Convert.ToString(QueueB);
                Protocol[3] = Convert.ToString(QueueC);
                Protocol[4] = Convert.ToString(isSolvingA);
                Protocol[5] = Convert.ToString(isSolvingB);
                Protocol[6] = Convert.ToString(isSolvingC);
                Protocol[7] = Convert.ToString(SolvedA);
                Protocol[8] = Convert.ToString(SolvedB);
                Protocol[9] = Convert.ToString(SolvedC);
                Protocol[10] = Convert.ToString(DeclinedA);
                Protocol[11] = Convert.ToString(DeclinedB);
                Protocol[12] = Convert.ToString(DeclinedC);
                dataGridView1.Rows.Add(Protocol);

                // � ��������� ����� � �������
                textBox13.Text = Convert.ToString(BufferAtTheMoment);
            }

            Cost = (SolvedA * CostA) + (SolvedB * CostB) + (SolvedC * CostC);
            textBox15.Text = Convert.ToString(Cost);
            Productivity = SolvedA + SolvedB + SolvedC;
            textBox14.Text = Convert.ToString(Productivity);


        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (!isRunning)
            {
                isRunning = true;
                button1.Text = "�����";
                Modeling();
            }
            else
            {
                isRunning = false;
                Delay = trackBar1.Value;
                button1.Text = "�����";
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {

            // ��������� ������ ����������
            QueueA = 0;
            QueueB = 0;
            QueueC = 0;
            SolvedA = 0;
            SolvedB = 0;
            SolvedC = 0;
            DeclinedA = 0;
            DeclinedB = 0;
            DeclinedC = 0;
            Productivity = 0;
            Cost = 0;

            isRunning = false;
            Time = 0;
            progressBar1.Value = 0;
            dataGridView1.Rows.Clear();
            button1_Click(sender, e);
        }
    }
}