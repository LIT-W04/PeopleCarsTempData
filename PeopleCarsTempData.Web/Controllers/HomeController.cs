using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PeopleCarsTempData.Data;
using PeopleCarsTempData.Web.Models;

namespace PeopleCarsTempData.Web.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            IndexViewModel viewModel = new IndexViewModel();
            PersonDb db = new PersonDb(Properties.Settings.Default.ConStr);
            viewModel.People = db.GetPeople();
            if (TempData.ContainsKey("green-message"))
            {
                viewModel.GreenMessage = (string)TempData["green-message"];
            }
            if (TempData.ContainsKey("red-message"))
            {
                viewModel.RedMessage = (string) TempData["red-message"];
            }
            return View(viewModel);
        }

        public ActionResult AddPerson()
        {
            return View();
        }

        [HttpPost]
        public ActionResult AddPerson(Person person)
        {
            PersonDb db = new PersonDb(Properties.Settings.Default.ConStr);
            db.AddPerson(person);
            TempData["green-message"] = "Person added successfully";
            return RedirectToAction("Index");
        }

        public ActionResult ViewCars(int personId)
        {
            CarDb carDb = new CarDb(Properties.Settings.Default.ConStr);
            PersonDb personDb = new PersonDb(Properties.Settings.Default.ConStr);
            ViewCarsViewModel viewModel = new ViewCarsViewModel();
            viewModel.Person = personDb.GetPerson(personId);
            viewModel.Cars = carDb.GetCars(personId);
            return View(viewModel);
        }

        [HttpPost]
        public ActionResult DeletePerson(int personId)
        {
            CarDb carDb = new CarDb(Properties.Settings.Default.ConStr);
            PersonDb personDb = new PersonDb(Properties.Settings.Default.ConStr);
            carDb.DeleteCarsForPerson(personId);
            personDb.Delete(personId);
            TempData["red-message"] = "Cars and Person were deleted successfully";
            return RedirectToAction("Index");
        }

        public ActionResult AddCar(int personId)
        {
            PersonDb personDb = new PersonDb(Properties.Settings.Default.ConStr);
            AddCarViewModel viewModel = new AddCarViewModel();
            viewModel.Person = personDb.GetPerson(personId);
            return View(viewModel);
        }

        [HttpPost]
        public ActionResult AddCar(Car car)
        {
            CarDb carDb = new CarDb(Properties.Settings.Default.ConStr);
            carDb.AddCar(car);
            TempData["green-message"] = "Car added successfully";
            return RedirectToAction("Index");
        }

        [HttpPost]
        public ActionResult DeleteCar(int carId)
        {
            CarDb carDb = new CarDb(Properties.Settings.Default.ConStr);
            carDb.Delete(carId);
            TempData["red-message"] = "Car deleted successfully";
            return Redirect("Index");
        }
    }
}