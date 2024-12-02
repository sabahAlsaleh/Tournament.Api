﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tournament.Core.Entities;

namespace Tournament.Data.Data
{
    public static class SeedData
    {
        public static async Task SeedAsync(TournamentContext context)
        {
            if (!context.TournamentDetails.Any())
            {
                var tournaments = new List<TournamentDetails>
                {
                    new TournamentDetails
                    {
                        Title = "Winter Cup",
                        StartDate = DateTime.Now.AddMonths(1),
                        Games = new List<Game>
                        {
                            new Game { Title = "Match 1", Time = DateTime.Now.AddMonths(1).AddHours(10) },
                            new Game { Title = "Match 2", Time = DateTime.Now.AddMonths(1).AddHours(14) }
                        }
                    },
                    new TournamentDetails
                    {
                        Title = "Spring Championship",
                        StartDate = DateTime.Now.AddMonths(3),
                        Games = new List<Game>
                        {
                            new Game { Title = "Match 1", Time = DateTime.Now.AddMonths(3).AddHours(9) },
                            new Game { Title = "Match 2", Time = DateTime.Now.AddMonths(3).AddHours(15) }
                        }
                    },
                    new TournamentDetails
                    {
                        Title = "Autumn Classic",
                        StartDate = DateTime.Now.AddMonths(6),
                        Games = new List<Game>
                        {
                            new Game { Title = "Match 1", Time = DateTime.Now.AddMonths(6).AddHours(10) },
                            new Game { Title = "Match 2", Time = DateTime.Now.AddMonths(6).AddHours(14) },
                            new Game { Title = "Match 3", Time = DateTime.Now.AddMonths(6).AddHours(18) }
                        }
                    },
                    new TournamentDetails
                    {
                        Title = "Summer Invitational",
                        StartDate = DateTime.Now.AddMonths(9),
                        Games = new List<Game>
                        {
                            new Game { Title = "Opening Match", Time = DateTime.Now.AddMonths(9).AddHours(11) },
                            new Game { Title = "Semi Final", Time = DateTime.Now.AddMonths(9).AddHours(15) },
                            new Game { Title = "Final", Time = DateTime.Now.AddMonths(9).AddHours(20) }
                        }
                    },
                    new TournamentDetails
                    {
                        Title = "Champions League",
                        StartDate = DateTime.Now.AddMonths(12),
                        Games = new List<Game>
                        {
                            new Game { Title = "Group Stage 1", Time = DateTime.Now.AddMonths(12).AddHours(13) },
                            new Game { Title = "Group Stage 2", Time = DateTime.Now.AddMonths(12).AddHours(16) },
                            new Game { Title = "Quarter Final", Time = DateTime.Now.AddMonths(12).AddHours(19) },
                            new Game { Title = "Semi Final", Time = DateTime.Now.AddMonths(12).AddHours(21) },
                            new Game { Title = "Grand Final", Time = DateTime.Now.AddMonths(12).AddHours(23) }
                        }
                    }
                };
                context.TournamentDetails.AddRange(tournaments);
                await context.SaveChangesAsync();
            }
        }
    }
}