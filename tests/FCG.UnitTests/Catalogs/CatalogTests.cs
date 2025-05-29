namespace FCG.UnitTests.Catalogs;
public class CatalogTests : TestBase
{
    public CatalogTests(FCGFixture fixture) : base(fixture)
    {
    }

    [Fact]
    public void ShouldAddGameToCatalog()
    {
        var game = _entityBuilder.Game.Generate();
        var catalog = _entityBuilder.Catalog.Generate();

        catalog.AddGame(game);

        catalog.Games.ShouldContain(g => g.Key == game.Key);
    }

    [Fact]
    public void ShouldThrowExceptionWhenAddingDuplicateGameToCatalog()
    {
        var game = _entityBuilder.Game.Generate();
        var catalog = _entityBuilder.Catalog.Generate();

        catalog.AddGame(game);

        Should.Throw<FCGDuplicateException>(() => catalog.AddGame(game));
    }

    [Fact]
    public void ShouldThrowExceptionWhenAddingGameWithSameTitleToCatalog()
    {
        var game = _entityBuilder.Game.Generate();
        var gameWithSameTitle = _entityBuilder.Game
            .RuleFor(g => g.Title, game.Title)
            .Generate();
        var catalog = _entityBuilder.Catalog.Generate();

        catalog.AddGame(game);

        Should.Throw<FCGDuplicateException>(() => catalog.AddGame(gameWithSameTitle));
    }

    [Fact]
    public void ShouldSetGameDataInCatalog()
    {
        var game = _entityBuilder.Game.Generate();
        var gameUpdated = _entityBuilder.Game
            .RuleFor(g => g.Key, game.Key)
            .Generate();
        var catalog = _entityBuilder.Catalog.Generate();

        catalog.AddGame(game);
        catalog.SetGame(gameUpdated);

        game.Title.ShouldBe(gameUpdated.Title);
        game.Description.ShouldBe(gameUpdated.Description);
    }

    [Fact]
    public void ShouldThrowExceptionWhenSettingGameDataForNonExistentGameInCatalog()
    {
        var game = _entityBuilder.Game.Generate();
        var catalog = _entityBuilder.Catalog.Generate();

        Should.Throw<FCGNotFoundException>(() => catalog.SetGame(game));
    }

    [Fact]
    public void ShouldRemoveGameFromCatalog()
    {
        var game = _entityBuilder.Game.Generate();
        var catalog = _entityBuilder.Catalog.Generate();
        
        catalog.AddGame(game);
        catalog.RemoveGame(game);

        catalog.Games.ShouldNotContain(g => g.Key == game.Key);
    }

    [Fact]
    public void ShouldThrowExceptionWhenRemovingNonExistentGameFromCatalog()
    {
        var game = _entityBuilder.Game.Generate();
        var catalog = _entityBuilder.Catalog.Generate();

        Should.Throw<FCGNotFoundException>(() => catalog.RemoveGame(game));
    }

    [Fact]
    public void ShouldGetGameFromCatalogByKey()
    {
        var game = _entityBuilder.Game.Generate();
        var catalog = _entityBuilder.Catalog.Generate();

        catalog.AddGame(game);

        var retrievedGame = catalog.GetGame(game.Key);

        retrievedGame.ShouldNotBeNull();
        retrievedGame.Key.ShouldBe(game.Key);
        retrievedGame.Title.ShouldBe(game.Title);
    }

    [Fact]
    public void ShouldThrowExceptionWhenGettingNonExistentGameFromCatalog()
    {
        var game = _entityBuilder.Game.Generate();
        var catalog = _entityBuilder.Catalog.Generate();

        Should.Throw<FCGNotFoundException>(() => catalog.GetGame(game.Key));
    }
}
