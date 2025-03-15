using Autofac;
using Evolution.Core;
using Evolution.Core.Entities;
using Evolution.Core.Evolution;
using Evolution.Core.Interfaces;
using Evolution.Core.Utils;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

public class Game1 : Game
{
    private IContainer _container;
    private SemulationLoop _simulationLoop;
    private GraphicsDeviceManager _graphics;
    private SpriteBatch _spriteBatch;
    private Texture2D pixel;
    private int cellSize = 10; // Размер клетки

    public Game1()
    {
        _graphics = new GraphicsDeviceManager(this);
        Content.RootDirectory = "Content";
        IsMouseVisible = true;

        _container = ConfigureContainer();
    }

    private IContainer ConfigureContainer()
    {
        var builder = new ContainerBuilder();

        builder.RegisterType<StandardWorld>().As<IWorld>().SingleInstance();
        builder.RegisterType<BotFactory>().As<IBotFactory>().SingleInstance();

        // BotManager теперь принимает Lazy<IEvolutionManager>
        builder.RegisterType<BotManager>().As<IBotManager>().SingleInstance();

        builder.RegisterType<BotBehavior>().As<IBotBehavior>().SingleInstance();
        builder.RegisterType<StandardEvolutionStrategy>().As<IEvolutionStrategy>().SingleInstance();

        // EvolutionManager зарегистрируем после BotManager
        builder.RegisterType<EvolutionManager>().As<IEvolutionManager>().SingleInstance();

        builder.RegisterType<SemulationLoop>().AsSelf().SingleInstance();

        return builder.Build();
    }


    protected override void Initialize()
    {
        _simulationLoop = _container.Resolve<SemulationLoop>();
        _simulationLoop.Start();

        base.Initialize();
    }

    protected override void LoadContent()
    {
        _spriteBatch = new SpriteBatch(GraphicsDevice);

        // Создаём текстуру 1x1 пиксель для отрисовки объектов
        pixel = new Texture2D(GraphicsDevice, 1, 1);
        pixel.SetData(new[] { Color.White });
    }

    protected override void Update(GameTime gameTime)
    {
        if (Keyboard.GetState().IsKeyDown(Keys.Escape))
            Exit();

        _simulationLoop.ExecuteTurn(); // Запускаем ход симуляции

        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(Color.Black);
        _spriteBatch.Begin();

        var world = _simulationLoop.World;

        for (int x = 0; x < world.Width; x++)
        {
            for (int y = 0; y < world.Height; y++)
            {
                var cell = world.GetCell(x, y);

                Color cellColor = cell.Type switch
                {
                    CellType.Bot => Color.Blue,    // Боты
                    CellType.Food => Color.Red,   // Еда
                    CellType.Poison => Color.Green, // Яд
                    CellType.Wall => Color.Gray,  // Стены
                    _ => Color.Black
                };

                DrawRectangle(x * cellSize, y * cellSize, cellSize, cellSize, cellColor);
            }
        }

        _spriteBatch.End();
        base.Draw(gameTime);
    }

    private void DrawRectangle(int x, int y, int width, int height, Color color)
    {
        _spriteBatch.Draw(pixel, new Rectangle(x, y, width, height), color);
    }
}
