using Microsoft.EntityFrameworkCore;
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

    public DbSet<Recommendation> Recommendations { get; set; } = null!;

    
    //from stored procedures
    public virtual DbSet<MutualFriendsRecommendation> MutualFriendsRecommendations { get; set; } = null!;
    public PeopleFinderDbContext(DbContextOptions<PeopleFinderDbContext> options) : base(options)
    {
    }

    protected override void ConfigureConventions(ModelConfigurationBuilder configurationBuilder)
    {
        //configurationBuilder.Conventions.Remove(typeof(PluralizingTableNameConvention));
        configurationBuilder.Conventions.Add(_=>new DateConvention());

    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(PeopleFinderDbContext).Assembly);
        
        
        
/*        modelBuilder.Entity<User>().ToTable("Users");
        modelBuilder.Entity<Profile>().ToTable("Users");*/

        List<Tag> tags= new List<Tag>()
        {
            new Tag() { Id = 1, Name = "Gaming" },
            new Tag() { Id = 2, Name = "Fishing" },
            new Tag() { Id = 3, Name = "Cycling" }
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
                Id = 1, Name = "Vova", UserId = 1, Gender = Gender.Male,Username ="ilya1",
                Bio =
                    "My name is Vova and I have a passion for hiking and exploring. I love nothing more than discovering new trails and taking in the breathtaking views. I find it's a great way to stay active and appreciate the natural beauty around us.",
                BirthDate = new DateTime(2000, 1, 1), City = "Kyiv"
            },
            new Profile()
            {
                Id = 2, Name = "Masha", UserId = 2, Gender = Gender.Female ,Username = "ilya2",
                Bio =
                    "Hello, my name is Masha! I'm a nature enthusiast and avid hiker. I love to explore new trails and experience the great outdoors. There's something about the fresh air and natural beauty that just speaks to me.",
                BirthDate = new DateTime(2000, 1, 1), City = "Kyiv"
            },
            new Profile()
            {
                Id = 3, Name = "Valya", UserId = 3, Gender = Gender.Female ,Username = "ilya3",
                Bio =
                    "Hey, I'm Valya and I'm a big fan of hiking and discovering new places. There's nothing quite like the feeling of accomplishment after completing a challenging trail. I enjoy pushing my limits and taking in the stunning scenery along the way.",
                BirthDate = new DateTime(2000, 1, 1), City = "Kyiv"
            },
            new Profile()
            {
                Id = 4, Name = "Sasha", UserId = 4, Gender = Gender.Female , Username ="ilya4",
                Bio =
                    "I'm Sasha and I love nothing more than exploring new places through hiking. It's a great way to stay active and connect with nature. I find the peace and serenity of the outdoors to be the perfect escape from the hustle and bustle of everyday life.",
                BirthDate = new DateTime(2000, 1, 1), City = "Kyiv"
            },
            new Profile()
            {
                Id = 5, Name = "John", UserId = 5, Gender = Gender.Male, Bio = "I love playing video games",Username ="john",
                BirthDate = new DateTime(1995, 2, 1), City = "New York"
            },
            new Profile()
            {
                Id = 6, Name = "Sarah", UserId = 6, Gender = Gender.Female,Username = "sarah",
                Bio = "I enjoy hiking and exploring new places", BirthDate = new DateTime(1990, 5, 10),
                City = "San Francisco"
            },
            new Profile()
            {
                Id = 7, Name = "Peter", UserId = 7, Gender = Gender.Male,Username = "peter",
                Bio = "I'm a big fan of sci-fi movies and books", BirthDate = new DateTime(1980, 10, 20),
                City = "London"
            },
            new Profile()
            {
                Id = 8, Name = "Emma", UserId = 8, Gender = Gender.Female,Username = "emma",
                Bio = "I'm a foodie and love trying new cuisines", BirthDate = new DateTime(1998, 7, 15), City = "Paris"
            },
            new Profile() { Id = 9, Name ="John", UserId = 9, Username ="user1", Gender = Gender.Male, Bio="I love hiking", BirthDate=new DateTime(1995,5,10), City ="Los Angeles" },
            new Profile() { Id = 10, Name ="Sarah", UserId= 10, Username ="user2", Gender = Gender.Female, Bio="I love traveling", BirthDate=new DateTime(1993,3,20), City ="New York" },
            new Profile() { Id = 11, Name ="Bob", UserId = 11, Username ="user3", Gender = Gender.Male, Bio="I love playing basketball", BirthDate=new DateTime(1990,8,15), City ="Chicago" },
            new Profile() { Id = 12, Name ="Alice", UserId = 12, Username ="user4", Gender = Gender.Female, Bio="I love cooking", BirthDate=new DateTime(1988,6,28), City ="San Francisco" },
            new Profile() { Id = 13, Name ="David", UserId = 13, Username ="user5", Gender = Gender.Male, Bio="I love reading books", BirthDate=new DateTime(1998,4,12), City ="Boston" },
            new Profile() { Id = 14, Name ="Emily", UserId= 14, Username ="user6", Gender = Gender.Female, Bio="I love dancing", BirthDate=new DateTime(1996,2,14), City ="Miami" },
            new Profile() { Id = 15, Name ="James", UserId = 15, Username ="user7", Gender = Gender.Male, Bio="I love playing guitar", BirthDate=new DateTime(1991,7,18), City ="Seattle" },
            new Profile() { Id = 16, Name ="Olivia", UserId = 16, Username ="user8", Gender = Gender.Female, Bio="I love painting", BirthDate=new DateTime(1989,9,5), City ="Philadelphia" },
        };

        List<Relationship> relationships = new List<Relationship>()
        {
            new Relationship()
            {
                Id = 1, InitiatorProfileId = 1, ReceiverProfileId = 2,
                Status = RelationshipStatus.Approved,
                SentAt = new DateTime(2021, 1, 1, 1, 1, 0),
                AcknowledgeAt = new DateTime(2021, 1, 1, 1, 1, 0)
            },
            new Relationship()
            {
                Id = 2, InitiatorProfileId = 1, ReceiverProfileId = 3,
                Status = RelationshipStatus.Approved,
                SentAt = new DateTime(2021, 1, 2, 1, 1, 0),
                AcknowledgeAt = new DateTime(2021, 1, 2, 1, 1, 0)
            },
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
        modelBuilder.Entity<Relationship>().HasData(relationships);


        //modelBuilder.ApplyConfigurationsFromAssembly(typeof(PeopleFinderDbContext).Assembly);
    }

}
