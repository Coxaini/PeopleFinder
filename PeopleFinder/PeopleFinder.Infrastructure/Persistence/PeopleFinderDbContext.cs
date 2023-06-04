using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using PeopleFinder.Domain.Entities;
using PeopleFinder.Domain.Entities.MappingEntities;
using PeopleFinder.Domain.Entities.MessagingEntities;
using PeopleFinder.Infrastructure.Persistence.Conventions;

namespace PeopleFinder.Infrastructure.Persistence;

public class PeopleFinderDbContext : DbContext
{
    public DbSet<User> Users { get; set; } = null!;
    public DbSet<Profile> Profiles { get; set; } = null!;
    
    public DbSet<Tag> Tags { get; set; } = null!;

    public DbSet<Chat> Chats { get; set; } = null!;

    public DbSet<ChatMember> ChatMembers { get; set; } = null!;
    
    public DbSet<Relationship> Relationships { get; set; } = null!;

    public DbSet<Message> Messages { get; set; } = null!;
    public DbSet<MediaFile> MediaFiles { get; set; } = null!;
    
    public DbSet<ProfilePicture> ProfilePictures { get; set; } = null!;
    
    //from stored procedures
    public virtual DbSet<MutualFriendsRecommendation> MutualFriendsRecommendations { get; set; } = null!;
    public PeopleFinderDbContext(DbContextOptions<PeopleFinderDbContext> options) : base(options)
    {
    }

    protected override void ConfigureConventions(ModelConfigurationBuilder configurationBuilder)
    {
        configurationBuilder.Conventions.Add(_=>new DateConvention());
    }

    private static void ConfigureConversions(ModelBuilder modelBuilder)
    {
        var dateTimeConverter = new ValueConverter<DateTime, DateTime>(
            v => v.ToUniversalTime(),
            v => DateTime.SpecifyKind(v, DateTimeKind.Utc));

        var nullableDateTimeConverter = new ValueConverter<DateTime?, DateTime?>(
            v => v.HasValue ? v.Value.ToUniversalTime() : v,
            v => v.HasValue ? DateTime.SpecifyKind(v.Value, DateTimeKind.Utc) : v);

        foreach (var entityType in modelBuilder.Model.GetEntityTypes())
        {
            if (entityType.IsKeyless)
            {
                continue;
            }

            foreach (var property in entityType.GetProperties())
            {
                if (property.ClrType == typeof(DateTime))
                {
                    property.SetValueConverter(dateTimeConverter);
                }
                else if (property.ClrType == typeof(DateTime?))
                {
                    property.SetValueConverter(nullableDateTimeConverter);
                }
            }
        }
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(PeopleFinderDbContext).Assembly);
        
        ConfigureConversions(modelBuilder);
       
        modelBuilder.Entity<MutualFriendsRecommendation>()
            .ToTable("MutualFriendsRecommendations", 
                t => t.ExcludeFromMigrations());
        
        List<Tag> tags = new List<Tag>()
        {
            new Tag() { Id = 1, Name = "Gaming" },
            new Tag() { Id = 2, Name = "Fishing" },
            new Tag() { Id = 3, Name = "Cycling" },
            new Tag() { Id = 4, Name = "Hiking" },
            new Tag() { Id = 5, Name = "Cooking" },
            new Tag() { Id = 6, Name = "Reading" },
            new Tag() { Id = 7, Name = "Photography" },
            new Tag() { Id = 8, Name = "Painting" },
            new Tag() { Id = 9, Name = "Traveling" },
            new Tag() { Id = 10, Name = "Writing" },
            new Tag() { Id = 11, Name = "Swimming" },
            new Tag() { Id = 12, Name = "Running" },
            new Tag() { Id = 13, Name = "Gardening" },
            new Tag() { Id = 14, Name = "Chess" },
            new Tag() { Id = 15, Name = "Knitting" },
            new Tag() { Id = 16, Name = "Music" },
            new Tag() { Id = 17, Name = "Dancing" },
            new Tag() { Id = 18, Name = "Singing" },
            new Tag() { Id = 19, Name = "Drawing" },
            new Tag() { Id = 20, Name = "Yoga" },
            new Tag() { Id = 21, Name = "Meditation" },
            new Tag() { Id = 22, Name = "Basketball" },
            new Tag() { Id = 23, Name = "Football" },
            new Tag() { Id = 24, Name = "Tennis" },
            new Tag() { Id = 25, Name = "Golf" },
            new Tag() { Id = 26, Name = "Soccer" },
            new Tag() { Id = 27, Name = "Volleyball" },
            new Tag() { Id = 28, Name = "Badminton" },
            new Tag() { Id = 29, Name = "Surfing" },
            new Tag() { Id = 30, Name = "Scuba Diving" },
            new Tag() { Id = 31, Name = "Programming" },
            new Tag() { Id = 32, Name = "Software Development" },
            new Tag() { Id = 33, Name = "Web Development" },
            new Tag() { Id = 34, Name = "Data Science" },
            new Tag() { Id = 35, Name = "Artificial Intelligence" },
            new Tag() { Id = 36, Name = "Machine Learning" },
            new Tag() { Id = 37, Name = "Cybersecurity" },
            new Tag() { Id = 38, Name = "Networking" },
            new Tag() { Id = 39, Name = "Robotics" },
            new Tag() { Id = 40, Name = "Embedded Systems" }
        };

        List<User> users = new List<User>()
        {
            new User() { Id = 1, Email="ilya@gmail.com", Password= "123456789"},
            new User() { Id = 2, Email="ilya1@gmail.com", Password= "123456789"},
            new User() { Id = 3, Email="ilya2@gmail.com", Password= "123456789"},
            new User() { Id = 4, Email="ilya3@gmail.com", Password= "123456789"},
            new User() { Id = 5, Email="john@gmail.com", Password= "password123"},
            new User() { Id = 6, Email="sarah@gmail.com", Password= "password456"},
            new User() { Id = 7, Email="peter@gmail.com", Password= "password789"},
            new User() { Id = 8, Email="emma@gmail.com", Password= "password101112"},
            new User() { Id = 9, Email="user1@gmail.com", Password= "password1"},
            new User() { Id = 10, Email="user2@gmail.com", Password= "password2"},
            new User() { Id = 11, Email="user3@gmail.com", Password= "password3"},
            new User() { Id = 12, Email="user4@gmail.com", Password= "password4"},
            new User() { Id = 13, Email="user5@gmail.com", Password= "password5"},
            new User() { Id = 14, Email="user6@gmail.com", Password= "password6"},
            new User() { Id = 15, Email="user7@gmail.com", Password= "password7"},
            new User() { Id = 16, Email="user8@gmail.com", Password= "password8"},
        }; 
        List<Profile> profiles = new List<Profile>()
        {
        new Profile()
        {
            Id = 1, Name = "Вова", UserId = 1, Gender = Gender.Male,Username ="ilya1",
            Bio =
            "Мене звати Вова і мене цікавить похід і дослідження. Я нічого не люблю більше, ніж відкривати нові маршрути і насолоджуватися захоплюючими краєвидами. Це прекрасний спосіб залишатися активним і оцінювати навколишню природну красу.",
            BirthDate = new DateTime(2000, 1, 1), City = "Київ"
        },
        new Profile()
        {
            Id = 2, Name = "Маша", UserId = 2, Gender = Gender.Female ,Username = "ilya2",
            Bio =
            "Привіт, мене звати Маша! Я фанат природи і пристрасна любителька походів. Обожнюю досліджувати нові маршрути і насолоджуватися прекрасними місцями. Щось у свіжому повітрі і природній красі привертає мене.",
            BirthDate = new DateTime(2000, 1, 1), City = "Київ"
        },
        new Profile()
        {
            Id = 3, Name = "Валя", UserId = 3, Gender = Gender.Female ,Username = "ilya3",
            Bio =
            "Привіт, я Валя, і я великий шанувальник походів та відкриття нових місць. Немає нічого кращого, ніж відчуття досягнення після пройдення важкого маршруту. Я люблю випробовувати свої можливості і насолоджуватися захоплюючими краєвидами по дорозі.",
            BirthDate = new DateTime(2000, 1, 1), City = "Київ"
        },
        new Profile()
        {
            Id = 4, Name = "Саша", UserId = 4, Gender = Gender.Female , Username ="ilya4",
            Bio =
            "Мене звуть Саша і ніщо мене не захоплює більше, ніж дослідження нових місць через походи. Це прекрасний спосіб залишатися активним і з'єднуватися з природою. Мені здається, що спокій і гармонія природи - це ідеальний втеча від суєти повсякденного життя.",
            BirthDate = new DateTime(2000, 1, 1), City = "Київ"
        },
        new Profile()
        {
            Id = 5, Name = "Джон", UserId = 5, Gender = Gender.Male, Bio = "Я люблю грати відеоігри",Username ="john",
            BirthDate = new DateTime(1995, 2, 1), City = "Нью-Йорк"
        },
        new Profile()
        {
            Id = 6, Name = "Сара", UserId = 6, Gender = Gender.Female,Username = "sarah",
            Bio = "Я люблю походи і дослідження нових місць", BirthDate = new DateTime(1990, 5, 10),
            City = "Сан-Франциско"
        },
        new Profile()
        {
            Id = 7, Name = "Петро", UserId = 7, Gender = Gender.Male,Username = "peter",
            Bio = "Я великий фанат науково-фантастичних фільмів і книжок", BirthDate = new DateTime(1980, 10, 20),
            City = "Лондон"
        },
        new Profile()
        {
        Id = 8, Name = "Емма", UserId = 8, Gender = Gender.Female,Username = "emma",
        Bio = "Я гурман і люблю спробувати нові кухні", BirthDate = new DateTime(1998, 7, 15), City = "Париж"
        },
            new Profile() { Id = 9, Name ="Джон", UserId = 9, Username ="user1", Gender = Gender.Male, Bio="Я люблю походи", BirthDate=new DateTime(1995,5,10), City ="Лос-Анджелес" },
            new Profile() { Id = 10, Name ="Сара", UserId= 10, Username ="user2", Gender = Gender.Female, Bio="Я люблю подорожувати", BirthDate=new DateTime(1993,3,20), City ="Нью-Йорк" },
            new Profile() { Id = 11, Name ="Боб", UserId = 11, Username ="user3", Gender = Gender.Male, Bio="Я люблю грати у баскетбол", BirthDate=new DateTime(1990,8,15), City ="Чикаго" },
            new Profile() { Id = 12, Name ="Аліса", UserId = 12, Username ="user4", Gender = Gender.Female, Bio="Я люблю готувати", BirthDate=new DateTime(1988,6,28), City ="Сан-Франциско" },
            new Profile() { Id = 13, Name ="Девід", UserId = 13, Username ="user5", Gender = Gender.Male, Bio="Я люблю читати книги", BirthDate=new DateTime(1998,4,12), City ="Бостон" },
            new Profile() { Id = 14, Name ="Емілі", UserId= 14, Username ="user6", Gender = Gender.Female, Bio="Я люблю танцювати", BirthDate=new DateTime(1996,2,14), City ="Маямі" },
            new Profile() { Id = 15, Name ="Джеймс", UserId = 15, Username ="user7", Gender = Gender.Male, Bio="Я люблю грати на гітарі", BirthDate=new DateTime(1991,7,18), City ="Сіетл" },
            new Profile() { Id = 16, Name ="Олівія", UserId = 16, Username ="user8", Gender = Gender.Female, Bio="Я люблю живопис", BirthDate=new DateTime(1989,9,5), City ="Філадельфія" },
        };

        object[] profileTag = {
            new {ProfilesId = 1, TagsId = 1},
            new {ProfilesId = 1,TagsId = 2},
            new {ProfilesId = 2, TagsId = 1 },
            new {ProfilesId = 2, TagsId = 2 },
            new {ProfilesId = 3, TagsId = 1 },
            new {ProfilesId = 3, TagsId = 3 },
        };

        modelBuilder.Entity<Profile>()
            .HasMany(p => p.Tags)
            .WithMany(t => t.Profiles)
            .UsingEntity(j => j.HasData(profileTag));

        modelBuilder.Entity<Tag>().HasData(tags);
        modelBuilder.Entity<User>().HasData(users);
        modelBuilder.Entity<Profile>().HasData(profiles);

    }

}
