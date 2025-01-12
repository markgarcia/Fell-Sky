﻿using System.Collections.Generic;
using Microsoft.Xna.Framework.Content;
using FellSky.Framework;

using System;
using FellSky.Components;
using Microsoft.Xna.Framework.Graphics;

namespace FellSky.Services
{
    public class SpriteManagerService : ISpriteManagerService
    {
        private ContentManager _content;

        public Dictionary<string, Sprite> Sprites { get; } = new Dictionary<string, Sprite>();

        public SpriteManagerService(ContentManager content)
        {
            _content = content;

            foreach (var item in Properties.Settings.Default.LoadedSpriteSheets)
                LoadSpriteSheet(item);
        }

        public SpriteSheet LoadSpriteSheet(string filename)
        {
            
            var sheet = Persistence.LoadFromFile<SpriteSheet>(filename);
            foreach(var s in sheet.Sprites)
            {
                s.Texture = sheet.Texture;
                Sprites[s.Id] = s;
            }
            return sheet;
        }

        public SpriteComponent CreateSpriteComponent(string spriteId)
        {
            var spriteDef = Sprites[spriteId];
            return new SpriteComponent(spriteDef, _content.Load<Texture2D>(spriteDef.Texture));
        }
    }
}
