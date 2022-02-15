﻿using NewKaratIk.Models.CustomModels;

namespace NewKaratIk.Models.ViewModels
{
    public class UserDepartmentModelView
    {
        public User? User { get; set; }
        public Pozisyon Pozisyon { get; set; }
        public Department? Department { get; set; }
        public IEnumerable<User>? UserList { get; set; }
        public IEnumerable<Department>? DepartmentList { get; set; }
        public IEnumerable<Pozisyon>? PozisyonListVM { get; set; }
    }
}
