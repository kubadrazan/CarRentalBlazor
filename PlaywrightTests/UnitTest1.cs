using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.Playwright;
using Microsoft.Playwright.NUnit;
using NUnit.Framework;

namespace PlaywrightTests;

[Parallelizable(ParallelScope.Self)]
[TestFixture]
public class ExampleTest : PageTest
{
	[Test]
	public async Task LinkWorkingTest()
	{
		await Page.GotoAsync("https://localhost:7156/");
	}
	[Test]
	public async Task FromLinkToRentMeTest()
	{
		await Page.GotoAsync("https://localhost:7156/");

		await Page.Locator("path").Nth(2).ClickAsync();
		await Page.GetByText("BMW").ClickAsync();
		await Page.GetByLabel("Model").ClickAsync();
		await Page.GetByText("i3").ClickAsync();
		await Page.GetByRole(AriaRole.Button, new() { Name = "Search" }).ClickAsync();
		await Expect(Page.GetByRole(AriaRole.Heading, new() { Name = "BMW i3" }).First).ToBeVisibleAsync();
		await Page.GetByRole(AriaRole.Button, new() { Name = "See offers" }).First.ClickAsync();
		await Expect(Page.GetByRole(AriaRole.Button, new() { Name = "Rent Me" }).First).ToBeVisibleAsync();
	}
	[Test]
	public async Task CarBrowserTest()
	{
		await Page.GotoAsync("https://localhost:7156/car-browser?brand=BMW");
		await Expect(Page.GetByRole(AriaRole.Button, new() { Name = "Apply filters" })).ToBeVisibleAsync();
		await Page.GetByLabel("Current page").ClickAsync();
		await Page.GetByLabel("Page 2").ClickAsync();
		await Page.GetByRole(AriaRole.Heading, new() { Name = "Available cars:" }).ClickAsync();
		await Page.GetByRole(AriaRole.Button, new() { Name = "See offers" }).First.ClickAsync();
		await Page.Locator("#selectxueqgqza").GetByText("BMW").ClickAsync();
	}

}