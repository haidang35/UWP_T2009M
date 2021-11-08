using System;
using System.Collections.Generic;
using System.Text;

namespace DemoDemoConsole.Models
{
    class Student
    {
        private string id;
        private string name;
        private string age;
        private DateTime birthday;


        public string Id
        {
            get { return id; }
            set
            {
                if(value.Length > 0)
                {
                    id = value;
                }
            }
        }

    }
}
