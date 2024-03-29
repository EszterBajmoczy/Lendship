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

namespace Lendship.Backend.Controllers
{
    /// <summary>
    /// 
    /// </summary>
    [Authorize]
    [ApiController]
    [Route("[controller]")]
    public class ConversationController : ControllerBase
    {
        private readonly IConversationService _conversationService;

        public ConversationController(IConversationService conversationService)
        {
            _conversationService = conversationService;
        }

        /// <summary>
        /// create conversation
        /// </summary>
        /// <remarks>Create a new conversation</remarks>
        /// <param name="conversation">Conversation to create</param>
        /// <response code="201">item created</response>
        /// <response code="400">bad request</response>
        /// <response code="401"></response>
        [HttpPost]
        public virtual IActionResult CreateConversation([FromBody]ConversationDto conversation)
        {
            try
            {
                int id = _conversationService.CreateConversation(conversation);
                return new ObjectResult(id);
            }
            catch (Exception e)
            {
                Console.WriteLine("Exception at creating conversation: " + e.Message);
                return this.BadRequest("Exception at creating conversation: " + e.Message);
            }
        }

        /// <summary>
        /// create new message in the conversation
        /// </summary>
        /// <remarks>Create new message in the conversation</remarks>
        /// <param name="conversationName"></param>
        /// <param name="message">message to send</param>
        /// <response code="200">message is sent</response>
        /// <response code="400">bad request</response>
        /// <response code="401"></response>
        [HttpPost]
        [Route("msg")]
        public virtual IActionResult CreateMessage([FromBody]MessageDto message)
        {
            try
            {
                message.New = true;
                _conversationService.CreateMessage(message);
                return StatusCode(201);
            }
            catch (Exception e)
            {
                Console.WriteLine("Exception at creating message: " + e.Message);
                return this.BadRequest("Exception at creating message: " + e.Message);
            }
        }

        /// <summary>
        /// set messages to seen
        /// </summary>
        /// <remarks>Sets messages to seen</remarks>
        /// <param name="conversationId"></param>
        /// <response code="200">message is sent</response>
        /// <response code="400">bad request</response>
        /// <response code="401"></response>
        [HttpPost]
        [Route("msg/{conversationId}")]
        public virtual IActionResult SetMessagesSeen([FromRoute][Required] int conversationId)
        {
            try
            {
                _conversationService.SetMessagesSeen(conversationId);
                return StatusCode(201);
            }
            catch (Exception e)
            {
                Console.WriteLine("Exception at setting message to seen: " + e.Message);
                return this.BadRequest("Exception at setting message to seen: " + e.Message);
            }
        }

        /// <summary>
        /// get all conversation
        /// </summary>
        /// <remarks>Gets the user&#39;s conversations</remarks>
        /// <param name="searchString"></param>
        /// <response code="200">conversations of the user</response>
        /// <response code="400">bad request</response>
        /// <response code="401"></response>
        [HttpGet]
        public virtual IActionResult GetAllConversation([FromQuery]string searchString)
        {
            var conversations = _conversationService.GetAllConversation(searchString);
            return new ObjectResult(JsonConvert.SerializeObject(conversations));
        }

        /// <summary>
        /// get all message in the conversation
        /// </summary>
        /// <remarks>Gets all message in the conversation</remarks>
        /// <param name="conversationName"></param>
        /// <response code="200">messages in the conversation</response>
        /// <response code="400">bad request</response>
        /// <response code="401"></response>
        [HttpGet]
        [Route("{conversationId}")]
        public virtual IActionResult GetAllMessage([FromRoute][Required]int conversationId)
        {
            var msgs = _conversationService.GetAllMessage(conversationId);
            return new ObjectResult(JsonConvert.SerializeObject(msgs));
        }

        /// <summary>
        /// get new msg count
        /// </summary>
        /// <remarks>Gets the new msg count</remarks>
        /// <response code="200">count of the new msgs</response>
        /// <response code="400">bad request</response>
        /// <response code="401"></response>
        [HttpGet]
        [Route("new")]
        public virtual IActionResult GetNewMessageCount()
        {
            var msgCount = _conversationService.GetNewMessageCount();
            return new ObjectResult(JsonConvert.SerializeObject(msgCount));
        }
    }
}
