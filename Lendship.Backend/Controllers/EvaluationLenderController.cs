/*
 * Simple Inventory API
 *
 * This is a simple API
 *
 * OpenAPI spec version: 1.0.0
 * Contact: you@your-company.com
 * Generated by: https://github.com/swagger-api/swagger-codegen.git
 */

using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Authorization;
using Lendship.Backend.DTO;
using Lendship.Backend.Interfaces.Services;

namespace Lendship.Backend.Controllers
{
    /// <summary>
    /// 
    /// </summary>
    [Authorize]
    [ApiController]
    [Route("[controller]")]
    public class EvaluationLenderController : ControllerBase
    {
        private readonly IEvaluationService _evaluationService;

        public EvaluationLenderController(IEvaluationService adService)
        {
            _evaluationService = adService;
        }

        /// <summary>
        /// add lender evaluation
        /// </summary>
        /// <remarks>Add lender evaluation</remarks>
        /// <param name="userId">id of the user</param>
        /// <param name="evaluation"></param>
        /// <response code="200">evaluation added</response>
        /// <response code="400">bad request</response>
        /// <response code="401"></response>
        [HttpPost]
        [Route("{userId}")]
        public virtual IActionResult AddLenderEvaluation([FromRoute][Required]string userId, [FromBody]EvaluationLenderDto evaluation)
        {
            try
            {
                _evaluationService.CreateLenderEvaluation(evaluation, userId);
                return StatusCode(201);
            }
            catch (Exception e)
            {
                Console.WriteLine("Exception at creating lender's evaluation: " + e.Message);
                return this.BadRequest("Exception at creating lender's evaluation: " + e.Message);
            }
        }

        /// <summary>
        /// get lender evaluation for user
        /// </summary>
        /// <remarks>Gets lender evaluation for user</remarks>
        /// <param name="userId">id of the user</param>
        /// <response code="200">Evaluation of the user</response>
        /// <response code="400">bad request</response>
        /// <response code="401"></response>
        [HttpGet]
        [Route("{userId}")]
        public virtual IActionResult GetLenderEvaluation([FromRoute][Required]string userId)
        {
            try
            {
                var evaluations = _evaluationService.GetLenderEvaluations(userId);
                return new ObjectResult(JsonConvert.SerializeObject(evaluations));
            }
            catch (Exception e)
            {
                Console.WriteLine("Exception at getting lender's evaluations: " + e.Message);
                return this.BadRequest("Exception at getting lender's evaluations: " + e.Message);
            }
        }
    }
}
