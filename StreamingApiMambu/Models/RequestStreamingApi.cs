using System;
using System.ComponentModel.DataAnnotations;

namespace StreamingApiMambu.Models
{
    public class RequestStreamingApi
    {
        [Required(ErrorMessage = "{0} is required")]
        [StringLength(50, MinimumLength = 3, ErrorMessage ="Debe de contener mas de 3 caracteres")]
        public string MambuServer { get; set; }

        [Required(ErrorMessage = "{0} is required")]
        [StringLength(50, MinimumLength = 3, ErrorMessage = "Debe de contener mas de 3 caracteres")]
        public string ApiKey { get; set; }

        [Required(ErrorMessage = "{0} is required")]
        [Range(0, int.MaxValue, ErrorMessage = "Please enter valid integer Number")]
        public int TimeExecution { get; set; }

        [Required(ErrorMessage = "{0} is required")]
        [StringLength(50, MinimumLength = 3, ErrorMessage = "Debe de contener mas de 3 caracteres")]
        public string Topic { get; set; }
        public string StringConnection { get; set; }
        public string Events { get; set; }
        public bool WriteInDB { get; set; }
    }
}
