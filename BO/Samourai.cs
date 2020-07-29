using System.Collections.Generic;
using System.ComponentModel;


namespace BO
{
    public class Samourai : DbEntity
    {
        public int Force { get; set; }
        public string Nom { get; set; }
        public virtual Arme Arme { get; set; }
        public virtual List<ArtMartial> ArtMartials { get; set; } = new List<ArtMartial>();

        [DisplayName("Potentiel")]
        public int Potentiel
        {
            get
            {
                int potentiel = this.Force;
                potentiel += this.Arme.Degats;
                potentiel *= (this.ArtMartials.Count + 1);
                return potentiel;
            }
        }
    }
}
