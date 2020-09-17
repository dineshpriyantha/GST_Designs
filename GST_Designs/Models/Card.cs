using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace GST_Designs.Models
{
    public class Card
    {
        public int CardId { get; set; }

        [Required(ErrorMessage = "Please Enter Title")]
        [Display(Name = "Title")]
        [MaxLength(100)]
        public string Title { get; set; }

        [Required(ErrorMessage = "Please Enter Description")]
        [Display(Name = "Description")]
        [MaxLength(500)]
        public string Description { get; set; }

        public virtual ICollection<FileDetail> FileDetails { get; set; }
    }


    public class FileDetail
    {
        public Guid Id { get; set; }
        public string FileName { get; set; }
        public string Extension { get; set; }
        public int CardId { get; set; }
        public virtual Card Card { get; set; }

    }

    public class ViewModel
    {
        public IEnumerable<Card> GST_Card { get; set; }
        public IEnumerable<Card> GST_Card1 { get; set; }
        public IEnumerable<FileDetail> GST_FileDetail { get; set; }
    }
}