//------------------------------------------------------------------------------
// <auto-generated>
//     Этот код создан по шаблону.
//
//     Изменения, вносимые в этот файл вручную, могут привести к непредвиденной работе приложения.
//     Изменения, вносимые в этот файл вручную, будут перезаписаны при повторном создании кода.
// </auto-generated>
//------------------------------------------------------------------------------

namespace ContosoSite.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class Exhibition
    {
        public Exhibition()
        {
            this.Pictures = new HashSet<Picture>();
        }
    
        public int Id_exhibition { get; set; }
        public string Name { get; set; }
        public string ThemeOf { get; set; }
        public string Address { get; set; }
        public Nullable<decimal> Price { get; set; }
        public Nullable<System.DateTime> Date_Open { get; set; }
        public Nullable<System.DateTime> Date_Close { get; set; }
        public int Status_id { get; set; }
    
        public virtual Status Status { get; set; }
        public virtual ICollection<Picture> Pictures { get; set; }
    }
}
