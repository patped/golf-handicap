using System;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using GolfHandicap.Models;
using System.IO;
// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace GolfHandicap.Controllers
{
    [Route("api/[action]")]
    [ApiController]
    public class GolfHandicapController : ControllerBase
    {
        private readonly GolfHandicapContext _context;


        public GolfHandicapController(GolfHandicapContext context)
        {
            _context = context;
            if (_context.PlayerHandicaps.Count() == 0)
            {
                StreamReader golfresults = new StreamReader(@"D:\Projects\GolfHandicap\Data\results.txt");
                for (int counter = 0; counter < 21; counter++)
                {
                    if (golfresults.Peek() != -1)
                    {
                        var splitLine = golfresults.ReadLine().Split(';');

                        _context.PlayerHandicaps.Add(new PlayerHandicap
                        {
                            PlayedDate = DateTime.Parse(splitLine[0]),
                            Result = int.Parse(splitLine[1]),
                            Handicap = decimal.Parse(splitLine[2]),
                            CoursePar = int.Parse(splitLine[3]),
                            CourseValue = decimal.Parse(splitLine[4]),
                            SlopeValue = int.Parse(splitLine[5]),
                            PlayedHandicap = int.Parse(splitLine[6]),
                        });
                        _context.SaveChanges();
                    }
                }
            }
        }

        // GET: api/GetPlayerHandicaps
        [HttpGet]
        public async Task<ActionResult<IEnumerable<PlayerHandicap>>> GetPlayerHandicaps()
        {
            return await _context.PlayerHandicaps.ToListAsync();
        }

        // GET: api/GetPlayerHandicap/5
        [HttpGet("{id}")]
        public async Task<ActionResult<PlayerHandicap>> GetPlayerHandicap(long id)
        {
            var handicapItem = await _context.PlayerHandicaps.FindAsync(id);

            if (handicapItem == null)
            {
                return NotFound();
            }

            return handicapItem;
        }

        // GET: api/GetNewPlayerHandicap
        [HttpGet()]
        public async Task<ActionResult<string>> GetNewPlayerHandicap()
        {
            var handicapList = await _context.PlayerHandicaps.ToListAsync();
            List<decimal> newHandicapList = new List<decimal>();
            foreach (PlayerHandicap handicap in handicapList)
            {
                decimal y = (36 - handicap.Result) + handicap.PlayedHandicap;
                decimal k = handicap.SlopeValue / 113.0M;
                decimal a = handicap.CourseValue - handicap.CoursePar;
                decimal x = (y / k) - a;
                newHandicapList.Add(x);
            }
            newHandicapList.Sort();
            List<decimal> eightBest = newHandicapList.GetRange(0, 8);
            return (eightBest.Sum() / 8).ToString();
        }

        // POST: api/PostPlayerHandicap
        [HttpPost]
        public async Task<ActionResult<PlayerHandicap>> PostPlayerHandicap(PlayerHandicap handicap)
        {
            _context.PlayerHandicaps.Add(handicap);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetPlayerHandicap), new { id = handicap.Id }, handicap);
        }

        // PUT: api/PutPlayerHandicap/
        [HttpPut]
        public async Task<ActionResult<PlayerHandicap>> PutPlayerHandicap(PlayerHandicap handicap)
        {
            try
            {
                _context.Entry(handicap).State = EntityState.Modified;
                await _context.SaveChangesAsync();
                return NoContent();
            } catch (DbUpdateConcurrencyException e)
            {
                Console.WriteLine(e);
                return BadRequest();
            }
        }

        // DELETE: api/DeletePlayerHandicap/
        [HttpDelete("{id}")]
        public async Task<ActionResult<PlayerHandicap>> DeletePlayerHandicap(long id)
        {
            try
            {
                _context.PlayerHandicaps.Remove(await _context.PlayerHandicaps.FindAsync(id));
                await _context.SaveChangesAsync();
                return NoContent();
            } catch (Exception e)
            {
                Console.WriteLine(e);
                return NotFound();
            }
        }
    }
}
