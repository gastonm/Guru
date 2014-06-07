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
    public class QuestionsController : Controller
    {
        protected static DataLayerDataContext db = new DataLayerDataContext();


        [HttpGet]
        public ActionResult BeforeMatchQuestion()
        {
            return View();

        }
        [HttpPost]
        public ActionResult BeforeMatchQuestion(StartedFirstQuestion question)
        {
            GolGuru.Instance.SetMatchEquipo1(question.equipo1);

            GolGuru.Instance.SetMatchEquipo2(question.equipo2);

            GolGuru.Instance.SetMatchHour(question.horario);


            var match = db.MatchQuestions.Where(r => r.Datetime.Value.Date == DateTime.Now.Date);

            if (!db.MatchQuestions.Any(r => r.Datetime.Value.Date == DateTime.Now.Date))
            {
                var question1 = new MatchQuestion()
                                    {
                                        Datetime = DateTime.Now,
                                        IsActive = true,
                                        Question = "Quien va a ganar el partido?",
                                        QuestionId = Convert.ToInt16(GolGuru.Instance.GetMatchLiveQuestionId())
                                    };


                db.MatchQuestions.InsertOnSubmit(question1);

                var question2 = new MatchQuestion()
                                    {
                                        Datetime = DateTime.Now,
                                        IsActive = true,
                                        Question = "Quien va a tener más la pelota?",
                                        QuestionId = Convert.ToInt16(GolGuru.Instance.GetMatchLiveQuestionId())
                                    };
                db.MatchQuestions.InsertOnSubmit(question2);

                var question3 = new MatchQuestion()
                                    {
                                        Datetime = DateTime.Now,
                                        IsActive = true,
                                        Question = "Quien va a rematar más al arco?",
                                        QuestionId = Convert.ToInt16(GolGuru.Instance.GetMatchLiveQuestionId())
                                    };
                db.MatchQuestions.InsertOnSubmit(question3);
                var question4 = new MatchQuestion()
                                    {
                                        Datetime = DateTime.Now,
                                        IsActive = true,
                                        Question = "Quien va a cometer más faltas?",
                                        QuestionId = Convert.ToInt16(GolGuru.Instance.GetMatchLiveQuestionId())
                                    };
                db.MatchQuestions.InsertOnSubmit(question4);

                var question5 = new MatchQuestion()
                                    {
                                        Datetime = DateTime.Now,
                                        IsActive = true,
                                        Question = "Quien va a tener más corners a favor?",
                                        QuestionId = Convert.ToInt16(GolGuru.Instance.GetMatchLiveQuestionId())
                                    };
                db.MatchQuestions.InsertOnSubmit(question5);

                var question6 = new MatchQuestion()
                                    {
                                        Datetime = DateTime.Now,
                                        IsActive = true,
                                        Question = "Quien va a recibir más amarillas?",
                                        QuestionId = Convert.ToInt16(GolGuru.Instance.GetMatchLiveQuestionId())
                                    };
                db.MatchQuestions.InsertOnSubmit(question6);
                try
                {
                    db.SubmitChanges();
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);

                }
            }
            GolGuru.Instance.SetMatchStatus(Helpers.Helpers.MatchStatusEnum.NotStarted);
            return RedirectToAction("MainMenu", "Home");

        }


        [HttpGet]
        public ActionResult AnswerMatchQuestion()
        {

            GolGuru.Instance.SetMatchStatus(Helpers.Helpers.MatchStatusEnum.FinishedSecond);
            ViewBag.equipo1 = GolGuru.Instance.GetMatchEquipo1();
            ViewBag.equipo2 = GolGuru.Instance.GetMatchEquipo2();
            return View();

        }


        [HttpPost]
        public ActionResult AnswerMatchQuestion(AfterMatchQuestions amQuestions)
        {
            var match = db.MatchQuestions.Where(r => r.Datetime.Value.Date == DateTime.Now.Date);
            if (match.Any())
            {
                var question = from c in match
                               where c.Question == "Quien va a ganar el partido?"
                                     && c.Datetime.Value.Date == DateTime.Now.Date
                               select c;

                var first = question.FirstOrDefault();
                if (first != null)
                {
                    var qstn = new RightAnswer()
                                   {
                                       Answer = amQuestions.equipoGanador,
                                       AnswerId = first.Id,
                                       Datetime = DateTime.Now,
                                       IsActive = true,
                                       Points = 250

                                   };

                    db.RightAnswers.InsertOnSubmit(qstn);
                }

                question = from c in match
                           where c.Question == "Quien va a tener más la pelota?"
                                 && c.Datetime.Value.Date == DateTime.Now.Date
                           select c;

                first = question.FirstOrDefault();
                if (first != null)
                {
                    var qstn = new RightAnswer()
                                   {
                                       Answer = amQuestions.equipoPosesion,
                                       AnswerId = first.Id,
                                       Datetime = DateTime.Now,
                                       IsActive = true,
                                       Points = 250

                                   };

                    db.RightAnswers.InsertOnSubmit(qstn);
                }
                question = from c in match
                           where c.Question == "Quien va a rematar más al arco?"
                                 && c.Datetime.Value.Date == DateTime.Now.Date
                           select c;

                first = question.FirstOrDefault();
                if (first != null)
                {
                    var qstn = new RightAnswer()
                                   {
                                       Answer = amQuestions.equipoRemate,
                                       AnswerId = first.Id,
                                       Datetime = DateTime.Now,
                                       IsActive = true,
                                       Points = 250

                                   };

                    db.RightAnswers.InsertOnSubmit(qstn);
                }
                question = from c in match
                           where c.Question == "Quien va a cometer más faltas?"
                                 && c.Datetime.Value.Date == DateTime.Now.Date
                           select c;

                first = question.FirstOrDefault();
                if (first != null)
                {
                    var qstn = new RightAnswer()
                                   {
                                       Answer = amQuestions.equipoFaltas,
                                       AnswerId = first.Id,
                                       Datetime = DateTime.Now,
                                       IsActive = true,
                                       Points = 250

                                   };

                    db.RightAnswers.InsertOnSubmit(qstn);
                }
                question = from c in match
                           where c.Question == "Quien va a tener más corners a favor?"
                                 && c.Datetime.Value.Date == DateTime.Now.Date
                           select c;

                first = question.FirstOrDefault();
                if (first != null)
                {
                    var qstn = new RightAnswer()
                                   {
                                       Answer = amQuestions.equipoCorner,
                                       AnswerId = first.Id,
                                       Datetime = DateTime.Now,
                                       IsActive = true,
                                       Points = 250
                                   };

                    db.RightAnswers.InsertOnSubmit(qstn);
                }
                question = from c in match
                           where c.Question == "Quien va a recibir más amarillas?"
                                 && c.Datetime.Value.Date == DateTime.Now.Date
                           select c;

                first = question.FirstOrDefault();
                if (first != null)
                {

                    var qstn = new RightAnswer()
                                   {
                                       Answer = amQuestions.equipoAmarilla,
                                       AnswerId = first.Id,
                                       Datetime = DateTime.Now,
                                       IsActive = true,
                                       Points = 250
                                   };

                    db.RightAnswers.InsertOnSubmit(qstn);
                }
                try
                {
                    db.SubmitChanges();
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);

                }
            }

            return RedirectToAction("MainMenu", "Home");
        }
    }
}
