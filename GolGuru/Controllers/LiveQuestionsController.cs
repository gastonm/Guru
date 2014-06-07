using System;
using System.Collections.Generic;
using System.Globalization;
using GolGuru.Data;
using GolGuru.Models;
using Microsoft.AspNet.SignalR;
using System.Linq;
using System.Web.Mvc;
using System.Web.Security;
using Newtonsoft.Json;

namespace GolGuru.Controllers
{
    public class LiveQuestionsController : Controller
    {
        protected static DataLayerDataContext db = new DataLayerDataContext();

        [HttpGet]
        public ActionResult CornerLiveQuestion()
        {
            var corner = new Question
            {
                Id = GolGuru.Instance.GetMatchLiveQuestionId().ToString(CultureInfo.InvariantCulture),
                tipo = "Corner",
                segundos = "10",
                pregunta = "Corner para Argentina",
                respuesta = new List<Answer>
                                                 {
                                                     new Answer { Id = "1", puntaje = "550", respuesta = "Gol Olimpico" },
                                                     new Answer { Id = "2", puntaje = "250", respuesta = "Defensa/arquero despeja" },
                                                     new Answer { Id = "3", puntaje = "350", respuesta = "Defensa/arquero controla" },
                                                     new Answer { Id = "4", puntaje = "350", respuesta = "Pase corto" },
                                                     new Answer { Id = "5", puntaje = "400", respuesta = "Ataque conecta" }
                                                 }
            };
            GolGuru.Instance.BrodcastMatchLiveQuestions("Corner", corner);
            return View();

        }
        [HttpPost]
        public ActionResult CornerLiveQuestion(Answer corner)
        {

            var answer = corner;
            answer.Id = GolGuru.Instance.GetMatchLiveQuestionIdResponse().ToString(CultureInfo.InvariantCulture);

            var rightAnswer = new RightAnswer()
                            {
                                AnswerId = Convert.ToInt16(answer.Id),
                                IsActive = true,
                                Datetime = DateTime.Now,
                                Points = Convert.ToInt16(answer.puntaje),
                                Answer = answer.respuesta
                            };
            db.RightAnswers.InsertOnSubmit(rightAnswer);

            try
            {
                db.SubmitChanges();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);

            }

            GolGuru.Instance.BrodcastMatchLiveAnswer(answer);

            var redirectUrl = new UrlHelper(Request.RequestContext).Action("MainMenu", "Home");
            return Json(new { Url = redirectUrl });
        }


        [HttpGet]
        public ActionResult PenalLiveQuestion()
        {
            var penal = new Question
            {
                Id = GolGuru.Instance.GetMatchLiveQuestionId().ToString(CultureInfo.InvariantCulture),
                tipo = "Penal",
                segundos = "10",
                pregunta = "Penal",
                respuesta = new List<Answer>
                                                 {
                                                     new Answer { Id = "1", puntaje = "500", respuesta = "Afuera" },
                                                     new Answer { Id = "2", puntaje = "300", respuesta = "Izquierda Arriba" },
                                                     new Answer { Id = "3", puntaje = "250", respuesta = "Izquierda Abajo"},
                                                     new Answer { Id = "4", puntaje = "350", respuesta = "Derecha Arriba" },
                                                     new Answer { Id = "5", puntaje = "250", respuesta = "Derecha Abajo" },
                                                     new Answer { Id = "6", puntaje = "200", respuesta = "Medio" },
                                                     new Answer { Id = "7", puntaje = "450", respuesta = "Ataja" }
                                                 }
            };
            GolGuru.Instance.BrodcastMatchLiveQuestions("Penal", penal);
            return View();

        }
        [HttpPost]
        public ActionResult PenalLiveQuestion(Answer penal)
        {

            var answer = penal;
            answer.Id = GolGuru.Instance.GetMatchLiveQuestionIdResponse().ToString(CultureInfo.InvariantCulture);


            var rightAnswer = new RightAnswer()
            {
                AnswerId = Convert.ToInt16(answer.Id),
                IsActive = true,
                Datetime = DateTime.Now,
                Points = Convert.ToInt16(answer.puntaje),
                Answer = answer.respuesta
            };
            db.RightAnswers.InsertOnSubmit(rightAnswer);

            try
            {
                db.SubmitChanges();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);

            }
            GolGuru.Instance.BrodcastMatchLiveAnswer(answer);

            var redirectUrl = new UrlHelper(Request.RequestContext).Action("MainMenu", "Home");
            return Json(new { Url = redirectUrl });
        }

        [HttpGet]
        public ActionResult FreeKickLiveQuestion()
        {
            var freeKick = new Question
            {
                Id = GolGuru.Instance.GetMatchLiveQuestionId().ToString(CultureInfo.InvariantCulture),
                tipo = "Tiro Libre",
                segundos = "10",
                pregunta = "Tiro Libre para Argentina",
                respuesta = new List<Answer>
                                                 {
                                                     new Answer { Id = "1", puntaje = "250", respuesta = "Gol directo" },
                                                     new Answer { Id = "2", puntaje = "250", respuesta = "Afuera directo" },
                                                     new Answer { Id = "3", puntaje = "250", respuesta = "Barrera"},
                                                     new Answer { Id = "4", puntaje = "250", respuesta = "Pase corto o centro" },
                                                     new Answer { Id = "5", puntaje = "250", respuesta = "Ataque conecta" }
                                                 }
            };

            GolGuru.Instance.BrodcastMatchLiveQuestions("TiroLibre", freeKick);
            return View();

        }
        [HttpPost]
        public ActionResult FreeKickLiveQuestion(Answer freeKick)
        {

            var answer = freeKick;
            answer.Id = GolGuru.Instance.GetMatchLiveQuestionIdResponse().ToString(CultureInfo.InvariantCulture);
            var rightAnswer = new RightAnswer()
            {
                AnswerId = Convert.ToInt16(answer.Id),
                IsActive = true,
                Datetime = DateTime.Now,
                Points = Convert.ToInt16(answer.puntaje),
                Answer = answer.respuesta
            };
            db.RightAnswers.InsertOnSubmit(rightAnswer);

            try
            {
                db.SubmitChanges();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);

            }
            GolGuru.Instance.BrodcastMatchLiveAnswer(answer);

            var redirectUrl = new UrlHelper(Request.RequestContext).Action("MainMenu", "Home");
            return Json(new { Url = redirectUrl });
        }
        [HttpGet]
        public void Figura()
        {
            var figura = new Question
            {
                Id = "5",
                tipo = "Figura",
                segundos = "10",
                //pregunta = "Quien sera la figura del partido?",
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
        public void Penal()
        {
            var penal = new Question
            {
                Id = "7",
                tipo = "Penal",
                segundos = "10",
                pregunta = "Penal",
                respuesta = new List<Answer>
                                                 {
                                                     new Answer { Id = "1", puntaje = "500", respuesta = "Afuera" },
                                                     new Answer { Id = "2", puntaje = "300", respuesta = "Izquierda Arriba" },
                                                     new Answer { Id = "3", puntaje = "250", respuesta = "Izquierda Abajo"},
                                                     new Answer { Id = "4", puntaje = "350", respuesta = "Derecha Arriba" },
                                                     new Answer { Id = "5", puntaje = "250", respuesta = "Derecha Abajo" },
                                                     new Answer { Id = "6", puntaje = "200", respuesta = "Medio" },
                                                     new Answer { Id = "7", puntaje = "450", respuesta = "Ataja" }
                                                 }
            };

            GolGuru.Instance.BrodcastMatchLiveQuestions("Penal", penal);

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


        [HttpGet]
        public ActionResult GolEvent()
        {
            return View();

        }
        [HttpPost]
        public ActionResult GolEvent(Event gol)
        {
            GolGuru.Instance.BrodcastMatchLiveEvents("Gol", gol);
            return RedirectToAction("MainMenu", "Home");

        }
        [HttpGet]
        public ActionResult ExpulsionEvent()
        {
            return View();

        }

        [HttpPost]
        public ActionResult ExpulsionEvent(Event exp)
        {
            GolGuru.Instance.BrodcastMatchLiveEvents("Expulsion", exp);
            return RedirectToAction("MainMenu", "Home");

        }


    }
}
