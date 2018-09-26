﻿using System;
using System.Collections.Generic;
using System.Linq;
using Player.Helpers;
using Player.Domain;

namespace Player
{
    public class Player : IPlayer<Song, String>
    {
        public Player(IVisualizer visualizer)
        {
            this._visualizer = visualizer;
        }

        private IVisualizer _visualizer { get; set; }

        public bool Locked { get; set; }

        public bool Playing { get; set; }

        public Song PlayingItem { get; set; }

        public List<Song> Items { get; set; }   
        
        public void Add(params Song[] items)
        {
            Items = items.ToList();
        }

        public void Add(Playlist playlist)
        {
            Items = playlist.Songs;
        }

        public void Add(Album album)
        {
            Items = album.Songs;
        }

        public void Add(Artist artist)
        {
            Items = artist.Songs;
        }

        public bool Play(out Song playingItem, bool loop = false)
        {
            if (PlayingItem == null)
            {
                PlayingItem = Items[0];
            }

            playingItem = PlayingItem;

            if (Locked == false)
            {
                Playing = true;
            }

            if (Playing)
            {
                int cycles = loop ? 5 : 1;
                for (int i = 0; i < cycles; i++)
                {
                    foreach (var song in Items)
                    {                        
                        PlayingItem = song;
                        _visualizer.NewScreen();
                        ListItems();
                        var(title, lyrics, _, _) = PlayingItem;
                        _visualizer.Render(title + ": " + lyrics);
                        _visualizer.Render();
                    }
                }
            }

            return Playing;
        }

        public bool Stop(out Song playingSong)
        {
            playingSong = PlayingItem;

            if (Locked == false)
            {
                Playing = false;
            }

            return Playing;
        }

        public bool Lock()
        {
            return Locked = true;
        }

        public bool Unlock()
        {
            return Locked = false;
        }

        public void Shuffle()
        {
            Items = Items.Shuffle();
        }

        public void SortByTitle()
        {
            Items = Items.SortByTitle();
        }

        public void ListItems()
        {
            foreach (var song in Items)
            {
                //

                /*var songData = new
                {
                    Title = song.Title,
                    Playing = song == PlayingSong,
                    Hours = (int)(song.Duration / (60 * 60)),
                    Minutes = (song.Duration % (60 * 60)) / 60,
                    Seconds = (song.Duration % (60)) / 60,
                };


                Console.WriteLine($"{songData.Title} {songData.Hours}:{songData.Minutes}:{songData.Seconds}");*/

                var (title, duration, playing, like) = GetItemData(song);
                var color = like.HasValue ? 
                    (like.Value ? ConsoleColor.Green : ConsoleColor.Red) 
                    : (ConsoleColor?)null;
                var startingMark = playing ? ">>>" : "";
                var endingMark = playing ? "<<<" : "";
                title = playing ? title.Fence() : title;
                _visualizer.Render($"{startingMark}{title} {duration.hours}:{duration.minutes}:{duration.seconds}{endingMark}", color);
            }
        }

        public (string title, (int hours, int minutes, int seconds) duration, bool playing, bool? like) GetItemData(Song song)
        {
            var hours = (int)(song.Duration / (60 * 60));
            var minutes = (song.Duration / 60) - hours * 60;
            var second = song.Duration - hours * 60 * 60 - minutes * 60;

            return (song.Title, (hours, minutes, second), song == PlayingItem, song.Like);
        }        
    }
}