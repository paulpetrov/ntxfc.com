using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Xml;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Configuration;

using FlyingClub.Common;
using FlyingClub.Data.Model.Entities;
using FlyingClub.WebApp.Extensions;
using FlyingClub.BusinessLogic;
using FlyingClub.WebApp.Models;

namespace FlyingClub.WebApp.Controllers
{
    public class AircraftController : BaseController
    {
        private IClubDataService _dataService;

        public AircraftController()
        {
            _dataService = new ClubDataService();
        }

        /// <summary>
        /// GET: /Aircraft/
        /// </summary>
        /// <returns></returns>
        public ViewResult Index()
        {
            AircraftIndexViewModel model = new AircraftIndexViewModel()
            {
                AircraftList = _dataService.GetAllAirplanes()
            };

            return View(model);
        }

        /// <summary>
        /// GET: /Aircraft/Details/5
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult Details(int id)
        {
            Aircraft aircraft = _dataService.GetAircraftById(id);

            if (aircraft == null)
                throw new HttpException(404, "Aircraft not found");

            AircraftImageListViewModel imageModel = new AircraftImageListViewModel();
            imageModel.ImageList = aircraft.Images;

            AircraftDetailsViewModel model = new AircraftDetailsViewModel()
            {
                Aircraft = aircraft,
                AircraftImages = imageModel
            };

            return View(model);
        }

        public ViewResult ListOwners(int? id)
        {
            List<Aircraft> aircraft = new List<Aircraft>();
            if (id == null)
                aircraft = _dataService.GetAllAircraftWithOwners();
            else
                aircraft.Add(_dataService.GetAircraftById((int)id));

            List<AircraftOwnersListViewModel> viewModel = new List<AircraftOwnersListViewModel>();
            foreach (var ac in aircraft)
            {
                AircraftOwnersListViewModel vmItem = new AircraftOwnersListViewModel();
                vmItem.AircraftId = ac.Id;
                vmItem.RegistrationNumber = ac.RegistrationNumber;
                foreach (var owner in ac.Owners)
                {
                    MemberViewModel memberVM = new MemberViewModel()
                    {
                        Id = owner.Id,
                        FirstName = owner.FirstName,
                        LastName = owner.LastName,
                        PrimaryEmail = owner.Login.Email,
                        Phone = owner.Phone,
                        AltPhone = owner.AltPhone
                    };

                    vmItem.Owners.Add(memberVM);
                }

                viewModel.Add(vmItem);
            }

            return View(ViewNames.AircraftOwners, viewModel);
        }

        public ActionResult ViewLog(int aircraftId, string regNum)
        {
            AircraftLogModel vm = new AircraftLogModel();
            vm.AircraftId = aircraftId;
            vm.RegistrationNumber = regNum;

            Aircraft aircraft = _dataService.GetAircraftById(vm.AircraftId);
            vm.LastUpdatedOn = aircraft.LogUpdloadedOn;
            if (vm.LastUpdatedOn != null)
            {
                Member member = _dataService.GetMember((int)aircraft.LogUploadedByMemberId);
                vm.UpdatedBy = member.FullName;
            }

            string relFolderUrl = Url.Content("~/Content/AircraftLogs/" + regNum);
            string absFolderUrl = Server.MapPath(relFolderUrl);
            if (System.IO.Directory.Exists(absFolderUrl))
            {
                string[] files = System.IO.Directory.GetFiles(Server.MapPath(relFolderUrl));
                if (files.Length > 0)
                {
                    vm.Pages = new List<LogPageModel>();
                    for (int i = 0; i < files.Length; ++i)
                    {
                        string fileName = Path.GetFileName(files[i]);
                        LogPageModel page = new LogPageModel()
                        {
                            PageNumber = Int32.Parse(Path.GetFileNameWithoutExtension(fileName)),
                            Url = relFolderUrl + "/" + fileName
                        };
                        vm.Pages.Add(page);
                        vm.Pages.Sort();
                    }
                } 
            }

            if (vm.Pages != null)
                vm.EditPageNumber = vm.Pages.Last().PageNumber + 1;
            else
                vm.EditPageNumber = 1;

            if (User.IsInRole(FlyingClub.Common.UserRoles.Admin.ToString()) 
                || User.IsInRole(FlyingClub.Common.UserRoles.AircraftOwner.ToString()) 
                || User.IsInRole(FlyingClub.Common.UserRoles.AircraftMaintenance.ToString()))
                vm.CanEdit = true;
            else
                vm.CanEdit = false;

            return View("MaintenanceLog", vm);
        }

        [HttpPost]
        [Authorize(Roles = "Admin,AircraftOwner,AircraftMaintenance")]
        public ActionResult AddLogPage(AircraftLogModel viewModel, HttpPostedFileBase fileBase)
        {
            string relFolderUrl = Url.Content("~/Content/AircraftLogs/" + viewModel.RegistrationNumber);
            string absFolderUrl = Server.MapPath(relFolderUrl);
            string origFileUrl = Server.MapPath(Path.Combine(relFolderUrl, viewModel.EditPageNumber.ToString() + "orig.jpg"));
            string pageFileName = viewModel.EditPageNumber.ToString() + ".jpg";
            string pageUrl = Server.MapPath(Path.Combine(relFolderUrl, pageFileName));
            if (System.IO.File.Exists(pageUrl))
            {
                ModelState.AddModelError(String.Empty, "The page number " + viewModel.EditPageNumber.ToString() + " already exists. Please delete it first or use different number.");
                return ViewLog(viewModel.AircraftId, viewModel.RegistrationNumber);
            }

            if (!System.IO.Directory.Exists(absFolderUrl))
                System.IO.Directory.CreateDirectory(absFolderUrl);

            fileBase.SaveAs(origFileUrl);
            ImageHelper.ScaleToWidth(origFileUrl, pageUrl, 800);

            if (System.IO.File.Exists(pageUrl))
            {
                Aircraft aircraft = _dataService.GetAircraftById(viewModel.AircraftId);
                aircraft.LogUpdloadedOn = DateTime.Now;
                aircraft.LogUploadedByMemberId = ProfileCommon.GetUserProfile().MemberId;
                _dataService.UpdateAircraft(aircraft);

                System.IO.File.Delete(origFileUrl);
            }

            ModelState.Clear();
            return ViewLog(viewModel.AircraftId, viewModel.RegistrationNumber);
        }

        [Authorize(Roles = "Admin,AircraftOwner,AircraftMaintenance")]
        public ActionResult DeleteLogPage(int aircraftId, string regNum, int pageNum)
        {
            string relFolderUrl = Url.Content("~/Content/AircraftLogs/" + regNum);
            string pageFileName = pageNum.ToString() + ".jpg";
            string fileUrl = Server.MapPath(Path.Combine(relFolderUrl, pageFileName));

            System.IO.File.Delete(fileUrl);

            return ViewLog(aircraftId, regNum);
        }

        //
        // GET: /Aircraft/Create
        [Authorize(Roles = "Admin")]
        public ActionResult Create()
        {
            return View(new AircraftEditViewModel());
        }

        /// <summary>
        /// POST: /Aircraft/Create
        /// </summary>
        /// <param name="viewModel"></param>
        /// <returns></returns>
        [Authorize(Roles = "Admin")]
        [HttpPost]
        public ActionResult Create(AircraftEditViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                if (_dataService.GetAircraftByRegNumber(viewModel.RegistrationNumber) == null)
                {
                    Aircraft entity = new Aircraft();
                    entity = CopyDataFromViewModel(entity, viewModel);
                    _dataService.AddAircraft(entity);
                    return RedirectToAction("Index");
                }
                else
                {
                    ModelState.AddModelError("RegistrationNumber", "Aircraft with registration number already exists in the database");
                }
            }

            return View(viewModel);
        }

        /// <summary>
        /// GET: /Aircraft/Edit/5
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [Authorize(Roles = "Admin,AircraftOwner,AircraftMaintenance")]
        public ActionResult Edit(int id)
        {
            Aircraft aircraft = _dataService.GetAircraftById(id);
            AircraftEditViewModel viewModel = new AircraftEditViewModel(aircraft);

            return View(viewModel);
        }


        /// <summary>
        /// POST: /Aircraft/Edit/5
        /// </summary>
        /// <param name="aircraft"></param>
        /// <returns></returns>
        [HttpPost]
        [Authorize(Roles = "Admin,AircraftOwner,AircraftMaintenance")]
        public ActionResult Edit(AircraftEditViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                Aircraft aircraft = _dataService.GetAircraftById(viewModel.Id);
                if (aircraft.RegistrationNumber != viewModel.RegistrationNumber &&
                    _dataService.GetAircraftByRegNumber(viewModel.RegistrationNumber) != null)
                {
                    ModelState.AddModelError("RegistrationNumber", "Aircraft with registration number already exists in the database");
                }
                else
                {
                    aircraft = CopyDataFromViewModel(aircraft, viewModel);
                    _dataService.UpdateAircraft(aircraft);
                    return RedirectToAction("Details", new { id = viewModel.Id });
                }
            }
            return View(viewModel);
        }

        /// <summary>
        /// GET: /Aircraft/Delete/5
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [Authorize(Roles = "Admin")]
        public ActionResult Delete(int id)
        {
            _dataService.DeleteAircraft(id);
            return RedirectToAction("Index");
        }

        public ActionResult Status(int? id)
        {
            var model = new List<AircraftStatusViewModel>();

            if (id == null)
            {
                model = _dataService.GetAllAirplanes().Select(x => x.ConvertToAircraftStatusViewModel()).ToList();
            }
            else
            {
                var aircraft = _dataService.GetAircraftById(id.Value).ConvertToAircraftStatusViewModel();
                if (aircraft != null)
                    model.Add(aircraft);
            }

            // format image url
            foreach (var ac in model)
            {
                if (ac.ImageUrl != String.Empty)
                    ac.ImageUrl = Url.Content(ConfigurationManager.AppSettings["AircraftImages"] + "/" + ac.ImageUrl);
            }

            return View(model);
        }

        public ActionResult GetRates()
        {
            List<Aircraft> aircraft = _dataService.GetAllAirplanes();
            List<AircraftListItemViewModel> vmList = new List<AircraftListItemViewModel>();
            foreach (var ac in aircraft)
            {
                AircraftListItemViewModel vm = new AircraftListItemViewModel()
                {
                    RegistrationNumber = ac.RegistrationNumber,
                    HourlyRate = ac.HourlyRate.ToString(),
                    Make = ac.Make,
                    Model = ac.Model,
                    CheckoutRequirements = ac.CheckoutRequirements
                };

                if (ac.Images.Count > 0)
                {
                    AircraftImage img = ac.Images.FirstOrDefault(im => im.Type == AircraftImageTypes.ExteriorMain.ToString());
                    if (img != null)
                        vm.ImageUrl = Url.Content(ConfigurationManager.AppSettings["AircraftImages"] + "/" + img.FileName_Small);
                }

                vmList.Add(vm);
            }

            return View(ViewNames.AircraftRates, vmList);
        }

        public ActionResult WeightAndBalance(int aircraftId)
        {
            Aircraft aircraft = _dataService.GetAircraftById(aircraftId);
            string wbUrl = Server.MapPath(ConfigurationManager.AppSettings["WeightAndBalanceFolder"] + "/" + aircraft.RegistrationNumber + ".xml");

            WeightAndBalanceViewModel viewModel = new WeightAndBalanceViewModel();
            viewModel.RegistrationNumber = aircraft.RegistrationNumber;
            viewModel.UsefulLoad = (double)aircraft.UsefulLoad;
            viewModel.Model = aircraft.Model;
            viewModel.TypeDesignation = aircraft.TypeDesignation;

            if (!System.IO.File.Exists(wbUrl))
            {
                viewModel.IsDataAvailable = false;
                return View(ViewNames.WeightAndBalance, viewModel);
            }

            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load(wbUrl);

            XmlNode node = xmlDoc.SelectSingleNode("/wb/aircraft");
            viewModel.Intialize(node);
            viewModel.PayloadWithFullFuel = viewModel.UsefulLoad - viewModel.FuelCapacity * 6;


            return View(ViewNames.WeightAndBalance, viewModel);
        }

        [Authorize(Roles = "Admin")]
        public ActionResult AddOwner(int aircraftId)
        {
            List<Member> members = _dataService.GetAllMembersByRole(UserRoles.AircraftOwner.ToString());
            Aircraft aircraft = _dataService.GetAircraftById(aircraftId);
            members.RemoveAll(m => aircraft.Owners.Any(o => o.Id == m.Id));

            AddAircraftOwnerViewModel viewModel = new AddAircraftOwnerViewModel();
            viewModel.AircraftId = aircraftId;
            viewModel.RegistrationNumber = aircraft.RegistrationNumber;
            viewModel.OwnerId = -1;
            viewModel.ClubMembers = members.Select(o => new AircraftOwnerInfo() { OwnerId = o.Id, Name = o.FirstName + " " + o.LastName }).ToList();
            viewModel.ClubMembers.Sort(CompareOwners);

            return View(ViewNames.AddAircraftOwner, viewModel);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public ActionResult AddOwner(AddAircraftOwnerViewModel viewModel)
        {
            _dataService.AddOwner(viewModel.AircraftId, viewModel.OwnerId);
            return RedirectToAction("Edit", new { id = viewModel.AircraftId });
        }

        private int CompareOwners(AircraftOwnerInfo a, AircraftOwnerInfo b)
        {
            return a.Name.CompareTo(b.Name);
        }

        [Authorize(Roles = "Admin")]
        public ActionResult RemoveOwner(int aircraftId, int ownerId)
        {
            _dataService.RemoveOwner(aircraftId, ownerId);
            return RedirectToAction("Edit", new { id = aircraftId });
        }

        [Authorize(Roles = "Admin")]
        public ActionResult RemoveOwnerConfirm(int aircraftId, int ownerId)
        {
            ViewBag.aircraftId = aircraftId;
            ViewBag.ownerId = ownerId;
            return View(ViewNames.RemoveAircraftOwner);
        }

        [HttpGet]
        [Authorize(Roles = "Admin,AircraftOwner")]
        public ActionResult AddImage(int aircraftId, string tailNumber)
        {
            AircraftImageViewModel viewModel = new AircraftImageViewModel()
            {
                AircraftId = aircraftId,
                RegistrationNumber = tailNumber
            };

            return View(ViewNames.AddAircraftImage, viewModel);
        }

        [HttpPost]
        [Authorize(Roles = "Admin,AircraftOwner")]
        public ActionResult AddImage(HttpPostedFileBase file, AircraftImageViewModel viewModel)
        {
            if (file != null && file.ContentLength > 0)
            {
                var fileName = Path.GetFileName(file.FileName);
                var path = Path.Combine(Server.MapPath(ConfigurationManager.AppSettings["AircraftImages"]), fileName);//Path.Combine(Server.MapPath("~/Content/AircraftImages"), fileName);
                file.SaveAs(path);

                // create small, medium and large versions
                List<string> validationErrors = ImageHelper.CreateImageSet(path);
                if (validationErrors.Count > 0)
                {
                    foreach (var error in validationErrors)
                    {
                        ModelState.AddModelError("", error);
                    }
                    return View(ViewNames.AddAircraftImage, viewModel);
                }

                AircraftImage image = new AircraftImage()
                {
                    AircraftId = viewModel.AircraftId,
                    Descritpion = viewModel.Description,
                    Title = viewModel.Title,
                    Type = viewModel.Type,
                    FileName = fileName,
                    FileName_Large = Path.GetFileNameWithoutExtension(fileName) + ".lrg" + Path.GetExtension(fileName),
                    FileName_Medium = Path.GetFileNameWithoutExtension(fileName) + ".med" + Path.GetExtension(fileName),
                    FileName_Small = Path.GetFileNameWithoutExtension(fileName) + ".small" + Path.GetExtension(fileName),
                };

                _dataService.AddAircraftImage(image);
            }
            return RedirectToAction("Edit", new { id = viewModel.AircraftId });
        }

        [Authorize(Roles = "Admin")]
        public ActionResult RemoveImageConfirm(int imageId, int aircraftId, string tailNumber)
        {
            ViewBag.imageId = imageId;
            ViewBag.aircraftId = aircraftId;
            ViewBag.tailNumber = tailNumber;
            return View("RemoveImage");
        }

        [Authorize(Roles = "Admin")]
        public ActionResult RemoveImage(int imageId, int aircraftId)
        {
            _dataService.RemoveAircraftImage(imageId);
            return RedirectToAction("Edit", new { id = aircraftId });
        }

        private Aircraft CopyDataFromViewModel(Aircraft entity, AircraftEditViewModel viewModel)
        {
            entity.Year = viewModel.Year;
            entity.BasedAt = viewModel.BasedAt;
            entity.Make = viewModel.Make;
            entity.CheckoutRequirements = viewModel.CheckoutRequirements;
            entity.CruiseSpeed = viewModel.CruiseSpeed;
            entity.Description = viewModel.Description;
            entity.EquipmentList = viewModel.EquipmentList;
            entity.FuelCapacity = viewModel.FuelCapacity;
            entity.HourlyRate = viewModel.HourlyRate;
            entity.MaxGrossWeight = viewModel.MaxGrossWeight;
            entity.MaxRange = viewModel.MaxRange;
            entity.Model = viewModel.Model;
            entity.Name = viewModel.Name;
            entity.OperationalNotes = viewModel.OperationalNotes;
            entity.RegistrationNumber = viewModel.RegistrationNumber;
            entity.Status = viewModel.Status;
            entity.StatusNotes = viewModel.StatusNotes;
            entity.TypeDesignation = viewModel.TypeDesignation;
            entity.UsefulLoad = viewModel.UsefulLoad;
            return entity;
        }
    }
}