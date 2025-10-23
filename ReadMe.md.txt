Summary of Task1 -----------------------------------------------------------------------------
the user entity and service interfaces  are changed to include age parameter, and the controller to  pass the age data.

include Age parameter in UserModel, ICreateUserService, CreateUserService,  IUpdateUserService, UpdateUserService so the api request body can supply it

For “Update User – A” POST request, the null value updating  for name and email fields, is handled. It is made to update those fields only if the passed in values are not null.

Requirement can be specified for a null or blank value, meaning say for email, the existing email data has to be cleared when user does not pass any value?. As of now I have updated the code so that it wont do update on name, email, age if they are null or “”
Somewhat like that for “Update User – B” POST request, the annualSalary value being null was throwing an exception and now it is handled to avoid the exception and so it takes the null and calculates even the monthly salary to be null.
For Update User – C request, the null value for the tags’ list is handled so that while updating any user’s data, if tags are not provided then the existing tags are not touched. So that this user can still be fetched if a search is done thru GetUserByTag() call.
For Get User By Tag request, the GetUsersByTag() api is now implemented in UserController. 

Summary of Task2 -----------------------------------------------------------------------------
Product and Order Entities are added.
APIs are written for CRUD operations.
An InMemoryDataProvider class is written which holds the products’ and orders’ data in lists when the session is active.

Appropriate messages have been given for all entities (users, products and orders) for better clarity in postman instead of “404 Not Found” message.
