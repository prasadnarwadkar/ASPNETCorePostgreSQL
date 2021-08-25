// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using System;
using AcceptVerbsAttribute = Microsoft.AspNetCore.Mvc.AcceptVerbsAttribute;
using RouteAttribute = Microsoft.AspNetCore.Mvc.RouteAttribute;
using AuthorizeAttribute = Microsoft.AspNetCore.Authorization.AuthorizeAttribute;
using FromBodyAttribute = Microsoft.AspNetCore.Mvc.FromBodyAttribute;
using Api.Models;
using Common.Models;
using Common;
using UnnaturalDeathsMicroservice.ServiceBus;
using System.Text;
using System.IO;
using System.Text.Json.Serialization;

namespace Api.Controllers
{
    [CORSActionFilter]
    [Route("api/unnaturaldeaths")]
    [Authorize]
    public class UnnaturalDeathsController : ControllerBase
    {
        private IUnnaturalDeathsRepository _UnnaturalDeathsFHIRRepository;

        IConfiguration _iconfiguration;
        private readonly string BaseUri = null;
        private readonly string GatewayUriApi1 = null;
        private readonly IDeathDetailsSender _deathDetailsSender;

        public UnnaturalDeathsController(IConfiguration configuration,
                                        IUnnaturalDeathsRepository repo,
                                        IDeathDetailsSender deathSender)
        {
            _UnnaturalDeathsFHIRRepository = repo;
            _deathDetailsSender = deathSender;

            _iconfiguration = configuration;

            BaseUri = _iconfiguration["BaseUri"];
            GatewayUriApi1 = _iconfiguration["GatewayUriApi1"];

            try
            {

            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("The remote address could not be resolved"))
                {
                    throw new Exception("No Internet Connectivity");
                }
            }
        }



        [AcceptVerbs(new string[] { "GET" })]
        [Route("")]
        public async Task<IActionResult> get()
        {
            var list = await _UnnaturalDeathsFHIRRepository.GetListAsync();



            return new JsonResult(list, new System.Text.Json.JsonSerializerOptions
            {
                IgnoreNullValues = true,
                WriteIndented = true,
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            });
        }


        [AcceptVerbs(new string[1] { "GET" })]
        [Route("{id:Guid}")]
        public async Task<IActionResult> get(Guid id)
        {
            try
            {
                var resource = await _UnnaturalDeathsFHIRRepository.FindAsync(id);

                if (resource != null)
                {
                    return new JsonResult(resource, new System.Text.Json.JsonSerializerOptions
                    {
                        IgnoreNullValues = true,
                        WriteIndented = true,
                        PropertyNamingPolicy = JsonNamingPolicy.CamelCase
                    });
                }

                return new NotFoundResult();
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// Update an exisitng address resource (entity).
        /// </summary>
        /// <param address="id"></param>
        /// <param address="address">UnnaturalDeaths resource to modify</param>
        /// <returns></returns>
        /// <response code="202">Returns the udpated item's Uri in the location header in the response.</response>
        /// <response code="500">In case of server error</response> 
        [AcceptVerbs(new string[1] { "PUT" })]
        [Route("put/{id:Guid}")]
        public async Task<IActionResult> put(Guid id, [FromBody] UnnaturalDeaths death)
        {
            try
            {
                if (death == null)
                {
                    return BadRequest();
                }

                var resource = await _UnnaturalDeathsFHIRRepository.FindAsync(death.Id);

                if (resource == null)
                {
                    return NotFound(string.Format("This resource with the given id {0} doesn't exist. Did you mean to send a POST request instead?", death.Id));
                }

                var url = await _UnnaturalDeathsFHIRRepository.UpdateAsync(death);

                return Accepted("", "Accepted");
            }
            catch (Exception ex)
            {
                return StatusCode(500, string.Format("Message: {0} Stack Trace: {1}", ex.Message, ex.StackTrace));
            }
        }

        /// <summary>
        /// Register a new UnnaturalDeaths resource (entity). Check the Schema to know which JSON you would need to
        /// send to this operation. 
        /// </summary>
        /// <param address="address">UnnaturalDeaths resource to create</param>
        /// <returns></returns>
        /// <response code="201">Returns the created item's Uri in the location header in the response.</response>
        /// <response code="500">In case of server error</response> 
        [AcceptVerbs(new string[1] { "POST" })]
        [Route("post2")]
        public async Task<IActionResult> post2(UnnaturalDeaths death)
        {
            try
            {

                using (var Reader = new StreamReader(Request.Body, Encoding.UTF8))
                {
                    var reqBody = await Reader.ReadToEndAsync();

                    if (string.IsNullOrWhiteSpace(reqBody))
                    {
                        return BadRequest();
                    }

                    death = JsonSerializer.Deserialize<UnnaturalDeaths>(reqBody,
                        new JsonSerializerOptions
                        {
                            // [...]
                            NumberHandling = JsonNumberHandling.AllowReadingFromString
                            
                        });

                    if (death == null)
                    {
                        return BadRequest();
                    }
                }

                var resource = await _UnnaturalDeathsFHIRRepository.FindAsync(death.Id);

                if (resource != null)
                {
                    return Conflict(string.Format("This resource with the given id {0} already exists. Did you mean to send a PUT request instead?", resource.Id));
                }

                var url = await _UnnaturalDeathsFHIRRepository.AddAsync(death);

                _deathDetailsSender.SendDeathDetails(death);
                return Created("", "Created");
            }
            catch (Exception ex)
            {
                return StatusCode(500, string.Format("Message: {0} Stack Trace: {1}", ex.Message, ex.StackTrace));
            }

        }

        /// <summary>
        /// Register a new UnnaturalDeaths resource (entity). Check the Schema to know which JSON you would need to
        /// send to this operation. 
        /// </summary>
        /// <param address="address">UnnaturalDeaths resource to create</param>
        /// <returns></returns>
        /// <response code="201">Returns the created item's Uri in the location header in the response.</response>
        /// <response code="500">In case of server error</response> 
        [AcceptVerbs(new string[1] { "POST" })]
        [Route("post")]
        public async Task<IActionResult> post([FromBody] UnnaturalDeaths death)
        {
            try
            {
                if (death == null)
                {
                    byte[] buffer = new byte[(long)Request.ContentLength];
                    var body = await Request.Body.ReadAsync(buffer, 0, (int)Request.ContentLength);
                    var bodyString = Encoding.UTF8.GetString(buffer);
                    return BadRequest();
                }

                var resource = await _UnnaturalDeathsFHIRRepository.FindAsync(death.Id);

                if (resource != null)
                {
                    return Conflict(string.Format("This resource with the given id {0} already exists. Did you mean to send a PUT request instead?", resource.Id));
                }

                var url = await _UnnaturalDeathsFHIRRepository.AddAsync(death);

                _deathDetailsSender.SendDeathDetails(death);
                return Created("", "Created");
            }
            catch (Exception ex)
            {
                return StatusCode(500, string.Format("Message: {0} Stack Trace: {1}", ex.Message, ex.StackTrace));
            }

        }
    }
}