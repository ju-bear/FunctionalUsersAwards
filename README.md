# FunctionalUsersAwards

It's an example of an application made in functional style with Domain-Driven Development practices.

# Domain description

UsersAwards consists of 2 main entities: 
  - Users 
  - Awards 
  
One user can have many awards and one award can belong to many users. There are certain restrictions for these entities:
- For Award:
  - Award title cannot be empty or longer than 55 characters
- For User:
  - User cannot have the same award twice. Awards are considered the same if they have the same title
  - Username cannot be empty or longer than 55 characters
  
# Application Requirements

Application should be able to do the following:
- Create a valid user
- Create a valid award
- Add an award to a user
- Delete an award
- Delete a user
- Get the list of all added awards
- Get the list of all added users (with their awards)
- Get an award by id
- Get a user by id
