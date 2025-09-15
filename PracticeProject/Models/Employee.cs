using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace PracticeProject.Models
{
    public class Employee
    {

        public Employee()
        {
            countrylist = new List<string>();
            hobbylist = new List<string>();
            countrylist.Add("India");
            countrylist.Add("USA");
            countrylist.Add("China");
            countrylist.Add("UK");
            countrylist.Add("Russia");
            hobbylist.Add("Singing");
            hobbylist.Add("Dancing");
            hobbylist.Add("Painting");
            hobbylist.Add("Writing");
        }
        [Required(ErrorMessage ="Name is Required !")]
        public int name { get; set; }
        [Required(ErrorMessage = "Hobby is Required !")]
        public string Hobbies { get; set; }
        [Required(ErrorMessage = "Country is Required !")]
        public string country { get; set; }
        public List<string> countrylist { get; set; }      
        public List<string> hobbylist { get; set; }





    }
}