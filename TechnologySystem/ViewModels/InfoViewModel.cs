using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using TechnologySystem.Models;

namespace TechnologySystem.ViewModels
{
    public class InfoViewModel
    {
        public ApplicationUser User { get; set; }
        public List<string> Roles { get; set; }
    }
}