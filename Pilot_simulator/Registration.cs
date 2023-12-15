using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
namespace Pilot_simulator
{
    class Registration
    {
        List<string> menu = new List<string> { "  регистрация  ", "  авторизация  " };
        Pilot pilot = new Pilot();
        List<Pilot> temp_pilot = new List<Pilot>();
        public bool isUser(List<Pilot> temp_pilot)
        {
            foreach (var item in temp_pilot)
            {
                if (pilot.Login == item.Login)
                {
                    return true;
                }
            }
            return false;
        }
        public void BlackBox(string time, int speed, int hight)
        {
            string filename = $"{GetPilot().Login}.txt";
            StreamWriter aw = File.AppendText(filename);
            aw.WriteLine($"Дата: {time} Скорость: {speed} Высота: {hight}");
            aw.Close();
        }
        public Pilot GetPilot()
        {
            return pilot;
        }
        public List<Pilot> getList()
        {
            return temp_pilot;
        }
        public void RegistrAvtorize()
        {
            bool isCorrect;
            int index, poz = -1;
            Menu new_menu = new Menu();
            XmlSerializer bf = new XmlSerializer(typeof(List<Pilot>));//Сериализация листа пилотов
            do
            {
                
                isCorrect = true;
                Console.Clear();
                index = new_menu.choiseMenu(menu, 25, 5);
                Console.Clear();
                Console.ForegroundColor = ConsoleColor.Blue;
                Console.BackgroundColor = ConsoleColor.White;
                Console.SetCursorPosition(25, 5);
                Console.Write(" Введите логин: ");
                Console.ResetColor();
                pilot.Login = Console.ReadLine();
                Console.SetCursorPosition(25, 6);
                Console.Write(" Введите пароль: ");
                Console.ResetColor();
                pilot.Password = Console.ReadLine();
                if (index == 0)
                {
                    poz = pilot.Login.IndexOfAny(new char[] { '/', '*', '+', '-', '&', '?', '.', ',', ' ', '!', '#', '$', '%', '^', '(', ')', '=', '<', '>' });
                    if (poz != -1)
                    {
                        Console.SetCursorPosition(25, 7);
                        Console.WriteLine("В логине запрещенный символ!");
                        isCorrect = false;
                        System.Threading.Thread.Sleep(500);
                    }
                    poz = pilot.Password.IndexOfAny(new char[] { '/', '*', '+', '-', '&', '?', '.', ',', ' ', '!', '#', '$', '%', '^', '(', ')', '=', '<', '>' });
                    if (poz != -1)
                    {
                        Console.SetCursorPosition(25, 7);
                        Console.WriteLine("В пароле запрещенный символ!");
                        isCorrect = false;
                        System.Threading.Thread.Sleep(500);
                    }
                    else
                    {
                        if (System.IO.File.Exists("pilot.xml"))
                        {
                            using (Stream fsteread = File.OpenRead("pilot.xml"))
                            {
                                temp_pilot = (List<Pilot>)bf.Deserialize(fsteread);
                            }
                            if (isUser(temp_pilot))
                            {
                                isCorrect = false;
                                Console.SetCursorPosition(25, 7);
                                Console.WriteLine("Логин занят!");
                                System.Threading.Thread.Sleep(500);
                            }
                            else
                            {
                                Console.SetCursorPosition(25, 7);
                                Console.WriteLine("Логин принят!");
                                System.Threading.Thread.Sleep(500);

                                Console.SetCursorPosition(25, 7);
                                Console.WriteLine("Пароль принят!");
                                System.Threading.Thread.Sleep(500);
                                temp_pilot.Add(pilot);
                                using (Stream fstr = File.Create("pilot.xml"))
                                {
                                    bf.Serialize(fstr, temp_pilot);
                                }
                                //Console.WriteLine("Сериализация прошла успешно");
                            }
                        }
                        else
                        {
                            temp_pilot.Add(pilot);
                            using (Stream fstr = File.Create("pilot.xml"))
                            {
                                bf.Serialize(fstr, temp_pilot);
                            }
                            //Console.WriteLine("Сериализация прошла успешно");
                        }
                    }
                }
                if (index == 1)
                {
                    if (System.IO.File.Exists("pilot.xml"))
                    {
                        using (Stream fsteread = File.OpenRead("pilot.xml"))
                        {
                            temp_pilot = (List<Pilot>)bf.Deserialize(fsteread);
                        }
                        foreach (var item in temp_pilot)
                        {
                            if(pilot.Login==item.Login && pilot.Password==item.Password)
                            {
                                isCorrect = true;
                                break;
                            }
                            if(pilot.Login == item.Login && pilot.Password != item.Password)
                            {
                                Console.SetCursorPosition(25, 7);
                                Console.WriteLine("Логин найден! ОШИБКА ПАРОЛЯ!");
                                isCorrect = false;
                                System.Threading.Thread.Sleep(500);
                                break;
                            }
                            else
                            {
                                Console.SetCursorPosition(25, 7);
                                Console.WriteLine("Пользовтель не зарегистрирован!");
                                isCorrect = false;
                                System.Threading.Thread.Sleep(500);
                            }
                        }
                    }
                    else
                    {
                        Console.SetCursorPosition(25, 7);
                        Console.WriteLine("Пользовтель не зарегистрирован!");
                        isCorrect = false;
                        System.Threading.Thread.Sleep(500);
                    }
                }

            } while (!isCorrect);
        }
    }
}
