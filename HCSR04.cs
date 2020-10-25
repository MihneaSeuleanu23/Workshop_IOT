using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Windows.Devices.Gpio;

namespace Workshop_IOT
{
    public class HCSR04
    {
        GpioController gpio = GpioController.GetDefault();

        GpioPin TriggerPin;
        GpioPin EchoPin;

        public HCSR04(int TriggerPin, int EchoPin)
        {
            this.TriggerPin = gpio.OpenPin(TriggerPin);
            this.EchoPin = gpio.OpenPin(EchoPin);

            this.TriggerPin.SetDriveMode(GpioPinDriveMode.Output);
            this.EchoPin.SetDriveMode(GpioPinDriveMode.Input);

            this.TriggerPin.Write(GpioPinValue.Low);
        }

        public double GetDistance()
        {
            ManualResetEvent mre = new ManualResetEvent(false);
            mre.WaitOne(500);
            Stopwatch pulseLength = new Stopwatch();

            //Send pulse
            TriggerPin.Write(GpioPinValue.High);
            mre.WaitOne(TimeSpan.FromMilliseconds(0.01));
            TriggerPin.Write(GpioPinValue.Low);

            while (EchoPin.Read() == GpioPinValue.Low) { }
            pulseLength.Start();

            while (EchoPin.Read() == GpioPinValue.High) { }
            pulseLength.Stop();

            TimeSpan timeBetween = pulseLength.Elapsed;
            Debug.WriteLine(timeBetween.ToString());
            double distance = timeBetween.TotalSeconds * 17000;

            return distance;
        }
    }

}
