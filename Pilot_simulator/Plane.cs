using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pilot_simulator
{
    delegate void PlaneDelegate(int speed, int height);
    delegate void BoxDelegate(string data, int speed, int height);
    internal class Plane
    {
        const int MAX_SPEED = 1100;
        const int MIN_SPEED = 0;
        const int GET_SPEED = 1000;
        const int MAX_HEIGHT = 7000;
        const int MIN_HEIGHT = 0;
        const int CHANGE_SPEED = 50;
        const int CHANGE_HIGHT = 150;
        public bool isSuccess { get; set; }//достигнута максимальная скорость
        public bool isOver {  get; set; }//выход из цикла полета
        public bool isCrashed { get; set; }
        event PlaneDelegate PlaneEvent;
        event BoxDelegate BoxEvent;
        List<Dispatcher> dispatchers = new List<Dispatcher>();// Список текущих диспетчеров
        List<string> add_menu = new List<string> { "  добавить диспетчера  ", "  удалить диспетчера   " };
        public int totalPenalty { get; set; }// Общая сумма штрафных очков
        int currentSpeed;// Текущая скорость
        public int CurrentSpeed
        {
            get { return currentSpeed; }
            set
            {
                if (value < MIN_SPEED) { value = MIN_SPEED; } else if (value > MAX_SPEED) { value = MAX_SPEED; }
                currentSpeed = value;
            }
        }
        int currentHeight;// Текущая Высота
        public int CurrentHeight
        {
            get { return currentHeight; }
            set
            {
                if (value < MIN_HEIGHT) { value = MIN_HEIGHT; } else if (value > MAX_HEIGHT) { value = MAX_HEIGHT; }
                currentHeight = value;
            }
        }
        public void AddDispatcher()// Добавление диспетчера
        {
            Menu add = new Menu();
            int ind = add.choiseMenu(add_menu, 1, 1);
            if (ind == 0)
            {
                Console.SetCursorPosition(25, 1);
                Console.Write("Введите имя диспетчера: ");
                Dispatcher dispatcher = new Dispatcher { Name = Console.ReadLine(), Weather = 0 };
                dispatchers.Add(dispatcher);
                PlaneEvent += dispatcher.RecomendHight;//подписка
            }
            else
            {
                if (dispatchers.Count > 0)
                {
                    ind = add.choiseMenu(dispatchers, 1, 1);
                    totalPenalty += dispatchers[ind].Penalty;
                    PlaneEvent -= dispatchers[ind].RecomendHight;//отписка
                    dispatchers.RemoveAt(ind);
                }
            }
        }
        public void ShowDispatcher()
        {
            foreach (var item in dispatchers)
            {
                Console.WriteLine(item);
            }
        }
        public void Show(int X, int Y)
        {
            char[,] plane =
             {
                { '/', '/', '/', '/', '/', '/', '.', '.', '.', '/', '/', '/', '/','/','/','/','/','/' },
                { '.', '.', '.', '/', '/', '/', '.', '*', '*', '.', '/', '/', '/','/','/','/','/','/' },
                { '.', '*', '*', '.', '/', '/', '/', '.', '*', '*', '.', '/', '/','/','/','/','/','/' },
                { '.', '*', '*', '*', '.', '.', '.', '.', '.', '.', '.', '.', '.','.','.','/','/','/' },
                { '.', '.', '.', '.', '*', '*', '*', '*', '*', '+', '*', '+', '*','+','*','.','.','/' },
                { '/', '/', '/', '/', '.', '.', '.', '*', '*', '*', '*', '*', '*','*','*','*','*','.' },
                { '/', '/', '/', '/', '/', '/', '.', '*', '*', '*', '.', '.', '.','.','.','.','.','/' },
                { '/', '/', '/', '/', '/', '.', '*', '*', '*', '.', '/', '/', '/','/','/','/','/','/' },
                { '/', '/', '/', '/', '.', '.', '.', '.', '/', '/', '/', '/', '/','/','/','/','/','/' }
            };
            for (int i = 0; i < plane.GetLength(0); i++)
            {
                Console.SetCursorPosition(X, Y + i);
                for (global::System.Int32 j = 0; j < plane.GetLength(1); j++)
                {
                    if (plane[i, j] == '*')
                    {
                        Console.BackgroundColor = ConsoleColor.Gray;
                        Console.ForegroundColor = ConsoleColor.Gray;

                    }
                    else if (plane[i, j] == '+')
                    {
                        Console.BackgroundColor = ConsoleColor.Blue;
                        Console.ForegroundColor = ConsoleColor.Blue;
                    }
                    else if (plane[i, j] == '.')
                    {
                        Console.BackgroundColor = ConsoleColor.DarkGray;
                        Console.ForegroundColor = ConsoleColor.DarkGray;
                    }
                    else
                    {
                        Console.BackgroundColor = ConsoleColor.Black;
                        Console.ForegroundColor = ConsoleColor.Black;
                    }
                    Console.Write(plane[i, j]);
                    Console.ResetColor();
                }
                Console.WriteLine();
            }
            Console.ResetColor();
        }
        public void SetPenalty()// В конце игры все штрафные баллы зачислить
        {
            foreach (var item in dispatchers)
            {
                totalPenalty += item.Penalty;
            }
        }
        public void NewGame(Registration obj)
        {

           
            Console.Clear();
            Console.WriteLine("Добро пожаловать в симулятор полета!" +
                    "\n" + "Для добавления диспетчера используйте Enter (минимум 2 человека)" +
                    "\n" + "Для начала полета Up+Right (полет возможен после добавления диспетчеров)" +
                    "\n" + "Набрать скорость Right (+50км/ч) , сбросить скорость Left (-50км/ч)" +
                    "\n" + "Набрать высоту Up (+250м), сбросить высоту Down(-250м)" +
                   "\n" + "Будьте внимательны, следите за рекомендациями диспетчеров! Хорошего полета!");
            int x = 5, y = 19;
            ConsoleKey key;
            BoxEvent += obj.BlackBox;
            do
            {
                Show(x, y);
                key = Console.ReadKey(true).Key;
                Console.Clear();
                switch (key)
                {
                    case ConsoleKey.UpArrow:
                        if (dispatchers.Count >= 2)
                        {
                            CurrentHeight += CHANGE_HIGHT;
                            if (y > 3) y--;
                            if (x < 80) x++;
                        }
                        break;
                    case ConsoleKey.DownArrow:
                        if (dispatchers.Count >= 2)
                        {
                            CurrentHeight -= CHANGE_HIGHT;
                            if (y < 20) y++;
                            if (x < 80) x++;
                        }
                        break;
                    case ConsoleKey.LeftArrow:
                        if (dispatchers.Count >= 2)
                        {
                            CurrentSpeed -= CHANGE_SPEED;
                        }
                        break;
                    case ConsoleKey.RightArrow:
                        if (dispatchers.Count >= 2)
                        {
                            CurrentSpeed += CHANGE_SPEED;
                        }
                        break;
                    case ConsoleKey.Enter://добавтить или удалить диспетчеров в любое время полета
                        AddDispatcher();
                        break;

                    default: Console.WriteLine("Error"); break;
                }
                Console.Clear();
                if (dispatchers.Count < 2 && CurrentSpeed < CHANGE_SPEED) { Console.WriteLine("Диспетчеров не может быть меньше 2!!"); }
                else
                {
                    PlaneEvent(CurrentSpeed, CurrentHeight);//принимают управление диспетчеры
                    BoxEvent(Convert.ToString(DateTime.Now), CurrentSpeed ,CurrentHeight);
                    foreach (var item in dispatchers)
                    {
                        if (!item.isAlive)
                        {
                            while (y != 20)
                            {
                                Show(x, y);
                                Console.Clear();
                                y++;//самолет падает
                            }
                            Console.WriteLine("НУЖНО ЕЩЕ ПОДУЧИТЬСЯ!");
                            isCrashed = true;
                            isOver = true;
                            break;
                        }
                    }
                    if (currentSpeed == GET_SPEED)
                    {
                        isSuccess = true;
                        Console.WriteLine("Вам нужно теперь посадить самолет!");

                    }
                }
                if (currentHeight < 1 && currentSpeed < 1 && isSuccess == true)
                {
                    Console.WriteLine("ПОЗДРАВЛЯЕМ ВЫ ЗАВЕРШИЛИ ПОЛЕТ УСПЕШНО!");
                    isOver = true;
                }
                Console.SetCursorPosition(0, 28);
                Console.WriteLine(this);
            } while (isOver!=true);
            System.Threading.Thread.Sleep(1000);
            Console.Clear();
            ShowDispatcher();
            SetPenalty();
            dispatchers.Clear();
            Console.WriteLine("Ваши штрафы: " + totalPenalty);
        }
        public override string ToString()
        {
            return $"Высота: {currentHeight} Скорость: {currentSpeed} Штрафы: {totalPenalty}";
        }
    }
}
