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
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Authorization;
using Lendship.Backend.DTO;
using Lendship.Backend.Interfaces.Services;
using Lendship.Backend.Models;

namespace Lendship.Backend.Controllers
{
    /// <summary>
    /// 
    /// </summary>
    [Authorize]
    [ApiController]
    [Route("[controller]")]
    public class ReservationController : ControllerBase
    {
        private readonly IReservationService _reservationService;

        public ReservationController(IReservationService reservationService)
        {
            _reservationService = reservationService;
        }

        /// <summary>
        /// create reservation
        /// </summary>
        /// <remarks>Creates a reservation</remarks>
        /// <param name="advertisementId">id of the user</param>
        /// <param name="reservation">reservation for the advertisement</param>
        /// <response code="200">reservation created</response>
        /// <response code="400">bad request</response>
        /// <response code="401"></response>
        [HttpPost]
        public virtual IActionResult CreateReservation([FromQuery][Required()]int advertisementId, [FromBody]ReservationDetailDto reservation)
        {
            try
            {
                _reservationService.CreateReservation(reservation, advertisementId);
                return StatusCode(201);
            }
            catch (Exception e)
            {
                Console.WriteLine("Exception at creating reservation: " + e.Message);
                return this.BadRequest("Exception at creating reservation: " + e.Message);
            }
        }

        /// <summary>
        /// get the user&#39;s reservations
        /// </summary>
        /// <remarks>Gets user&#39;s reservations</remarks>
        /// <response code="200">user&#39;s reservations</response>
        /// <response code="400">bad request</response>
        /// <response code="401"></response>
        [HttpGet]
        public virtual IActionResult GetReservations()
        {
            var reservations = _reservationService.GetReservations();
            return new ObjectResult(JsonConvert.SerializeObject(reservations));
        }

        /// <summary>
        /// get the reservations to the users advertisements
        /// </summary>
        /// <remarks>Gets the reservations to the users advertisements</remarks>
        /// <response code="200">reservations to the user&#39;s advertisements</response>
        /// <response code="400">bad request</response>
        /// <response code="401"></response>
        [HttpGet]
        [Route("for")]
        public virtual IActionResult GetReservationsForUsersAdvertisements()
        {
            var reservations = _reservationService.GetReservationsForUser();
            return new ObjectResult(JsonConvert.SerializeObject(reservations));
        }

        /// <summary>
        /// updates reservation
        /// </summary>
        /// <remarks>Update reservation</remarks>
        /// <param name="reservation">reservation to update</param>
        /// <response code="201">item updated</response>
        /// <response code="400">bad request</response>
        /// <response code="401"></response>
        [HttpPut]
        public virtual IActionResult UpdateReservation([FromBody]ReservationDetailDto reservation)
        {
            try
            {
                _reservationService.UpdateReservation(reservation);
                return StatusCode(200);
            }
            catch (Exception e)
            {
                Console.WriteLine("Exception at updating reservation: " + e.Message);
                return this.BadRequest("Exception at updating reservation: " + e.Message);
            }
        }

        /// <summary>
        /// updates reservation's state
        /// </summary>
        /// <remarks>Update reservation</remarks>
        /// <param name="reservationId">id of the reservation</param>
        /// <param name="state">reservation state</param>
        /// <response code="201">item updated</response>
        /// <response code="400">bad request</response>
        /// <response code="401"></response>
        [HttpPost]
        [Route("state")]
        public virtual IActionResult UpdateReservationState([FromQuery][Required()] int reservationId, [FromQuery][Required()] string state)
        {
            try
            {
                _reservationService.UpdateReservationState(reservationId, state);
                return StatusCode(200);
            }
            catch (Exception e)
            {
                Console.WriteLine("Exception at updating reservation: " + e.Message);
                return this.BadRequest("Exception at updating reservation: " + e.Message);
            }
        }

        /// <summary>
        /// get the reservations for the advertisements
        /// </summary>
        /// <remarks>Gets the reservations for the advertisements</remarks>
        /// <param name="advertisementId">property or service</param>
        /// <response code="200">reservations for the advertisements</response>
        /// <response code="400">bad request</response>
        /// <response code="401"></response>
        [HttpGet]
        [Route("{advertisementId}")]
        public virtual IActionResult GetReservationsForAdvertisements([FromRoute][Required] int advertisementId)
        {
            var reservations = _reservationService.GetReservationsForAdvertisement(advertisementId);
            return new ObjectResult(JsonConvert.SerializeObject(reservations));
        }

        /// <summary>
        /// admit reservation (set close state)
        /// </summary>
        /// <remarks>Gets the reservations for the advertisements</remarks>
        /// <param name="advertisementId">property or service</param>
        /// <response code="200">reservations for the advertisements</response>
        /// <response code="400">bad request</response>
        /// <response code="401"></response>
        [HttpPost]
        [Route("admit/{advertisementId}")]
        public virtual IActionResult AdmitReservation([FromRoute][Required] int advertisementId)
        {
            try
            {
                _reservationService.AdmitReservation(advertisementId);
                return StatusCode(200);
            }
            catch (Exception e)
            {
                Console.WriteLine("Exception at updating reservation: " + e.Message);
                return this.BadRequest("Exception at updating reservation: " + e.Message);
            }
        }

        /// <summary>
        /// get recent reservations
        /// </summary>
        /// <remarks>Gets recent reservations</remarks>
        /// <response code="200"> recent reservations</response>
        /// <response code="400">bad request</response>
        /// <response code="401"></response>
        [HttpGet]
        [Route("recent")]
        public virtual IActionResult GetRecentReservationsForUser()
        {
            var reservations = _reservationService.GetRecentReservations();
            return new ObjectResult(JsonConvert.SerializeObject(reservations));
        }

        /// <summary>
        /// get reservation token
        /// </summary>
        /// <remarks>Gets reservation token</remarks>
        /// <param name="reservationId">property or service</param>
        /// <response code="200">reservation token</response>
        /// <response code="400">bad request</response>
        /// <response code="401"></response>
        [HttpGet]
        [Route("reservationtoken/{reservationId}/{closing}")]
        public virtual IActionResult GetReservationtoken([FromRoute][Required] int reservationId, [FromRoute][Required] bool closing)
        {
            var token = _reservationService.GetReservationToken(reservationId, closing);
            return new ObjectResult(JsonConvert.SerializeObject(token));
        }

        /// <summary>
        /// validate reservation token
        /// </summary>
        /// <remarks>Validate reservation token</remarks>
        /// <param name="reservationId">property or service</param>
        /// <param name="closing">property or service</param>
        /// <response code="200">Validation succeeded</response>
        /// <response code="400">bad request</response>
        /// <response code="401"></response>
        [HttpPost]
        [Route("reservationtoken")]
        public virtual IActionResult ValidateReservationToken([FromBody] ReservationTokenDto reservationToken)
        {
            try
            {
                var succees = _reservationService.ValidateReservationToken(reservationToken.ReservationToken);
                return new ObjectResult(JsonConvert.SerializeObject(succees));
            }
            catch (Exception e)
            {
                Console.WriteLine("Exception at validating reservation token: " + e.Message);
                return this.BadRequest("Exception at validating reservation token: " + e.Message);
            }
        }
    }
}
