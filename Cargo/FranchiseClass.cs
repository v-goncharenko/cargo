using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Cargo
{
    class FranchiseClass {
        public bool enabled { get; set; }
        public int typeID { get; set; }
        public int sizeID { get; set; }
        public int insSumID { get; set; }
        public double rate { get; private set; }
        private int ID;

        private cargoDataSet dataSet;

        public FranchiseClass(cargoDataSet DataSet) {
            dataSet = DataSet;
            enabled = false;
        }

        public void assign() {
            string query = "typeID = '" + typeID.ToString() + "' AND " +
                            "sizeID = '" + sizeID.ToString() + "' AND " +
                            "insSumID = '" + insSumID.ToString() + "'";
            System.Data.DataRow[] found = dataSet.FranchiseRates.Select(query);
            if (found.Length == 1) {
                ID = (int)found[0]["ID"];
                rate = (double)found[0]["rate"];
            }
            else if (found.Length == 0) {
                throw new InvalidOperationException("Некоректное сочетание страховой суммы и размера франшизы");
            } else {
                throw new Exception("Corrupted data");
            }
        }
    }
}
