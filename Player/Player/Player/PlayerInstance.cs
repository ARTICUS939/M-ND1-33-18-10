﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Player
{
    public class PlayerInstance
    {

        public bool Locked { get; set; }

        public bool Playing { get; set; }

        public Song PlayingSong { get; set; }

        public List<Song> Songs { get; set; }   
        
        public void Add(params Song[] songs)
        {
            Songs = songs.ToList();
        }

        public void Add(Playlist playlist)
        {
            Songs = playlist.Songs;
        }

        public void Add(Album album)
        {
            Songs = album.Songs;
        }

        public void Add(Artist artist)
        {
            Songs = artist.Songs;
        }

        public bool Play(out Song playingSong, bool loop = false)
        {
            playingSong = PlayingSong = PlayingSong ?? Songs[0];

            if (!Locked)
            {
                Playing = true;
            }

            if (Playing)
            {
                int cycles = loop ? 5 : 1;
                for (int i = 0; i < cycles; i++)
                {
                    foreach (var song in Songs)
                    {
                        PlayingSong = song;

                        Console.Clear();                       
                        Console.WriteLine(PlayingSong.Title + ": " + PlayingSong.Lyrics);

                        System.Threading.Thread.Sleep(2000);
                    }
                }
            }

            return Playing;
        }

        public bool Stop(out Song playingSong)
        {
            playingSong = PlayingSong;

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
            List<Song> suffledSongs = new List<Song>();
            int step = 3;
            for (int i = 0; i < step; i++)
            {
                int index = i;

                while(index < Songs.Count)
                {
                    suffledSongs.Add(Songs[index]);
                    index += step;
                }
            }

            Songs = suffledSongs;
        }

        public void SortByTitle()
        {
            List<string> names = new List<string>();
            List<Song> sorted = new List<Song>();

            foreach (var song in Songs)
            {
                names.Add(song.Title);
            }

            names.Sort();

            foreach (var name in names)
            {
                foreach (var song in Songs)
                {
                    if (song.Title == name)
                    {
                        sorted.Add(song);
                        continue;
                    }
                }
            }

            Songs = sorted;
        }
    }
}