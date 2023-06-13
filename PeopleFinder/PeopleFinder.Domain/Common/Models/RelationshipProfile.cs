using PeopleFinder.Domain.Entities;

namespace PeopleFinder.Domain.Common.Models;

public record RelationshipProfile(Profile Profile, Relationship? Relationship);