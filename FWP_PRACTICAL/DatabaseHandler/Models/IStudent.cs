using DatabaseHandler.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatabaseHandler.Models
{
    interface IStudent
    {
        bool Save();
        List<Student> FindAll();
        Student FindById();
        bool Delete();
        bool Update();
    }
}
