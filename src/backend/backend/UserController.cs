using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace backend
{
    public class UserController : Controller
    {
        private readonly IMediator _mediator;

        public UserController(IMediator mediator)
        {
            _mediator = mediator;
        }

        //[HttpPost]
        //public ActionResult Create(CreateUser user)
        //{
        //    var id = _mediator.Send(user);
        //    return View();
        //}
    }
}