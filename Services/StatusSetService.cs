using System.Linq;
using System;
using System.Collections.Generic;

using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

using Crunchy.Models;
using Crunchy.Services.Interfaces;

namespace Crunchy.Services {

    public class StatusSetService : CrunchyService, IStatusSetService {


        public IActionResult GetAllStatusSets() {
            using (var context = new TodoContext()) {
                var results = context.StatusSets
                    .Select(ss => GetShortModel(ss))
                    .ToArray();
                return Ok(results);
            }
        }


        public IActionResult GetStatusSet(long ssId) {
            using (var context = new TodoContext()) {
                var result = context.StatusSets
                    .Find(ssId);
                if (result != null)
                    return Ok(GetDetailedModel(result));
                return NotFound();
            }
        }


        public IActionResult CreateStatusSet(string newStatusSetJson) {
            StatusSet newStatusSet = StatusSetFromJson(newStatusSetJson);
            if (newStatusSet == null)
                return BadRequest();
            using (var context = new TodoContext()) {
                context.StatusSets.Add(newStatusSet);
                context.SaveChanges();
                return CreatedAtRoute("GetStatusSet", new { ssId = newStatusSet.Id },
                        GetDetailedModel(newStatusSet));
            }
        }

        public IActionResult UpdateStatusSet(long ssId, string statusSetJson) {
            StatusSet newStatusSet = StatusSetFromJson(statusSetJson);
            if (newStatusSet == null)
                return BadRequest();
            using (var context = new TodoContext()) {
                StatusSet oldStatusSet = context.StatusSets.Find(ssId);
                if (oldStatusSet == null) return NotFound();
                var oldEnt = context.Attach(oldStatusSet);
                oldEnt.Collection("Statuses").Load();
                oldStatusSet.Name = newStatusSet.Name;
                foreach (var status in oldStatusSet.Statuses) {
                    var ent = context.Statuses.Remove(status);
                }
                oldStatusSet.Statuses.Clear();
                foreach (var status in newStatusSet.Statuses) {
                    context.Statuses.Add(status);
                    oldStatusSet.Statuses.Add(status);
                }
                context.SaveChanges();
                return NoContent();
            }
        }


        public IActionResult DeleteStatusSet(long ssId) {
            using (var context = new TodoContext()) {
                StatusSet oldStatusSet = context.StatusSets.Find(ssId);
                if (oldStatusSet == null) return NotFound();
                var oldEnt = context.Attach(oldStatusSet);
                oldEnt.Collection("Statuses").Load();
                foreach (var status in oldStatusSet.Statuses)
                    context.Statuses.Remove(status);
                context.StatusSets.Remove(oldStatusSet);
                context.SaveChanges();
                return NoContent();
            }
        }


        /// <summary>
        /// Get a short representation of the status set.
        /// </summary>
        /// <param name="ss">The StatusSet to format</param>
        /// <returns>An anonymous object representing the StatusSet</returns>
        public object GetShortModel(StatusSet ss) {
            using (var context = new TodoContext()) {
                var entity = context.Attach(ss);
                entity.Collection("Statuses").Load();
                ss = entity.Entity;
            }
            return ss;
        }


        /// <summary>
        /// Get a detailed representation of the status set.
        /// </summary>
        /// <param name="ss">The StatusSet to format</param>
        /// <returns>An anonymous object representing the StatusSet</returns>
        public object GetDetailedModel(StatusSet ss) {
            return GetShortModel(ss);
        }


        /// <summary>
        /// Parse a StatusSet from JSON string.
        /// </summary>
        /// <param name="json">The JSON string</param>
        /// <returns>The parsed StatusSet, or null on failure</returns>
        public StatusSet StatusSetFromJson(string json) {
            StatusSet res;
            try {
                res = JsonConvert.DeserializeObject<StatusSet>(json);
            }
            catch (JsonReaderException) {
                System.Console.WriteLine("Failed to parse json: " + json);
                return null;
            }
            if (String.IsNullOrEmpty(res.Name))
                return null;
            foreach (var status in res.Statuses)
                if (String.IsNullOrEmpty(status.Name))
                    return null;
            return res;
        }
    }
}