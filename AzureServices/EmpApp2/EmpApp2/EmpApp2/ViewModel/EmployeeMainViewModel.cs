﻿using EmpApp2.Enums;
using EmpApp2.Model;
using EmpApp2.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;
using XLabs.Platform.Device;

namespace EmpApp2.ViewModel
{
    public class EmployeeMainViewModel : BaseViewModel
    {
        public EmployeeDB EmpDb;
        #region ImageKConstants
        public string d1_dessert_v1 = "https://lh3.googleusercontent.com/-Xh8aY2RRwd0/VeFCHSv2lzI/AAAAAAAAAOY/8SY3a6qk7WM/d1_icecream.png";
        public string flkr_image1 = "https://lh3.googleusercontent.com/-Xh8aY2RRwd0/VeFCHSv2lzI/AAAAAAAAAOY/8SY3a6qk7WM/d1_icecream.png";
        public const string login = "Check In";
        public const string logOut = "Check Out";
        #endregion

        public EmployeeDetails EmpDetails { get; set; }
        public EmployeeMainViewModel(Page page)
            : base(page)
        {
            Title = "Employee Roll Check";
            GetEmployeeDetails();

            BtnName = login;
            EmpDb = new EmployeeDB();
        }

        public EmployeeMainViewModel(IDevice device)
            : base(device)
        {
            Message = String.Format("Hello Xamarin Forms Labs MVVM Basics!! How is your {0} device", device.Manufacturer);
            Title = "Employee Roll Check";
            Message = "Employee Roll Check";
            EmpDb = new EmployeeDB();
        }
        public EmployeeMainViewModel()
        {
            Title = "Employee Roll Check";
            Message = "Employee Roll Check";
            EmpDb = new EmployeeDB();
        }

        private Command getEmployeesCommand;
        public Command GetEmployeesCommand
        {
            get
            {
                return getEmployeesCommand ??
                    (getEmployeesCommand = new Command(async () => await ExecuteEmployeeLogCommand(), () => { return !IsBusy; }));
            }
        }
        public async Task ExecuteEmployeeLogCommand()
        {
           
            if (IsBusy)
            return;
            
            IsBusy = true;
            //Delay for 5 seconds
            //await Task.Delay(5000);
            GetEmployeesCommand.ChangeCanExecute();
            var showAlert = false;
           
            try
            {
                var empList = GetEmployeeLog();
                var empDb = new EmployeeDetails() {
                    EmpID = 1,
                    EmpName = "James CodeNutz",
                    JobTitle = "Xam Developer",
                    ThumbUrl = d1_dessert_v1,
                    EmployeeLogs = empList.ToList(),

                };

                EmpDetails = empDb;
                IsBusy = false;
            }
            catch (Exception ex)
            {
                showAlert = true;
                //Xamarin.Insights.Report(ex);
            }
            finally
            {
                IsBusy = false;
                GetEmployeesCommand.ChangeCanExecute();
            }
        }


        Command checkInCommand;
        public Command CheckInCommand
        {
            get
            {
                return checkInCommand ??
                    (checkInCommand = new Command(async () => await ExecuteCheckInCommand(), () => { return !IsBusy; }));
            }
        }

        public async Task ExecuteCheckInCommand()
        {
            if (IsBusy)
                return;
            CheckInCommand.ChangeCanExecute();
            try
            {
                if (btnName == login)
                    btnName = logOut;
                else
                    btnName = login;
                OnPropertyChanged("BtnName");
            }
            catch (Exception ex)
            {
                IsBusy = false;
            }
            finally
            {
                IsBusy = false;
                CheckInCommand.ChangeCanExecute();
            }
        }

        string btnName = string.Empty;
        public string BtnName
        {
            get { return btnName; }
            set{SetProperty(ref btnName, value);}
        }

        public void GetEmployeeDetails()
        {
            var empList = GetEmployeeLog();
            var empDb = new EmployeeDetails()
            {
                EmpID = 1,
                EmpName = "James CodeNutz",
                JobTitle = "Xam Developer",
                ThumbUrl = flkr_image1,
                EmployeeLogs = empList.ToList(),
            };

            EmpDetails = empDb;
        }

        public IEnumerable<EmployeeLog> GetEmployeeLog()
        {
            EmpDb = new EmployeeDB();
            var dCurrent = DateTime.Now;
            var empsorted = new List<EmployeeLog>();
            try
            {
                var IempLogs = EmpDb.GetEmployeeLogList();
                var empLogs = IempLogs.Where(c => c.EmpId == 1).ToList();

                var emplogDetails = empLogs.Select(c => new EmployeeLog()
                {
                    EmpID = c.EmpId,
                    LogTime = Convert.ToDateTime(c.LogTime),
                    LogType = c.LogType
                }).ToList();
                empsorted = emplogDetails.OrderByDescending(c => c.LogTime).ToList();
            }
            catch (Exception ex)
            {
                string msg = ex.Message;
            }

            

            
            return empsorted;
        }


        //private void Sort()
        //{
        //    var empdetails = EmpDetails;
        //    var sorted = (from emp in EmpDetails.EmployeeLogs
        //                 orderby emp.LogTime descending
        //                 select emp).ToList<EmployeeLog>();



        //}

    }
}
