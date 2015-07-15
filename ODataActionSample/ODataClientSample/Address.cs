using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ODataClientSample.ODataActionSample
{
    public partial class Address
    {
        // Dynamic property "Street"
        [global::Microsoft.OData.Client.OriginalNameAttribute("Street")]
        public string Street
        {
            get
            {
                return this._Street;
            }
            set
            {
                this.OnStreetChanging(value);
                this._Street = value;
                this.OnStreetChanged();
                this.OnPropertyChanged("Street");
            }
        }

        private string _Street;
        partial void OnStreetChanging(string value);
        partial void OnStreetChanged();

        // Dynamic property "PostCode"
        [global::Microsoft.OData.Client.OriginalNameAttribute("Postcode")]
        public int? Postcode
        {
            get
            {
                return this._Postcode;
            }
            set
            {
                this.OnPostcodeChanging(value);
                this._Postcode = value;
                this.OnPostcodeChanged();
                this.OnPropertyChanged("Postcode");
            }
        }

        private int? _Postcode;
        partial void OnPostcodeChanging(int? value);
        partial void OnPostcodeChanged();
    }
}
