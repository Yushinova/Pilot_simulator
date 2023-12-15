using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Pilot_simulator
{
    internal class Program
    {
        static void Main(string[] args)
        {
            ConsoleKey key;
            do
            {
                Registration reg = new Registration();
                reg.RegistrAvtorize();
                Plane plane = new Plane();
                plane.NewGame(reg);
                System.Threading.Thread.Sleep(1000);
                if (plane.isCrashed == true || plane.totalPenalty > 1000 ) { reg.GetPilot().IsReady = "Не допущен к полетам!"; }
                else
                {
                    reg.GetPilot().IsReady = "Допущен к полетам!";
                }
                reg.GetPilot().Penalty = plane.totalPenalty;
                XmlSerializer bf = new XmlSerializer(typeof(List<Pilot>));
                foreach (var item in reg.getList())
                {
                    if (item.Login == reg.GetPilot().Login)
                    {
                        item.Penalty = reg.GetPilot().Penalty;
                        item.IsReady = reg.GetPilot().IsReady;
                    }    
                }
                using (Stream fstr = File.Create("pilot.xml"))
                {
                    bf.Serialize(fstr, reg.getList());
                }
                Console.WriteLine("Для нового полета нажмите любую клавишу/выход Escape");
                key = Console.ReadKey(true).Key;
            } while (key != ConsoleKey.Escape);
        }
     
    }
}
