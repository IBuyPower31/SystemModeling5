namespace SystemModeling5
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        // Что будет взято из формы
        private static double TimeIncomeA; // Время поступления задач класса А
        private static double TimeIncomeB; // Время поступления задач класса Б
        private static double TimeIncomeC; // Время поступления задач класса С
        private static double TimeSolvingA; // Время решения задач класса А
        private static double TimeSolvingB; // Время решения задач класса Б
        private static double TimeSolvingC; // Время решения задач класса С
        private static double BufferA; // Буффер задач класса А
        private static double BufferB; // Буффер задач класса Б
        private static double BufferC; // Буффер задач класса С
        private static double GeneralBuffer; // Общий буффер
        private static double t; // Время моделирования
        private static double MaxQueue; // Максимальная очередь каждого задания

        // Что нужно для моделирования
        private static int MinutesInDay = 24 * 60; // Времени в сутках
        private static double Delay; // Задержка моделирования
        private static int QueueA; // Заданий А в очереди 
        private static int QueueB; // Заданий Б в очереди
        private static int QueueC; // Заданий С в очереди
        private static double Time; // Итерируемый объект цикла
        private static string[] Protocol; // Протокол
        private static int SolvedA; // Решено заданий А
        private static int SolvedB; // Решено заданий Б
        private static int SolvedC; // Решено заданий С
        private static int DeclinedA; // Отказано заданий А
        private static int DeclinedB; // Отказано заданий Б
        private static int DeclinedC; // Отказано заданий С

        // Показатели работы
        private static double Productivity; // Производительность ЭВМ
        private static int Cost; // Стоимость работы в час

        // Вспомогательные переменные
        private static double IsSolvingTimeA = 0; // Осталось времени решать задачу А 
        private static double IsSolvingTimeB = 0; // Осталось времени решать задачу Б
        private static double IsSolvingTimeC = 0; // Осталось времени решать задачу С
        private static double BufferAtTheMoment = 0; // Свободного места в буффере в текущий момент времени
        private static int CostA;
        private static int CostB;
        private static int CostC;

        // Флаги моделирования
        private static bool isRunning = false; // Происходит моделирование или нет
        private static bool isSolvingA = false; // Решается ли задача А
        private static bool isSolvingB = false; // Решается ли задача Б
        private static bool isSolvingC = false; // Решается ли задача С

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

            // Стоимости задаач
            textBox16.Text = "50";
            textBox17.Text = "60";
            textBox18.Text = "80";

            // Инициализируем PictureBox -- пока вычислительный центр ничего не делает
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
       

            // Заполнение изначальных значений
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

            // Заполняем стоимости
            CostA = Convert.ToInt32(textBox16.Text);
            CostB = Convert.ToInt32(textBox17.Text);
            CostC = Convert.ToInt32(textBox18.Text);

            BufferAtTheMoment = GeneralBuffer; // Заполненность буффера в текущий момент времени

            // Заведем переменные для хранения локального времени моделирования
            double localTime1 = TimeIncomeA; // Локальное время прихода А
            double localTime2 = TimeIncomeB; // Локальное время прихода Б
            double localTime3 = TimeIncomeC; // Локальное время прихода С

            // Работа с формой
            Delay = Convert.ToInt32(trackBar1.Value); // Получим величину задержки
            progressBar1.Value = 0; // Ставим значение полосы загрузки на минимум (т.е. на нолик)
            progressBar1.Maximum = MinutesInDay * 10 + 10; // Для максимального значения полосы загрузки

            int iterationCounter = 0; // Количество итераций в моделировании

            // Цикл модельного времени: от нуля минут до числа минут в сутках, при условии запущенного моделирования.В каждой итерации цикла
            // увеличивается на значение, полученное из формы (можно поставить 0.5 = это будет 30 секунд)

            for (; Time <= MinutesInDay && isRunning; Time += t)
            {
                iterationCounter++;

                Protocol = new string[13] { "", "", "", "", "", "", "", "", "", "", "", "", "" }; // Инициализация экземпляра протокола
                
                // ПРИХОД ЗАДАНИЙ РАЗНЫХ КЛАССОВ
                // Если пришли задания класса А
                if (Time - localTime1 >= 0)
                {
                    // Если достигнута максимальная очередь
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

                // Если пришли задания класса Б
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

                // Если пришли задания класса С
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

                // ПРОВЕРКА, НЕ РЕШИЛИСЬ ЛИ ЗАДАНИЯ
                if (isSolvingA)
                {
                    IsSolvingTimeA -= t;
                    // Если задача закончила решаться
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
                    // Если задача закончила решаться
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
                    // Если задача закончила решаться
                    if (IsSolvingTimeC <= 0)
                    {
                        isSolvingC = false;
                        SolvedC += 1;
                        BufferAtTheMoment = GeneralBuffer;
                        pictureBox3.Image = Properties.Resources.NoWorkingCat;
                    }
                }

                // ОТПРАВКА ЗАДАНИЙ НА РЕШЕНИЕ

                // В первую очередь проверяем: пустая ли ЭВМ
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
                    // Если ничего не решается, но при этом очередь C пустая, пробуем закинуть задание А
                    if (QueueA > 0)
                    {
                        QueueA -= 1;
                        isSolvingA = true;
                        IsSolvingTimeA = TimeSolvingA;
                        BufferAtTheMoment -= BufferA;
                    }

                    // Если очередь А оказалась пустой, проверяем очередь Б
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

                    int IndexMaxQueue = FindMaxQueue(QueueA, QueueB, QueueC); // Поиск максимальной очереди

                    // Проверяем очереди: в какой больше всего заданий, та и пойдет на решение. Начинаем с задачи C.
                    // Если очереди равны во всех, то приоритет отдадим заданиям С 
                    if (IndexMaxQueue == 3 || IndexMaxQueue == 0)
                    {
                        QueueC -= 1;
                        isSolvingC = true;
                        IsSolvingTimeC = TimeSolvingC;
                        BufferAtTheMoment -= BufferC;
                        pictureBox3.Image = Properties.Resources.WorkingCat;
                    }

                    // Проверяем задачу А на самую большую очередь
                    if (IndexMaxQueue == 1)
                    {
                        QueueA -= 1;
                        isSolvingA = true;
                        IsSolvingTimeA = TimeSolvingA;
                        BufferAtTheMoment -= BufferA;
                        pictureBox1.Image = Properties.Resources.WorkingCat;
                    }

                    // Проверяем задачу Б на самую большую очередь
                    if (IndexMaxQueue == 2)
                    {
                        QueueB -= 1;
                        isSolvingB = true;
                        IsSolvingTimeB = TimeSolvingB;
                        BufferAtTheMoment -= BufferB;
                        pictureBox2.Image = Properties.Resources.WorkingCat;
                    }
                    
                }

                // Затем проверяем, какие задачи отправились на решение: начинаем с задач класса А.
                if (isSolvingA)
                {
                    // И при этом задачи Б не решаются, хотя в очереди Б есть задачи
                    if (!isSolvingB && QueueB > 0)
                    {
                        // То пробуем отправить на параллельное решение задачу Б
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

                // И проверяем, решаются ли задачи класса Б.
                if (isSolvingB)
                {
                    // И при не решаются задачи А, хотя в очереди А есть задачи
                    if (!isSolvingA && QueueA > 0)
                    {
                        // То пробуем отправить на параллельное решение задачу А
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

                // И проверяем, решаются ли задачи класса С.
                if (isSolvingC)
                {
                    // И при этом не решаются задачи А, хотя в очереди А есть задачи
                    if (!isSolvingA && QueueA > 0)
                    {
                        // То пробуем отправить на параллельное решение задачу А
                        if (BufferA <= BufferAtTheMoment)
                        {
                            QueueA -= 1;
                            isSolvingA = true;
                            IsSolvingTimeA = TimeSolvingA;
                            BufferAtTheMoment -= BufferA;
                            pictureBox1.Image = Properties.Resources.WorkingCat;
                        }
                    }

                    // И при этом не решаются задачи B, хотя в очереди А есть задачи
                    if (!isSolvingB && QueueB > 0)
                    {
                        // То пробуем отправить на параллельное решение задачу А
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


                await Task.Delay((100 * Convert.ToInt32(t * 10) / 25) * Convert.ToInt32(Delay)); // Программная задержка

                progressBar1.Value += Convert.ToInt32(t * 10); // Продвинем полоску загрузки вперед

                // Заполним протокол
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

                // И свободное место в буффере
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
                button1.Text = "Пауза";
                Modeling();
            }
            else
            {
                isRunning = false;
                Delay = trackBar1.Value;
                button1.Text = "Старт";
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {

            // Обнуление нужных переменных
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