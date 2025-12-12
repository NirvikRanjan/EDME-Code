using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Text;

namespace AutoSherpa_project.Models.ViewModels
{
    public class WinbackRolloverPolicyVM
    {

        public string PreviousPolicyNo { get; set; }
        public string RenewalType { get; set; }
        public string PolicyType { get; set; }
        public string PolicyEndDate { get; set; }
        public string PolicyStartDate { get; set; }
        public string TTPolicyEndDate { get; set; }
        public string TTPolicyStartDate { get; set; }
        public string ProductID { get; set; }
        public string CPOrDealerCode { get; set; }
        public string ProposerType { get; set; }
        public string Salutation { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public string Gender { get; set; }
        public string Address { get; set; }
        public string CustomerResidentState { get; set; }
        public string City { get; set; }
        public string Pincode { get; set; }
        public string PhoneNumber { get; set; }
        public string EmailAddress { get; set; }
        public string DOB { get; set; }
        public string CompanySalutation { get; set; }
        public string CompanyName { get; set; }
        public string Designation { get; set; }
        public string DOI { get; set; }
        public string VehicleClass { get; set; }
        public string ChassisNo { get; set; }
        public string EngineNo { get; set; }
        public string Make { get; set; }
        public string Model { get; set; }
        public string Variant { get; set; }
        public string SeatingCapacity { get; set; }
        public string CubicCapacity { get; set; }
        public string FuelType { get; set; }
        public string BatteryCapacity { get; set; }
        public string YearOfManufacture { get; set; }
        public string InvoiceDate { get; set; }
        public string RTO { get; set; }
        public string RegistrationNo { get; set; }
        public string RegistrationDate { get; set; }
        public string ExShowroomPrice { get; set; }
        public string VoluntaryDisc { get; set; }
        public string AntiTheft { get; set; }
        public string AAIMembership { get; set; }
        public string PaidDriver { get; set; }
        public string UnnamedPassenger { get; set; }
        public string CoverAmount { get; set; }
        public string LegalLiabilityPaidDriver { get; set; }
        public string IsOtherEmployee { get; set; }
        public string OtherEmployeeCount { get; set; }
        public string IMT23 { get; set; }
        public string IsHandicapped { get; set; }
        public string GeoArea { get; set; }
        public string NCBSlabPer { get; set; }
        public string CPATenure { get; set; }
        public string CoverType { get; set; }
        public string IDV { get; set; }
        public string VehicleType { get; set; }
        public string BiFuelKitPrice { get; set; }
        public string ElectricalAccessoriesPrice { get; set; }
        public string NonElectricalAccessoriesPrice { get; set; }
        public string OEMID { get; set; }
        public string PanNo { get; set; }
        public string AadharNo { get; set; }
        public string Claim { get; set; }
        public string ClaimCount { get; set; }
        public string LastClaimDate { get; set; }
        public string TPPolicyNo { get; set; }
        public string TransferCase { get; set; }
        public string LastYearEP { get; set; }
        public string RTICover { get; set; }
        public string NDCover { get; set; }
        public string BHRegistration { get; set; }
    }
}