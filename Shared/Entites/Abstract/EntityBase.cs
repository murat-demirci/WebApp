namespace Shared.Entites.Abstract
{
    //shared
    //Ortak sinif ve yapilar burada bulunur,
    //bu yapilar ve siniflar baska projelerde de kullanilailir
    //EntityBase ortak alanlar
    //IEntity yazilmasi gereken tablolar imzalanir ve karisiklik engellenir
    public abstract class EntityBase
    {
        //base degerlerin diger siniflarda degisime ugramasi (abstract)
        public virtual int ID { get; set; }
        public virtual DateTime CreatedDate { get; set; } = DateTime.Now;
        public virtual DateTime ModifiedDate { get; set; } = DateTime.Now;
        public virtual bool IsDeleted { get; set; } = false;
        public virtual bool IsActive { get; set; } = true;
        public virtual string CreatedByName { get; set; } = "Admin";
        public virtual string ModifiedByName { get; set; } = "Admin";
        public virtual string? Note { get; set; }

    }
}
