using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;

namespace Task6
{
    class Tests
    {

        [Test]
        public void CanCreateCamera()
        {
            var x = new Kamera("Sony STC300IR", 3.0, 800m, Currency.EUR);

            Assert.IsTrue(x.GetDescription == "Sony STC300IR");
            Assert.IsTrue(x.Aufloesung == 3.0);
            Assert.IsTrue(x.Price == 800m);
        }

        [Test]
        public void CannotCreateCameraWithoutModell()
        {
            Assert.Catch(() =>
            {
                var x = new Kamera(null, 1.3, 670m, Currency.EUR);
            });
        }

        [Test]
        public void CannotCreateCamerakWithoutModell2()
        {
            Assert.Catch(() =>
            {
                var x = new Kamera("", 1.3, 670m, Currency.EUR);
            });
        }

        [Test]
        public void CannotCreateCameraWithNegativeResolution()
        {
            Assert.Catch(() =>
            {
                var x = new Kamera("Geovision BL1300", -1.3, 670m, Currency.EUR);
            });
        }

        [Test]
        public void CannotCreateCameraWithNoMegapixelResolution()
        {
            Assert.Catch(() =>
            {
                var x = new Kamera("Geovision BL1300", 0.7, 670m, Currency.EUR);
            });
        }

        [Test]
        public void CannotCreateCameraWithNegativePrice()
        {
            Assert.Catch(() =>
            {
                var x = new Kamera("Geovision BL1300", 1.3, -670m, Currency.EUR);
            });
        }

        [Test]
        public void CanUpdateCameraWithPrice()
        {
            var x = new Kamera("Sony SNC-EP550", 5.0, 1200m, Currency.EUR);
            x.UpdatePreis(1339.90m, Currency.GBP);

            Assert.IsTrue(x.Price == 1339.90m);
            Assert.IsTrue(x.Currency == Currency.GBP);
        }

        [Test]
        public void CannotUpdateCameraWithNegativePrice()
        {
            Assert.Catch(() =>
            {
                var x = new Kamera("Sony SNC-EP550", 5.0, 1200m, Currency.EUR);
                x.UpdatePreis(-1339.90m, Currency.GBP);
            });
        }

        [Test]
        public void CanCreateNetworkSwitch()
        {
            var x = new NetworkSwitch("Cisco SG 300-20", 20, 4, true, false, true, 243m, Currency.GBP);

            Assert.IsTrue(x.GetDescription == "Cisco SG 300-20");
            Assert.IsTrue(x.Cupper_Ports == 20);
            Assert.IsTrue(x.SFP_Ports == 4);
            Assert.IsTrue(x.Gigabit == true);
            Assert.IsTrue(x.PoE == false);
            Assert.IsTrue(x.Managed == true);
            Assert.IsTrue(x.Price == 243);

        }

        [Test]
        public void CannotUpdateNetworkSwitchWithNegativePrice()
        {
            Assert.Catch(() =>
            {
                var x = new NetworkSwitch("Cisco SG 300-20", 20, 4, true, false, true, 243m, Currency.GBP);
                x.UpdatePrice(-243m, Currency.GBP);
            });
        }

        [Test]
        public void CannotCreateNetworkSwitchWithNegativePorts()
        {
            Assert.Catch(() =>
            {
                var x = new NetworkSwitch("Cisco SG 300-20", -20, 4, true, false, true, 243m, Currency.GBP);
            });
        }

        [Test]
        public void CannotCreateNetworkSwitchWithToManyPorts()
        {
            Assert.Catch(() =>
            {
                var x = new NetworkSwitch("Cisco SG 300-20", 65, 4, true, false, true, 243m, Currency.GBP);
            });
        }
    }
}
