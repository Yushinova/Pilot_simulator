using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pilot_simulator
{
    public class Pilot
    {
        public string Login { get; set; }
        public string Password { get; set; }
        public string IsReady { get; set; }
        public int Penalty { get; set; }
        public Pilot() { }
        public override string ToString()
        {
            return $"Логин: {Login}  Готовность:{IsReady} Штрафы: {Penalty}";
        }
    }
}
