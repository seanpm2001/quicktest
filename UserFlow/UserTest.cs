﻿using NUnit.Framework;
using Xamarin.Forms;
using Xamarin.Forms.Mocks;

namespace UserFlow
{
	public abstract class UserTest<T> where T : Application, new()
	{
		User user;

		protected T App { get; private set; }

		[SetUp]
		protected virtual void SetUp()
		{
			MockForms.Init();

			App = new T();
			user = new User(App);
		}

		protected void Tap(params string[] texts)
		{
			foreach (var text in texts)
				user.Tap(text);
		}

		protected void Input(string automationId, string text)
		{
			user.Input(automationId, text);
		}

		protected void ShouldSee(params string[] texts)
		{
			foreach (var text in texts) {
				if (user.CanSee(text))
					continue; // NOTE: prevent Assert from waiting 10 ms each time if text is seen immediately
				Assert.That(() => user.CanSee(text), Is.True.After(100, 10), $"User can't see \"{text}\"");
			}
		}

		protected void ShouldNotSee(params string[] texts)
		{
			foreach (var text in texts)
				Assert.That(user.CanSee(text), Is.False, $"User can see \"{text}\"");
		}

		protected void OpenMenu(string textToTap = null)
		{
			user.OpenMenu();

			if (textToTap != null)
				Tap(textToTap);
		}

		protected void GoBack()
		{
			user.GoBack();
		}

		protected string Render()
		{
			return user.Render();
		}

		[TearDown]
		protected virtual void TearDown()
		{
			user?.Print();
		}
	}
}
