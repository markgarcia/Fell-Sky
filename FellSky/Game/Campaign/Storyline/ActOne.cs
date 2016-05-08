﻿using System;
using Artemis;
using FellSky.Components;
using FellSky.Services;
using Stateless;
using FellSky.Framework;
using System.Collections;

namespace FellSky.Game.Campaign.Storyline
{
    public class ActOne : IStoryAct
    {
        public static class Triggers
        {
            public const string Start = "Start";
        }

        public IEnumerable Master(Story story, EntityWorld world)
        {
            story.State.Fire(Triggers.Start);
            yield return null;
        }

        public void ConfigureStates(StateMachine<StoryUpdateFunction, string> state)
        {
            state.Configure(Master)
                .Permit(Triggers.Start, Intro);

            state.Configure(Intro)
                .SubstateOf(Master)
                .Permit(Story.Triggers.NextScene, Tutorial);

            state.Configure(Tutorial)
                .SubstateOf(Master)
                .Permit(Story.Triggers.NextScene, AlienInvasionIntro);

            state.Configure(AlienInvasionIntro)
                .SubstateOf(Master)
                .Permit(Story.Triggers.NextScene, AlienInvasionGameplay);
        }

        private IEnumerable AlienInvasionGameplay(Story story, EntityWorld world)
        {
            yield return null;
        }

        private IEnumerable Intro(Story story, EntityWorld world)
        {
            var entity = world.CreateStoryOverlay(Properties.Resources.String_ActOne_Intro01);
            yield return Coroutine.WaitFor(TimeSpan.FromSeconds(2));
            entity.FadeGuiElement(TimeSpan.FromSeconds(1.5), 0)
                .OnDone = () => entity.Delete();
            yield return Coroutine.WaitFor(TimeSpan.FromSeconds(2));
            story.State.Fire(Story.Triggers.NextScene);

        }

        private IEnumerable Tutorial(Story story, EntityWorld world)
        {
            var entity = world.CreateStoryOverlay(Properties.Resources.String_ActOne_Tutorial01,"");
            yield return Coroutine.WaitFor(TimeSpan.FromSeconds(2));
            entity.FadeGuiElement(TimeSpan.FromSeconds(1.5), 0)
                .OnDone = () => entity.Delete();
            yield return Coroutine.WaitFor(TimeSpan.FromSeconds(2));
            story.State.Fire(Story.Triggers.NextScene);
        }

        private IEnumerable AlienInvasionIntro(Story story, EntityWorld world)
        {
            yield return null;
        }
    }
}