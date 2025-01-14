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
	public async Task MyTest()
	{
		await Page.GotoAsync("https://localhost:7156/");
	}
	[Test]
	public async Task Test2()
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
}