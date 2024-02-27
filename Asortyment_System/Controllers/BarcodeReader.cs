using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

namespace Asortyment_System.Controllers
{
    internal class BarcodeReader
    {
        //Public
        public string _barcode { get; set; } = "";
        public bool busy = false;

        //Private 
        private long _timestamp;
        private Timer _timer;

        // Public methods

        public string CodeToText()
        {
            var bcode = this._barcode.Replace("D", "");
            this._barcode = "";
            return bcode;
        }

        public void initalizeRead()
        {
            if (!this.busy)
            {
                this.busy = true;
                this.StartRead();
            }
        }


        public bool isBarcodeDevice(int threshold)
        {
            var ts = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
            this.busy = false;
            this._timer.Stop();
            if ((ts - _timestamp) <= threshold && this._barcode.Length >= 9)
            {
                return true;
            }

            this._barcode = "";
            return false;
        }

        // Private Methods

        private void checkBarcodeTimeout(Object source, System.Timers.ElapsedEventArgs e)
        {
            if (this.busy)
            {
                this.busy = false;
                this._barcode = "";
            }
            this._timer.Stop();
        }

        private void StartRead()
        {
            _timestamp = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
            _timer = new System.Timers.Timer();
            _timer.Interval = 310;
            _timer.Elapsed += checkBarcodeTimeout;
            _timer.Start();
        }
    }
}
