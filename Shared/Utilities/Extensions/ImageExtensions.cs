namespace Shared.Utilities.Extensions
{
    public static class ImageExtensions
    {
        /// <summary>
        /// Create an unique string for image files
        /// </summary>
        /// <returns></returns>
        public static string CreateGuid()
        {
            string uniqueName = Guid.NewGuid().ToString();
            //bize gelen datetime nesnesini '_' ler ile ms den yila kadar yazdirmak
            return uniqueName;
            //resim eklenirken kullanicinin dai ve bu gelen degerler birlestirilecek ve resim bu sekilde kaydedilecek
        }
    }
}
