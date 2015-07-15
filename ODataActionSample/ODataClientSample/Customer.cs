using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ODataClientSample.ODataActionSample
{
    public partial class Customer
    {
        // Dynamic property "Email"
        [global::Microsoft.OData.Client.OriginalNameAttribute("Email")]
        public string Email
        {
            get
            {
                return this._Email;
            }
            set
            {
                this.OnEmailChanging(value);
                this._Email = value;
                this.OnEmailChanged();
                this.OnPropertyChanged("Email");
            }
        }

        private string _Email;
        partial void OnEmailChanging(string value);
        partial void OnEmailChanged();

        // Dynamic property "Age"
        [global::Microsoft.OData.Client.OriginalNameAttribute("Age")]
        public int Age
        {
            get
            {
                return this._Age;
            }
            set
            {
                this.OnAgeChanging(value);
                this._Age = value;
                this.OnAgeChanged();
                this.OnPropertyChanged("Age");
            }
        }

        private int _Age;
        partial void OnAgeChanging(int value);
        partial void OnAgeChanged();

        // Dynamic property "Address"
        public Address Address
        {
            get
            {
                return this._Address;
            }
            set
            {
                this.OnAddressChanging(value);
                this._Address = value;
                this.OnAddressChanged();
                this.OnPropertyChanged("Address");
            }
        }
        private Address _Address;
        partial void OnAddressChanging(Address value);
        partial void OnAddressChanged();

        // Dynamic property "Birthday"
        public DateTimeOffset? Birthday
        {
            get
            {
                return this._Birthday;
            }
            set
            {
                this.OnBirthdayChanging(value);
                this._Birthday = value;
                this.OnBirthdayChanged();
                this.OnPropertyChanged("Birthday");
            }
        }
        private DateTimeOffset? _Birthday;
        partial void OnBirthdayChanging(DateTimeOffset? value);
        partial void OnBirthdayChanged();
    }
}
