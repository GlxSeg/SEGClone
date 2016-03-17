using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Windows.Media.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SEG.Domain.Model
{
    public class Image
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }

        public string Name { get; set; }

        public string Filename { get; set; }

        public int Width { get; set; }

        public int Height { get; set; }

        public byte[] Data { get; set; }

        [NotMapped]
        public BitmapImage DataImage { get; set; } 

        public int ThumbWidth { get; set; }

        public int ThumbHeight { get; set; }

        public byte[] ThumbData { get; set; }

        [NotMapped]
        public BitmapImage ThumbDataImage { get; set; } 


        //public DateTime Created { get; set; }

        //public User CreatedBy { get; set; }

        //public DateTime LastModified { get; set; }

        //public User ModifiedBy { get; set; }
    }
}
