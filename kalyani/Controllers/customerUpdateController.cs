using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AutoSherpa_project.Models;
using AutoSherpa_project.Models.ViewModels;
using MySql.Data.MySqlClient;
using Newtonsoft.Json;

namespace AutoSherpa_project.Controllers
{
    [AuthorizeFilter]
    public class customerUpdateController : Controller
    {
        [HttpPost]
        public ActionResult Saves(string customerData)
        {
            long cusId = Convert.ToInt32(Session["CusId"].ToString());
            long vehiId = Convert.ToInt32(Session["VehiId"].ToString());
            string phoneNumber, email, DOB, DOA, reason, submit, DOI/*, custFirstName, custMiddleName,custLastName*/;
            long phoneId;
            bool DND;
            customerupdateVM getcustomerData = new customerupdateVM();
            if (customerData != null)
            {
                getcustomerData = JsonConvert.DeserializeObject<customerupdateVM>(customerData);
            }
            phoneNumber = getcustomerData.phonenumber == null ? "" : getcustomerData.phonenumber;
            reason = getcustomerData.reasons == null ? "" : getcustomerData.reasons;
            email = getcustomerData.email == null ? "" : getcustomerData.email;
            DOB = getcustomerData.dob == null ? "" : getcustomerData.dob;
            DOA = getcustomerData.doa == null ? "" : getcustomerData.doa;
            phoneId = getcustomerData.phoneId == null || getcustomerData.phoneId == "" ? 0 : Convert.ToInt64(getcustomerData.phoneId);
            submit = getcustomerData.Type == null ? "" : getcustomerData.Type;
            DOI = getcustomerData.doi == null ? "" : getcustomerData.doi;
            //custFirstName = getcustomerData.customerFirstName == null ? "" : getcustomerData.customerFirstName;
            //custMiddleName = getcustomerData.customerMiddleName == null ? "" : getcustomerData.customerMiddleName;
            //custLastName = getcustomerData.customerLastName == null ? "" : getcustomerData.customerLastName;
            DND = Convert.ToBoolean(getcustomerData.dnd);
            try
            {

                using (var db = new AutoSherDBContext())
                {

                    if (submit == "SavePhone")
                    {
                        bool result = phoneAdd(getcustomerData);
                        if (result)
                        {
                            //var phoneList = db.phones.Where(e => e.customer_id == cusId).Select(e => new { phoneNo = e.phoneNumber, id = e.phone_Id, pref = e.isPreferredPhone }).ToList().OrderByDescending(m => m.pref == true);
                            List<phone>  phoneList = db.Database.SqlQuery<phone>("select * from phone p join(select max(phone_Id)phid from phone where customer_id=@custId group by phoneNumber)Ph on Ph.phid=p.phone_Id and p.customer_id=@custId order by phone_Id desc Limit 0,5", new MySqlParameter("@custId", cusId)).ToList();
                            phoneList = phoneList.OrderByDescending(m => m.isPreferredPhone == true).ToList();

                            var newphoneList = phoneList.Select(e => new { phoneNo = e.phoneNumber, id = e.phone_Id, pref = e.isPreferredPhone }).ToList();
                            return Json(new { successPhone = true, phnum = newphoneList });

                        }
                        else
                        {
                            return Json(new { success = false });
                        }
                    }
                    else if (submit == "SaveEmail")
                    {

                        bool result = emailAdd(getcustomerData);
                        if (result)
                        {
                            //var emailList = db.emails.Where(e => e.customer_id == cusId).OrderByDescending(m=>m.isPreferredEmail==true).Select(e => new { email = e.emailAddress, id = e.email_Id, pref = e.isPreferredEmail }).ToList().OrderByDescending(m => m.pref == true);
                            List<email> emailList = db.Database.SqlQuery<email>("select * from email p join(select max(email_Id)emid from email where customer_id=@custId group by emailAddress)Ph on Ph.emid=p.email_Id and p.customer_id=@custId order by email_Id desc Limit 0,5", new MySqlParameter("@custId", cusId)).ToList();
                            emailList = emailList.OrderByDescending(m => m.isPreferredEmail == true).ToList();

                            var newemailList = emailList.Select(e => new { email = e.emailAddress, id = e.email_Id, pref = e.isPreferredEmail }).ToList();

                            return Json(new { successEmail = true, emailAddress = newemailList });
                        }

                        else
                        {
                            return Json(new { success = false });
                        }
                    }
                    else if (submit == "DeletePhone")
                    {
                        bool result = deletePhone(phoneId, getcustomerData.reasons);
                        if (result)
                        {
                            //var phoneList = db.phones.Where(e => e.customer_id == cusId).Select(e => new { phoneNo = e.phoneNumber, id = e.phone_Id, pref = e.isPreferredPhone }).ToList().OrderByDescending(m => m.pref == true);
                            List<phone> phoneList = db.Database.SqlQuery<phone>("select * from phone p join(select max(phone_Id)phid from phone where customer_id=@custId group by phoneNumber)Ph on Ph.phid=p.phone_Id and p.customer_id=@custId order by phone_Id desc Limit 0,5", new MySqlParameter("@custId", cusId)).ToList();
                            phoneList = phoneList.OrderByDescending(m => m.isPreferredPhone == true).ToList();

                            var newphoneList = phoneList.Select(e => new { phoneNo = e.phoneNumber, id = e.phone_Id, pref = e.isPreferredPhone }).ToList();

                            return Json(new { successDelete = true, phnum = newphoneList });
                        }
                        else
                        {
                            return Json(new { success = false });
                        }
                    }
                    else if (submit == "SaveCustomer")
                    {
                        bool result = updatecustomerDetails(DND, DOB, DOA, DOI/*, custFirstName, custMiddleName, custLastName*/);
                        if (result)
                        {
                            bool dnd = db.customers.FirstOrDefault(e => e.id == cusId).doNotDisturb;
                            var vehicleData = db.vehicles.Where(m => m.vehicle_id == vehiId && m.customer_id == cusId).FirstOrDefault();

                            //string customerFullName = string.Empty;
                            //if (vehicleData != null)
                            //{
                            //    var firstName = vehicleData.first_name?.Trim() ?? "";
                            //    var middleName = vehicleData.Middle_Name?.Trim() ?? "";
                            //    var lastName = vehicleData.last_name?.Trim() ?? "";

                            //    if (!string.IsNullOrEmpty(vehicleData.salutation?.Trim()))
                            //    {
                            //        string customerFullNameWithoutSalutation = string.Join(" ", new[] { firstName, middleName, lastName }.Where(s => !string.IsNullOrEmpty(s)));
                            //        customerFullName = $"{vehicleData.salutation}{customerFullNameWithoutSalutation}";
                            //    }
                            //    else
                            //    {
                            //        customerFullName = string.Join(" ", new[] { firstName, middleName, lastName }.Where(s => !string.IsNullOrEmpty(s)));
                            //    }
                            //}

                            return Json(new { successCustomerInfo = true, dnd = dnd /*, customerFullName = customerFullName*/ });
                        }
                        else
                        {
                            return Json(new { success = false });
                        }
                    }
                }
            }
            catch (Exception ex)
            {

            }
            return View();
        }


        public string addressAdd(CallLoggingViewModel CVM)
        {
            int custId = Convert.ToInt32(Session["CusId"].ToString());
            int vehiId = Convert.ToInt32(Session["VehiId"].ToString());
            int UserId = Convert.ToInt32(Session["UserId"].ToString());
            try
            {
                using (var db = new AutoSherDBContext())
                {
                    string addrs = CVM.addressesAdd.addressLine1 + "," + CVM.addressesAdd.addressLine2 + " " + CVM.addressesAdd.city; ;
                    int count = db.addresses.Count(c => c.customer_Id == custId);
                    if (count > 0)
                    {
                        db.addresses.Where(x => x.customer_Id == custId).ToList().ForEach(x => { x.isPreferred = false; });
                        address add = new address();
                        add.customer_Id = custId;
                        add.concatenatedAdress = addrs;
                        add.city = CVM.addressesAdd.city;
                        add.state = CVM.addressesAdd.state;
                        add.pincode = CVM.addressesAdd.pincode;
                        add.addressType = 1;
                        add.isPreferred = true;
                        add.wyzUserName = Session["UserName"].ToString();
                        add.updatedDateTime = DateTime.Now;
                        db.addresses.Add(add);

                    }
                    else
                    {
                        address add = new address();
                        add.customer_Id = custId;
                        add.concatenatedAdress = addrs;
                        add.city = CVM.addressesAdd.city;
                        add.state = CVM.addressesAdd.state;
                        add.pincode = CVM.addressesAdd.pincode;
                        add.addressType = 1;
                        add.isPreferred = true;
                        add.wyzUserName = Session["UserName"].ToString();
                        add.updatedDateTime = DateTime.Now;
                        db.addresses.Add(add);
                    }
                    db.SaveChanges();
                    return addrs;
                }
            }
            catch (Exception ex)
            {

            }
            return "false";
        }

        public bool emailAdd(customerupdateVM CVM)
        {
            int custId = Convert.ToInt32(Session["CusId"].ToString());
            int vehiId = Convert.ToInt32(Session["VehiId"].ToString());
            int UserId = Convert.ToInt32(Session["UserId"].ToString());
            try
            {
                using (var db = new AutoSherDBContext())
                {
                    int countEmail = db.emails.Count(x => x.customer_id == custId);
                    if (countEmail > 0)
                    {
                        var result = db.emails.FirstOrDefault(b => b.emailAddress == CVM.email && b.isPreferredEmail == true && b.customer_id == custId);//if he enters same number with already active no action needed
                        if (result == null)//if he adds ne phone i.e record does not exists
                        {
                            var checkifEmailWxist = db.emails.SingleOrDefault(b => b.emailAddress == CVM.email && b.customer_id == custId);//checks whether enter phone number matches any of his previous phone number
                            if (checkifEmailWxist != null) //if exists
                            {
                                db.emails.Where(x => x.customer_id == custId && x.isPreferredEmail == true).ToList().ForEach(x =>
                                  {
                                      x.isPreferredEmail = false;
                                  });
                                checkifEmailWxist.isPreferredEmail = true;//make that phone number as active
                                db.SaveChanges();
                                return true;
                            }
                            else
                            {

                                db.emails.Where(x => x.customer_id == custId && x.isPreferredEmail == true).ToList().ForEach(x =>
                                   {
                                       x.isPreferredEmail = false;
                                   });
                                //create new phone number
                                email em = new email();
                                em.customer_id = custId;
                                em.emailAddress = CVM.email;
                                em.updatedBy = Session["UserName"].ToString();
                                em.isPreferredEmail = true;
                                db.emails.Add(em);
                                db.SaveChanges();
                                return true;
                            }
                        }
                        else
                        {
                            return true;
                        }
                    }
                    else  //if not found any phone number add one
                    {
                        email em = new email();
                        em.customer_id = custId;
                        em.emailAddress = CVM.email;
                        em.updatedBy = Session["UserName"].ToString();
                        em.isPreferredEmail = true;
                        db.emails.Add(em);
                        db.SaveChanges();
                        return true;
                    }
                }
            }
            catch (Exception ex)
            {

            }
            return false;
        }

        public bool deletePhone(long phnId, string remarks)
        {
            int custId = Convert.ToInt32(Session["CusId"].ToString());
            int vehiId = Convert.ToInt32(Session["VehiId"].ToString());
            int UserId = Convert.ToInt32(Session["UserId"].ToString());
            try
            {

                using (var db = new AutoSherDBContext())
                {
                    var phNum = phnId;

                    var checkifPhonexist = db.phones.SingleOrDefault(b => b.phone_Id == phNum && b.customer_id == custId);
                    if (checkifPhonexist != null)
                    {
                        if (checkifPhonexist.isPreferredPhone == true)
                        {
                            //return Json(new { success = false });
                            phonenodeletion phonenodeletion = new phonenodeletion();
                            phonenodeletion.customerId = custId;
                            phonenodeletion.userId = UserId;
                            phonenodeletion.remarks = remarks;
                            phonenodeletion.phoneNumber = checkifPhonexist.phoneNumber;
                            phonenodeletion.deletedDate = DateTime.Now;
                            db.phonenodeletions.Add(phonenodeletion);

                            db.phones.Remove(checkifPhonexist);
                            db.SaveChanges();

                            db.phones.Where(m => m.customer_id == custId).ToList().ForEach(k => k.isPreferredPhone = false);
                            db.SaveChanges();

                            var firstPh = db.phones.FirstOrDefault(m => m.customer_id == custId);
                            firstPh.isPreferredPhone = true;
                            db.SaveChanges();

                            //var phoneList = db.phones.Where(e => e.customer_id == custId).Select(e => new { phoneNo = e.phoneNumber, id = e.phone_Id, pref = e.isPreferredPhone }).ToList().OrderByDescending(m => m.pref == true);
                            //return Json(new { success = true, phnum = phoneList });
                            return true;

                        }
                        else
                        {
                            phonenodeletion phonenodeletion = new phonenodeletion();
                            phonenodeletion.customerId = custId;
                            phonenodeletion.userId = UserId;
                            phonenodeletion.remarks = remarks;
                            phonenodeletion.phoneNumber = checkifPhonexist.phoneNumber;
                            phonenodeletion.deletedDate = DateTime.Now;
                            db.phonenodeletions.Add(phonenodeletion);

                            db.phones.Remove(checkifPhonexist);
                            db.SaveChanges();


                            //var phoneList = db.phones.Where(e => e.customer_id == custId).Select(e => new { phoneNo = e.phoneNumber, id = e.phone_Id, pref = e.isPreferredPhone }).ToList().OrderByDescending(m => m.pref == true);
                            //return Json(new { success = true, phnum = phoneList });
                            return true;
                        }
                    }
                }
            }
            catch (Exception ex)
            {

            }
            return false;
        }

        public bool phoneAdd(customerupdateVM CVM)
        {
            int custId = Convert.ToInt32(Session["CusId"].ToString());
            int vehiId = Convert.ToInt32(Session["VehiId"].ToString());
            int UserId = Convert.ToInt32(Session["UserId"].ToString());
            try
            {

                using (var db = new AutoSherDBContext())
                {
                    int count = db.phones.Count(x => x.customer_id == custId);
                    if (count > 0)
                    {
                        var result = db.phones.FirstOrDefault(b => b.phoneNumber == CVM.phonenumber && b.isPreferredPhone == true && b.customer_id == custId);//if he adds ne phone i.e record does not exists
                        if (result == null)
                        {
                            var checkifPhoneWxist = db.phones.SingleOrDefault(b => b.phoneNumber == CVM.phonenumber && b.customer_id == custId);//checks whether enter phone number matches any of his previous phone number
                            if (checkifPhoneWxist != null) //if exists
                            {
                                db.phones.Where(x => x.customer_id == custId && x.isPreferredPhone == true).ToList().ForEach(x =>
                                {
                                    x.isPreferredPhone = false;
                                });
                                checkifPhoneWxist.isPreferredPhone = true;//make that phone number as active
                                checkifPhoneWxist.remarks = CVM.reasons;
                                db.SaveChanges();
                                return true;
                            }
                            else
                            {

                                db.phones.Where(x => x.customer_id == custId && x.isPreferredPhone == true).ToList().ForEach(x =>
                                {
                                    x.isPreferredPhone = false;
                                });
                                //create new phone number
                                phone ph = new phone();
                                ph.customer_id = custId;
                                ph.updatedBy = Session["UserName"].ToString();
                                ph.phoneNumber = CVM.phonenumber;
                                ph.isPreferredPhone = true;
                                ph.remarks = CVM.reasons;
                                ph.uploadedDate = DateTime.Now;
                                db.phones.Add(ph);
                                db.SaveChanges();

                            }

                            //phone ph = new phone();
                            //ph.customer_id = custId;
                            //ph.updatedBy = Session["UserName"].ToString();
                            //ph.phoneNumber = CVM.phoneAdd.phoneNumber;
                            //ph.isPreferredPhone = true;
                            //ph.remarks = CVM.phoneAdd.remarks;
                            //db.phones.Add(ph);
                            //db.SaveChanges();
                            return true;
                        }
                        else
                        {
                            return true;
                        }
                    }
                    else  //if not found any phone number add one
                    {
                        phone ph = new phone();
                        ph.customer_id = custId;
                        ph.updatedBy = Session["UserName"].ToString();
                        ph.phoneNumber = CVM.phonenumber;
                        ph.remarks = CVM.reasons;
                        ph.isPreferredPhone = true;
                        db.phones.Add(ph);
                        db.SaveChanges();
                        return true;
                    }
                }
            }
            catch (Exception ex)
            {
                
            }
            return false;
        }

        public bool updatecustomerDetails(bool donotd, string dobirth, string dateofanniversary, string doi/*, string firstName, string middleName, string lastName*/)
        {
            try
            {
                using (var db = new AutoSherDBContext())
                {
                    int custId = Convert.ToInt32(Session["CusId"].ToString());
                    int vehId = Convert.ToInt32(Session["VehiId"].ToString());
                    customer updateCustomer = db.customers.FirstOrDefault(b => b.id == custId);
                    vehicle updateVehicle = db.vehicles.FirstOrDefault(b => b.vehicle_id == vehId && b.customer_id == custId);

                    if (updateVehicle != null)
                    {
                        if (!string.IsNullOrEmpty(doi) && doi != "NA")
                        {
                            updateVehicle.doi = Convert.ToDateTime(doi).Date;
                        }
                        //updateVehicle.first_name = firstName?.Trim() ?? "";
                        //updateVehicle.Middle_Name = middleName?.Trim() ?? "";
                        //updateVehicle.last_name = lastName?.Trim() ?? "";

                        db.vehicles.AddOrUpdate(updateVehicle);
                        db.SaveChanges();
                    }

                    if (updateCustomer != null)
                    {
                        //string customerFullName = string.Empty;
                        //var custSalutation = updateVehicle?.salutation.Trim() ?? "";
                        //var custFirstName = firstName?.Trim() ?? "";
                        //var custMiddleName = middleName?.Trim() ?? "";
                        //var custLastName = lastName?.Trim() ?? "";

                        //if (!string.IsNullOrEmpty(custSalutation))
                        //{
                        //    string customerFullNameWithoutSalutation = string.Join(" ", new[] { firstName, middleName, lastName }.Where(s => !string.IsNullOrEmpty(s)));
                        //    customerFullName = $"{custSalutation}{customerFullNameWithoutSalutation}";
                        //}
                        //else
                        //{
                        //    customerFullName = string.Join(" ", new[] { firstName, middleName, lastName }.Where(s => !string.IsNullOrEmpty(s)));
                        //}

                        updateCustomer.dob = dobirth;
                        updateCustomer.doNotDisturb = donotd;
                        //updateCustomer.customerName = customerFullName;
                        //updateCustomer.dob = dateofanniversary;
                        db.customers.AddOrUpdate(updateCustomer);
                        db.SaveChanges();
                    }
                    
                    return true;

                }
            }
            catch (Exception ex)
            {
                return true;
            }

        }

        #region side Address

        //By Nisarga
        public ActionResult getStateCityByPincode(string values)
        {
            try
            {
                using (AutoSherDBContext dBContext = new AutoSherDBContext())
                {
                    var ListOfCity = dBContext.citystates.Where(x => x.pincode == values).Select(m => m.city).ToList();
                    var state = dBContext.citystates.Where(x => x.pincode == values).Select(m => m.state).Distinct().ToList();
                    return Json(new { success = true, ListOfCity = ListOfCity, state = state });
                }

            }
            catch (Exception ex)
            {
                return Json(new { success = false });
            }
            return Json(new { success = true });
        }

        //By Nisarga
        public ActionResult getDataForAddressTable()
        {
            long? customerId = long.Parse(Session["CusId"].ToString());

            using (AutoSherDBContext dBContext = new AutoSherDBContext())
            {
                //var customerAddress = dBContext.addresses.Where(m => m.customer_Id == customerId).OrderByDescending(m => m.isPreferred == true).Skip(0).Take(10).ToList();
                var customerAddress = dBContext.Database.SqlQuery<address>("select * from address p join(select max(id)addId from address where customer_id=@custId group by concatenatedAdress)Ph on ph.addId=p.id and p.customer_id=@custId where p.concatenatedAdress!='' order by isPreferred=true  desc,id desc Limit 0,5", new MySqlParameter("@custId", customerId)).ToList();

                return Json(new { data = customerAddress.Select(m => new { m.id, m.concatenatedAdress, m.city, m.pincode, m.state, m.isPreferred }) }, JsonRequestBehavior.AllowGet);

            }
        }


        //By Nisarga
        public ActionResult AddNewAddress(string values)
        {
            var listOfAddr = JsonConvert.DeserializeObject<CallLoggingCustomerAddressVM>(values);
            long custId = long.Parse(Session["CusId"].ToString());

            try
            {
                using (var db = new AutoSherDBContext())
                {
                    address add = new address();

                    if (listOfAddr.isPrimary == true)
                    {
                        int count = db.addresses.Count(c => c.customer_Id == custId);
                        if (count > 0)
                        {
                            db.addresses.Where(x => x.customer_Id == custId).ToList().ForEach(x => { x.isPreferred = false; });
                        }
                        add.isPreferred = true;
                    }
                    else
                    {
                        add.isPreferred = false;
                    }
                    add.customer_Id = custId;
                    add.concatenatedAdress = listOfAddr.address;
                    add.city = listOfAddr.city;
                    add.state = listOfAddr.state;
                    add.pincode = listOfAddr.pincode;
                    add.addressType = 1;
                    add.wyzUserName = Session["UserName"].ToString();
                    add.updatedDateTime = DateTime.Now;
                    db.addresses.Add(add);


                    db.SaveChanges();
                    return Json(new { success = true, address = listOfAddr.address, city = listOfAddr.city, pincode = listOfAddr.pincode, isPrimary = listOfAddr.isPrimary });
                }
            }
            catch (Exception ex)
            {
                string exception = "";
                if (ex.Message.Contains("inner exception"))
                {
                    exception = ex.InnerException.Message;
                }
                else
                {
                    exception = ex.Message;
                }
                return Json(new { success = false, error = exception }, JsonRequestBehavior.AllowGet);
            }

        }

        public ActionResult deleteRowFromPopUp(long? AddressId)
        {
            string addres = string.Empty;
            try
            {
                using (AutoSherDBContext dBContext = new AutoSherDBContext())
                {
                    address delRow = dBContext.addresses.FirstOrDefault(m => m.id == AddressId);
                    if (delRow != null)
                    {
                        dBContext.addresses.Remove(delRow);
                        dBContext.SaveChanges();
                    }
                        return Json(new { success = true });
                    
                }
            }
            catch (Exception ex)
            {
                return Json(new { success = false });
            }
        }

        public ActionResult setasDefaultAddress(long? AddressId)
        {
            try
            {
                using (AutoSherDBContext dBContext = new AutoSherDBContext())
                {
                    string prefferedAddress = string.Empty;
                    long custId = long.Parse(Session["CusId"].ToString());
                    int count = dBContext.addresses.Count(c => c.customer_Id == custId);
                    if (count > 0)
                    {
                        dBContext.addresses.Where(x => x.customer_Id == custId).ToList().ForEach(x => { x.isPreferred = false; });
                    }
                    var makepreferedAddress = dBContext.addresses.FirstOrDefault(x => x.id == AddressId);
                    if (makepreferedAddress!=null)
                    {
                        makepreferedAddress.isPreferred = true;
                        prefferedAddress = makepreferedAddress.concatenatedAdress;
                    }
                    dBContext.SaveChanges();
                    return Json(new { success = true, prefAddress = prefferedAddress });
                }
            }
            catch (Exception ex)
            {
                return Json(new { success = false });
            }
        }

        #endregion

        public ActionResult getAddressDetails(long? custId)
        {
            try
            {


                using (var db = new AutoSherDBContext())
                {
                    db.Configuration.ProxyCreationEnabled = true;
                    db.Configuration.LazyLoadingEnabled = false;
                    var customerList = db.customers.FirstOrDefault(x => x.id == custId);
                    var phoneList = db.phones.Where(x => x.customer_id == custId).ToList();
                    List<address> addList = db.addresses.Where(x => x.customer_Id == custId && x.isPreferred == true).ToList();
                    return Json(new { success = true, cust = customerList }, JsonRequestBehavior.AllowGet);



                }
            }
            catch (Exception ex)
            {

            }
            return View();
        }

    }
    public class customerupdateVM
    {
        public string phonenumber { get; set; }
        public string reasons { get; set; }
        public string email { get; set; }
        public bool dnd { get; set; }
        public string dob { get; set; }
        public string doa { get; set; }
        public string Type { get; set; }
        public long customerId { get; set; }
        public string phoneId { get; set; }
        public string doi { get; set; }
        //public string customerFirstName { get; set; }
        //public string customerMiddleName { get; set; }
        //public string customerLastName { get; set; }

    }
}