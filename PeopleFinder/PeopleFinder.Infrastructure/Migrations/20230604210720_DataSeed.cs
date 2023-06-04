using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace PeopleFinder.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class DataSeed : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Tags",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 1, "Gaming" },
                    { 2, "Fishing" },
                    { 3, "Cycling" },
                    { 4, "Hiking" },
                    { 5, "Cooking" },
                    { 6, "Reading" },
                    { 7, "Photography" },
                    { 8, "Painting" },
                    { 9, "Traveling" },
                    { 10, "Writing" },
                    { 11, "Swimming" },
                    { 12, "Running" },
                    { 13, "Gardening" },
                    { 14, "Chess" },
                    { 15, "Knitting" },
                    { 16, "Music" },
                    { 17, "Dancing" },
                    { 18, "Singing" },
                    { 19, "Drawing" },
                    { 20, "Yoga" },
                    { 21, "Meditation" },
                    { 22, "Basketball" },
                    { 23, "Football" },
                    { 24, "Tennis" },
                    { 25, "Golf" },
                    { 26, "Soccer" },
                    { 27, "Volleyball" },
                    { 28, "Badminton" },
                    { 29, "Surfing" },
                    { 30, "Scuba Diving" },
                    { 31, "Programming" },
                    { 32, "Software Development" },
                    { 33, "Web Development" },
                    { 34, "Data Science" },
                    { 35, "Artificial Intelligence" },
                    { 36, "Machine Learning" },
                    { 37, "Cybersecurity" },
                    { 38, "Networking" },
                    { 39, "Robotics" },
                    { 40, "Embedded Systems" }
                });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "Email", "Password", "RefreshToken", "RefreshTokenExpiryTime" },
                values: new object[,]
                {
                    { 1, "ilya@gmail.com", "123456789", null, null },
                    { 2, "ilya1@gmail.com", "123456789", null, null },
                    { 3, "ilya2@gmail.com", "123456789", null, null },
                    { 4, "ilya3@gmail.com", "123456789", null, null },
                    { 5, "john@gmail.com", "password123", null, null },
                    { 6, "sarah@gmail.com", "password456", null, null },
                    { 7, "peter@gmail.com", "password789", null, null },
                    { 8, "emma@gmail.com", "password101112", null, null },
                    { 9, "user1@gmail.com", "password1", null, null },
                    { 10, "user2@gmail.com", "password2", null, null },
                    { 11, "user3@gmail.com", "password3", null, null },
                    { 12, "user4@gmail.com", "password4", null, null },
                    { 13, "user5@gmail.com", "password5", null, null },
                    { 14, "user6@gmail.com", "password6", null, null },
                    { 15, "user7@gmail.com", "password7", null, null },
                    { 16, "user8@gmail.com", "password8", null, null }
                });

            migrationBuilder.InsertData(
                table: "Profiles",
                columns: new[] { "Id", "Bio", "BirthDate", "City", "CreatedAt", "Gender", "IsOnline", "LastActivity", "MainPictureId", "Name", "UserId", "Username" },
                values: new object[,]
                {
                    { 1, "Мене звати Вова і мене цікавить похід і дослідження. Я нічого не люблю більше, ніж відкривати нові маршрути і насолоджуватися захоплюючими краєвидами. Це прекрасний спосіб залишатися активним і оцінювати навколишню природну красу.", new DateTime(1999, 12, 31, 22, 0, 0, 0, DateTimeKind.Utc), "Київ", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 1, false, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "Вова", 1, "ilya1" },
                    { 2, "Привіт, мене звати Маша! Я фанат природи і пристрасна любителька походів. Обожнюю досліджувати нові маршрути і насолоджуватися прекрасними місцями. Щось у свіжому повітрі і природній красі привертає мене.", new DateTime(1999, 12, 31, 22, 0, 0, 0, DateTimeKind.Utc), "Київ", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 2, false, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "Маша", 2, "ilya2" },
                    { 3, "Привіт, я Валя, і я великий шанувальник походів та відкриття нових місць. Немає нічого кращого, ніж відчуття досягнення після пройдення важкого маршруту. Я люблю випробовувати свої можливості і насолоджуватися захоплюючими краєвидами по дорозі.", new DateTime(1999, 12, 31, 22, 0, 0, 0, DateTimeKind.Utc), "Київ", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 2, false, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "Валя", 3, "ilya3" },
                    { 4, "Мене звуть Саша і ніщо мене не захоплює більше, ніж дослідження нових місць через походи. Це прекрасний спосіб залишатися активним і з'єднуватися з природою. Мені здається, що спокій і гармонія природи - це ідеальний втеча від суєти повсякденного життя.", new DateTime(1999, 12, 31, 22, 0, 0, 0, DateTimeKind.Utc), "Київ", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 2, false, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "Саша", 4, "ilya4" },
                    { 5, "Я люблю грати відеоігри", new DateTime(1995, 1, 31, 22, 0, 0, 0, DateTimeKind.Utc), "Нью-Йорк", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 1, false, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "Джон", 5, "john" },
                    { 6, "Я люблю походи і дослідження нових місць", new DateTime(1990, 5, 9, 21, 0, 0, 0, DateTimeKind.Utc), "Сан-Франциско", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 2, false, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "Сара", 6, "sarah" },
                    { 7, "Я великий фанат науково-фантастичних фільмів і книжок", new DateTime(1980, 10, 19, 21, 0, 0, 0, DateTimeKind.Utc), "Лондон", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 1, false, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "Петро", 7, "peter" },
                    { 8, "Я гурман і люблю спробувати нові кухні", new DateTime(1998, 7, 14, 21, 0, 0, 0, DateTimeKind.Utc), "Париж", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 2, false, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "Емма", 8, "emma" },
                    { 9, "Я люблю походи", new DateTime(1995, 5, 9, 21, 0, 0, 0, DateTimeKind.Utc), "Лос-Анджелес", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 1, false, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "Джон", 9, "user1" },
                    { 10, "Я люблю подорожувати", new DateTime(1993, 3, 19, 22, 0, 0, 0, DateTimeKind.Utc), "Нью-Йорк", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 2, false, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "Сара", 10, "user2" },
                    { 11, "Я люблю грати у баскетбол", new DateTime(1990, 8, 14, 21, 0, 0, 0, DateTimeKind.Utc), "Чикаго", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 1, false, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "Боб", 11, "user3" },
                    { 12, "Я люблю готувати", new DateTime(1988, 6, 27, 21, 0, 0, 0, DateTimeKind.Utc), "Сан-Франциско", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 2, false, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "Аліса", 12, "user4" },
                    { 13, "Я люблю читати книги", new DateTime(1998, 4, 11, 21, 0, 0, 0, DateTimeKind.Utc), "Бостон", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 1, false, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "Девід", 13, "user5" },
                    { 14, "Я люблю танцювати", new DateTime(1996, 2, 13, 22, 0, 0, 0, DateTimeKind.Utc), "Маямі", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 2, false, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "Емілі", 14, "user6" },
                    { 15, "Я люблю грати на гітарі", new DateTime(1991, 7, 17, 21, 0, 0, 0, DateTimeKind.Utc), "Сіетл", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 1, false, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "Джеймс", 15, "user7" },
                    { 16, "Я люблю живопис", new DateTime(1989, 9, 4, 21, 0, 0, 0, DateTimeKind.Utc), "Філадельфія", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 2, false, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "Олівія", 16, "user8" }
                });

            migrationBuilder.InsertData(
                table: "ProfileTag",
                columns: new[] { "ProfilesId", "TagsId" },
                values: new object[,]
                {
                    { 1, 1 },
                    { 1, 2 },
                    { 2, 1 },
                    { 2, 2 },
                    { 3, 1 },
                    { 3, 3 }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "ProfileTag",
                keyColumns: new[] { "ProfilesId", "TagsId" },
                keyValues: new object[] { 1, 1 });

            migrationBuilder.DeleteData(
                table: "ProfileTag",
                keyColumns: new[] { "ProfilesId", "TagsId" },
                keyValues: new object[] { 1, 2 });

            migrationBuilder.DeleteData(
                table: "ProfileTag",
                keyColumns: new[] { "ProfilesId", "TagsId" },
                keyValues: new object[] { 2, 1 });

            migrationBuilder.DeleteData(
                table: "ProfileTag",
                keyColumns: new[] { "ProfilesId", "TagsId" },
                keyValues: new object[] { 2, 2 });

            migrationBuilder.DeleteData(
                table: "ProfileTag",
                keyColumns: new[] { "ProfilesId", "TagsId" },
                keyValues: new object[] { 3, 1 });

            migrationBuilder.DeleteData(
                table: "ProfileTag",
                keyColumns: new[] { "ProfilesId", "TagsId" },
                keyValues: new object[] { 3, 3 });

            migrationBuilder.DeleteData(
                table: "Profiles",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Profiles",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "Profiles",
                keyColumn: "Id",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "Profiles",
                keyColumn: "Id",
                keyValue: 7);

            migrationBuilder.DeleteData(
                table: "Profiles",
                keyColumn: "Id",
                keyValue: 8);

            migrationBuilder.DeleteData(
                table: "Profiles",
                keyColumn: "Id",
                keyValue: 9);

            migrationBuilder.DeleteData(
                table: "Profiles",
                keyColumn: "Id",
                keyValue: 10);

            migrationBuilder.DeleteData(
                table: "Profiles",
                keyColumn: "Id",
                keyValue: 11);

            migrationBuilder.DeleteData(
                table: "Profiles",
                keyColumn: "Id",
                keyValue: 12);

            migrationBuilder.DeleteData(
                table: "Profiles",
                keyColumn: "Id",
                keyValue: 13);

            migrationBuilder.DeleteData(
                table: "Profiles",
                keyColumn: "Id",
                keyValue: 14);

            migrationBuilder.DeleteData(
                table: "Profiles",
                keyColumn: "Id",
                keyValue: 15);

            migrationBuilder.DeleteData(
                table: "Profiles",
                keyColumn: "Id",
                keyValue: 16);

            migrationBuilder.DeleteData(
                table: "Tags",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Tags",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "Tags",
                keyColumn: "Id",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "Tags",
                keyColumn: "Id",
                keyValue: 7);

            migrationBuilder.DeleteData(
                table: "Tags",
                keyColumn: "Id",
                keyValue: 8);

            migrationBuilder.DeleteData(
                table: "Tags",
                keyColumn: "Id",
                keyValue: 9);

            migrationBuilder.DeleteData(
                table: "Tags",
                keyColumn: "Id",
                keyValue: 10);

            migrationBuilder.DeleteData(
                table: "Tags",
                keyColumn: "Id",
                keyValue: 11);

            migrationBuilder.DeleteData(
                table: "Tags",
                keyColumn: "Id",
                keyValue: 12);

            migrationBuilder.DeleteData(
                table: "Tags",
                keyColumn: "Id",
                keyValue: 13);

            migrationBuilder.DeleteData(
                table: "Tags",
                keyColumn: "Id",
                keyValue: 14);

            migrationBuilder.DeleteData(
                table: "Tags",
                keyColumn: "Id",
                keyValue: 15);

            migrationBuilder.DeleteData(
                table: "Tags",
                keyColumn: "Id",
                keyValue: 16);

            migrationBuilder.DeleteData(
                table: "Tags",
                keyColumn: "Id",
                keyValue: 17);

            migrationBuilder.DeleteData(
                table: "Tags",
                keyColumn: "Id",
                keyValue: 18);

            migrationBuilder.DeleteData(
                table: "Tags",
                keyColumn: "Id",
                keyValue: 19);

            migrationBuilder.DeleteData(
                table: "Tags",
                keyColumn: "Id",
                keyValue: 20);

            migrationBuilder.DeleteData(
                table: "Tags",
                keyColumn: "Id",
                keyValue: 21);

            migrationBuilder.DeleteData(
                table: "Tags",
                keyColumn: "Id",
                keyValue: 22);

            migrationBuilder.DeleteData(
                table: "Tags",
                keyColumn: "Id",
                keyValue: 23);

            migrationBuilder.DeleteData(
                table: "Tags",
                keyColumn: "Id",
                keyValue: 24);

            migrationBuilder.DeleteData(
                table: "Tags",
                keyColumn: "Id",
                keyValue: 25);

            migrationBuilder.DeleteData(
                table: "Tags",
                keyColumn: "Id",
                keyValue: 26);

            migrationBuilder.DeleteData(
                table: "Tags",
                keyColumn: "Id",
                keyValue: 27);

            migrationBuilder.DeleteData(
                table: "Tags",
                keyColumn: "Id",
                keyValue: 28);

            migrationBuilder.DeleteData(
                table: "Tags",
                keyColumn: "Id",
                keyValue: 29);

            migrationBuilder.DeleteData(
                table: "Tags",
                keyColumn: "Id",
                keyValue: 30);

            migrationBuilder.DeleteData(
                table: "Tags",
                keyColumn: "Id",
                keyValue: 31);

            migrationBuilder.DeleteData(
                table: "Tags",
                keyColumn: "Id",
                keyValue: 32);

            migrationBuilder.DeleteData(
                table: "Tags",
                keyColumn: "Id",
                keyValue: 33);

            migrationBuilder.DeleteData(
                table: "Tags",
                keyColumn: "Id",
                keyValue: 34);

            migrationBuilder.DeleteData(
                table: "Tags",
                keyColumn: "Id",
                keyValue: 35);

            migrationBuilder.DeleteData(
                table: "Tags",
                keyColumn: "Id",
                keyValue: 36);

            migrationBuilder.DeleteData(
                table: "Tags",
                keyColumn: "Id",
                keyValue: 37);

            migrationBuilder.DeleteData(
                table: "Tags",
                keyColumn: "Id",
                keyValue: 38);

            migrationBuilder.DeleteData(
                table: "Tags",
                keyColumn: "Id",
                keyValue: 39);

            migrationBuilder.DeleteData(
                table: "Tags",
                keyColumn: "Id",
                keyValue: 40);

            migrationBuilder.DeleteData(
                table: "Profiles",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Profiles",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Profiles",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Tags",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Tags",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Tags",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 7);

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 8);

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 9);

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 10);

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 11);

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 12);

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 13);

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 14);

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 15);

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 16);

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 3);
        }
    }
}
