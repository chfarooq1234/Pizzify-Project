using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using OnlinePizzaWebApplication.Models;
using Microsoft.EntityFrameworkCore;
using OnlinePizzaWebApplication.Data;
using OnlinePizzaWebApplication.Repositories;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;

namespace OnlinePizzaWebApplication
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            var data = ApplicationData();
            services.AddDbContext<AppDbContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));

            services.AddIdentity<IdentityUser, IdentityRole>()
                    .AddEntityFrameworkStores<AppDbContext>()
                    .AddDefaultTokenProviders();




            services.AddTransient<IPizzaRepository, PizzaRepository>();
            services.AddTransient<ICategoryRepository, CategoryRepository>();
            services.AddTransient<IOrderRepository, OrderRepository>();
            services.AddTransient<IAdminRepository, AdminRepository>();
            services.AddTransient<IPizzaIngredientsRepository, PizzaIngredientsRepository>();
            services.AddTransient<IIngredientsRepository, IngredientsRepository>();
            services.AddTransient<IHttpContextAccessor, HttpContextAccessor>();
            services.AddTransient(sp => ShoppingCart.GetCart(sp));
            services.AddTransient((IServiceProvider arg) => data);

            services.AddMvc();

            services.AddMemoryCache();
            services.AddSession();

            services.Configure<IdentityOptions>(options =>
            {
                // Password settings
                options.Password.RequireDigit = true;
                options.Password.RequiredLength = 8;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = true;
                options.Password.RequireLowercase = false;
                options.Password.RequiredUniqueChars = 6;

                // Lockout settings
                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(30);
                options.Lockout.MaxFailedAccessAttempts = 10;
                options.Lockout.AllowedForNewUsers = true;

                // User settings
                options.User.RequireUniqueEmail = true;
            });

            services.ConfigureApplicationCookie(options =>
            {
                // Cookie settings
                options.Cookie.HttpOnly = true;
                options.ExpireTimeSpan = TimeSpan.FromMinutes(30);
                // If the LoginPath isn't set, ASP.NET Core defaults 
                // the path to /Account/Login.
                options.LoginPath = "/Account/Login";
                options.LogoutPath = "/Account/LogOut";
                // If the AccessDeniedPath isn't set, ASP.NET Core defaults 
                // the path to /Account/AccessDenied.
                options.AccessDeniedPath = "/Account/AccessDenied";
                options.SlidingExpiration = true;
            });

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();

            if (env.IsDevelopment())
            {
                app.UseBrowserLink();
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            app.UseStaticFiles();
            app.UseSession();
            app.UseAuthentication();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });

        }

        public ApplicationData ApplicationData()
        {
            var cat1 = new Categories { Name = "Veg Pizza", Description = "The Bakery's Standard pizzas all year around." };
            var cat2 = new Categories { Name = "Non-Veg Pizza", Description = "The Bakery's Speciality pizzas only for a limited time." };
            var cat3 = new Categories { Name = "Custom Pizza", Description = "The Bakery's Custom pizzas on the menu." };

            var cats = new List<Categories>()
            {
                cat1, cat2, cat3
            };
            var top1 = new Toppings { Id = 1, Name = "broccoli",price = 20.00M };
            var top2 = new Toppings { Id = 2, Name = "Black olives",price = 24.00M };
            var top3 = new Toppings { Id = 3, Name = "Chicken breasts",price = 35.00M };
            List<Toppings> toppings = new List<Toppings>
            {
                top1,top2,top3
            };
            var piz1 = new Pizzas {Id =1, Name = "Capricciosa", Price = 70.00M, Category = cat1, Description = "A normal pizza with a taste from the forest.", ImageUrl = "../images/Eq_it-na_pizza-margherita_sep2005_sml.jpg", IsPizzaOfTheWeek = false };
            var piz2 = new Pizzas { Id = 2, Name = "Veggie", Price = 70.00M, Category = cat1, Description = "Veggie Pizza for vegitarians", ImageUrl = "../images/Vegetarian_pizza.jpg", IsPizzaOfTheWeek = false };
            var piz3 = new Pizzas { Id = 3, Name = "Hawaii", Price = 75.00M, Category = cat1, Description = "A nice tasting pizza from Hawaii.", ImageUrl = "../images/Hawaiian_pizza_1.jpg", IsPizzaOfTheWeek = true };
            var piz4 = new Pizzas { Id = 4, Name = "Margarita", Price = 65.00M, Category = cat1, Description = "A basic pizza for everyone.", ImageUrl = "../images/Eq_it-na_pizza-margherita_sep2005_sml.jpg", IsPizzaOfTheWeek = false };
            var piz5 = new Pizzas { Id = 5, Name = "Kebab Special", Price = 85.00M, Category = cat1, Description = "A special pizza with kebab for the hungry one.", ImageUrl = "../images/54150_640x428.jpg", IsPizzaOfTheWeek = true };
            var piz6 = new Pizzas { Id = 6, Name = "Chicken Pizza", Price = 80.00M, Category = cat2, Description = "A pizza with taste from the ocean.", ImageUrl = "../images/dsc_0231.jpg", IsPizzaOfTheWeek = true };
            var piz7 = new Pizzas { Id = 7, Name = "Ham Pizza", Price = 70.00M, Category = cat2, Description = "A pizza with taste from Spain, Barcelona", ImageUrl = "../images/matsläkten 002.jpg", IsPizzaOfTheWeek = false };
            var piz8 = new Pizzas { Id = 8, Name = "Meat Pizza", Price = 89.00M, Category = cat2, Description = "Flying pizza from the sky, with taste of banana.", ImageUrl = "../images/Pizza_Hawaii_Special_på_Pizzeria_Papillon_i_Sala_1343.jpg", IsPizzaOfTheWeek = false };
            var piz9 = new Pizzas { Id = 9, Name = "Kentucky", Price = 69.00M, Category = cat1, Description = "A pizza from America with the taste of Kuntucky Chicken.", ImageUrl = "../images/pizza-jamon-dulce-y-champinone.jpg", IsPizzaOfTheWeek = false };
            

            var pizs = new List<Pizzas>()
            {
                piz1, piz2, piz3, piz4, piz5, piz6, piz7, piz8, piz9
            };

            var user1 = new IdentityUser { UserName = "user1@gmail.com", Email = "user1@gmail.com" };
            var user2 = new IdentityUser { UserName = "user2@gmail.com", Email = "user2@gmail.com" };
            var user3 = new IdentityUser { UserName = "user3@gmail.com", Email = "user3@gmail.com" };
            var user4 = new IdentityUser { UserName = "user4@gmail.com", Email = "user4@gmail.com" };
            var user5 = new IdentityUser { UserName = "user5@gmail.com", Email = "user5@gmail.com" };

            string userPassword = "Password123";

            var users = new List<IdentityUser>()
            {
                user1, user2, user3, user4, user5
            };

            

            

            var ing1 = new Ingredients {Id =1, Name = "Cheese" ,price = 35.00M};
            var ing2 = new Ingredients { Id = 2, Name = "Flour", price = 45.00M };
            var ing3 = new Ingredients { Id = 3, Name = "Tomatoe sauce", price = 55.00M };
            var ing4 = new Ingredients { Id = 4, Name = "Ham" ,price = 56.00M};
            var ing5 = new Ingredients { Id = 5, Name = "Broccoli", price = 65.00M };
            var ing6 = new Ingredients { Id = 6, Name = "Onions", price = 45.00M };
            var ing7 = new Ingredients { Id = 7, Name = "Bananas",price = 35.00M };
            var ing8 = new Ingredients { Id = 8, Name = "Chicken" ,price = 67.00M };
            var ing9 = new Ingredients { Id = 9, Name = "Minced Meat", price = 65.00M };

            var ings = new List<Ingredients>()
            {
                ing1, ing2, ing3, ing4, ing5, ing6, ing7, ing8, ing9
            };

            var pizIngs = new List<PizzaIngredients>()
            {
                new PizzaIngredients {Id = 1,PizzaId = 1,IngredientId=1, Ingredient = ing1, Pizza = piz1 },
                new PizzaIngredients {Id =2,PizzaId = 1,IngredientId=2, Ingredient = ing2, Pizza = piz1 },
                new PizzaIngredients {Id =3,PizzaId = 1,IngredientId=3,  Ingredient = ing3, Pizza = piz1 },
                new PizzaIngredients {Id =4,PizzaId = 1,IngredientId=4, Ingredient = ing5, Pizza = piz1 },
                new PizzaIngredients {Id =5,PizzaId = 1,IngredientId=5,  Ingredient = ing9, Pizza = piz1 },

                new PizzaIngredients {Id = 1,PizzaId = 2,IngredientId=1, Ingredient = ing1, Pizza = piz2 },
                new PizzaIngredients {Id = 2,PizzaId = 2,IngredientId=2, Ingredient = ing2, Pizza = piz2 },
                new PizzaIngredients {Id = 3,PizzaId = 2,IngredientId=3, Ingredient = ing3, Pizza = piz2 },
                new PizzaIngredients {Id = 4,PizzaId = 2,IngredientId=4, Ingredient = ing4, Pizza = piz2 },
                

                new PizzaIngredients {Id = 1,PizzaId = 3,IngredientId=1, Ingredient = ing1, Pizza = piz3 },
                new PizzaIngredients {Id = 2,PizzaId = 3,IngredientId=2, Ingredient = ing2, Pizza = piz3 },
                new PizzaIngredients {Id = 3,PizzaId = 3,IngredientId=3,Ingredient = ing3, Pizza = piz3 },
                new PizzaIngredients {Id = 4,PizzaId = 3,IngredientId=4,Ingredient = ing9, Pizza = piz3 },

                new PizzaIngredients {Id = 1,PizzaId = 4,IngredientId=1, Ingredient = ing1, Pizza = piz4 },
                new PizzaIngredients {Id = 2,PizzaId = 4,IngredientId=2, Ingredient = ing2, Pizza = piz4 },
                new PizzaIngredients {Id = 3,PizzaId = 4,IngredientId=3, Ingredient = ing3, Pizza = piz4 },

                new PizzaIngredients {Id = 1,PizzaId = 5,IngredientId=1, Ingredient = ing1, Pizza = piz5 },
                new PizzaIngredients {Id = 2,PizzaId = 5,IngredientId=2,Ingredient = ing2, Pizza = piz5 },
                new PizzaIngredients {Id = 3,PizzaId = 5,IngredientId=3,Ingredient = ing3, Pizza = piz5 },
                new PizzaIngredients {Id = 4,PizzaId = 5,IngredientId=4,Ingredient  = ing6, Pizza = piz5 },
                new PizzaIngredients {Id = 5,PizzaId = 5,IngredientId=5,Ingredient = ing4, Pizza = piz5 },
                

                new PizzaIngredients {Id = 1,PizzaId = 6,IngredientId=1, Ingredient = ing8, Pizza = piz6 },
                

                new PizzaIngredients {Id = 1,PizzaId = 7,IngredientId=1, Ingredient = ing4, Pizza = piz7 },
                

                new PizzaIngredients {Id = 1,PizzaId = 8,IngredientId=1, Ingredient = ing9, Pizza = piz8 },
                

                new PizzaIngredients {Id = 1,PizzaId = 9,IngredientId=1, Ingredient = ing1, Pizza = piz9 },
                new PizzaIngredients {Id = 2,PizzaId = 9,IngredientId=2,Ingredient = ing2, Pizza = piz9 },
                new PizzaIngredients {Id = 3,PizzaId = 9,IngredientId=3,Ingredient = ing3, Pizza = piz9 },
                

                

            };

            var ord1 = new Order
            {
                FirstName = "Pelle",
                LastName = "Andersson",
                AddressLine1 = "MainStreet 12",
                City = "Gothenburg",
                Country = "Sweden",
                Email = "pelle22@gmail.com",
                OrderPlaced = DateTime.Now.AddDays(-2),
                PhoneNumber = "0705123456",
                User = user1,
                ZipCode = "43210",
                OrderTotal = 370.00M,
            };

            var ord2 = new Order { };
            var ord3 = new Order { };

            var orderLines = new List<OrderDetail>()
            {
                new OrderDetail { Order=ord1, Pizza=piz1, Amount=2, Price=piz1.Price},
                new OrderDetail { Order=ord1, Pizza=piz3, Amount=1, Price=piz3.Price},
                new OrderDetail { Order=ord1, Pizza=piz5, Amount=3, Price=piz5.Price},
            };

            var orders = new List<Order>()
            {
                ord1
            };

            ApplicationData _context = new ApplicationData();
            
            _context.Categories.AddRange(cats);
            _context.Pizzas.AddRange(pizs);
            _context.Orders.AddRange(orders);
            _context.OrderDetails.AddRange(orderLines);
            _context.Ingredients.AddRange(ings);
            _context.PizzaIngredients.AddRange(pizIngs);
            _context.Toppings.AddRange(toppings);
            return _context;
        }

    }
}
