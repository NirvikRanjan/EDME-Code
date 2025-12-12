using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AutoSherpa_project.Models;
using AutoSherpa_project.Models.ViewModels;
using Newtonsoft.Json;
using OfficeOpenXml.FormulaParsing.Excel.Functions.DateTime;
//using NPOI.SS.Util;

namespace AutoSherpa_project.Controllers
{
	[AuthorizeFilter]
	public class InsuranceOutBoundController : Controller
	{
		// GET: InsuranceOutBound
		public ActionResult Index()
		{
			return View();
		}
		/******************* Calculator *********************************/
		public ActionResult insuranceQuote()
		{
			try
			{
				using (var db = new AutoSherDBContext())
				{
					db.Configuration.LazyLoadingEnabled = false;

					var vehTypZone = db.irda_od_premium.Select(m=>m.zone).Distinct().ToList(); 
					return Json(new {  vehTypZone });
				}
			}
			catch (Exception ex)
			{

			}
			return View();
		}
		public ActionResult insuranceCompanyList()
		{
			try
			{
				using (var db = new AutoSherDBContext())
				{
					db.Configuration.LazyLoadingEnabled = false;

					List<string> insCompanyList = db.insurancecompanies.Select(m=>m.companyName).Distinct().ToList();
					return Json(new { success = true, insData = insCompanyList });
				}
			}
			catch(Exception ex)
			{
			}

			return View();
		}
		public ActionResult ccByVehTypeData(string selectedType)
		{
			try
			{
				using (var db = new AutoSherDBContext())
				{
					List<string> ccData = db.irda_od_premium.Where(m => m.vehicleType == selectedType).Select(m => m.cubicCapacity).Distinct().ToList();
					return Json(new { success = true, ccData = ccData });
				}
			}
			catch (Exception ex)
			{

			}

			return Json(new { success = false });
		}

		public ActionResult getBasicODVaue(double odvValue, double idvValue)
		{
			double val=0;

			try
			{
				// logger.info(" odvValue : "+odvValue+" idvValue : "+idvValue);

				//DecimalFormat df2 = new DecimalFormat(".##");

				double OD_Value = (odvValue) / 100;

				double OD_Basic = (OD_Value) * (idvValue);

				val = OD_Basic;
				 //val = Convert.ToDouble(df2.Format(OD_Basic));
			}
			catch(Exception ex)
			{

			}
			
			return Json(new { val });

			//return ok(toJson(df2.format(OD_Basic)));
		}

		public ActionResult vehicleTypeByZoneData(string selectedZone)
		{
				try
			{
				using(var db=new AutoSherDBContext())
				{
					db.Configuration.LazyLoadingEnabled = false;
					List<string> vehTypeData = db.irda_od_premium.Where(m => m.zone == selectedZone).Select(m => m.vehicleType).Distinct().ToList();

					return Json(new { vehTypeData });
				}
			}
			catch(Exception ex)
			{

			}
			return View();

		}
		public ActionResult ageByCCTypeData(string selectedcc)
		{
			try
			{
				using (var db = new AutoSherDBContext())
				{
					List<string> ageData = db.irda_od_premium.Where(m => m.cubicCapacity == selectedcc).Select(m => m.vehicleAge).Distinct().ToList();

					return Json(new { ageData });
				}
			}

			catch (Exception ex)
			{
			}

			return View();

		}

		public ActionResult IRBasedOnFilter(string selectedZone, string selectedType, string selectedcc, string selectedAge)
		{
			try
			{
				using (var db = new AutoSherDBContext())
				{
					db.Configuration.LazyLoadingEnabled = false;
					var irDatas = db.irda_od_premium.Where(u => u.zone == selectedZone && u.vehicleType == selectedType && u.cubicCapacity == selectedcc && u.vehicleAge == selectedAge).ToList();
				     
				if(irDatas.Count>0)
					{
						return Json(new { irData= irDatas });
					}
				else
					{
						return Json(" ");
					}
				}
			}
			catch(Exception ex)
			{

			}
			return View();


		}
		public ActionResult saveInsuranceQuotation(string jsonData)
		{
			try
			{
				insurancequotation insurancequotations = JsonConvert.DeserializeObject<insurancequotation>(jsonData);

				
				using (var db = new AutoSherDBContext())
				{
					insurancequotations.createdCRE = Session["UserName"].ToString();
					insurancequotations.createdDate = DateTime.Now;
					db.insurancequotations.Add(insurancequotations);
					db.SaveChanges();
					return Json(new { success = true });
				}
			}
			catch(Exception ex)
			{

			}
			return View();
		}

		/******************** Calculator End **********************/

		/*********************** Scheduling *********************/


		public ActionResult WalkinExecutivesByLocation(long locaId)
		{

			try
			{

				List<insuranceagent> walkinExe = walkinExecutivesByLoca(locaId, "2");
				var data = JsonConvert.SerializeObject(walkinExe);
				return Json(new { success = true, insuranceAgentList = walkinExe }) ;
			}
			catch (Exception ex)
			{
				return Json("false");
			}
			
		}

		public List<insuranceagent> walkinExecutivesByLoca(long locaId, string type)
		{
			List<insuranceagent> newuserList = new List<insuranceagent>();
			try
			{
				using (var db = new AutoSherDBContext())
				{
					db.Configuration.LazyLoadingEnabled = false;
					long walkinLocationExist =db.fieldwalkinlocations.Count(u=>u.id==locaId);
					if (walkinLocationExist > 0)
					{
						List<userfieldwalkinlocation> userList = db.userfieldwalkinlocations.Where(m=>m.fieldWalkinLocationList_id==locaId).ToList();
						foreach (userfieldwalkinlocation sa in  userList)
						{
							Console.WriteLine("sa userlist : " + sa.fieldWalkinLocationList_id);
							long userId = sa.userFieldWalkinLocation_id;
							insuranceagent saData = db.insuranceagents.FirstOrDefault(u => u.wyzuser.id == userId);
							if (saData != null)
							{
								newuserList.Add(saData);
							}
						}
					}
				}
			}
			catch(Exception ex)
			{

			}
			return newuserList;
		}
		public List<string> getFSEScheduleByWyzUser(long userId, string scheduleDate)
		{
			
			try
			{
				using (var db = new AutoSherDBContext())
				{
					insuranceagent ins = db.insuranceagents.Where(u => u.wyzUser_id == userId).FirstOrDefault();

					long id = ins.insuranceAgentId;


					var pickupid = db.appointmentbookeds.FirstOrDefault(u => u.insuranceAgent_insuranceAgentId == userId).pickupDrop_id;

					var pickupListOfTodayFrom = db.pickupdrops.Where(u => u.id == pickupid && u.pickupDate.ToString()==scheduleDate).Select(u => u.timeFrom);
					var pickupListOfTodayTo = db.pickupdrops.Where(u => u.id == pickupid && u.pickupDate.ToString() == scheduleDate).Select(u => u.timeTo).ToList();

					List<appointmentbooked> d = new List<appointmentbooked>();
					
				}
			}
			catch(Exception ex)
			{

			}
			return null;
			}
		//public List<bookingdatetime> getTimeSlot()
		//{
		//	try
		//	{
		//		using (var db = new AutoSherDBContext())
		//		{
		//			return db.bookingdatetimes.ToList();
		//		}
		//	}
		//	catch (Exception ex)
		//	{

		//	}
		//	return null;
		//	}

		//public ActionResult filedExecutivesListToSchedule(string scheduleDate, long? locaId)
		//{
		//	try
		//	{
		//		using (var db = new AutoSherDBContext())
		//		{
		//			db.Configuration.LazyLoadingEnabled = false;
		//			long customerId=Convert.ToInt64(Session["CusId"]);
		//			List<PickupDropDataOnTabLoad> UsersList = new List<PickupDropDataOnTabLoad>();
		//			List<bookingdatetime> timeSlots =getTimeSlot();
		//			var fSElist = (from f in db.fieldwalkinlocations.Where(x => x.id == locaId && (x.typeOfLocation == "1"|| x.typeOfLocation=="3"))
		//						   from u in db.userfieldwalkinlocations.Where(x => x.fieldWalkinLocationList_id == f.id)
		//						   select new
		//						   {
		//							   u.userFieldWalkinLocation_id
		//						   }).ToList();

		//			foreach(var lst in fSElist)
		//			{
		//				PickupDropDataOnTabLoad pickup = new PickupDropDataOnTabLoad();
		//				insuranceagent ins = db.insuranceagents.Where(u => u.wyzUser_id == lst.userFieldWalkinLocation_id).FirstOrDefault();
		//				long id = ins.insuranceAgentId;
		//				if (db.appointmentbookeds.Any(x => x.insuranceAgent_insuranceAgentId == id))
		//				{
		//					List<TimeSpan> datesList = new List<TimeSpan>();
		//					DateTime sDate = Convert.ToDateTime(scheduleDate);

		//					var pickupidList = (from app in db.appointmentbookeds where app.insuranceBookStatus_id!=35 && app.insuranceAgent_insuranceAgentId == id && app.appointmentDate == sDate select app.pickupDrop_id).ToList();
		//					foreach (var pickupid in pickupidList)
		//					{
		//						var pickupListOfTodayFrom = db.pickupdrops.FirstOrDefault(u => u.id == pickupid && u.pickupDate == sDate);
		//						var pickupListOfTodayTo = db.pickupdrops.FirstOrDefault(u => u.id == pickupid && u.pickupDate == sDate);
		//						if (pickupListOfTodayFrom != null && pickupListOfTodayTo != null)
		//						{
		//							TimeSpan t1 = pickupListOfTodayFrom.timeFrom ?? default(TimeSpan);
		//							TimeSpan t2 = pickupListOfTodayFrom.timeTo ?? default(TimeSpan);

		//							TimeSpan span = t1 - t2;


		//							int start = t1.Hours;
		//							int End = t2.Hours;
		//							int EntriesCount;
									
		//							string startTime = t1.Hours + ":" + t1.Minutes;
		//							string endTime = t2.Hours + ":" + t2.Minutes;

		//							TimeSpan duration = DateTime.Parse(endTime).Subtract(DateTime.Parse(startTime));
		//							EntriesCount = duration.Hours;
		//							EntriesCount = EntriesCount + EntriesCount;
		//							if(duration.Minutes==30)

		//							{
		//								EntriesCount = EntriesCount + 1;
		//							}

		//							TimeSpan StartTime = TimeSpan.FromHours(start);
		//							int Difference = 30; //In minutes.
		//							int j;
		//							if (t1.Minutes==0)
		//							{
		//								j = 0;
		//								//EntriesCount=EntriesCount + 1;
		//							}
		//							else
		//							{
		//								j = 1;
		//								EntriesCount = EntriesCount + 1;
		//							}

		//							Dictionary<TimeSpan, TimeSpan> Entries = new Dictionary<TimeSpan, TimeSpan>();
		//							for (int i = j; i < EntriesCount; i++)
		//							{
		//									Entries.Add(StartTime.Add(TimeSpan.FromMinutes(Difference * i)),StartTime.Add(TimeSpan.FromMinutes(Difference * i)));
										
		//							}
		//							foreach (var e in Entries)
		//							{
		//								datesList.Add(e.Key);
		//							}
		//						}
								
		//						pickup.listTime = datesList;
		//					}
		//				}

		//				pickup.id = ins.insuranceAgentId;
		//				pickup.userName = ins.insuranceAgentName;
		//				UsersList.Add(pickup);
		//			}
		//			SchedularDataOnTabLoad driverData = new SchedularDataOnTabLoad();
		//			return Json(new { UsersList, timeSlots , JsonRequestBehavior.AllowGet });
		//		}
		//	}
		//	catch(Exception ex)
		//	{

		//	}
		//	return View();
		//}
	}
}