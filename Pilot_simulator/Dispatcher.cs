using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pilot_simulator
{
    internal class Dispatcher
    {
        const int PENALTY_25 = 25;
        const int PENALTY_50 = 50;
        public string Name { get; set; }// Имя диспетчера
        public int Penalty { get; set; }// Счетчик штрафных очков
        public bool isAlive = true;
        int weather;// Корректировка погодных условий
        private static Random r;
        public int Weather
        {
            get { return weather; }
            set
            {
                r = new Random();
                value = r.Next(-200, 200);
                weather = value;
            }
        }
        public void RecomendHight(int speed, int height)
        {
            int recomend = 7 * speed - Weather;// Значение рекомендованой высоты

            int diff;// Разница между рекомендованой и текущей высотой
            if (height > recomend)// Вычитаем из большего меньшее
                diff = height - recomend;
            else
                diff = recomend - height;
            Console.WriteLine($"Диспетчер {Name}: Рекомендуемая высота полета: {recomend} м. Penalty: {Penalty}");

            if (speed > 1000)// Превышение максимальной скорости
            {
                Penalty += 100;
                Console.WriteLine($"Диспетчер {Name}: Немедленно снизьте скорость!");
                System.Threading.Thread.Sleep(1000);
            }

            if (diff >= 300 && diff <= 600) Penalty += 25;
            else if (diff > 600 && diff <= 1000) Penalty += 50;
            else if (diff > 1000 || (speed == 0 && height > 150) || (speed > 50 && height == 0))
            {
                Console.WriteLine("Самолет разбился");
                System.Threading.Thread.Sleep(1000);
                isAlive = false;
            }
            if (Penalty >= 1000)
            {
                Console.WriteLine("Непригоден к полетам");
                System.Threading.Thread.Sleep(1000);
                isAlive = false;
            }
        }
        public override string ToString()
        {
            return $"Диспетчер {Name}: Cумма штрафов: {Penalty}";
        }
    }
}
