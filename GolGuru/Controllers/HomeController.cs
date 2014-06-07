using GolGuru.Data;
using GolGuru.Models;
using System.Collections.Generic;
using System.Web.Mvc;
using System.Web.Security;

namespace GolGuru.Controllers
{
    public class HomeController : Controller
    {
        protected static DataLayerDataContext db = new DataLayerDataContext();
        //mocking
        public static class UsersMock
        {
            public static string Username = "gaston";
            public static string Password = "gaston";
        }

        [HttpPost]
        public ActionResult Login(string username, string password)
        {

            if (username != null && password != null)
            {
                if (username == UsersMock.Username && password == UsersMock.Password)
                {
                    FormsAuthentication.SetAuthCookie(username, false);
                    return this.RedirectToAction("MainMenu");
                }

            }
            return this.View("Index");

        }
        public ActionResult MainMenu()
        {
            var matchStatus = GolGuru.Instance.GetMatchStatus();
            ViewBag.MatchStatus = matchStatus;
            return this.View();
        }
        public ActionResult Index()
        {
            ViewBag.Message = "Modify this template to jump-start your ASP.NET MVC application.";

            return View();
        }
        [HttpGet]
        public void NotReady()
        {
            GolGuru.Instance.SetMatchStatus(Helpers.Helpers.MatchStatusEnum.NotReady);

        }
        [HttpGet]
        public void NotStarted()
        {
            GolGuru.Instance.SetMatchStatus(Helpers.Helpers.MatchStatusEnum.NotStarted);

        }
        [HttpGet]
        public void StartedFirst()
        {
            GolGuru.Instance.SetMatchStatus(Helpers.Helpers.MatchStatusEnum.StartedFirst);

        }
        [HttpGet]
        public void FinishedFirst()
        {
            GolGuru.Instance.SetMatchStatus(Helpers.Helpers.MatchStatusEnum.FinishedFirst);

        }
        [HttpGet]
        public void StartedSecond()
        {
            GolGuru.Instance.SetMatchStatus(Helpers.Helpers.MatchStatusEnum.StartedSecond);

        }
        //[HttpGet]
        //public void FinishedSecond()
        //{
        //    GolGuru.Instance.SetMatchStatus(Helpers.Helpers.MatchStatusEnum.FinishedSecond);

        //}
        [HttpGet]
        public void Results()
        {
            GolGuru.Instance.SetMatchStatus(Helpers.Helpers.MatchStatusEnum.Results);

        }

        [HttpGet]
        public void Figura()
        {
            var figura = new Question
            {
                Id = "5",
                tipo = "Figura",
                segundos = "10",
                pregunta = "Quien sera la figura del partido?",
                respuesta = new List<Answer>
                                                 {
                                                     new Answer { Id = "1", puntaje = "250", respuesta = "Messi" },
                                                     new Answer { Id = "2", puntaje = "250", respuesta = "Romero" },
                                                     new Answer { Id = "3", puntaje = "250", respuesta = "Aguero"},
                                                     new Answer { Id = "4", puntaje = "250", respuesta = "Higuain" },
                                                     new Answer { Id = "5", puntaje = "250", respuesta = "Mascherano" }
                                                 }
            };

            GolGuru.Instance.BrodcastMatchLiveQuestions("Figura", figura);

        }
        [HttpGet]
        public void UltimoGol()
        {
            var ultimoGol = new Question
            {
                Id = "6",
                tipo = "Ultimo Gol",
                segundos = "15",
                pregunta = "Cuando se marcara el ultimo gol",
                respuesta = new List<Answer>
                                                 {
                                                     new Answer { Id = "1", puntaje = "250", respuesta = "70/75 min" },
                                                     new Answer { Id = "2", puntaje = "250", respuesta = "75/80 min" },
                                                     new Answer { Id = "3", puntaje = "250", respuesta = "80/85 min"},
                                                     new Answer { Id = "4", puntaje = "250", respuesta = "85/90 min" },
                                                     new Answer { Id = "5", puntaje = "250", respuesta = "Nunca" }
                                                 }
            };

            GolGuru.Instance.BrodcastMatchLiveQuestions("UltimoGol", ultimoGol);

        }

        [HttpGet]
        public void EntreTiempo()
        {
            var entreTiempo = new Question
            {
                Id = "8",
                tipo = "Entretiempo",
                segundos = "900",
                pregunta = "Quien entrara primero",
                respuesta = new List<Answer>
                                                 {
                                                     new Answer { Id = "1", puntaje = "500", respuesta = "Orion" },
                                                     new Answer { Id = "2", puntaje = "300", respuesta = "Zabaleta" },
                                                     new Answer { Id = "3", puntaje = "250", respuesta = "Rojo"},
                                                     new Answer { Id = "4", puntaje = "350", respuesta = "Pastore" },
                                                     new Answer { Id = "5", puntaje = "250", respuesta = "Maxi Rodriguez" },
                                                     new Answer { Id = "6", puntaje = "200", respuesta = "Palacio" },
                                                     new Answer { Id = "7", puntaje = "450", respuesta = "Lavezzi" }
                                                 }
            };

            GolGuru.Instance.BrodcastMatchLiveQuestions("EntreTiempo", entreTiempo);

        }




        public ActionResult About()
        {
            ViewBag.Message = "Your app description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

    }
}
