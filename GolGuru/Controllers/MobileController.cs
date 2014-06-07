using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web.Script.Serialization;
using GolGuru.Models;
using System.Web.Mvc;
using GolGuru.Data;

namespace GolGuru.Controllers
{

    [Authorize]
    public class MobileController : Controller
    {
        protected static DataLayerDataContext db = new DataLayerDataContext();
        [AllowAnonymous]
        public JsonResult Login(LoginModel model)
        {
            var result = new { Success = "false", Time = "0" };
            try
            {
                if (model.UserName != null && model.Password != null)
                {
                    var user = from c in db.Users
                               where c.Email == model.UserName
                               select c;
                    if (user.Any() && model.Password == user.Select(x => x.Pass).SingleOrDefault())
                    {
                        result = new { Success = GolGuru.Instance.GetMatchStatus(), Time = GolGuru.Instance.GetMatchTime().ToString(CultureInfo.CurrentCulture) };
                    }
                }
            }
            catch (Exception es)
            {
                throw;
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        [AllowAnonymous]
        public JsonResult Register(LoginModel model)
        {
            var result = new { Success = "false", Time = "0" };
            try
            {
                if (model.UserName != null && model.Password != null)
                {
                    var user = from c in db.Users
                               where c.Email == model.UserName
                               select c;

                    if (!user.Any())
                    {
                        var newUser = new User()
                                          {
                                              Email = model.UserName,
                                              IsActive = true,
                                              LastLogin = DateTime.UtcNow,
                                              Pass = model.Password
                                          };
                        db.Users.InsertOnSubmit(newUser);

                        // Submit the change to the database. 
                        try
                        {
                            db.SubmitChanges();
                        }
                        catch (Exception e)
                        {
                            Console.WriteLine(e);

                        }

                        result = new { Success = GolGuru.Instance.GetMatchStatus(), Time = GolGuru.Instance.GetMatchTime().ToString(CultureInfo.CurrentCulture) };
                    }
                }
            }
            catch (Exception es)
            {
                throw;
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [AllowAnonymous]
        public JsonResult Results(string answers)
        {

            var answersList = new JavaScriptSerializer().Deserialize<IList<Answer>>(answers);

            foreach (var answer in answersList)
            {
                var userAnswer = new UserAnswer()
                                      {
                                          AnswerId = Convert.ToInt16(answer.Id),
                                          IsActive = true,
                                          Datetime = DateTime.UtcNow,
                                          Email = answer.email,
                                          Answer = answer.respuesta
                                      };
                db.UserAnswers.InsertOnSubmit(userAnswer);
            }
            try
            {
                db.SubmitChanges();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);

            }

            var result = new { stats = "true" };

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [AllowAnonymous]
        public JsonResult Positions(string userName)
        {
            try
            {

            var userTodayAnswers = from c in db.UserAnswers
                                   where c.Datetime.Date == DateTime.UtcNow.Date && c.Email == userName
                                   select c;
            var todayRightAnswers = from c in db.RightAnswers
                                    where c.Datetime.Value.Date == DateTime.UtcNow.Date
                                    select c;

                var userPoints = 0;
                userPoints = Enumerable.Sum(userTodayAnswers, rightAnswer => Enumerable.Sum(todayRightAnswers.Where(answer => rightAnswer.AnswerId == answer.AnswerId && rightAnswer.Answer == answer.Answer), answer => answer.Points));

            var user = (from c in db.Users
                        where c.Email == userName
                        select c).First();

            user.Puntos += userPoints;

            db.SubmitChanges();


            var users = (from c in db.Users
                         orderby c.Puntos descending
                         select c).Take(5);

            var position = (from c in db.Users
                            where c.Puntos > user.Puntos
                            select c).Count();


            var posicion = new Position()
            {
                puntos = user.Puntos.ToString(),
                puntosHoy = userPoints.ToString(CultureInfo.InvariantCulture),
                puesto = (position + 1).ToString(),
                posiciones = new List<Positions>()
            };

            
            
                int i = 1;
                foreach (var usr in users)
                {
                    if (usr.Puntos != null)
                        posicion.posiciones.Add(
                            new Positions() { email = usr.Email, puntos = usr.Puntos.Value.ToString(CultureInfo.InvariantCulture), puesto = i.ToString(CultureInfo.InvariantCulture) });
                    i++;
                }
                return Json(posicion, JsonRequestBehavior.AllowGet);

            }
            catch (Exception ex)
            {

                throw ex;
            }
            return new JsonResult();
        }

        [AllowAnonymous]
        public JsonResult QuestionsBeforeMatch()
        {
            var questions = new List<Question>();

            var match = db.MatchQuestions.Where(r => r.Datetime.Value.Date == DateTime.UtcNow.Date);


            if (match.Any())
            {
                var question = from c in match
                               where c.Question == "Quien va a ganar el partido?"
                               && c.Datetime.Value.Date == DateTime.UtcNow.Date
                               select c;

                var first = question.FirstOrDefault();
                if (first != null)
                    questions.Add(new Question()
                                      {
                                          pregunta = first.Question,
                                          Id = first.Id.ToString(CultureInfo.InvariantCulture),
                                          tipo = "antes-partido",
                                          order = "1",
                                          respuesta = new List<Answer>()
                                                          {
                                                              new Answer { Id = "1", puntaje = "250", respuesta = GolGuru.Instance.GetMatchEquipo1()},
                                                              new Answer { Id = "2", puntaje = "250", respuesta = GolGuru.Instance.GetMatchEquipo2() },
                                                              new Answer { Id = "3", puntaje = "250", respuesta = "Empate" }
                                                          }
                                      });
                question = from c in match
                           where c.Question == "Quien va a tener más la pelota?"
                           && c.Datetime.Value.Date == DateTime.UtcNow.Date
                           select c;

                first = question.FirstOrDefault();
                if (first != null)
                    questions.Add(new Question()
                    {
                        pregunta = first.Question,
                        Id = first.Id.ToString(CultureInfo.InvariantCulture),
                        tipo = "antes-partido",
                        order = "2",
                        respuesta = new List<Answer>()
                                                          {
                                                              new Answer { Id = "1", puntaje = "250", respuesta = GolGuru.Instance.GetMatchEquipo1()},
                                                              new Answer { Id = "2", puntaje = "250", respuesta = GolGuru.Instance.GetMatchEquipo2() },
                                                              new Answer { Id = "3", puntaje = "250", respuesta = "Empate" }
                                                          }
                    });
                question = from c in match
                           where c.Question == "Quien va a rematar más al arco?"
                           && c.Datetime.Value.Date == DateTime.UtcNow.Date
                           select c;

                first = question.FirstOrDefault();
                if (first != null)
                    questions.Add(new Question()
                    {
                        pregunta = first.Question,
                        Id = first.Id.ToString(CultureInfo.InvariantCulture),
                        tipo = "antes-partido",
                        order = "3",
                        respuesta = new List<Answer>()
                                                          {
                                                              new Answer { Id = "1", puntaje = "250", respuesta = GolGuru.Instance.GetMatchEquipo1()},
                                                              new Answer { Id = "2", puntaje = "250", respuesta = GolGuru.Instance.GetMatchEquipo2() },
                                                              new Answer { Id = "3", puntaje = "250", respuesta = "Empate" }
                                                          }
                    });
                question = from c in match
                           where c.Question == "Quien va a cometer más faltas?"
                           && c.Datetime.Value.Date == DateTime.UtcNow.Date
                           select c;

                first = question.FirstOrDefault();
                if (first != null)
                    questions.Add(new Question()
                    {
                        pregunta = first.Question,
                        Id = first.Id.ToString(CultureInfo.InvariantCulture),
                        tipo = "antes-partido",
                        order = "4",
                        respuesta = new List<Answer>()
                                                          {
                                                              new Answer { Id = "1", puntaje = "250", respuesta = GolGuru.Instance.GetMatchEquipo1()},
                                                              new Answer { Id = "2", puntaje = "250", respuesta = GolGuru.Instance.GetMatchEquipo2() },
                                                              new Answer { Id = "3", puntaje = "250", respuesta = "Empate" }
                                                          }
                    });
                question = from c in match
                           where c.Question == "Quien va a tener más corners a favor?"
                           && c.Datetime.Value.Date == DateTime.UtcNow.Date
                           select c;

                first = question.FirstOrDefault();
                if (first != null)
                    questions.Add(new Question()
                    {
                        pregunta = first.Question,
                        Id = first.Id.ToString(CultureInfo.InvariantCulture),
                        tipo = "antes-partido",
                        order = "5",
                        respuesta = new List<Answer>()
                                                          {
                                                              new Answer { Id = "1", puntaje = "250", respuesta = GolGuru.Instance.GetMatchEquipo1()},
                                                              new Answer { Id = "2", puntaje = "250", respuesta = GolGuru.Instance.GetMatchEquipo2() },
                                                              new Answer { Id = "3", puntaje = "250", respuesta = "Empate" }
                                                          }
                    });
                question = from c in match
                           where c.Question == "Quien va a recibir más amarillas?"
                           && c.Datetime.Value.Date == DateTime.UtcNow.Date
                           select c;

                first = question.FirstOrDefault();
                if (first != null)
                    questions.Add(new Question()
                    {
                        pregunta = first.Question,
                        Id = first.Id.ToString(CultureInfo.InvariantCulture),
                        tipo = "antes-partido",
                        order = "6",
                        respuesta = new List<Answer>()
                                                          {
                                                              new Answer { Id = "1", puntaje = "250", respuesta = GolGuru.Instance.GetMatchEquipo1()},
                                                              new Answer { Id = "2", puntaje = "250", respuesta = GolGuru.Instance.GetMatchEquipo2() },
                                                              new Answer { Id = "3", puntaje = "250", respuesta = "Empate" }
                                                          }
                    });
            }

            return Json(questions, JsonRequestBehavior.AllowGet);
        }
        [AllowAnonymous]
        public JsonResult MatchHour()
        {
            var matchStartTime = new { horario_partido = GolGuru.Instance.GetMatchHour() };
            return Json(matchStartTime, JsonRequestBehavior.AllowGet);
        }
    }
}

